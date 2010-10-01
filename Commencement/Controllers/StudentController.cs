using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using MvcContrib.Attributes;
using UCDArch.Core.Utils;
using UCDArch.Web.Helpers;
using Commencement.Controllers.Helpers;

namespace Commencement.Controllers
{
    [StudentsOnly]
    public class StudentController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;

        public StudentController(IStudentService studentService, 
            IEmailService emailService,
            IRepositoryWithTypedId<Student, Guid> studentRepository, 
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
        [PageTrackingFilter]
        public ActionResult Index()
        {
            //Check for prior registration
            var priorRegistration = _studentService.GetPriorRegistration(GetCurrentStudent());
            
            if (priorRegistration != null)
            {
                // show an existing registration
                return this.RedirectToAction(x => x.DisplayRegistration(priorRegistration.Id));
            }

            //Check student untis and major))))
            return this.RedirectToAction(x => x.ChooseCeremony());
        }
        [PageTrackingFilter]
        public ActionResult ChooseCeremony()
        {
            var currentStudent = GetCurrentStudent();
            // student has ceremony pre-assigned
            if (currentStudent.Ceremony != null)
            {
                return this.RedirectToAction(x => x.Register(currentStudent.Ceremony.Id, string.Empty));
            }

            var majorsAndCeremonies = _studentService.GetMajorsAndCeremoniesForStudent(currentStudent);

            var numPossibleCeremonies = majorsAndCeremonies.Count();
            
            // no matching ceremony for student's major
            if (numPossibleCeremonies == 0)
            {
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.NoCeremony));
                //throw new NotImplementedException("No possible ceremonies found");
            }
            // only one matching ceremony
            if (numPossibleCeremonies == 1)
            {
                var ceremony = majorsAndCeremonies.Single();
                
                if (currentStudent.Majors.Count != 1) return this.RedirectToAction(x => x.Register(ceremony.Ceremony.Id, majorsAndCeremonies.Single().MajorCode.Id));
                return this.RedirectToAction(x => x.Register(ceremony.Ceremony.Id, string.Empty));
            }
            
