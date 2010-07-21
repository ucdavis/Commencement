using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using MvcContrib.Attributes;
using UCDArch.Web.Helpers;
using Commencement.Controllers.Helpers;

namespace Commencement.Controllers
{
    public class StudentController : ApplicationController
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IStudentService _studentService;

        public StudentController(IRepository<Student> studentRepository, IRepository<Ceremony> ceremonyRepository, IStudentService studentService)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _studentService = studentService;
        }

        //
        // GET: /Student/

        public ActionResult Index()
        {
            //Check for prior registration

            //Check student untis and major))))

            return this.RedirectToAction(x => x.ChooseCeremony());
        }

        public ActionResult ChooseCeremony()
        {
            var majorsAndCeremonies = _studentService.GetMajorsAndCeremoniesForStudent(GetCurrentStudent());

            if (majorsAndCeremonies.Count == 1)
            {
                var ceremony = majorsAndCeremonies.Single();
                
                return this.RedirectToAction(x => x.Register(ceremony.Ceremony.Id));
            }

            return View(majorsAndCeremonies);
        }

        public ActionResult Register(int id /* ceremony id */)
        {
            var ceremony = _ceremonyRepository.GetNullableById(id);

            if (ceremony == null) return this.RedirectToAction(x => x.Index());
            
            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(Repository, GetCurrentStudent(), ceremony);

            return View(viewModel);
        }

        [AcceptPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(int id, Registration registration)
        {
            registration.Student = GetCurrentStudent();
            registration.Ceremony = _ceremonyRepository.GetNullableById(id);
            
            registration.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Message = "Testing:  Save Successful";
                return RedirectToAction("Register");
            }
            
            var viewModel = RegistrationModel.Create(Repository, registration.Student, registration.Ceremony);
            viewModel.Registration = registration;

            return View(viewModel);
        }

        private Student GetCurrentStudent()
        {
            var currentStudent = _studentService.GetCurrentStudent(CurrentUser);
            
            if (currentStudent == null)
            {
                //Student not found, go to petition workflow
                throw new NotImplementedException("Current Student Not Found");
                //return RedirectToAction("Petition");
            }

            return currentStudent;
        }

    }
}
