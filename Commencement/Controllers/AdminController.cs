using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using UCDArch.Core.Utils;

namespace Commencement.Controllers
{
    public class AdminController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IStudentService _studentService;

        public AdminController(IRepositoryWithTypedId<Student, Guid> studentRepository, IStudentService studentService)
        {
            _studentRepository = studentRepository;
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

            return View(student);
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
