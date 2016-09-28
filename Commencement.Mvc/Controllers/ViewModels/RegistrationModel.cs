using System;
using System.Web.Mvc;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;
using System.Linq;
using Commencement.Core.Helpers;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationModel
    {
        // Values for dropdowns
        public IEnumerable<State> States { get; set; }
        //public IEnumerable<CollegeCeremonyInfo> CollegeCeremonyInfos { get; set; }
        public MultiSelectList SpecialNeedsOld { get; set; }
        public IQueryable<vTermCode> FutureTerms { get; set; }

        public List<SpecialNeed> FullSpecialNeeds { get; set; }

        // registration information
        public Registration Registration { get; set; }
        public Student Student { get; private set; }
        public IEnumerable<CeremonyParticipation> Participations { get; set; }

        public int? FirstVisaLetterRequest { get; set; }

        // not sure if these are even used
        //public IEnumerable<Ceremony> Ceremonies { get; private set; }
        

        public static RegistrationModel Create(IRepository repository, IList<Ceremony> ceremonies, Student student, Registration registration = null, List<CeremonyParticipation> ceremonyParticipations = null, bool edit = false, bool admin = false)
        {
            var viewModel = new RegistrationModel
                                {
                                    States = repository.OfType<State>().GetAll(),
        
                                    Registration = registration ?? new Registration() {Student = student},
                                    Student = student,
                                    //Ceremonies = ceremonies,
                                    FutureTerms = repository.OfType<vTermCode>().Queryable.Where(a=>a.EndDate > DateTime.UtcNow.ToPacificTime())
                                };

            // since the change to using a drop down, this is somewhat unnecessary, but should we move back to checkboxes, it would be a simple change
            var specialNeeds = repository.OfType<SpecialNeed>().Queryable.Where(a=>a.IsActive).ToList();
            viewModel.FullSpecialNeeds = specialNeeds;
            if (viewModel.Registration.Id != 0 && viewModel.Registration.SpecialNeeds != null)
            {
                viewModel.SpecialNeedsOld = new MultiSelectList(specialNeeds, "Id", "Name", registration.SpecialNeeds.Select(a=>a.Id).ToList());
            }
            else
            {
                viewModel.SpecialNeedsOld = new MultiSelectList(specialNeeds, "Id", "Name");
            }

            var participations = new List<CeremonyParticipation>();

            if (ceremonyParticipations == null)
            {
                // populate in any ceremonies that the student has registered for
                if (registration != null)
                {
                    foreach (var a in registration.RegistrationParticipations)
                    {
                        if (!participations.Any(b => b.Major == a.Major))
                        {
                            var part = CreateCeremonyParticipation(participations.Count, edit, student, a.Major, a.Ceremony, registration, null, repository);
                            if (part != null) participations.Add(part);
                        }
                    }
                }

                foreach (var major in student.Majors)
                {
                    Ceremony ceremony = GetCeremony(repository, major);

                    if (ceremony != null && !participations.Any(a => a.Ceremony == ceremony))
                    {
                        var part = CreateCeremonyParticipation(participations.Count, edit, student, major, ceremony, registration, null, repository, admin);
                        if (part != null)
                        {
                            participations.Add(part);
                        }
                    }
                }    
            }
            else
            {
                // fill in the majors
                for (var i = 0; i < ceremonyParticipations.Count; i++)
                {
                    var part = ceremonyParticipations[i];
                    part.MajorCodes = part.Ceremony.Majors.OrderBy(x => x.MajorName).ToList();
                    part.Index = i;
                }

                participations = ceremonyParticipations;
            }

            viewModel.Participations = participations;

            return viewModel;
        }

        private static CeremonyParticipation CreateCeremonyParticipation(int index, bool edit, Student student, MajorCode major, Ceremony ceremony, Registration registration, List<CeremonyParticipation> ceremonyParticipations, IRepository repository, bool admin = false)
        {
            if (ceremony != null)
            {
                var part = new CeremonyParticipation();
                part.Index = index;
                part.Major = major;
                part.Ceremony = ceremony;
                part.Edit = edit;
                part.NeedsPetition = (student.TotalUnits < ceremony.MinUnits && student.TotalUnits >= ceremony.PetitionThreshold) || (ceremony.TermCode.CanRegister() && !ceremony.TermCode.CanRegister(true));

                if (ceremonyParticipations != null)
                {
                    var existingPart = ceremonyParticipations.FirstOrDefault(a => a.Ceremony == ceremony && a.Major == major);
                    if (existingPart != null)
                    {
                        part.Tickets = existingPart.Tickets;
                        part.Participate = existingPart.Participate;
                        part.Cancel = existingPart.Cancel;
                        part.NeedsPetition = existingPart.NeedsPetition;
                        part.TicketDistributionMethod = existingPart.TicketDistributionMethod;
                    }
                }

                if (registration != null)
                {
                    var regPart = registration.RegistrationParticipations.FirstOrDefault(a => a.Ceremony.Id == ceremony.Id && a.Major.Id == major.Id);

                    if (regPart != null)
                    {
                        part.ParticipationId = regPart.Id;
                        part.Tickets = regPart.NumberTickets;
                        part.Participate = !regPart.Cancelled;
                        part.Cancel = regPart.Cancelled;
                        part.NeedsPetition = false;                  // registered, don't need to petition
                        part.TicketDistributionMethod = regPart.TicketDistributionMethod;
                    }
                }

                if (admin)
                {
                    // set the college/ceremony lookup values
                    var college = repository.OfType<College>().Queryable.FirstOrDefault(x => x.Display && x.Id == major.College.Id);
                    var ceremonies = repository.OfType<Ceremony>().Queryable.Where(x => x.TermCode == TermService.GetCurrent() && x.Colleges.Contains(college)).ToList();
                    var majors = ceremonies.SelectMany(x => x.Majors).Where(x => x.IsActive);

                    part.College = college;
                    part.Ceremonies = ceremonies;
                    part.MajorCodes = majors.Where(x => x.College == college && x.ConsolidationMajor == null).OrderBy(x => x.MajorName).ToList();    
                }
                else
                {
                    part.MajorCodes = part.Ceremony.Majors.OrderBy(x => x.MajorName).ToList();
                }

                return part;
            }

            return null;
        }

        private static Ceremony GetCeremony(IRepository repository, MajorCode major)
        {
            return repository.OfType<Ceremony>().Queryable.FirstOrDefault(x => x.TermCode == TermService.GetCurrent() && x.Majors.Contains(major));
        }
    }

    public class CeremonyParticipation
    {
        public CeremonyParticipation()
        {
            Participate = false;
            Cancel = false;
            Edit = false;
        }

        public int? ParticipationId { get; set; }

        public int Index { get; set; }
        public bool Participate { get; set; }   // if this should be just a regular participation
        public bool Cancel { get; set; }        // if this should cancel a participation
        public bool Petition { get; set; }      // if this should be a petition
        public bool Edit { get; set; }
        public int Tickets { get; set; }
        public Ceremony Ceremony { get; set; }
        public MajorCode Major { get; set; }

        public TicketDistributionMethod TicketDistributionMethod { get; set; }

        public bool NeedsPetition { get; set; }
        public string PetitionReason { get; set; }
        public vTermCode CompletionTerm { get; set; }
        public string TransferCollege { get; set; }
        public string TransferUnits { get; set; }

        // lookup values for the individual college/ceremony information
        public College College { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public IEnumerable<MajorCode> MajorCodes { get; set; }
    }
}
