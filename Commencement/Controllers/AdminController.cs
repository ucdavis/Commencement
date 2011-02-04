using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
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

        public AdminController(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepositoryWithTypedId<MajorCode, string> majorRepository, IStudentService studentService, IEmailService emailService, IMajorService majorService, ICeremonyService ceremonyService, IRegistrationService registrationService, IRegistrationPopulator registrationPopulator, IRepository<Registration> registrationRepository, IErrorService errorService)
        {
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
        }

        /// <summary>
        /// GET: /Admin/
        /// #1
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //var ceremonies = Repository.OfType<Ceremony>().GetAll(); //Was not being used.

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
        public ActionResult Students(string studentid, string lastName, string firstName, string majorCode)
        {
            // get the newest active term
            var term = TermService.GetCurrent();

            var viewModel = AdminStudentViewModel.Create(Repository, _majorService, term, studentid, lastName, firstName, majorCode);

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
                return this.RedirectToAction(a => a.Students(null, null, null, null));
            }

            // check if the student has a registration already
            var registration = Repository.OfType<Registration>().Queryable.Where(a => a.Student == student && a.TermCode == TermService.GetCurrent()).SingleOrDefault();

            // load the current user's ceremonies, to determine what they can register the student for
            var ceremonies = _ceremonyService.GetCeremonies(CurrentUser.Identity.Name);

            //var viewModel = RegistrationModel.Create(Repository, ceremonies, student, registration);
            var registrationModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student: student, registration: registration, edit: true);
            var viewModel = AdminRegisterForStudentViewModel.Create(Repository, registrationModel);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RegisterForStudent(Guid id, RegistrationPostModel registrationPostModel)
        {
            // load the student
            var student = _studentRepository.GetNullableById(id);
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction(a => a.Students(null, null, null, null));
            }

            // check for an existing registration
            var registration = Repository.OfType<Registration>().Queryable.Where(a => a.Student == student && a.TermCode == TermService.GetCurrent()).SingleOrDefault();

            if (registration == null)
            {
                registration = _registrationPopulator.PopulateRegistration(registrationPostModel, student, ModelState, true);
            }
            else
            {
                _registrationPopulator.UpdateRegistration(registration, registrationPostModel, student, ModelState, true);
            }

            registration.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _registrationRepository.EnsurePersistent(registration);

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

                // put message up for student
                Message += StaticValues.Student_Register_Successful;

                return this.RedirectToAction(a => a.StudentDetails(id, false));
            }

            // load the current user's ceremonies, to determine what they can register the student for
            var registrationModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student: student, registration: registration, edit: true);
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

            var viewModel = AdminEditStudentViewModel.Create(Repository, student);
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

            var viewModel = AdminEditStudentViewModel.Create(Repository, student);
            return View(viewModel);
        }
        #endregion

        #region Add Student Functions
        public ActionResult AddStudent(string studentId)
        {
            var student = _studentService.BannerLookup(studentId);
            var viewModel = AdminEditStudentViewModel.Create(Repository, student);
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

            var viewModel = AdminEditStudentViewModel.Create(Repository, student);
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

            //var ceremonies = _ceremonyService.GetCeremonies(CurrentUser.Identity.Name);
            //// find the ceremony with the major
            //var origCeremony = ceremonies.Where(a => a.Majors.Contains(major)).FirstOrDefault();

            //foreach (var a in ceremonies)
            //{
            //    if (a.Majors.Select(b => b.Id).Contains(major.Id))
            //        origCeremony = a;
            //}

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
                //return Json(message, JsonRequestBehavior.AllowGet);
                return false;
            }

            // figure out how many students/tickets are getting moved
            var participations = Repository.OfType<RegistrationParticipation>().Queryable
                                 .Where(a => a.Ceremony == origCeremony && a.Major == majorCode).ToList();

            message = string.Format("You have requested to move {0} to {1} ceremony.  This will move {2} students with {3} tickets leaving {4} available."
                                    , majorCode.Name, destCeremony.DateTime.ToString("g"), participations.Count(), participations.Sum(a => a.TotalTickets)
                                    , destCeremony.AvailableTickets - participations.Sum(a => a.TotalTickets));

            //return Json(message, JsonRequestBehavior.AllowGet);            
            return true;
        }
        #endregion

        public ActionResult Majors()
        {
            var viewModel = AdminMajorsViewModel.Create(Repository, _ceremonyService,_registrationService, CurrentUser);
            return View(viewModel);
        }
    }
}
