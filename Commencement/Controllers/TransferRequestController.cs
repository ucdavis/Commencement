using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class TransferRequestController : ApplicationController
    {
        private readonly ICeremonyService _ceremonyService;

        public TransferRequestController(ICeremonyService ceremonyService)
        {
            _ceremonyService = ceremonyService;
        }

        //
        // GET: /TransferRequest/

        public ActionResult Index()
        {
            var ceremonyIds = _ceremonyService.GetCeremonyIds(User.Identity.Name, TermService.GetCurrent());
            var requests = Repository.OfType<TransferRequest>().Queryable.Where(a => a.Pending && ceremonyIds.Contains(a.Ceremony.Id));

            return View(requests);
        }

        public ActionResult Review(int id)
        {
            var ceremonyIds = _ceremonyService.GetCeremonyIds(User.Identity.Name, TermService.GetCurrent());
            var request = Repository.OfType<TransferRequest>().GetNullableById(id);

            if (request == null)
            {
                Message = "Request could not be found.";
                return RedirectToAction("Index");
            }

            if (!ceremonyIds.Contains(request.Ceremony.Id))
            {
                Message = "You do not have access to approve this request.";
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpPost]
        public ActionResult Review(int id, bool approved)
        {
            var ceremonyIds = _ceremonyService.GetCeremonyIds(User.Identity.Name, TermService.GetCurrent());
            var request = Repository.OfType<TransferRequest>().GetNullableById(id);

            if (request == null)
            {
                Message = "Request could not be found.";
                return RedirectToAction("Index");
            }

            if (!ceremonyIds.Contains(request.Ceremony.Id))
            {
                Message = "You do not have access to approve this request.";
                return RedirectToAction("Index");
            }

            if (approved)
            {
                var rp = request.RegistrationParticipation;
                
                rp.Ceremony = request.Ceremony;
                if (rp.NumberTickets > request.Ceremony.TicketsPerStudent)
                {
                    rp.NumberTickets = request.Ceremony.TicketsPerStudent;
                }
                
                if (rp.ExtraTicketPetition != null)
                {
                    if (rp.ExtraTicketPetition.NumberTickets > request.Ceremony.ExtraTicketPerStudent)
                    {
                        rp.ExtraTicketPetition.NumberTickets = request.Ceremony.ExtraTicketPerStudent;
                    }
                }

                Repository.OfType<RegistrationParticipation>().EnsurePersistent(rp);
            }

            request.Pending = false;
            Repository.OfType<TransferRequest>().EnsurePersistent(request);

            Message = string.Format("Transfer request for {0} has been {1}.", request.RegistrationParticipation.Registration.Student.FullName, approved ? "approved" : "denied");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Create a transfer request
        /// </summary>
        /// <param name="id">Registration Participation Id</param>
        /// <returns></returns>
        public ActionResult Create(int id)
        {
            var rp = Repository.OfType<RegistrationParticipation>().GetNullableById(id);

            if (rp == null)
            {
                Message = "Registration participation could not be loaded, please try again.";
                return RedirectToAction("Index", "Admin");
            }

            var viewModel = TransferRequestViewModel.Create(Repository, rp, User.Identity.Name);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(int id, TransferRequest transferRequest)
        {
            var rp = Repository.OfType<RegistrationParticipation>().GetNullableById(id);

            if (rp == null)
            {
                Message = "Registration participation could not be loaded, please try again.";
                return RedirectToAction("Index", "Admin");
            }

            transferRequest.User = Repository.OfType<vUser>().Queryable.FirstOrDefault(a => a.LoginId == User.Identity.Name);
            transferRequest.RegistrationParticipation = rp;

            ModelState.Clear();
            transferRequest.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Message = "Transfer request created.";
                Repository.OfType<TransferRequest>().EnsurePersistent(transferRequest);

                return RedirectToAction("StudentDetails", "Admin", new {id = rp.Registration.Student.Id});
            }

            var viewModel = TransferRequestViewModel.Create(Repository, rp, User.Identity.Name);
            return View(viewModel);
        }

    }
}
