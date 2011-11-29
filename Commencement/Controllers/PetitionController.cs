using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using MvcContrib;
using UCDArch.Core.PersistanceSupport;


namespace Commencement.Controllers
{
    
    public class PetitionController : ApplicationController
    {
        private readonly IEmailService _emailService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IPetitionService _petitionService;
        private readonly IErrorService _errorService;


        public PetitionController(IEmailService emailService, ICeremonyService ceremonyService, IPetitionService petitionService, IErrorService errorService)
        {
            _emailService = emailService;
            _ceremonyService = ceremonyService;
            _petitionService = petitionService;
            _errorService = errorService;
        }

        //
        // GET: /Petition/
        /// <summary>
        /// #1
        /// </summary>
        /// <returns></returns>
        [AnyoneWithRole]
        public ActionResult Index()
        {
            return View();
        }

        #region Extra Ticket Petitions
        /// <summary>
        /// #2
        /// </summary>
        /// <param name="ceremonyId"></param>
        /// <returns></returns>
        [AnyoneWithRole]
        public ActionResult ExtraTicketPetitions(int? ceremonyId)
        {
            var viewModel = AdminExtraTicketPetitionViewModel.Create(Repository, _ceremonyService, _petitionService, CurrentUser, TermService.GetCurrent(), ceremonyId);
            return View(viewModel);
        }

        /// <summary>
        /// #3
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        [HttpPost]
        [AnyoneWithRole]
        public JsonResult DecideExtraTicketPetition(int id /* Registration Participation Id */, bool isApproved)
        {
            var participation = Repository.OfType<RegistrationParticipation>().GetNullableById(id);

            if (participation == null) return Json("Could not find registration.");
            if (participation.ExtraTicketPetition == null) return Json("Could not find extra ticket petition.");
            if (!_ceremonyService.HasAccess(participation.Ceremony.Id, CurrentUser.Identity.Name)) return Json("You do not have access to that ceremony.");

            var petition = participation.ExtraTicketPetition;
            petition.MakeDecision(isApproved);

            Repository.OfType<ExtraTicketPetition>().EnsurePersistent(petition);

            if (petition.IsApproved)
            {
                try
                {
                    _emailService.QueueExtraTicketPetitionDecision(participation);
                }
                catch (Exception ex)
                {
                    _errorService.ReportError(ex);
                }
            }

            return Json(string.Empty);
        }

        /// <summary>
        /// #4
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tickets"></param>
        /// <param name="streaming"></param>
        /// <returns></returns>
        [AnyoneWithRole]
        [HttpPost]
        public JsonResult UpdateTicketAmount(int id /* Registration Participation Id */, int tickets, bool streaming)
        {
            var participation = Repository.OfType<RegistrationParticipation>().GetNullableById(id);

            if (participation == null) return Json(new UpdateTicketModel(null, "Could not locate registration."));

            var petition = participation.ExtraTicketPetition;
            var ceremony = participation.Ceremony;

            if (petition == null) return Json(new UpdateTicketModel(ceremony, "Could not find petition."));
            if (ceremony == null) return Json(new UpdateTicketModel(ceremony,"Could not find ceremony."));
            if (!_ceremonyService.HasAccess(ceremony.Id, CurrentUser.Identity.Name)) return Json(new UpdateTicketModel(ceremony, "You do not have access to ceremony."));
            if (!petition.IsPending) return Json(new UpdateTicketModel(ceremony, "Petition is not pending"));

            if (streaming) petition.NumberTicketsStreaming = tickets > 0 ? tickets : 0;
            else petition.NumberTickets = tickets > 0 ? tickets : 0;

            Repository.OfType<ExtraTicketPetition>().EnsurePersistent(petition);

            return Json(new UpdateTicketModel(ceremony, string.Empty));
        }

