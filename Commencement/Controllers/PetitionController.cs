using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Commencement.Controllers.ViewModels;


namespace Commencement.Controllers
{
    public class PetitionController : ApplicationController
    {
        //
        // GET: /Petition/

        public ActionResult Index()
        {
            return View();
        }

         public ActionResult Register()
        {
            //Get student info and create registration model
            var viewModel = RegistrationPetitionModel.Create(Repository);

            return View(viewModel);
        }

         public ActionResult ExtraTicketPetition()
         {
             //Get student info and create ExtraTicketPetition model
             var viewModel = ExtraTicketPetitionModel.Create(Repository);

             return View(viewModel);
         }

    }
}
