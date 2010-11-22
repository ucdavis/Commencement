using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using MvcContrib.Attributes;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using UCDArch.Core.Utils;
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

        public AdminController(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepositoryWithTypedId<MajorCode, string> majorRepository, IStudentService studentService, IEmailService emailService, IMajorService majorService, ICeremonyService ceremonyService, IRegistrationService registrationService)
        {
            _studentRepository = studentRepository;
            _majorRepository = majorRepository;
            _studentService = studentService;
            _emailService = emailService;
            _majorService = majorService;
            _ceremonyService = ceremonyService;
            _registrationService = registrationService;
        }

        /// <summary>
        /// GET: /Admin/
        /// #1
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var ceremonies = Repository.OfType<Ceremony>().GetAll();

            return View();
        }

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

        #region Student Details
        /// <summary>
        /// Students the details.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="registration">This value is used in the pages, not the controller. Don't remove it.</param>
        /// <returns></returns>
        public ActionResult StudentDetails(Guid id, bool? registration)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null) return this.RedirectToAction<AdminController>(a => a.Index());

            var viewModel = RegistrationModel.Create(Repository, _ceremonyService.GetCeremonies(CurrentUser.Identity.Name), student);
            viewModel.Registration = Repository.OfType<Registration>().Queryable.Where(a => a.Student == student).FirstOrDefault();

            return View(viewModel);
        }
        public ActionResult ToggleSJAStatus(Guid id)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null) return this.RedirectToAction(a => a.Index());

            student.SjaBlock = !student.SjaBlock;
            Repository.OfType<Student>().EnsurePersistent(student);

            return this.RedirectToAction(a => a.StudentDetails(id, false));
        }
        public ActionResult ToggleBlock(Guid id)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null) return this.RedirectToAction(a => a.Index());

            student.Blocked = !student.Blocked;
            _studentRepository.EnsurePersistent(student);

            return this.RedirectToAction(a => a.StudentDetails(id, false));
        }
        #endregion







        /// <summary>
        /// Change the major on a registration
        /// </summary>
        /// <param name="id">Registration Id</param>
        /// <returns></returns>
        public ActionResult ChangeMajor(int id)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null) return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null));

            var viewModel = ChangeMajorViewModel.Create(Repository, _majorService, registration, _ceremonyService.GetCeremonies(CurrentUser.Identity.Name));
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ChangeMajor(int id, string majorCode)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            var major = _majorRepository.GetNullableById(majorCode);
            if (registration == null || major == null)
            {
                Message = "Registration or major information was missing.";
                return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null));
            }

            registration.RegistrationParticipations[0].Major = major;
            //registration.College = major.College;

            var ceremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode == TermService.GetCurrent() && a.Majors.Contains(major)).FirstOrDefault();
            //if (!CeremonyHasAvailability(ceremony, registration)) ModelState.AddModelError("Major Code", ValidateMajorChange(registration, major));
            //var validationMessages = ValidateMajorChange(registration, major);
            //if(!string.IsNullOrEmpty(validationMessages))
            //{
            //    ModelState.AddModelError("Major Code", validationMessages); //TODO: Review
            //}
            if (!CeremonyHasAvailability(ceremony, registration))
            {
                //registration.Ceremony = ceremony;
                ModelState.AddModelError("Ceremony", "Ceremony does not have enough tickets to move this student.");
            }
            else
            {
                registration.RegistrationParticipations[0].Ceremony = ceremony;
            }

            registration.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Registration>().EnsurePersistent(registration);

                return this.RedirectToAction<AdminController>(a => a.StudentDetails(registration.Student.Id, true));
            }

            var viewModel = ChangeMajorViewModel.Create(Repository, _majorService, registration, _ceremonyService.GetCeremonies(CurrentUser.Identity.Name));
            return View(viewModel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Student Id</param>
        /// <returns></returns>
        public ActionResult AddStudent(string studentId)
        {
            var viewModel = SearchStudentViewModel.Create(Repository);

            if (!string.IsNullOrEmpty(studentId))
            {
                // lookup the student
                viewModel.SearchStudents = _studentService.SearchStudent(studentId, TermService.GetCurrent().Id);
                viewModel.StudentId = studentId;

                if (viewModel.SearchStudents.Count <= 0)
                {
                    Message = "No results for the student were found.";
                }
            }

            return View(viewModel);
        }
        public ActionResult AddStudentConfirm(string studentId, string major)
        {
            // either variable is invalid redirect back to the add student page
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(major)) return this.RedirectToAction(a => a.AddStudent(studentId));

            var searchResults = _studentService.SearchStudent(studentId, TermService.GetCurrent().Id);
            var searchStudent = searchResults.Where(a => a.MajorCode == major).FirstOrDefault();

            Check.Require(searchStudent != null, "Unable to find requested record.");

            var student = new Student(searchStudent.Pidm, searchStudent.Id, searchStudent.FirstName, searchStudent.MI,
                                      searchStudent.LastName, searchStudent.HoursEarned, searchStudent.Email,
                                      searchStudent.LoginId, TermService.GetCurrent());

            student.Majors.Add(_majorRepository.GetNullableById(major));

            return View(student);
        }
        [HttpPost]
        public ActionResult AddStudentConfirm(string studentId, string majorCode, Student student)
        {
            Check.Require(student != null, "Student cannot be null.");
            Check.Require(!string.IsNullOrEmpty(majorCode), "Major code is required.");

            student.TermCode = TermService.GetCurrent();

            MajorCode major = _majorRepository.GetNullableById(majorCode);
            Check.Require(major != null, "Unable to find major.");

            var ceremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode == TermService.GetCurrent() && a.Majors.Contains(major)).FirstOrDefault();
            //Check.Require(ceremony != null, "Ceremony is required.");
            if (ceremony == null) { ModelState.AddModelError("Ceremony", "No ceremony exists for this major for the current term.");}

            Student newStudent = null;

            // validate student does not already exist, and if they do make sure we are just adding a new major
            var existingStudent = Repository.OfType<Student>().Queryable.Where(a=>a.StudentId == studentId && a.TermCode == TermService.GetCurrent()).FirstOrDefault();

            if (existingStudent == null)
            {
                newStudent = new Student(student.Pidm, student.StudentId, student.FirstName, student.MI, student.LastName, student.TotalUnits, student.Email, student.Login, student.TermCode);
                newStudent.Majors.Add(major);
            }
            else if (!existingStudent.Majors.Contains(major))
            {
                existingStudent.Majors.Add(major);
                newStudent = existingStudent;       // just need to save the update to the major
            }
            else
            {
                // student already exists with major, bail out
                Message = "Student already exists.";
                newStudent = student;
                newStudent.Majors.Add(major);
                return View(newStudent);
            }

            // either new student or adding a major
            newStudent.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Student>().EnsurePersistent(newStudent);

                try
                {
                    _emailService.SendAddPermission(student, ceremony);
                }
                catch (Exception)
                {
                    Message += string.Format(StaticValues.Student_Add_Permission_Problem, newStudent.FullName);
                }

                return this.RedirectToAction(a => a.Students(studentId, null, null, null));
            }

            return View(newStudent);
        }



        public ActionResult ChangeCeremony(int id)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null) return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null));

            var viewModel = ChangeCeremonyViewModel.Create(Repository, registration);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult ChangeCeremony(int id, int ceremonyId)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyId);

            if (registration == null || ceremony == null)
            {
                Message = "Registration or ceremony information was missing.";
                return this.RedirectToAction(a => a.Students(null, null, null, null));
            }
            registration.RegistrationParticipations[0].Ceremony = ceremony;

            if (!CeremonyHasAvailability(ceremony, registration)) ModelState.AddModelError("Major Code", ValidateAvailabilityAtCeremony(ceremony, registration));

            registration.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Registration>().EnsurePersistent(registration);

                return this.RedirectToAction<AdminController>(a => a.StudentDetails(registration.Student.Id, true));
            }

            var viewModel = ChangeMajorViewModel.Create(Repository, _majorService, registration, _ceremonyService.GetCeremonies(CurrentUser.Identity.Name));
            return View(viewModel);
        }

        #region Validation Functions for Changing Registration
        public JsonResult ChangeMajorValidation(int regId, string major)
        {
            var majorCode = _majorRepository.GetNullableById(major);
            var registration = Repository.OfType<Registration>().GetNullableById(regId);

            var message = ValidateMajorChange(registration, majorCode);

            if (string.IsNullOrEmpty(message)) message = "There are no problems changing this student's major.";

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        private string ValidateMajorChange(Registration registration, MajorCode majorCode)
        {
            var term = TermService.GetCurrent();

            Check.Require(term != null, "Unable to locate current term.");
            Check.Require(majorCode != null, "Major code is required to check validation.");
            Check.Require(registration != null, "Registration is required.");

            var ceremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode == term && a.Majors.Contains(majorCode)).FirstOrDefault();
            return ValidateAvailabilityAtCeremony(ceremony, registration);
        }
        public JsonResult ChangeCeremonyValidation(int regId, int ceremonyId)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(regId);
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyId);

            Check.Require(registration != null, "Registration is required.");
            Check.Require(ceremony != null, "Ceremony is required.");

            var message = ValidateAvailabilityAtCeremony(ceremony, registration);
            if (string.IsNullOrEmpty(message)) message = "This is the student's currently assigned ceremony.";

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        private string ValidateAvailabilityAtCeremony(Ceremony ceremony, Registration registration)
        {
            StringBuilder message = new StringBuilder();

            if (ceremony == null) message.Append("There is no matching ceremony for the current term with the major specified.");
            else if (ceremony != registration.RegistrationParticipations[0].Ceremony)
            {
                if (ceremony.AvailableTickets - registration.TotalTickets >= 0)
                {
                    message.Append("There are enough tickets to move this students major.");
                    message.Append("Student will be moved into a different ceremony if you proceed.");
                }
                else
                {
                    message.Append("There are not enough tickets to move this student to the ceremony.");                     
                }

                
            }

            return message.ToString();
        }

        private bool CeremonyHasAvailability(Ceremony ceremony, Registration registration)
        {
            Check.Require(ceremony != null, "Ceremony is required.");
            Check.Require(registration != null, "Registration is required.");

            if (ceremony != registration.RegistrationParticipations[0].Ceremony)
            {
                if (ceremony.AvailableTickets - registration.TotalTickets > 0) return true;
                return false;
            }

            return true;
        }
        #endregion

    }
}
