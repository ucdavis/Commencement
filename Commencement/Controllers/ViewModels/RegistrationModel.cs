using System;
using System.Web.Mvc;
using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;
using System.Linq;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationModel
    {
        public Registration Registration { get; set; }
        public Student Student { get; private set; }
        public IEnumerable<Ceremony> Ceremonies { get; private set; }
        public IEnumerable<State> States { get; set; }
        public MultiSelectList SpecialNeeds { get; set; }
        public IEnumerable<CeremonyParticipation> Participations { get; set; }
        public IQueryable<vTermCode> FutureTerms { get; set; }

        public static RegistrationModel Create(IRepository repository, IList<Ceremony> ceremonies, Student student, Registration registration = null, List<CeremonyParticipation> ceremonyParticipations = null, bool edit = false)
        {
            var viewModel = new RegistrationModel
                                {
                                    States = repository.OfType<State>().GetAll(),
                                    Registration = registration ?? new Registration() {Student = student},
                                    Ceremonies = ceremonies,
                                    Student = student,
                                    FutureTerms = repository.OfType<vTermCode>().Queryable.Where(a=>a.EndDate > DateTime.Now)
                                };

            var specialNeeds = repository.OfType<SpecialNeed>().Queryable.Where(a=>a.IsActive).ToList();
            if (viewModel.Registration.Id != 0 && viewModel.Registration.SpecialNeeds != null)
            {
                viewModel.SpecialNeeds = new MultiSelectList(specialNeeds, "Id", "Name", registration.SpecialNeeds.Select(a=>a.Id).ToList());
            }
            else
            {
                viewModel.SpecialNeeds = new MultiSelectList(specialNeeds, "Id", "Name");
            }

            var participations = new List<CeremonyParticipation>();

            // populate a list of available participations that a student can attend
            for (int i = 0; i < viewModel.Student.Majors.Count; i++)
            {
                var major = viewModel.Student.Majors[i];
                var ceremony = viewModel.Ceremonies.Where(a => a.Majors.Contains(major)).FirstOrDefault();

                // set the override ceremony if the student has one
                if (student.Ceremony != null) ceremony = student.Ceremony;

                var part = CreateCeremonyParticipation(i, edit, student, major, ceremony, registration, ceremonyParticipations);
                if (part != null) participations.Add(part);
            }

            // populate in any ceremonies that the student has registered for
            if (registration != null)
            {
                foreach (var a in registration.RegistrationParticipations)
                {
                    if (!participations.Any(b => b.Major == a.Major))
                    {
                        var part = CreateCeremonyParticipation(participations.Count, edit, student, a.Major, a.Ceremony, registration, null);
                        if (part != null) participations.Add(part);
                    }
                }
            }

            viewModel.Participations = participations;

            return viewModel;
        }

        private static CeremonyParticipation CreateCeremonyParticipation(int index, bool edit, Student student, MajorCode major, Ceremony ceremony, Registration registration, List<CeremonyParticipation> ceremonyParticipations)
        {
            if (ceremony != null)
            {
                var part = new CeremonyParticipation();
                part.Index = index;
                part.Major = major;
                part.Ceremony = ceremony;
                part.Edit = edit;
                part.NeedsPetition = student.TotalUnits < ceremony.MinUnits && student.TotalUnits >= ceremony.PetitionThreshold;

                if (ceremonyParticipations != null)
                {
                    var existingPart =
                        ceremonyParticipations.Where(a => a.Ceremony == ceremony && a.Major == major).FirstOrDefault
                            ();
                    if (existingPart != null)
                    {
                        part.Tickets = existingPart.Tickets;
                        part.Participate = existingPart.Participate;
                        part.Cancel = existingPart.Cancel;
                        part.NeedsPetition = existingPart.NeedsPetition;
                    }
                }

                if (registration != null)
                {
                    var regPart =
                        registration.RegistrationParticipations.Where(
                            a => a.Ceremony.Id == ceremony.Id && a.Major.Id == major.Id).FirstOrDefault();

                    if (regPart != null)
                    {
                        part.ParticipationId = regPart.Id;
                        part.Tickets = regPart.NumberTickets;
                        part.Participate = !regPart.Cancelled;
                        part.Cancel = regPart.Cancelled;
                        part.NeedsPetition = false;                  // registered, don't need to petition
                    }
                }

                return part;
            }

            return null;
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
        
        public bool NeedsPetition { get; set; }
        public string PetitionReason { get; set; }
        public vTermCode CompletionTerm { get; set; }
        public string TransferCollege { get; set; }
        public string TransferUnits { get; set; }
    }
}
