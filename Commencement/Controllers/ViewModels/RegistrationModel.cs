using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationModel
    {
        public Registration Registration { get; set; }

        public Student Student { get; private set; }

        public Ceremony Ceremony { get; private set; }
        
        public IEnumerable<State> States { get; set; }
        
        public static RegistrationModel Create(IRepository repository, Student student, Ceremony ceremony)
        {
            var viewModel = new RegistrationModel
                                {
                                    States = repository.OfType<State>().GetAll(),
                                    Registration = new Registration(),
                                    Ceremony = ceremony,
                                    Student = student
                                };

            return viewModel;
        }
    }
}
