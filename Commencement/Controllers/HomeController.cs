using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Attributes;
using MvcContrib;

namespace Commencement.Controllers
{
    //[HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, string> _studentRepository;

        public HomeController(IRepositoryWithTypedId<Student,string> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
            // authorized user
            if (User.IsInRole(RoleNames.RoleAdmin) || User.IsInRole(RoleNames.RoleUser)) return this.RedirectToAction<AdminController>(a => a.Index());
            // student
            if (StudentAccess.IsStudent(_studentRepository, User.Identity.Name)) return this.RedirectToAction<StudentController>(a => a.Index());
            // not authorized, potentially student who needs to petition
            return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.UnauthorizedAccess));
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
