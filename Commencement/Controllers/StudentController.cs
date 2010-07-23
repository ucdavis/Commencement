using System;
using System.Linq;
using System.Web.Mvc;
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
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;

        public StudentController(IStudentService studentService, 
            IEmailService emailService,
            IRepository<Student> studentRepository, 
            IRepository<Ceremony> ceremonyRepository, 
            IRepository<Registration> registrationRepository)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
            _studentService = studentService;
            _emailService = emailService;
        }

        //
        // GET: /Student/

        public ActionResult Index()
        {
            //Check for prior registration
            var priorRegistration = _studentService.GetPriorRegistration(GetCurrentStudent());
            
            if (priorRegistration != null)
            {
                return this.RedirectToAction(x => x.DisplayRegistration(priorRegistration.Id));
            }

            //Check student untis and major))))

            return this.RedirectToAction(x => x.ChooseCeremony());
        }

        public ActionResult ChooseCeremony()
        {
            var majorsAndCeremonies = _studentService.GetMajorsAndCeremoniesForStudent(GetCurrentStudent());

            var numPossibleCeremonies = majorsAndCeremonies.Count();
            
            if (numPossibleCeremonies == 0)
            {
                throw new NotImplementedException("No possible ceremonies found");
            }
            else if (numPossibleCeremonies == 1)
            {
                var ceremony = majorsAndCeremonies.Single();
                
                return this.RedirectToAction(x => x.Register(ceremony.Ceremony.Id, string.Empty));
            }
            
            return View(majorsAndCeremonies);
        }

        public ActionResult DisplayRegistration(int id /* registration id */)
        {
            var registration = _registrationRepository.GetNullableById(id);

            if (registration == null) return this.RedirectToAction(x => x.Index());

            return View(registration);
        }

        public ActionResult RegistrationConfirmation(int id /* registration id */)
        {
            var registration = _registrationRepository.GetNullableById(id);

            if (registration == null) return this.RedirectToAction(x => x.Index());

            return View(registration);
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
        public ActionResult Register(int id, Registration registration, bool agreeToDisclaimer)
        {
            registration.Student = GetCurrentStudent();
            registration.Ceremony = _ceremonyRepository.GetNullableById(id);
            
            registration.TransferValidationMessagesTo(ModelState);

            if (agreeToDisclaimer == false)
            {
                ModelState.AddModelError("agreeToDisclaimer", "You must agree to the disclaimer");
            }

            if (ModelState.IsValid)
            {
                //Save the registration
                _registrationRepository.EnsurePersistent(registration);

                _emailService.SendRegistrationConfirmation(Repository, registration);

                Message = "You have successfully registered for this conference.";

                return this.RedirectToAction(x => x.RegistrationConfirmation(registration.Id));
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
