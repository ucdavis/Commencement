using System;
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
            var viewModel = AdminPetitionsViewModel.Create(Repository, _ceremonyService, _petitionService, CurrentUser.Identity.Name, TermService.GetCurrent());

            return View(viewModel);
        }

        [AnyoneWithRole]
        public ActionResult DecideExtraTicketPetition(int id, bool isApproved)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null) return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.UnknownError));

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
            //}

            return this.RedirectToAction(a => a.Index());
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
        public JsonResult UpdateTicketAmount(int id, int tickets)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null) return Json(false);
            //if (!registration.ExtraTicketPetition.IsPending) return Json(false);    // do not change non-pending petitions

            //registration.ExtraTicketPetition.NumberTickets = tickets > 0 ? tickets : 0;

            //Repository.OfType<ExtraTicketPetition>().EnsurePersistent(registration.ExtraTicketPetition);

            return Json(true);
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

            // extra ticket deadline has passed or no more tickets
            if (registration.RegistrationParticipations[0].Ceremony.ExtraTicketDeadline <= DateTime.Now || registration.RegistrationParticipations[0].Ceremony.AvailableTickets < registration.RegistrationParticipations[0].Ceremony.TicketsPerStudent)
            {
                Message = "Deadline for extra ticket requests has passed or there are no more tickets available.";
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
            }

            // already submitted extra ticket petition
            //if (registration.ExtraTicketPetition != null)
            //{
            //    Message = "You have already submitted an extra ticket petition.";
            //    return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
            //}

            var viewModel = ExtraTicketPetitionModel.Create(Repository, registration);

            return View(viewModel);
         }

        [HttpPost]
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

            // extra ticket deadline has passed or no more tickets
            if (registration.RegistrationParticipations[0].Ceremony.ExtraTicketDeadline <= DateTime.Now || registration.RegistrationParticipations[0].Ceremony.AvailableTickets < registration.RegistrationParticipations[0].Ceremony.TicketsPerStudent)
            {
                Message = "Deadline for extra ticket requests has passed or there are no more tickets available.";
                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
            }

            // already submitted extra ticket petition
            //if (registration.ExtraTicketPetition != null)
            //{
            //    Message = "You have already submitted an extra ticket petition.";
            //    return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
            //}

            var ticketPetition = new ExtraTicketPetition(numberTickets);

            //registration.ExtraTicketPetition = ticketPetition;

            // validate the object
            ticketPetition.TransferValidationMessagesTo(ModelState);
            registration.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Registration>().EnsurePersistent(registration);

                Message = "Ticket petition has been successfully submitted.";

                try
                {
                    _emailService.SendExtraTicketPetitionConfirmation(registration);
                }
                catch (Exception ex)
                {
                    _errorService.ReportError(ex);
                    Message += StaticValues.Student_Email_Problem;
                }

                return this.RedirectToAction<StudentController>(a => a.DisplayRegistration(id));
            }

            var viewModel = ExtraTicketPetitionModel.Create(Repository, registration);
            viewModel.ExtraTicketPetition = ticketPetition;
            return View(viewModel);
        }
    }
}
