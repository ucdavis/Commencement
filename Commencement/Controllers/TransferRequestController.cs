using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using FluentNHibernate.Conventions.Inspections;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class TransferRequestController : ApplicationController
    {
        private readonly ICeremonyService _ceremonyService;
        private readonly IErrorService _errorService;
        private readonly IEmailService _emailService;

        public TransferRequestController(ICeremonyService ceremonyService, IErrorService errorService, IEmailService emailService)
        {
            _ceremonyService = ceremonyService;
            _errorService = errorService;
            _emailService = emailService;
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
        public ActionResult Review(int id, bool? approved, int numberTickets)//, int? numberTicketsRequested, int? numberTicketsRequestedStreaming, int? numberExtraTickets, int? numberExtraTicketsStreaming)
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

            if (approved.HasValue)
            {
                if (approved.Value)
                {
                    var additionalMessage = string.Empty;

                    var rp = request.RegistrationParticipation;
                    rp.Ceremony = request.Ceremony;
                    if (rp.NumberTickets != numberTickets) additionalMessage += "<br/><strong>Please note that the # of tickets you have requested has changed.</strong>";
                    rp.NumberTickets = numberTickets;
                    rp.Major = request.MajorCode;
                    rp.DateUpdated = DateTime.Now;

                    if (rp.ExtraTicketPetition != null)
                    {
                        // reset
                        rp.ExtraTicketPetition.IsPending = true;
                        rp.ExtraTicketPetition.IsApproved = false;
                        rp.ExtraTicketPetition.NumberTickets = null;
                        rp.ExtraTicketPetition.NumberTicketsStreaming = null;

                        // adjust tickets if necessary
                        if (rp.ExtraTicketPetition.NumberTicketsRequested > request.Ceremony.ExtraTicketPerStudent)
                        {
                            rp.ExtraTicketPetition.NumberTicketsRequested = request.Ceremony.ExtraTicketPerStudent;
                            additionalMessage += "<br/><strong>Please note that your extra ticket petition has changed, please log in to see the updated # of tickets.</strong>";
                        }
                    }

                    Repository.OfType<RegistrationParticipation>().EnsurePersistent(rp);

                    // trigger the confirmation email
                    if (rp.Registration.RegistrationParticipations.Count > 0)
                    {
                        try
                        {
                            // add email for registration into queue
                            _emailService.QueueRegistrationConfirmation(rp.Registration, additionalMessage);
                        }
                        catch (Exception ex)
                        {
                            _errorService.ReportError(ex);
                            Message += StaticValues.Student_Email_Problem;
                        }
                        Message += StaticValues.Student_Register_Successful;
                    }

                }

                request.Pending = false;
                Repository.OfType<TransferRequest>().EnsurePersistent(request);

                Message = string.Format("Transfer request for {0} has been {1}.", request.RegistrationParticipation.Registration.Student.FullName, approved.Value ? "approved" : "denied");

                return RedirectToAction("Index");    
            }

            Message = "Approval decision is required.";
            return View(request);
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

        public JsonNetResult GetMajorsByCeremony(int ceremonyId)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyId);

            if (ceremony != null)
            {
                return new JsonNetResult(ceremony.Majors.Select(a => new {Id = a.Id, Name = a.MajorName}).Distinct());
            }

            return new JsonNetResult(false);
        }

        public ActionResult MajorList(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);

            if (ceremony != null)
            {
                return View(ceremony.Majors);
            }

            return RedirectToAction("Index", "Error");
        }

    }
}
