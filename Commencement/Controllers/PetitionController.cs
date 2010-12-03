using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using MvcContrib;
using MvcContrib.Attributes;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.Helpers;


namespace Commencement.Controllers
{
    
    public class PetitionController : ApplicationController
    {
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IPetitionService _petitionService;
        private readonly IErrorService _errorService;


        public PetitionController(IStudentService studentService, IEmailService emailService, IRepositoryWithTypedId<MajorCode, string> majorService, ICeremonyService ceremonyService, IPetitionService petitionService, IErrorService errorService)
        {
            _studentService = studentService;
            _emailService = emailService;
            _majorService = majorService;
            _ceremonyService = ceremonyService;
            _petitionService = petitionService;
            _errorService = errorService;
        }

        //
        // GET: /Petition/

        [AnyoneWithRole]
        public ActionResult Index()
        {
            return View();
        }

        [AnyoneWithRole]
        public ActionResult ExtraTicketPetitions(int? ceremonyId)
        {
            var viewModel = AdminExtraTicketPetitionViewModel.Create(Repository, _ceremonyService, _petitionService, CurrentUser, TermService.GetCurrent(), ceremonyId);
            return View(viewModel);
        }

        [AnyoneWithRole]
        public ActionResult RegistrationPetitions()
        {
            return View();
        }
        [AnyoneWithRole]
        public ActionResult RegistrationPetition(int id)
        {
            var registrationPetition = Repository.OfType<RegistrationPetition>().GetNullableById(id);
            if (registrationPetition == null)
            {
                Message = "Unable to find registration petition.";
                return this.RedirectToAction(a => a.Index());
            }

            return View(registrationPetition);
        }

        [HttpPost]
        [AnyoneWithRole]
        public ActionResult DecideExtraTicketPetition(int id /* Registration Participation Id */, bool isApproved)
        {
            var participation = Repository.OfType<RegistrationParticipation>().GetNullableById(id);

            if (participation == null) return Json("Could not find registration.");
            if (participation.ExtraTicketPetition == null) return Json("Could not find extra ticket petition.");

            var petition = participation.ExtraTicketPetition;

            petition.IsPending = false;
            petition.IsApproved = isApproved;
            petition.DateDecision = DateTime.Now;

            Repository.OfType<ExtraTicketPetition>().EnsurePersistent(petition);
            //_emailService.QueueExtraTicketPetitionDecision(participation);

            return Json(string.Empty);

            //registration.ExtraTicketPetition.IsPending = false;
            //registration.ExtraTicketPetition.IsApproved = isApproved;
            //registration.ExtraTicketPetition.DateDecision = DateTime.Now;

            //registration.ExtraTicketPetition.TransferValidationMessagesTo(ModelState);

            //if (ModelState.IsValid)
            //{
            //    Repository.OfType<ExtraTicketPetition>().EnsurePersistent(registration.ExtraTicketPetition);

            //    Message = string.Format("Decision for {0} has been saved.", registration.Student.FullName);

            //    try
            //    {
            //        _emailService.SendExtraTicketPetitionDecision(registration);
            //    }
            //    catch (Exception ex)
            //    {
            //        _errorService.ReportError(ex);
            //        Message += StaticValues.Student_Email_Problem;
            //    }
            //}
            //else
            //{
            //    Message = string.Format("There was a problem saving decision for {0}", registration.Student.FullName);
            //}););););

         //   return this.RedirectToAction(a => a.Index());
        }

        [AnyoneWithRole]
        public ActionResult DecideRegistrationPetition(int id, bool isApproved)
        {
            var registrationPetition = Repository.OfType<RegistrationPetition>().GetNullableById(id);
            if (registrationPetition == null) return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.UnknownError));

            registrationPetition.SetDecision(isApproved);

            var student = new Student(registrationPetition.Pidm, registrationPetition.StudentId,
                                          registrationPetition.FirstName, registrationPetition.MI,
                                          registrationPetition.LastName, registrationPetition.Units,
                                          registrationPetition.Email, registrationPetition.Login,
                                          registrationPetition.TermCode);

            Check.Require(registrationPetition.MajorCode != null, "Major is required.");

            student.Majors.Add(registrationPetition.MajorCode);

            // check if the major has a ceremony or not, if not fill the override
            if (!registrationPetition.Ceremony.Majors.Contains(registrationPetition.MajorCode))
                student.Ceremony = registrationPetition.Ceremony;

