using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationPetitionModel
    {
        public RegistrationPetition RegistrationPetition { get; set; }
        public Student Student { get; set; }

        public IEnumerable<State> States { get; set; }

        public static RegistrationPetitionModel Create(IRepository repository)
        {
            var viewModel = new RegistrationPetitionModel { States = repository.OfType<State>().GetAll() };

            viewModel.RegistrationPetition = new RegistrationPetition(); //TODO: Get registration info

            return viewModel;
        }
    }
}
