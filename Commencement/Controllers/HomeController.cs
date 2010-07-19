using System.Web.Mvc;
using UCDArch.Web.Attributes;

namespace Commencement.Controllers
{
    [HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
