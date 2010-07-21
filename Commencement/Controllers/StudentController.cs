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
using UCDArch.Core.Utils;

namespace Commencement.Controllers
{
    public class StudentController : ApplicationController
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService, IRepository<Student> studentRepository, IRepository<Ceremony> ceremonyRepository)
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
                
                return this.RedirectToAction(x => x.Register(ceremony.Ceremony.Id, string.Empty));
            }

            return View(majorsAndCeremonies);
        }

        /// <summary>
        /// Registers a student for commencement
        /// </summary>
        /// <param name="id">Id of the commencement to register for</param>
        /// <param name="major">Major to register with.  Only required if a student has multiple majors</param>
        /// <returns></returns>
        public ActionResult Register(int id /* ceremony id */, string major)
        {
            var ceremony = _ceremonyRepository.GetNullableById(id);

            if (ceremony == null)
            {
                Message = "No matching ceremony found.  Please try your registration again.";
                return this.RedirectToAction(x => x.Index());
            }

            var student = GetCurrentStudent();
            
            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(Repository, ceremony, student);

            if (string.IsNullOrEmpty(major))
            {
                //If major is not supplied, the student must be a single major
                if ( student.Majors.Count() != 1)
                {
                    Message = "Student has multiple majors but did not supply a major code.";
                    return this.RedirectToAction(x => x.Index());
                } 

                viewModel.Registration.Major = student.Majors.Single();
            }
            else
            {
                //if the major is supplied, make it the registration's major
                viewModel.Registration.Major = student.Majors.Where(x => x.Id == major).Single();
            }

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
            
            var viewModel = RegistrationModel.Create(Repository, registration.Ceremony, registration.Student);
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
