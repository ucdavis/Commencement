using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Commencement.Resources;
using NHibernate.Mapping;
using NPOI.SS.Formula.Functions;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class AdminController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorRepository;
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;
        private readonly IMajorService _majorService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IRegistrationService _registrationService;
        private readonly IRegistrationPopulator _registrationPopulator;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IErrorService _errorService;
        private readonly IReportService _reportService;

        public AdminController(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepositoryWithTypedId<MajorCode, string> majorRepository, IStudentService studentService, IEmailService emailService, IMajorService majorService, ICeremonyService ceremonyService, IRegistrationService registrationService, IRegistrationPopulator registrationPopulator, IRepository<Registration> registrationRepository, IErrorService errorService, IReportService reportService)
        {
            if (emailService == null) throw new ArgumentNullException("emailService");
            _studentRepository = studentRepository;
            _majorRepository = majorRepository;
            _studentService = studentService;
            _emailService = emailService;
            _majorService = majorService;
            _ceremonyService = ceremonyService;
            _registrationService = registrationService;
            _registrationPopulator = registrationPopulator;
            _registrationRepository = registrationRepository;
            _errorService = errorService;
            _reportService = reportService;
        }

        /// <summary>
        /// GET: /Admin/
        /// #1
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //var ceremonies = Repository.OfType<Ceremony>().GetAll(); //Was not being used.

            ViewBag.PendingLetters = Repository.OfType<VisaLetter>().Queryable.Any(a => a.IsPending);

            return View();
        }

        [AdminOnly]
        public ActionResult AdminLanding()
        {
            return View();
        }

        #region List Pages
        /// <summary>
        /// Students
        /// </summary>
        /// <param name="studentid"></param>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="majorCode"></param>
        /// <returns></returns>
        public ActionResult Students(string studentid, string lastName, string firstName, string majorCode, string college)
        {
            // get the newest active term
            var term = TermService.GetCurrent();

            var viewModel = AdminStudentViewModel.Create(Repository, _majorService, _ceremonyService, term, studentid, lastName, firstName, majorCode, college, CurrentUser.Identity.Name);

            return View(viewModel);
        }
        public ActionResult Registrations(string studentid, string lastName, string firstName, string majorCode, int? ceremonyId, string collegeCode)
        {
            var term = TermService.GetCurrent();
            var viewModel = AdminRegistrationViewModel.Create(Repository, _majorService, _ceremonyService, _registrationService, term, User.Identity.Name, studentid, lastName, firstName, majorCode, ceremonyId, collegeCode);
            return View(viewModel);
        }
        #endregion

        #region Student Details
        public RedirectToRouteResult SearchStudent(string studentId /* Student Id */)
        {
            var student = _studentRepository.Queryable.Where(a => a.StudentId == studentId && a.TermCode == TermService.GetCurrent()).FirstOrDefault();
            if (student == null)
            {
                Message = string.Format("Unable to find student with id {0}", studentId);
                return this.RedirectToAction(a => a.Index());
            }

            return this.RedirectToAction(a => a.StudentDetails(student.Id, false));
        }
        /// <summary>
        /// Students the details.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="registration">This value is used in the pages, not the controller. Don't remove it.</param>
        /// <returns></returns>
        public ActionResult StudentDetails(Guid id, bool? registration)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction<AdminController>(a => a.Index());
            }

            var viewModel = RegistrationModel.Create(Repository, _ceremonyService.GetCeremonies(CurrentUser.Identity.Name), student);
            viewModel.Registration = Repository.OfType<Registration>().Queryable.Where(a => a.Student == student).FirstOrDefault();
            var temp = Repository.OfType<VisaLetter>().Queryable.FirstOrDefault(a => a.Student == student);
            if (temp != null)
            {
                viewModel.FirstVisaLetterRequest = temp.Id;
            }
            ViewData["IsAdmin"] = true;

            return View(viewModel);
        }
        #endregion

        #region Edit Student Functions
        public ActionResult Block(Guid id)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction<AdminController>(a => a.Index());
            }

            return View(student);
        }
        [HttpPost]
        public ActionResult Block(Guid id, bool block, string reason)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction<AdminController>(a => a.Index());
            }

            if (block)
            {
                if (reason == "sja") student.SjaBlock = true;
                else student.Blocked = true;

                Message = "Student has been blocked from the registration system.";
            }
            else
            {
                student.SjaBlock = false;
                student.Blocked = false;

                Message = "Student has been unblocked and is allowed into the system.";
            }

            _studentRepository.EnsurePersistent(student);
            return this.RedirectToAction(a => a.StudentDetails(id, false));
        }

        public ActionResult RegisterForStudent(Guid id)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction(a => a.Students(null, null, null, null, null));
            }

            // check if the student has a registration already
            var registration = Repository.OfType<Registration>().Queryable.SingleOrDefault(a => a.Student == student && a.TermCode == TermService.GetCurrent());

            // load the current user's ceremonies, to determine what they can register the student for
            var ceremonies = _ceremonyService.GetCeremonies(CurrentUser.Identity.Name);

            //var viewModel = RegistrationModel.Create(Repository, ceremonies, student, registration);
            var overrideCeremonyId = student.Ceremony != null ? (int?) student.Ceremony.Id : null;
            var registrationModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(majors: student.Majors.ToList(), totalUnits: student.TotalUnits, ceremonyIdOverride: overrideCeremonyId), student: student, registration: registration, edit: true, admin: true);
            var viewModel = AdminRegisterForStudentViewModel.Create(Repository, registrationModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RegisterForStudent(Guid id, RegistrationPostModel registrationPostModel, bool supressEmail)
        {
            // load the student
            var student = _studentRepository.GetNullableById(id);
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction(a => a.Students(null, null, null, null, null));
            }

            // check for an existing registration
            var registration = Repository.OfType<Registration>().Queryable.SingleOrDefault(a => a.Student == student && a.TermCode == TermService.GetCurrent());

            if (registration == null)
            {
                registration = _registrationPopulator.PopulateRegistration(registrationPostModel, student, ModelState, true);
            }
            else
            {
                _registrationPopulator.UpdateRegistration(registration, registrationPostModel, student, ModelState, true);
            }

            registration.TransferValidationMessagesTo(ModelState);

            if (string.IsNullOrWhiteSpace(registration.CellCarrier) && !string.IsNullOrWhiteSpace(registration.CellNumberForText)) //TODO: Need to do on student side too
            {
                ModelState.AddModelError("Registration.CellCarrier", "You must select a Cell Phone Carrier if you enter a cell number.");
            }
            if (!string.IsNullOrWhiteSpace(registration.CellCarrier) && string.IsNullOrWhiteSpace(registration.CellNumberForText))
            {
                ModelState.AddModelError("Registration.CellNumberForText", "You must enter a cell number if you select a Cell Phone Carrier.");
            }
            if (!string.IsNullOrWhiteSpace(registration.CellNumberForText))
            {

                if (registration.CellNumberForText.Length != 10 || !Regex.Match(registration.CellNumberForText, @"^\d{10}$").Success)
                {
                    ModelState.AddModelError("Registration.CellNumberForText", "Cell Number must be empty or a 10 digit number, no spaces.");
                }
            }

            if (ModelState.IsValid)
            {
                _registrationRepository.EnsurePersistent(registration);

                if (!supressEmail)
                {
                    try
                    {

                        // add email for registration into queue
                        _emailService.QueueRegistrationConfirmation(registration);
                    }
                    catch (Exception ex)
                    {
                        _errorService.ReportError(ex);
                        Message += StaticValues.Student_Email_Problem;
                    }    
                }

                // put message up for student
                Message += StaticValues.Student_Register_Successful;

                return this.RedirectToAction(a => a.StudentDetails(id, false));
            }

            // load the current user's ceremonies, to determine what they can register the student for
            var registrationModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student: student, registration: registration, edit: true, admin: true);
            var viewModel = AdminRegisterForStudentViewModel.Create(Repository, registrationModel);
            return View(viewModel);
        }

        public ActionResult EditStudent(Guid id)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction<AdminController>(a => a.Index());
            }

            var viewModel = AdminEditStudentViewModel.Create(Repository, _ceremonyService, student, CurrentUser.Identity.Name);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditStudent(Guid id, Student student)
        {
            var existingStudent = _studentRepository.GetNullableById(id);
            if (existingStudent == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction<AdminController>(a => a.Index());
            }

            CopyHelper.CopyStudentValues(student, existingStudent);

            student.TransferValidationMessagesTo(ModelState);

            // save the student object
            if (ModelState.IsValid)
            {
                Repository.OfType<Student>().EnsurePersistent(existingStudent);
                Message = "Student has been updated.";
                return this.RedirectToAction<AdminController>(a => a.StudentDetails(id, false));
            }

            var viewModel = AdminEditStudentViewModel.Create(Repository, _ceremonyService, student, CurrentUser.Identity.Name);
            return View(viewModel);
        }
        #endregion

        #region Add Student Functions
        public ActionResult AddStudent(string studentId)
        {
            var student = _studentService.BannerLookup(studentId);
            if (!string.IsNullOrWhiteSpace(studentId) && student == null)
            {
                student = _studentService.BannerLookupName(studentId);
            }
            var viewModel = AdminEditStudentViewModel.Create(Repository, _ceremonyService, student, CurrentUser.Identity.Name);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddStudent(string studentId, Student student)
        {
            if (_studentService.CheckExisting(student.Login, TermService.GetCurrent()))
            {
                ModelState.AddModelError("Exists", "Student already exists in the system.");
                Message = string.Format("{0} already exists, you can edit the student record or registration by going through the student details page.", student.FullName);
            }

            student.TermCode = TermService.GetCurrent();
            student.AddedBy = CurrentUser.Identity.Name;
            student.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _studentRepository.EnsurePersistent(student);
                Message = string.Format("{0} has been added to the registration system.", student.FullName);
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = AdminEditStudentViewModel.Create(Repository, _ceremonyService, student, CurrentUser.Identity.Name);
            return View(viewModel);
        }
        #endregion

        #region Move Major
        public ActionResult MoveMajor()
        {
            var viewModel = MoveMajorViewModel.Create(Repository, CurrentUser, _ceremonyService);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult MoveMajor(string majorCode, int ceremonyId)
        {
            var major = _majorRepository.GetNullableById(majorCode);
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyId);
            
            var origCeremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.Majors.Contains(major) && a.TermCode == TermService.GetCurrent()).FirstOrDefault();

            var message = string.Empty;
            if (!ValidateMajorMove(major, ceremony, origCeremony, out message))
            {
                // not valid
                ModelState.AddModelError("Validation", message);
            }
            
            // move is valid, let's go
            var participations = Repository.OfType<RegistrationParticipation>().Queryable
                                 .Where(a => a.Ceremony == origCeremony && a.Major == major).ToList();

            // move each student
            foreach (var a in participations)
            {
                a.Ceremony = ceremony;
                Repository.OfType<RegistrationParticipation>().EnsurePersistent(a);

                // queue an email confirmation, reflecting the updated major
                _emailService.QueueMajorMove(a.Registration, a);
            }

            // move the major in ceremony list
            origCeremony.Majors.Remove(major);
            ceremony.Majors.Add(major);

            Repository.OfType<Ceremony>().EnsurePersistent(origCeremony);
            Repository.OfType<Ceremony>().EnsurePersistent(ceremony);

            // redirect back to....
            Message = string.Format("{0} has been successfully moved to {1}.", major.Name, ceremony.DateTime.ToString("g"));
            return this.RedirectToAction(a => a.Index());
        }

        public JsonResult ValidateMoveMajor(string majorCode, int ceremonyId)
        {
            var major = _majorRepository.GetNullableById(majorCode);
            var origCeremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.Majors.Contains(major) && a.TermCode == TermService.GetCurrent()).FirstOrDefault();
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyId);

            var message = string.Empty;
            ValidateMajorMove(major, ceremony, origCeremony, out message);

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        private bool ValidateMajorMove(MajorCode majorCode, Ceremony destCeremony, Ceremony origCeremony, out string message)
        {
            if (majorCode == null || origCeremony == null || destCeremony == null)
            {
                message = "There was an error, validating this move.";
                return false;
            }

            if (destCeremony.Majors.Contains(majorCode))
            {
                message = "Major is already in the selected ceremony.";
                return false;
            }

            // figure out how many students/tickets are getting moved
            var participations = Repository.OfType<RegistrationParticipation>().Queryable
                                 .Where(a => a.Ceremony == origCeremony && a.Major == majorCode).ToList();

            message = string.Format("You have requested to move {0} to {1} ceremony.  This will move {2} students with {3} tickets leaving {4} available."
                                    , majorCode.Name, destCeremony.DateTime.ToString("g"), participations.Count(), participations.Sum(a => a.TotalTickets)
                                    , destCeremony.AvailableTickets - participations.Sum(a => a.TotalTickets));

            return true;
        }
        #endregion

        public ActionResult Majors()
        {
            var viewModel = AdminMajorsViewModel.Create(Repository, _ceremonyService,_registrationService, CurrentUser);
            return View(viewModel);
        }

        public ActionResult VisaLetters(DateTime? startDate, DateTime? endDate, string collegeCode ,bool showAll = false)
        {
            var visaLetters = Repository.OfType<VisaLetter>().Queryable;
            if (!showAll)
            {
                visaLetters = visaLetters.Where(a => a.IsPending && !a.IsCanceled);
            }
            if (startDate.HasValue)
            {
                visaLetters = visaLetters.Where(a => a.DateCreated >= startDate.Value.Date);
            }
            if (endDate.HasValue)
            {
                visaLetters = visaLetters.Where(a => a.DateCreated <= endDate.Value.Date.AddDays(1));
            }
            if (!string.IsNullOrWhiteSpace(collegeCode))
            {
                visaLetters = visaLetters.Where(a => a.CollegeCode == collegeCode);
            }

            var model = AdminVisaLetterListViewModel.Create(visaLetters.ToList(), showAll, startDate, endDate, collegeCode);

            return View(model);
        }

        public ActionResult VisaLetterDetails(int id)
        {
            var letter = Repository.OfType<VisaLetter>().Queryable.Single(a => a.Id == id);
            var relatedLetters = Repository.OfType<VisaLetter>().Queryable.Where(a => a.Student.Id == letter.Student.Id && a.Id != id).ToList();

            var model = AdminVisaDetailsModel.Create(letter, relatedLetters);

            return View(model);
        }

        public ActionResult VisaLetterDecide(int id)
        {
            var letter = Repository.OfType<VisaLetter>().Queryable.Single(a => a.Id == id);
            if (!letter.CeremonyDateTime.HasValue)
            {
                var ceremony = GetCeremonyForVisaLetter(letter);
                if (ceremony != null)
                {
                    letter.CeremonyDateTime = ceremony.DateTime;
                }
            }

            return View(letter);
        }

        private Ceremony GetCeremonyForVisaLetter(VisaLetter letter, bool useDefaultCeremony = false)
        {
            var termCode = TermService.GetCurrent();
            var currentReg = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == letter.Student && a.TermCode.Id == termCode.Id);

            // has this student registered yet?
            if (currentReg != null)
            {
                // display previous registration
                var participation = currentReg.RegistrationParticipations.FirstOrDefault(a => !a.Cancelled && !a.Registration.Student.SjaBlock && !a.Registration.Student.Blocked);
                if (participation != null && participation.Ceremony.Colleges.Any(a => a.Id == letter.CollegeCode))
                {
                    return participation.Ceremony;
                }
            }

            if (useDefaultCeremony) //So we can grab a template. But we don't want to use this for defaulting the date
            {
                return Repository.OfType<Ceremony>().Queryable.FirstOrDefault(a => a.Id == 1); 
            }
            return null;
        }

        [HttpPost]
        public ActionResult VisaLetterDecide(int id, AdminVisaLetterPostModel model) 
        {
            var letter = Repository.OfType<VisaLetter>().Queryable.Single(a => a.Id == id);

            var saveStatus = letter.Status;

            var url = HttpContext.Server.MapPath(string.Format("~/Images/vl_{0}_signature.png", CurrentUser.Identity.Name.ToLower().Trim()));
            if (!System.IO.File.Exists(url))
            {
                Message = "You must set up a signature to be able to decide Visa Letter Requests";
                return View(letter);
            }
            
            // -- Student
            letter.StudentFirstName = model.StudentFirstName;
            letter.StudentLastName = model.StudentLastName;
            // -- Date Created
            letter.Gender = model.Gender;
            letter.Ceremony = model.Ceremony;
            letter.HardCopy = model.HardCopy;

            letter.RelativeTitle = model.RelativeTitle;
            letter.RelativeFirstName = model.RelativeFirstName;
            letter.RelativeLastName = model.RelativeLastName;
            letter.RelationshipToStudent = model.RelationshipToStudent;
            letter.RelativeMailingAddress = model.RelativeMailingAddress;

            letter.CollegeCode = model.CollegeCode;
            letter.CollegeName = SelectLists.CollegeNames.Single(a => a.Value == letter.CollegeCode).Text;
            letter.MajorName = model.MajorName;
            letter.CeremonyDateTime = model.CeremonyDateTime;
            
            //TODO: Text for "Bachelor of Science degree"
            letter.Degree = model.Degree;


            if (model.Decide != "N") //Just have a check box that says "decide"
            {
                letter.DateDecided = DateTime.Now;
            }
            else
            {
                letter.DateDecided = null;
            }
            letter.LastUpdateDateTime = DateTime.Now;
            letter.IsPending = model.Decide == "N";
            letter.IsApproved = model.Decide == "A";
            letter.IsDenied = model.Decide == "D";
            

            letter.ApprovedBy = CurrentUser.Identity.Name;

            letter.TransferValidationMessagesTo(ModelState);
            if (letter.IsApproved && letter.CeremonyDateTime == null)
            {
                ModelState.AddModelError("SpecialCheck", "Must Enter Ceremony Date when approving.");
            }

            if (ModelState.IsValid)
            {
                Repository.OfType<VisaLetter>().EnsurePersistent(letter);
                //TODO: Email notification
                if (saveStatus != letter.Status)
                {
                    try
                    {
                        _emailService.QueueVisaLetterDecision(letter, GetCeremonyForVisaLetter(letter, true), Request, Url);
                    }
                    catch (Exception ex)
                    {
                        _errorService.ReportError(ex);
                    }
                }
                Message = "Letter updated";
                return this.RedirectToAction("VisaLetterDetails", new {id});
            }

            Message = "Please correct errors and try again.";
            return View(letter);
        }

        public ActionResult VisaLetterPreviewPdf(int id)
        {

            var letter = Repository.OfType<VisaLetter>().Queryable.Single(a => a.Id == id);
            if (string.IsNullOrWhiteSpace(letter.ApprovedBy))
            {
                letter.ApprovedBy = CurrentUser.Identity.Name;
            }

            var url = HttpContext.Server.MapPath(string.Format("~/Images/vl_{0}_signature.png", letter.ApprovedBy.ToLower().Trim()));
            if (!System.IO.File.Exists(url))
            {
                Message = "You must set up a signature to be able to decide Visa Letter Requests";
                return File(_reportService.WritePdfWithErrorMessage(Message), "application/pdf");

            }

            return File(_reportService.GenerateLetter(letter), "application/pdf");//, string.Format("{0}.pdf", letter.ReferenceGuid));

        }
    }


}
