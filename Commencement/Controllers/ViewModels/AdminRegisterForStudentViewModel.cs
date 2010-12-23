using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminRegisterForStudentViewModel
    {
        public RegistrationModel RegistrationModel { get; set; }

        public SelectList Majors { get; set; }
        public SelectList Ceremonies { get; set; }

        public static AdminRegisterForStudentViewModel Create(IRepository repository, RegistrationModel registrationModel)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(registrationModel != null, "registrationModel is required.");

            var viewModel = new AdminRegisterForStudentViewModel()
            {
                RegistrationModel = registrationModel,
                Majors = new SelectList(repository.OfType<MajorCode>().GetAll(), "Id", "Name"),
                Ceremonies = new SelectList(repository.OfType<Ceremony>().Queryable.Where(a=>a.TermCode == TermService.GetCurrent()).ToList(), "Id", "DateTime")
            };
            
            return viewModel;
        }
    }
}