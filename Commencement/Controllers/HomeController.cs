using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using UCDArch.Web.Attributes;
using MvcContrib;

namespace Commencement.Controllers
{
    //[HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            if (User.IsInRole(RoleNames.RoleAdmin) || User.IsInRole(RoleNames.RoleUser)) return this.RedirectToAction<AdminController>(a => a.Index());

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
