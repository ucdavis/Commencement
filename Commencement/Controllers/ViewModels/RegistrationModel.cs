using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationModel
    {
        public Registration Registration { get; set; }
        
        //public string Address1 { get; set; }
        //public string Address2 { get; set; }
        //public string Address3 { get; set; }
        
        //public string City { get; set; }
        //public string Zip { get; set; }
        //public string Email { get; set; }

        public IEnumerable<State> States { get; set; }
        
        public static RegistrationModel Create(IRepository repository)
        {
            var viewModel = new RegistrationModel {States = repository.OfType<State>().GetAll()};

            viewModel.Registration = new Registration(); //TODO: Get registration info

            return viewModel;
        }
    }
}
