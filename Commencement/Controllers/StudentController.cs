using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
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
    [SessionExpirationFilter]
    public class StudentController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private readonly IRepository<Ceremony> _ceremonyRepository;
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IErrorService _errorService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IRepository<RegistrationPetition> _registrationPetitionRepository;
        private readonly IRepository<RegistrationParticipation> _participationRepository;
        private readonly IRegistrationPopulator _registrationPopulator;
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;

        public StudentController(IStudentService studentService, 
            IEmailService emailService,
            IRepositoryWithTypedId<Student, Guid> studentRepository, 
            IRepository<Ceremony> ceremonyRepository, 
            IRepository<Registration> registrationRepository,
            IErrorService errorService,
            ICeremonyService ceremonyService, IRepository<RegistrationPetition> registrationPetitionRepository,
            IRepository<RegistrationParticipation> participationRepository, IRegistrationPopulator registrationPopulator)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
            _errorService = errorService;
            _ceremonyService = ceremonyService;
            _registrationPetitionRepository = registrationPetitionRepository;
            _participationRepository = participationRepository;
            _registrationPopulator = registrationPopulator;
            _studentService = studentService;
            _emailService = emailService;
        }

        /// <summary>
        /// automatically redirect to the routing
        /// #1
        /// </summary>
        /// <returns></returns>
        [PageTrackingFilter]
        [IgnoreStudentsOnly]
        public RedirectToRouteResult Index()
        {
            // validate student is in our DB, otherwise we need to do a lookup
            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return this.RedirectToAction<ErrorController>(a => a.NotFound());

            // student was not null, student either in our system or in banner, proceed
            return this.RedirectToAction(a => a.RegistrationRouting());
        }

        /// <summary>
        /// #2
        /// </summary>
        /// <returns></returns>
        [PageTrackingFilter]
        public RedirectToRouteResult RegistrationRouting()
        {
            var term = TermService.GetCurrent();
            var student = _studentService.GetCurrentStudent(CurrentUser);

            var redirect = CheckStudentForRegistration(student, term);
            if (redirect != null) return redirect;

            // redirect the student to the register page
            return this.RedirectToAction(a => a.Register());
        }

        [PageTrackingFilter]
        public ActionResult Register()
        {
            var term = TermService.GetCurrent();
            var student = _studentService.GetCurrentStudent(CurrentUser);

            var redirect = CheckStudentForRegistration(student, term);
            if (redirect != null) return redirect;

            var viewModel = RegistrationModel.Create(Repository, _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Register(RegistrationPostModel registrationModel)
        {
            var term = TermService.GetCurrent();
            var student = GetCurrentStudent();

            if (student == null) return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null));

            // validate they can register, also checks for duplicate registrations
            var redirect = CheckStudentForRegistration(student, term);
            if (redirect != null) return redirect;

            var registration = _registrationPopulator.PopulateRegistration(registrationModel, student, ModelState);

            registration.TransferValidationMessagesTo(ModelState);

            if (!registrationModel.AgreeToDisclaimer) ModelState.AddModelError("agreeToDisclaimer", StaticValues.Student_agree_to_disclaimer);
            if (registration.RegistrationPetitions.Any(a=>string.IsNullOrEmpty(a.ExceptionReason))) ModelState.AddModelError("Exception Reason", "Exception reason is required.");

            if (ModelState.IsValid)
            {
                if (registration.RegistrationParticipations.Count > 0 || registration.RegistrationPetitions.Count > 0)
                {
                    // save registration
                    _registrationRepository.EnsurePersistent(registration);

                    if (registration.RegistrationParticipations.Count > 0)
                    {
                        try
                        {
                            // add email for registration into queue
                            _emailService.QueueRegistrationConfirmation(registration);
                        }
                        catch (Exception ex)
                        {
                            _errorService.ReportError(ex);
                            Message += StaticValues.Student_Email_Problem;
                        }
                    }

                    if (registration.RegistrationPetitions.Count > 0)
                    {
                        try
                        {
                            _emailService.QueueRegistrationPetition(registration);
                        }
                        catch (Exception ex)
                        {
                            _errorService.ReportError(ex);
                            Message += StaticValues.Student_Email_Problem;
                        }
                    }

                    // put message up for student
                    Message += StaticValues.Student_Register_Successful;
                }

                return this.RedirectToAction(a => a.DisplayRegistration());
            }

            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student: student, ceremonyParticipations: registrationModel.CeremonyParticipations, registration: registration);
            return View(viewModel);
        }

        [PageTrackingFilter]
        public ActionResult DisplayRegistration()
        {
            var registration = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == GetCurrentStudent());
            //var petitions = _registrationPetitionRepository.Queryable.Where(a => a.Registration.Student == GetCurrentStudent()).ToList();

            // must have either registration or at least one petition
            if (registration == null) return this.RedirectToAction(a => a.Index());

            var viewModel = StudentDisplayRegistrationViewModel.Create(Repository, registration);

            return View(viewModel);
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
            if (!registration.RegistrationParticipations.Any(a=>a.Ceremony.CanRegister()))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotOpen());
            }

            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.GetCeremonies(CurrentUser.Identity.Name), student: student, registration: registration, edit: true);            
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditRegistration(int id /* Registration Id */, RegistrationPostModel registrationPostModel)
        {
            var registrationToEdit = _registrationRepository.GetNullableById(id);
            var student = GetCurrentStudent();

            if (registrationToEdit == null || registrationToEdit.Student != student)
            {
                Message = StaticValues.Student_No_Registration_Found;
                return this.RedirectToAction(a => a.Index());
            }
            if (!registrationToEdit.RegistrationParticipations.Any(a => a.Ceremony.CanRegister()))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotOpen());
            }

            _registrationPopulator.UpdateRegistration(registrationToEdit, registrationPostModel, student, ModelState);


            registrationToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                //Save the registration
                _registrationRepository.EnsurePersistent(registrationToEdit);

                Message = StaticValues.Student_Register_Edit_Successful;

                try
                {
                    _emailService.QueueRegistrationConfirmation(registrationToEdit);
                }
                catch (Exception ex)
                {
                    _errorService.ReportError(ex);
                    Message += StaticValues.Student_Email_Problem;
                }

                //return this.RedirectToAction(x => x.RegistrationConfirmation(registrationToEdit.Id, null));
                return this.RedirectToAction(a => a.DisplayRegistration());
            }

            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student: student, registration: registrationToEdit, edit: true);            
            return View(viewModel);
        }

        #region Helper Methods
        /// <summary>
        /// Does initial checks so that students are always redirected correctly for first time registration
        /// </summary>
        /// <param name="student"></param>
        /// <param name="termCode"></param>
        /// <returns>Redirect instruction, if null, student is valid to register</returns>
        private RedirectToRouteResult CheckStudentForRegistration(Student student, TermCode termCode)
        {
            // unable to find record for some reason, either from download or banner lookup
            if (student == null || student.Blocked)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotEligible());
            }

            if (termCode == null)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotOpen());
            }

            if (student.SjaBlock)
            {
                return this.RedirectToAction<ErrorController>(a => a.SJA());
            }

            // has this student registered yet?
            var reg = _registrationRepository.Queryable.Where(a => a.Student == student).ToList();
            var currentReg = reg.Where(a => a.TermCode.Id == termCode.Id).FirstOrDefault();
            if (currentReg != null)
            {
                // display previous registration
                // return this.RedirectToAction(a => a.DisplayRegistration(currentReg.Id));
                return this.RedirectToAction(a => a.DisplayRegistration());
            }

            // has this student registered in a previous term?
            var pastReg = reg.Where(a => a.TermCode.Id != termCode.Id).FirstOrDefault();
            if (pastReg != null)
            {
                // redirect to past registration message
                return this.RedirectToAction<ErrorController>(a => a.PreviouslyWalked());
            }

            // see if student can register with this system
            var eligibleCeremonies = _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits);
            if (eligibleCeremonies == null || eligibleCeremonies.Count == 0)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotEligible());
            }

            // see if registration is open for at least one ceremony
            if (!eligibleCeremonies.Where(a => a.RegistrationBegin < DateTime.Now).Any())
            {
                return this.RedirectToAction<ErrorController>(a => a.NotOpen());
            }

            return null;
        }

        private Student GetCurrentStudent()
        {
            var currentStudent = _studentService.GetCurrentStudent(CurrentUser);
            
            return currentStudent;
        }
        #endregion

        [PageTrackingFilter]
        public ActionResult NoCeremony()
        {
            return View();
        }
    }



    public class RegistrationConfirmationViewModel
    {
        public Registration Registration { get; set; }
        public List<RegistrationPetition> RegistrationPetitions { get; set; }
    }
}
