using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib;
using MvcContrib.Attributes;
using UCDArch.Web.Helpers;


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Registration Id</param>
        /// <returns></returns>
         [PageTrackingFilter]
         [StudentsOnly]
         public ActionResult ExtraTicketPetition(int id)
         {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null                                        // requires registration
                || registration.Student.Login != CurrentUser.Identity.Name  // validate the user
                ) 
            { 
                return this.RedirectToAction<StudentController>(a => a.Index()); 
            }

            // already submitted extra ticket petition
            if (registration.ExtraTicketPetition != null)
            {
                Message = "You have already submitted an extra ticket petition.";
                return this.RedirectToAction<StudentController>(a => a.Index()); 
            }

            var viewModel = ExtraTicketPetitionModel.Create(Repository, registration);

            return View(viewModel);
         }

        [AcceptPost]
         [StudentsOnly]
        public ActionResult ExtraTicketPetition(int id, int numberTickets)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null                                        // requires registration
                || registration.Student.Login != CurrentUser.Identity.Name  // validate the user
                )
            {
                return this.RedirectToAction<StudentController>(a => a.Index());
            }

            // already submitted extra ticket petition
            if (registration.ExtraTicketPetition != null)
            {
                Message = "You have already submitted an extra ticket petition.";
                return this.RedirectToAction<StudentController>(a => a.Index());
            }

            var ticketPetition = new ExtraTicketPetition(numberTickets);

            registration.ExtraTicketPetition = ticketPetition;

            // validate the object
            ticketPetition.TransferValidationMessagesTo(ModelState);
            registration.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                
                Repository.OfType<Registration>().EnsurePersistent(registration);

                Message = "Ticket petition has been successfully submitted.";
                return this.RedirectToAction<StudentController>(a => a.Index());
            }

            var viewModel = ExtraTicketPetitionModel.Create(Repository, registration);
            viewModel.ExtraTicketPetition = ticketPetition;
            return View();
        }
    }
}
