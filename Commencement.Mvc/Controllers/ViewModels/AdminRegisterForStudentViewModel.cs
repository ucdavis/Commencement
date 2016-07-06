using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class AdminRegisterForStudentViewModel
    {
        public RegistrationModel RegistrationModel { get; set; }
        public IEnumerable<MajorCode> Majors { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }

        public static AdminRegisterForStudentViewModel Create(IRepository repository, RegistrationModel registrationModel)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(registrationModel != null, "registrationModel is required.");

            var viewModel = new AdminRegisterForStudentViewModel()
            {
                RegistrationModel = registrationModel,
                Majors = repository.OfType<MajorCode>().GetAll(),
                Ceremonies = repository.OfType<Ceremony>().Queryable.Where(a=>a.TermCode == TermService.GetCurrent()).ToList(),
            };
            
            return viewModel;
        }
    }
}