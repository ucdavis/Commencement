using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib.Attributes;
using Resources;
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

        public AdminController(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepositoryWithTypedId<MajorCode, string> majorRepository, IStudentService studentService, IEmailService emailService, IMajorService majorService)
        {
            _studentRepository = studentRepository;
            _majorRepository = majorRepository;
            _studentService = studentService;
            _emailService = emailService;
            _majorService = majorService;
        }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Students(string studentid, string lastName, string firstName, string majorCode)
        {
            // get the newest active term
            var term = TermService.GetCurrent();

            var viewModel = AdminStudentViewModel.Create(Repository, _majorService, term, studentid, lastName, firstName, majorCode);

            return View(viewModel);
        }
        public ActionResult StudentDetails(Guid id, bool? registration)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null) return this.RedirectToAction<AdminController>(a => a.Index());

            var viewModel = RegistrationModel.Create(Repository, null, student);

            viewModel.Registration = Repository.OfType<Registration>().Queryable.Where(a => a.Student == student).FirstOrDefault();

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

            var student = new Student(searchStudent.Pidm, searchStudent.Id, searchStudent.FirstName,
                                      searchStudent.LastName, searchStudent.HoursEarned, searchStudent.Email,
                                      searchStudent.LoginId, TermService.GetCurrent());

            student.Majors.Add(_majorRepository.GetNullableById(major));

            return View(student);
        }
        [AcceptPost]
        public ActionResult AddStudentConfirm(string studentId, string majorCode, Student student)
        {
            Check.Require(student != null, "Student cannot be null.");
            Check.Require(!string.IsNullOrEmpty(majorCode), "Major code is required.");
            
            MajorCode major = _majorRepository.GetNullableById(majorCode);
            Check.Require(major != null, "Unable to find major.");

            Student newStudent = null;

            // validate student does not already exist, and if they do make sure we are just adding a new major
            var existingStudent = Repository.OfType<Student>().Queryable.Where(a=>a.StudentId == studentId && a.TermCode == TermService.GetCurrent()).FirstOrDefault();

            if (existingStudent == null)
            {
                newStudent = new Student(student.Pidm, student.StudentId, student.FirstName, student.LastName, student.Units, student.Email, student.Login, student.TermCode);
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
                    _emailService.SendAddPermission(Repository, student);
                }
                catch (Exception)
                {
                    Message += StaticValues.Student_Email_Problem;
                }

                return this.RedirectToAction(a => a.Students(studentId, null, null, null));
            }

            return View(newStudent);
        }

        /// <summary>
        /// Change the major on a registration
        /// </summary>
        /// <param name="id">Registration Id</param>
        /// <returns></returns>
        public ActionResult ChangeMajor(int id)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            if (registration == null) return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null));

            var viewModel = ChangeMajorViewModel.Create(Repository, _majorService, registration);
            return View(viewModel);
        }
        [AcceptPost]
        public ActionResult ChangeMajor(int id, string majorCode)
        {
            var registration = Repository.OfType<Registration>().GetNullableById(id);
            var major = _majorRepository.GetNullableById(majorCode);
            if (registration == null || major == null)
            {
                Message = "Registration or major information was missing.";
                return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null)); 
            }
            registration.Major = major;

            var message = ValidateMajorChange(registration, major);
            if (!string.IsNullOrEmpty(message)) ModelState.AddModelError("Major Code", message);

            registration.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Registration>().EnsurePersistent(registration);

                return this.RedirectToAction<AdminController>(a => a.StudentDetails(registration.Student.Id, true));
            }

            var viewModel = ChangeMajorViewModel.Create(Repository, _majorService, registration);
            return View(viewModel);
        }
        public JsonResult ChangeMajorValidation(int regId, string major)
        {
            var majorCode = _majorRepository.GetNullableById(major);
            var registration = Repository.OfType<Registration>().GetNullableById(regId);

            var message = ValidateMajorChange(registration, majorCode);

            if (string.IsNullOrEmpty(message)) message = "There are no problems changing this student's major.";

            return Json(message);
        }
        private string ValidateMajorChange(Registration registration, MajorCode majorCode)
        {
            var term = TermService.GetCurrent();

            Check.Require(term != null, "Unable to locate current term.");
            Check.Require(majorCode != null, "Major code is required to check validation.");
            Check.Require(registration != null, "Registration is required.");

            var ceremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode == term && a.Majors.Contains(majorCode)).FirstOrDefault();
            
            var message = new StringBuilder();

            if (ceremony == null) message.Append("There is no matching ceremony for the current term with the major specified.");
            else if (ceremony != registration.Ceremony)
            {
                if (ceremony.AvailableTickets - registration.TotalTickets > 0) message.Append("There are enough tickets to move this students major.");
                else message.Append("There are not enough tickets to move this student to the ceremony.");

                message.Append("Student is being moved into a different ceremony");
            }

            return message.ToString();
        }

        public ActionResult Registrations(string studentid, string lastName, string firstName, string majorCode, int? ceremonyId)
        {
            var term = TermService.GetCurrent();
            var viewModel = AdminRegistrationViewModel.Create(Repository, _majorService, term, studentid, lastName, firstName, majorCode, ceremonyId);
            return View(viewModel);
        }
    }
}
