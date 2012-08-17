using System;
using System.Web.Mvc;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;
using System.Linq;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationModel
    {
        // Values for dropdowns
        public IEnumerable<State> States { get; set; }
        //public IEnumerable<CollegeCeremonyInfo> CollegeCeremonyInfos { get; set; }
        public MultiSelectList SpecialNeeds { get; set; }
        public IQueryable<vTermCode> FutureTerms { get; set; }

        public List<SpecialNeed> FullSpecialNeeds { get; set; }

        // registration information
        public Registration Registration { get; set; }
        public Student Student { get; private set; }
        public IEnumerable<CeremonyParticipation> Participations { get; set; }

        // not sure if these are even used
        //public IEnumerable<Ceremony> Ceremonies { get; private set; }
        

        public static RegistrationModel Create(IRepository repository, IList<Ceremony> ceremonies, Student student, Registration registration = null, List<CeremonyParticipation> ceremonyParticipations = null, bool edit = false)
        {
            var viewModel = new RegistrationModel
                                {
                                    States = repository.OfType<State>().GetAll(),
        
                                    Registration = registration ?? new Registration() {Student = student},
                                    Student = student,
                                    //Ceremonies = ceremonies,
                                    FutureTerms = repository.OfType<vTermCode>().Queryable.Where(a=>a.EndDate > DateTime.Now)
                                };

            // since the change to using a drop down, this is somewhat unnecessary, but should we move back to checkboxes, it would be a simple change
            var specialNeeds = repository.OfType<SpecialNeed>().Queryable.Where(a=>a.IsActive).ToList();
            viewModel.FullSpecialNeeds = specialNeeds;
            if (viewModel.Registration.Id != 0 && viewModel.Registration.SpecialNeeds != null)
            {
                viewModel.SpecialNeeds = new MultiSelectList(specialNeeds, "Id", "Name", registration.SpecialNeeds.Select(a=>a.Id).ToList());
            }
            else
            {
                viewModel.SpecialNeeds = new MultiSelectList(specialNeeds, "Id", "Name");
            }

            var participations = new List<CeremonyParticipation>();

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

            // the student's colleges
            var studentsColleges = viewModel.Student.Majors.Select(a => a.College).Distinct().ToList();

            // load this quarter's ceremony information
            //var ceremonyInfos = GatherCollegeCeremonyInfo(repository);
            
            // go through all the student's colleges
            for (var i = 0; i < studentsColleges.Count(); i++)
            {
                MajorCode major = null;
                Ceremony ceremony = null;
                College college = studentsColleges[i];
                //var ceremonyInfo = ceremonyInfos.Where(a => a.College == college).FirstOrDefault();

                foreach (var maj in viewModel.Student.Majors.Where(a => a.College == college))
                {
                    major = maj;
                    ceremony = GetCeremony(repository, maj);

                    if (major != null && ceremony != null) break;
                }

                // found a valid ceremony to go with the major
                if (major != null && ceremony != null)
                {
                    var part = CreateCeremonyParticipation(i, edit, student, major, ceremony, registration, null, repository);
                    if (part != null && !participations.Any(a => a.Major.College == college))
                    {
                        participations.Add(part);
                    }
                }
            }

            viewModel.Participations = participations;
            //viewModel.CollegeCeremonyInfos = ceremonyInfos;

            return viewModel;
        }

        private static CeremonyParticipation CreateCeremonyParticipation(int index, bool edit, Student student, MajorCode major, Ceremony ceremony, Registration registration, List<CeremonyParticipation> ceremonyParticipations, IRepository repository)
        {
            if (ceremony != null)
            {
                var part = new CeremonyParticipation();
                part.Index = index;
                part.Major = major;
                part.Ceremony = ceremony;
                part.Edit = edit;
                part.NeedsPetition = (student.TotalUnits < ceremony.MinUnits && student.TotalUnits >= ceremony.PetitionThreshold) || (ceremony.TermCode.RegistrationDeadline < DateTime.Now);

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

                // set the college/ceremony lookup values
                var college = repository.OfType<College>().Queryable.FirstOrDefault(x => x.Display && x.Id == major.College.Id);
                var ceremonies = repository.OfType<Ceremony>().Queryable.Where(x => x.TermCode == TermService.GetCurrent() && x.Colleges.Contains(college)).ToList();
                var majors = ceremonies.SelectMany(x => x.Majors);

                part.College = college;
                part.Ceremonies = ceremonies;
                part.MajorCodes = majors.Where(x => x.College == college && x.ConsolidationMajor == null).OrderBy(x => x.MajorName).ToList();

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