            registrationPetition.TransferValidationMessagesTo(ModelState);
            student.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                // save the decision
                Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);

                // if approved and does not already exist for this term the save the student,
                // otherwise the student has already been added either by auto download or admin addition
                // do not save a duplicate
                if (isApproved && !_studentService.CheckExisting(student.Login, TermService.GetCurrent()))
                {
                    // persist the student
                    Repository.OfType<Student>().EnsurePersistent(student);
                }

                if (isApproved)
                {
                    try
                    {
                        _emailService.SendRegistrationPetitionApproved(registrationPetition);
                    }
                    catch (Exception ex)
                    {
                        _errorService.ReportError(ex);
                        Message += StaticValues.Student_Email_Problem;
                    }
                }

                Message += string.Format("Decision was saved for {0}", registrationPetition.FullName);
            }
            else
            {
                Message = string.Format("There was a problem saving decision for {0}", registrationPetition.FullName);
            }

            return this.RedirectToAction(a => a.Index());
        }

        [AnyoneWithRole]
        [HttpPost]
        public JsonResult UpdateTicketAmount(int id /* Registration Participation Id */, int tickets, bool streaming)
        {
            var participation = Repository.OfType<RegistrationParticipation>().GetNullableById(id);

            if (participation == null) return Json(new UpdateTicketModel(null, "Count not locate registration."));

            var petition = participation.ExtraTicketPetition;
            var ceremony = participation.Ceremony;
            
            if (petition == null) return Json(new UpdateTicketModel(ceremony, "Could not find petition."));
            if (ceremony == null) return Json("Could not find ceremony.");
            if (!_ceremonyService.HasAccess(ceremony.Id, CurrentUser.Identity.Name)) return Json(new UpdateTicketModel(ceremony, "You do not have access to ceremony."));
            if (!petition.IsPending) return Json(new UpdateTicketModel(ceremony, "Petition is not pending"));

            if (streaming) petition.NumberTicketsStreaming = tickets > 0 ? tickets : 0;
            else petition.NumberTickets = tickets > 0 ? tickets : 0;

            Repository.OfType<ExtraTicketPetition>().EnsurePersistent(petition);

            return Json(new UpdateTicketModel(ceremony, string.Empty));
        }

        #region Student Forms       
        [PageTrackingFilter]
        [Authorize]
        public ActionResult Register()
        {
            // do a check to see if a student has already submitted a petition
            if (Repository.OfType<RegistrationPetition>().Queryable.Where(a=>a.TermCode == TermService.GetCurrent() && a.Login == CurrentUser.Identity.Name).Any())
            {
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.SubmittedPetition));
            }

            //Get student info and create registration model
            var viewModel = RegistrationPetitionModel.Create(Repository, _majorService, _studentService, CurrentUser);

            if (viewModel.SearchStudent == null)
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.StudentNotFound));

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Register(RegistrationPetition registrationPetition)
        {
            // validate the object
            registrationPetition.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
                Message = "Your registration petition has been submitted.";

                try
                {
                    _emailService.SendRegistrationPetitionConfirmation(registrationPetition);
                }
                catch (Exception ex)
                {
                    _errorService.ReportError(ex);
                    Message += StaticValues.Student_Email_Problem;
                }

                return this.RedirectToAction(a => a.RegisterConfirmation());
            }

            var viewModel = RegistrationPetitionModel.Create(Repository, _majorService, _studentService, CurrentUser);
            viewModel.RegistrationPetition = registrationPetition;
            return View(viewModel);
        }

        [PageTrackingFilter]
        [Authorize]
        public ActionResult RegisterConfirmation()
        {
            return View();
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

            var ceremonies = registration.RegistrationParticipations.Select(a => a.Ceremony).ToList();
            var minBeginDate = ceremonies.Min(a => a.ExtraTicketBegin);
            var maxEndDate = ceremonies.Max(a => a.ExtraTicketDeadline);

            // extra ticket deadline has passed or no more tickets)))
            if (DateTime.Now > maxEndDate)
            {
                Message = "Deadline for all extra ticket requests has passed or there are no more tickets available.";
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
            }

            if (DateTime.Now < minBeginDate)
            {
                Message = string.Format("You cannot petition for extra tickets until at least {0}", minBeginDate.ToString("d"));
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
            }

            if (!registration.RegistrationParticipations.Any(a => a.ExtraTicketPetition == null))
            {
                Message = string.Format("You have already submitted your extra ticket request(s).");
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
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
                // validate the deadline before creating valid request, and no previous
                else 
                {
                    var etp = new ExtraTicketPetition(a.NumberTickets, a.Ceremony.HasStreamingTickets ? a.NumberStreamingTickets : 0);
                    a.RegistrationParticipation.ExtraTicketPetition = etp;
                    ceremonyParticipations.Add(a.RegistrationParticipation);
                }
            }

            if (ModelState.IsValid)
            {
                foreach (var registrationParticipation in ceremonyParticipations)
                {
                    Repository.OfType<RegistrationParticipation>().EnsurePersistent(registrationParticipation);    
                }

                try
                {
                    _emailService.SendExtraTicketPetitionConfirmation(registration);
                }
                catch (Exception ex)
                {
                    _errorService.ReportError(ex);
                    Message += StaticValues.Student_Email_Problem;
                }

                Message = "Successfully submitted extra ticket petition(s).";
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
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
            ProjectedAvailableTickets = ceremony.ProjectedAvailableTickets;
            ProjectedAvailableStreamingTickets = ceremony.ProjectedAvailableStreamingTickets;
            ProjectedTicketCount = ceremony.ProjectedTicketCount;
            ProjectedStreamingCount = ceremony.ProjectedTicketStreamingCount;
        }

        public int ProjectedAvailableTickets { get; set; }
        public int? ProjectedAvailableStreamingTickets { get; set; }
        public int ProjectedTicketCount { get; set; }
        public int? ProjectedStreamingCount { get; set; }

        public string Message { get; set; }
    }
}
