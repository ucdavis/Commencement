using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Commencement.Controllers.ViewModels;

namespace Commencement.Controllers
{
    public class StudentController : ApplicationController
    {
        //
        // GET: /Student/

        public ActionResult Index()
        {
            //Gather student info

            //Check for prior registration

            //Check student untis and major

            return View();
        }

        public ActionResult Register()
        {
            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(Repository);

            return View(viewModel);
        }
    }
}
