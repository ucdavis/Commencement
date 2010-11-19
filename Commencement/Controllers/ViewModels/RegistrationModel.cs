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
        
        public static RegistrationModel Create(IRepository repository, IList<Ceremony> ceremonies, Student student, Registration registration = null)
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

            return viewModel;
        }
    }

    public class CeremonyParticipation
    {
        public bool Participate { get; set; }
        public int Tickets { get; set; }
        public Ceremony Ceremony { get; set; }
        public MajorCode Major { get; set; }
    }
}