        /// <summary>
        /// #5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AnyoneWithRole]
        [HttpPost]
        public RedirectToRouteResult ApproveAllExtraTicketPetition(int id /* Ceremony Id */)
        {
            var ceremonies = _ceremonyService.GetCeremonies(CurrentUser.Identity.Name);
            var ceremony = ceremonies.SingleOrDefault(a => a.Id == id);

            // ceremony not found or user does not have access, redirect to the page so they can select a valid ceremony
            if (ceremony == null) return this.RedirectToAction(a => a.ExtraTicketPetitions(null));
            if (!_ceremonyService.HasAccess(ceremony.Id, CurrentUser.Identity.Name))
            {
                Message = "You do not have rights to that ceremony";
                return this.RedirectToAction(a => a.ExtraTicketPetitions(null));
            }
            // load up all pending extra ticket petitions
            var participations = _petitionService.GetPendingExtraTicket(CurrentUser.Identity.Name, ceremony.Id);

            var totalCount = 0;
            var ticketCount = 0;
            var streamingCount = 0;

            // approve all
            foreach (var a in participations)
            {
                a.ExtraTicketPetition.MakeDecision(true);           // make the decision information
                
                try
                {
                    _emailService.QueueExtraTicketPetitionDecision(a);  // queue the email for the decision
                }
                catch (Exception ex)
                {
                    
                    _errorService.ReportError(ex);
                }
                Repository.OfType<ExtraTicketPetition>().EnsurePersistent(a.ExtraTicketPetition);

                totalCount++;
                ticketCount += a.ExtraTicketPetition.NumberTickets.HasValue ? a.ExtraTicketPetition.NumberTickets.Value : 0;
                streamingCount += a.ExtraTicketPetition.NumberTicketsStreaming.HasValue ? a.ExtraTicketPetition.NumberTicketsStreaming.Value: 0;
            }

            Message = string.Format("You have successfully approved {0} with a total of {1} regular tickets", totalCount, ticketCount);
            if (ceremony.HasStreamingTickets) Message += string.Format(" and a total of {0} streaming tickets.", streamingCount);
            else Message += ".";

            return this.RedirectToAction(a => a.ExtraTicketPetitions(id));
        }
        #endregion

        #region Registration Petitions
        /// <summary>
        /// #6
        /// </summary>
        /// <returns></returns>
        [AnyoneWithRole]
        public ActionResult RegistrationPetitions()
        {
            var viewModel = AdminPetitionsViewModel.Create(Repository, _ceremonyService, _petitionService, CurrentUser.Identity.Name, TermService.GetCurrent());
            return View(viewModel);
        }

        /// <summary>
        /// #7
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AnyoneWithRole]
        public ActionResult RegistrationPetition(int id)
        {
            var registrationPetition = Repository.OfType<RegistrationPetition>().GetNullableById(id);
            if (registrationPetition == null)
            {
                Message = "Unable to find registration petition.";
                return this.RedirectToAction(a => a.Index());
            }

            if (!_ceremonyService.HasAccess(registrationPetition.Ceremony.Id, CurrentUser.Identity.Name))
            {
                Message = "You do not have rights to that ceremony";
                return this.RedirectToAction(a => a.ExtraTicketPetitions(null));
            }

            return View(registrationPetition);
        }

        /// <summary>
        /// #8
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        [HttpPost]
        [AnyoneWithRole]
        public ActionResult DecideRegistrationPetition(int id, bool isApproved)
        {
            var registrationPetition = Repository.OfType<RegistrationPetition>().GetNullableById(id);
            if (registrationPetition == null)
            {
                Message = "Petition not found.";
                return this.RedirectToAction<ErrorController>(a => a.Index());
            }
            if (!_ceremonyService.HasAccess(registrationPetition.Ceremony.Id, CurrentUser.Identity.Name))
            {
                Message = "You do not have rights to that ceremony";
                return this.RedirectToAction(a => a.ExtraTicketPetitions(null));
            }
            var registration = registrationPetition.Registration;

            // set the decision
            registrationPetition.SetDecision(isApproved);

            var isValid = true;

            // validate to make sure that they don't already have registrations
            if (Repository.OfType<RegistrationParticipation>().Queryable.Where(a => (a.Ceremony == registrationPetition.Ceremony 
                                                                                 || a.Major == registrationPetition.MajorCode)
                                                                                 && a.Registration == registrationPetition.Registration
                                                                                 && isApproved).Any())
            {
                isValid = false;
                Message = "Student has already been registered for this major/ceremony.";
            }

            if (isValid)
            {
                // automatically register student
                if (isApproved)
                {
                    registration.AddParticipation(registrationPetition.MajorCode, registrationPetition.Ceremony,
                                                  registrationPetition.NumberTickets);

                    try
                    {
                        _emailService.QueueRegistrationPetitionDecision(registrationPetition);
                    }
                    catch (Exception ex)
                    {
                        _errorService.ReportError(ex);
                        Message += StaticValues.Student_Email_Problem;
                    }
                }

                Repository.OfType<Registration>().EnsurePersistent(registration);
            }

            return this.RedirectToAction(a => a.RegistrationPetition(id));
        }
        #endregion

        #region Student Forms
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

            var ceremonies = registration.RegistrationParticipations.Select(a => a.Ceremony).ToList();
            var minBeginDate = ceremonies.Min(a => a.ExtraTicketBegin);
            var maxEndDate = ceremonies.Max(a => a.ExtraTicketDeadline);

