using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers
{
    public class AdminController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, string> _studentRepository;

        public AdminController(IRepositoryWithTypedId<Student, string> studentRepository)
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

        public ActionResult StudentDetails()
        {
            var student = new Student();
            var ceremony = new Ceremony();
            var viewModel = RegistrationModel.Create(Repository, ceremony, student);

            throw new NotImplementedException();
        }

        public ActionResult Registrations(string studentid, string lastName, string firstName, string majorCode)
        {
            var term = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();
            var viewModel = AdminRegistrationViewModel.Create(Repository, term, studentid, lastName, firstName, majorCode);
            return View(viewModel);
        }
    }
}