            // multiple ceremonies, let the student pick one
            return View(majorsAndCeremonies);
        }
        [PageTrackingFilter]
        public ActionResult DisplayRegistration(int id /* registration id */)
        {
            var registration = _registrationRepository.GetNullableById(id);

            // not valid registration or current logged in student doesn't match owner of registration
            if (registration == null  || registration.Student != _studentService.GetCurrentStudent(CurrentUser)) return this.RedirectToAction(x => x.Index());

            ViewData["CanEditRegistration"] = registration.Ceremony.RegistrationDeadline > DateTime.Now;

            return View(registration);
        }
        [PageTrackingFilter]
        public ActionResult RegistrationConfirmation(int id /* registration id */)
        {
            var registration = _registrationRepository.GetNullableById(id);

            if (registration == null || registration.Student != _studentService.GetCurrentStudent(CurrentUser)) return this.RedirectToAction(x => x.Index());

            return View(registration);
        }

        /// <summary>
        /// Registers a student for commencement
        /// </summary>
        /// <param name="id">Id of the commencement to register for</param>
        /// <param name="major">Major to register with.  Only required if a student has multiple majors</param>
        /// <returns></returns>
        [PageTrackingFilter]
        public ActionResult Register(int id /* ceremony id */, string major)
        {
            var ceremony = _ceremonyRepository.GetNullableById(id);

            // ceremony was not found
            if (ceremony == null)
            {
                Message = StaticValues.Student_No_Ceremony_Found;
                return this.RedirectToAction(x => x.Index());
            }
            if(ceremony.RegistrationDeadline <= DateTime.Now)
            {
                //Message = StaticValues.Student_CeremonyDeadlinePassed;
                return this.RedirectToAction<ErrorController>(x => x.Index(ErrorController.ErrorType.RegistrationClosed));
            }

            var student = GetCurrentStudent();
            
            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(Repository, ceremony, student);

            if (string.IsNullOrEmpty(major))
            {
                //If major is not supplied, the student must be a single major
                if (student.Majors.Count() != 1)
                {
                    Message = StaticValues.Student_Major_Code_Not_Supplied;
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
        public ActionResult Register(int id, Registration registration, bool agreeToDisclaimer)
        {
            registration.Student = GetCurrentStudent();
            registration.Ceremony = _ceremonyRepository.GetNullableById(id);
            NullOutBlankFields(registration);
            
            //The check of a null ceremony will get caught by the domain values check.)
            if (registration.Ceremony != null && registration.Ceremony.RegistrationDeadline <= DateTime.Now)
            {
                //Message = StaticValues.Student_CeremonyDeadlinePassed;
                //return this.RedirectToAction(a => a.Index());
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            }
            
            registration.TransferValidationMessagesTo(ModelState);

            if (agreeToDisclaimer == false)
            {
                ModelState.AddModelError("agreeToDisclaimer", StaticValues.Student_agree_to_disclaimer);
            }

            if (ModelState.IsValid)
            {
                //Save the registration
                _registrationRepository.EnsurePersistent(registration);

                Message = StaticValues.Student_Register_Successful;

                try
                {
                    _emailService.SendRegistrationConfirmation(Repository, registration);    
                }
                catch(Exception)
                {
                    Message += StaticValues.Student_Email_Problem;
                }
                
                return this.RedirectToAction(x => x.RegistrationConfirmation(registration.Id));
            }
            
            var viewModel = RegistrationModel.Create(Repository, registration.Ceremony, registration.Student);
            viewModel.Registration = registration;

            return View(viewModel);
        }

        private void NullOutBlankFields(Registration registration)
        {
            //registration.Address2 = string.IsNullOrEmpty(registration.Address2) ? null : registration.Address2;
            registration.Address2 = registration.Address2.IsNullOrEmpty(true) ? null : registration.Address2;
            registration.Address3 = registration.Address3.IsNullOrEmpty(true) ? null : registration.Address3;

            registration.Email = registration.Email.IsNullOrEmpty(true) ? null : registration.Email;

            registration.Comments = registration.Comments.IsNullOrEmpty(true) ? null : registration.Comments;
        }

        [PageTrackingFilter]
        public ActionResult EditRegistration(int id)
        {
            var registration = _registrationRepository.GetNullableById(id);
            
            var student = GetCurrentStudent();
            
            if (registration == null || registration.Student != student)
            {
                Message = StaticValues.Student_No_Registration_Found;
                return this.RedirectToAction(a => a.Index());
            }
            if(registration.Ceremony.RegistrationDeadline <= DateTime.Now)
            {                
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            }
            
            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(Repository, registration.Ceremony, student);
            viewModel.Registration = registration;

            return View(viewModel);
        }

        [AcceptPost]
        public ActionResult EditRegistration(int id, Registration registration, bool agreeToDisclaimer)
        {
            var registrationToEdit = _registrationRepository.GetNullableById(id);
            var student = GetCurrentStudent();

            if (registrationToEdit == null || registrationToEdit.Student != student)
            {
                Message = StaticValues.Student_No_Registration_Found;
                return this.RedirectToAction(a => a.Index());
            }
            if (registrationToEdit.Ceremony.RegistrationDeadline <= DateTime.Now)
            {
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            }

            registrationToEdit.Student = student;

            CopyHelper.CopyRegistrationValues(registration, registrationToEdit);
            NullOutBlankFields(registrationToEdit);

            registrationToEdit.TransferValidationMessagesTo(ModelState);

            if (agreeToDisclaimer == false)
            {
                ModelState.AddModelError("agreeToDisclaimer", StaticValues.Student_agree_to_disclaimer);
            }

            if (ModelState.IsValid)
            {
                //Save the registration
                _registrationRepository.EnsurePersistent(registrationToEdit);

                Message = StaticValues.Student_Register_Edit_Successful;

                try
                {
                    _emailService.SendRegistrationConfirmation(Repository, registrationToEdit);
                }
                catch (Exception)
                {
                    Message += StaticValues.Student_Email_Problem;
                }

                return this.RedirectToAction(x => x.RegistrationConfirmation(registrationToEdit.Id));
            }

            var viewModel = RegistrationModel.Create(Repository, registration.Ceremony, registrationToEdit.Student);
            viewModel.Registration = registration;

            return View(viewModel);
        }

        [PageTrackingFilter]
        public ActionResult NoCeremony()
        {
            return View();
        }

        private Student GetCurrentStudent()
        {
            var currentStudent = _studentService.GetCurrentStudent(CurrentUser);

            Check.Require(currentStudent != null, "Current user is not a student or student not found.");
            
            return currentStudent;
        }
    }
}
