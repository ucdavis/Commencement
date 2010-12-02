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

        public static RegistrationModel Create(IRepository repository, IList<Ceremony> ceremonies, Student student, Registration registration = null, List<CeremonyParticipation> ceremonyParticipations = null, bool edit = false)
        {
            var viewModel = new RegistrationModel
                                {
                                    States = repository.OfType<State>().GetAll(),
                                    Registration = registration ?? new Registration() {Student = student},
                                    Ceremonies = ceremonies,
                                    Student = student
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

                var part = new CeremonyParticipation();
                part.Index = i;
                part.Major = major;
                part.Ceremony = ceremony;
                part.Edit = edit;

                if (ceremonyParticipations != null)
                {
                    var existingPart = ceremonyParticipations.Where(a => a.Ceremony == ceremony && a.Major == major).FirstOrDefault();
                    if (existingPart != null)
                    {
                        part.Tickets = existingPart.Tickets;
                        part.Participate = existingPart.Participate;
                        part.Cancel = existingPart.Cancel;
                    }
                }

                if (registration != null)
                {
                    var regPart = registration.RegistrationParticipations.Where(a=>a.Ceremony == ceremony && a.Major == major).FirstOrDefault();
                    if (regPart != null)
                    {
                        part.Tickets = regPart.NumberTickets;
                        part.Participate = !regPart.Cancelled;
                        part.Cancel = regPart.Cancelled;
                    }
                }

                participations.Add(part);
            }

            viewModel.Participations = participations;

            return viewModel;
        }

        public void SetTickets(Registration registration)
        {
            foreach (var a in registration.RegistrationParticipations)
            {
                var part = this.Participations.Where(b => b.Ceremony == a.Ceremony && b.Major == a.Major).FirstOrDefault();

                if (part != null)
                {
                    // registered, set the ticket numbers
                    part.Participate = true;
                    part.Tickets = a.NumberTickets;
                }
            }
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

        public int Index { get; set; }
        public bool Participate { get; set; }
        public bool Cancel { get; set; }
        public bool Edit { get; set; }
        public int Tickets { get; set; }
        public Ceremony Ceremony { get; set; }
        public MajorCode Major { get; set; }
    }
}
