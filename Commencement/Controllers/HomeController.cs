using System.Web.Mvc;
using Commencement.Controllers.Helpers;
using UCDArch.Web.Attributes;

namespace Commencement.Controllers
{
    //[HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            var term = TermService.GetCurrent();

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
