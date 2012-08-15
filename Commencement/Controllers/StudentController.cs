using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
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
            var redirect = CheckStudentForRegistration();
            if (redirect != null) return redirect;

            // redirect the student to the register page
            return this.RedirectToAction(a => a.Register());
        }

        /// <summary>
        /// #3
        /// </summary>
        /// <returns></returns>
        [PageTrackingFilter]
        public ActionResult Register()
        {
            var student = _studentService.GetCurrentStudent(CurrentUser);

            var redirect = CheckStudentForRegistration();
            if (redirect != null) return redirect;

            var viewModel = RegistrationModel.Create(Repository, GetEligibleCeremonies(student), student);
            return View(viewModel);
        }

        /// <summary>
        /// #4
        /// </summary>
        /// <param name="registrationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegistrationPostModel registrationModel)
        {
            var student = GetCurrentStudent();

            if (student == null) return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null, null));

            // validate they can register, also checks for duplicate registrations
            var redirect = CheckStudentForRegistration();
            if (redirect != null) return redirect;

            var registration = _registrationPopulator.PopulateRegistration(registrationModel, student, ModelState);

            registration.TransferValidationMessagesTo(ModelState);

            if (!registrationModel.AgreeToDisclaimer) ModelState.AddModelError("agreeToDisclaimer", StaticValues.Student_agree_to_disclaimer);
            if (registration.RegistrationPetitions.Any(a=>string.IsNullOrWhiteSpace(a.ExceptionReason))) ModelState.AddModelError("Exception Reason", "Exception reason is required.");

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
                        Message += StaticValues.Student_Register_Successful;
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
                        Message += StaticValues.Student_RegistrationPetition_Successful;
                    }
                    
                }

                //// successful registration
                //// redirect to exit survey, change requested by francesca on 8/16/2011
                //var url = ConfigurationManager.AppSettings["ExitSurvey"];

                //// if specified in web config, redirect to exit survey
                //if (!string.IsNullOrWhiteSpace(url))
                //{
                //    return Redirect(url);
                //}

                // exist survey not specified, just display the registration
                return this.RedirectToAction(a => a.DisplayRegistration());
            }

            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: GetEligibleCeremonies(student), student: student, ceremonyParticipations: registrationModel.CeremonyParticipations, registration: registration);
            return View(viewModel);
        }

        /// <summary>
        /// #5
        /// </summary>
        /// <returns></returns>
        [PageTrackingFilter]
        public ActionResult DisplayRegistration()
        {
            var student = GetCurrentStudent();
            if (student == null)
            {
                Message = StaticValues.Error_StudentNotFound;
                return this.RedirectToAction(a => a.Index());
            }
            var registration = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == student);
            //var petitions = _registrationPetitionRepository.Queryable.Where(a => a.Registration.Student == GetCurrentStudent()).ToList();

            // must have either registration or at least one petition
            if (registration == null) return this.RedirectToAction(a => a.Index());

            var viewModel = StudentDisplayRegistrationViewModel.Create(Repository, registration);

            return View(viewModel);
        }

        /// <summary>
        /// #6
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: GetEligibleCeremonies(student), student: student, registration: registration, edit: true);            
            
            return View(viewModel);
        }

        /// <summary>
        /// #7
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registrationPostModel"></param>
        /// <returns></returns>
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

            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: GetEligibleCeremonies(student), student: student, registration: registrationToEdit, edit: true);            
            return View(viewModel);
        }

        #region Helper Methods
        /// <summary>
        /// Does initial checks so that students are always redirected correctly for first time registration
        /// </summary>
        /// <param name="student"></param>
        /// <param name="termCode"></param>
        /// <returns>Redirect instruction, if null, student is valid to register</returns>
        private RedirectToRouteResult CheckStudentForRegistration()
        {
            var termCode = TermService.GetCurrent();
            var student = _studentService.GetCurrentStudent(CurrentUser);

            // unable to find record for some reason, either from download or banner lookup
            if (student == null || student.Blocked)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotEligible());
            }

            // no active term, or current term's reg is not open, includes 3 day grace period
            if (termCode == null && (DateTime.Now.Date < termCode.RegistrationBegin.Date || DateTime.Now.Date > termCode.RegistrationDeadline.Date.AddDays(7)))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotOpen());
            }

            // student is blocked becuase of sja
            if (student.SjaBlock)
            {
                return this.RedirectToAction<ErrorController>(a => a.SJA());
            }

            // check for a current registration, there should only be one
            var currentReg = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == student && a.TermCode.Id == termCode.Id);

            // has this student registered yet?
            if (currentReg != null)
            {
                // display previous registration
                return this.RedirectToAction(a => a.DisplayRegistration());
            }

            // load all part participations that were never cancelled or blocked
            var participations = _participationRepository.Queryable.Where(a => a.Registration.Student == student && !a.Cancelled && !a.Registration.Student.SjaBlock && !a.Registration.Student.Blocked).ToList();

            // get the list of all colleges for the student, that the student has walked for
            var pastColleges = participations.Where(a => a.Registration.TermCode.Id != termCode.Id).Select(a => a.Major.College).Distinct().ToList();
            
            // all current colleges match those of previously walked
            if (student.Majors.All(a => pastColleges.Contains(a.College)) && pastColleges.Count > 0)
            {
                // redirect to past registration message
                return this.RedirectToAction<ErrorController>(a => a.PreviouslyWalked());
            }

            // see if student can register with this system
            var eligibleCeremonies = GetEligibleCeremonies(student);
            if (eligibleCeremonies == null || eligibleCeremonies.Count == 0)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotEligible());
            }

            return null;
        }

        private List<Ceremony> GetEligibleCeremonies(Student student)
        {
            var ceremonyIdOverride = student.Ceremony != null ? (int?)student.Ceremony.Id : null;
            return _ceremonyService.StudentEligibility(majors: student.Majors.ToList(), totalUnits: student.TotalUnits,
                                                       ceremonyIdOverride: ceremonyIdOverride);
        }

        private Student GetCurrentStudent()
        {
            var currentStudent = _studentService.GetCurrentStudent(CurrentUser);
            
            return currentStudent;
        }
        #endregion
    }

    public class RegistrationConfirmationViewModel
    {
        public Registration Registration { get; set; }
        public List<RegistrationPetition> RegistrationPetitions { get; set; }
    }
}
