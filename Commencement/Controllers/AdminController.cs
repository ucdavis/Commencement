using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib.Attributes;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using UCDArch.Core.Utils;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    public class AdminController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorRepository;
        private readonly IStudentService _studentService;

        public AdminController(IRepositoryWithTypedId<Student, Guid> studentRepository, IRepositoryWithTypedId<MajorCode, string> majorRepository, IStudentService studentService)
        {
            _studentRepository = studentRepository;
            _majorRepository = majorRepository;
            _studentService = studentService;
        }

        //
        // GET: /Admin/

        [AnyoneWithRole]
        public ActionResult Index()
        {
            return View();
        }
        [AnyoneWithRole]
        public ActionResult Students(string studentid, string lastName, string firstName, string majorCode)
        {
            // get the newest active term
            var term = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();

            var viewModel = AdminStudentViewModel.Create(Repository, term, studentid, lastName, firstName, majorCode);

            return View(viewModel);
        }
        [AnyoneWithRole]
        public ActionResult StudentDetails(Guid id)
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
        [AnyoneWithRole]
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

        [AnyoneWithRole]
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

        [AnyoneWithRole]
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

                return this.RedirectToAction(a => a.Students(studentId, null, null, null));
            }

            return View(newStudent);
        }

        [AnyoneWithRole]
        public ActionResult Registrations(string studentid, string lastName, string firstName, string majorCode, int? ceremonyId)
        {
            var term = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();
            var viewModel = AdminRegistrationViewModel.Create(Repository, term, studentid, lastName, firstName, majorCode, ceremonyId);
            return View(viewModel);
        }
    }
}
