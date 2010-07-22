using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;

namespace Commencement.Controllers
{
    public class AdminController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;

        public AdminController(IRepositoryWithTypedId<Student, Guid> studentRepository)
        {
            _studentRepository = studentRepository;
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
            var term = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();

            var viewModel = AdminStudentViewModel.Create(Repository, term, studentid, lastName, firstName, majorCode);

            return View(viewModel);
        }

        public ActionResult StudentDetails(Guid id)
        {
            var student = _studentRepository.GetNullableById(id);
            if (student == null) return this.RedirectToAction<AdminController>(a => a.Index());

            var viewModel = RegistrationModel.Create(Repository, null, student);

            viewModel.Registration = Repository.OfType<Registration>().Queryable.Where(a => a.Student == student).FirstOrDefault();

            return View(viewModel);
        }

        public ActionResult Registrations(string studentid, string lastName, string firstName, string majorCode, int? ceremonyId)
        {
            var term = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();
            var viewModel = AdminRegistrationViewModel.Create(Repository, term, studentid, lastName, firstName, majorCode, ceremonyId);
            return View(viewModel);
        }
    }
}
