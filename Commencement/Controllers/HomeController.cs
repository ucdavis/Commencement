using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Attributes;
using MvcContrib;

namespace Commencement.Controllers
{
    public class HomeController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;

        public HomeController(IRepositoryWithTypedId<Student,Guid> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
            // authorized user
            if (User.IsInRole(RoleNames.RoleAdmin) || User.IsInRole(RoleNames.RoleUser)) return this.RedirectToAction<AdminController>(a => a.Index());

            // display a landing page
            return View(TermService.GetCurrent());
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
