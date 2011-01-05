using System.Web.Mvc;
using MvcContrib;

namespace Commencement.Controllers
{
    public class ErrorController : ApplicationController
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult SJA()
        {
            return View();
        }

        public ActionResult PreviouslyWalked()
        {
            return View();
        }

        /// <summary>
        /// Displays message for students who cannot be found, or do not meet criteria
        /// for commencement registration
        /// </summary>
        /// <returns></returns>
        public ActionResult NotEligible()
        {
            return View();
        }

        /// <summary>
        /// Message for students who need to register with other systems
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterOtherSystem()
        {
            return View();
        }

        /// <summary>
        /// Error message page for when no ceremonies are available for registration
        /// </summary>
        /// <returns></returns>
        public ActionResult NotOpen()
        {
            return View();
        }

        public ActionResult UnauthorizedAccess()
        {
            return View();
        }
    }
}