            // extra ticket deadline has passed or no more tickets
            if (DateTime.Now > maxEndDate)
            {
                Message = "Deadline for all extra ticket requests has passed or there are no more tickets available.";
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration());
            }

            if (DateTime.Now < minBeginDate)
            {
                Message = string.Format("You cannot petition for extra tickets until at least {0}", minBeginDate.ToString("d"));
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration());
            }

            if (!registration.RegistrationParticipations.Any(a => a.ExtraTicketPetition == null))
            {
                Message = string.Format("You have already submitted your extra ticket request(s).");
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration());
            }

            var viewModel = ExtraTicketPetitionModel.Create(Repository, registration);

            return View(viewModel);
         }

        [HttpPost]
        [StudentsOnly]
        public ActionResult ExtraTicketPetition(int id, List<ExtraTicketPetitionPostModel> extraTicketPetitions)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null                                        // requires registration
                || registration.Student.Login != CurrentUser.Identity.Name  // validate the user
                )
            {
                return this.RedirectToAction<StudentController>(a => a.Index());
            }

            var ceremonyParticipations = new List<RegistrationParticipation>();

            foreach (var a in extraTicketPetitions.Where(b=>b.NumberTickets > 0 || b.NumberStreamingTickets > 0))
            {
                var tickets = a.Ceremony.HasStreamingTickets ? a.NumberTickets + a.NumberStreamingTickets : a.NumberTickets;

                if (tickets > a.Ceremony.ExtraTicketPerStudent)
                {
                    ModelState.AddModelError("# Tickets", string.Format("Petition for {0} has too many tickets selected.  The max is {1} for this ceremony.", a.Ceremony.Name, a.Ceremony.ExtraTicketPerStudent));
                }
                else if (DateTime.Now > a.Ceremony.ExtraTicketDeadline)
                {
                    ModelState.AddModelError("Deadline", string.Format("Petition for {0} has pasted the deadline.", a.Ceremony.Name));
                }
                else if (a.RegistrationParticipation.ExtraTicketPetition != null)
                {
                    ModelState.AddModelError("ExtraTicketPetition", string.Format("Our records indicate that you have already submitted a petition for {0}", a.Ceremony.Name));
                }
                else if (string.IsNullOrEmpty(a.Reason))
                {
                    ModelState.AddModelError("Reason", string.Format("Reason cannot be blank for petition {0}", a.Ceremony.Name));
                }
                else if (a.Reason.Length > 100)
                {
                    ModelState.AddModelError("Reason", string.Format("Reason must be 100 characters or less for petition {0}", a.Ceremony.Name));
                }
                // validate the deadline before creating valid request, and no previous
                else
                {
                    var etp = new ExtraTicketPetition(a.NumberTickets, a.Reason, a.Ceremony.HasStreamingTickets ? a.NumberStreamingTickets : 0);
                    a.RegistrationParticipation.ExtraTicketPetition = etp;
                    ceremonyParticipations.Add(a.RegistrationParticipation);
                }

            }

            if (ModelState.IsValid)
            {
                foreach (var registrationParticipation in ceremonyParticipations)
                {
                    Repository.OfType<RegistrationParticipation>().EnsurePersistent(registrationParticipation);

                    try
                    {
                        _emailService.QueueExtraTicketPetition(registrationParticipation);
                    }
                    catch (Exception ex)
                    {
                        _errorService.ReportError(ex);
                        Message += StaticValues.Student_Email_Problem;
                    }
                }

                Message = "Successfully submitted extra ticket petition(s).";
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration());
            }

            var viewModel = ExtraTicketPetitionModel.Create(Repository, registration);
            viewModel.ExtraTicketPetitionPostModels = extraTicketPetitions;
            return View(viewModel);
        }
        #endregion
    }

    public class UpdateTicketModel
    {
        public UpdateTicketModel(Ceremony ceremony, string message)
        {
            if (ceremony != null)
            {
                ProjectedAvailableTickets = ceremony.ProjectedAvailableTickets;
                ProjectedAvailableStreamingTickets = ceremony.ProjectedAvailableStreamingTickets;
                ProjectedTicketCount = ceremony.ProjectedTicketCount;
                ProjectedStreamingCount = ceremony.ProjectedTicketStreamingCount;
            }
            Message = message;
        }

        public int ProjectedAvailableTickets { get; set; }
        public int? ProjectedAvailableStreamingTickets { get; set; }
        public int ProjectedTicketCount { get; set; }
        public int? ProjectedStreamingCount { get; set; }

        public string Message { get; set; }
    }
}

