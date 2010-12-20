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
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;

        public StudentController(IStudentService studentService, 
            IEmailService emailService,
            IRepositoryWithTypedId<Student, Guid> studentRepository, 
            IRepository<Ceremony> ceremonyRepository, 
            IRepository<Registration> registrationRepository,
            IErrorService errorService,
            ICeremonyService ceremonyService, IRepository<RegistrationPetition> registrationPetitionRepository,
            IRepository<RegistrationParticipation> participationRepository)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
            _errorService = errorService;
            _ceremonyService = ceremonyService;
            _registrationPetitionRepository = registrationPetitionRepository;
            _participationRepository = participationRepository;
            _studentService = studentService;
            _emailService = emailService;
        }

        /// <summary>
        /// Loads a landing page
        /// </summary>
        /// <returns></returns>
        [PageTrackingFilter]
        public ActionResult Index()
        {
            return View(TermService.GetCurrent());
        }

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
            // setup variables to be used for this method
            var ceremonyParticipations = registrationModel.CeremonyParticipations;
            var specialNeeds = registrationModel.SpecialNeeds;
            var term = TermService.GetCurrent();
            var student = GetCurrentStudent();

            var registration = registrationModel.Registration;
            registration.TermCode = term;
            registration.Student = student;
            NullOutBlankFields(registration);
            registration.SpecialNeeds = LoadSpecialNeeds(specialNeeds);
            registration.GradTrack = registrationModel.GradTrack;

            // validate they can register, also checks for duplicate registrations
            var redirect = CheckStudentForRegistration(student, term);
            if (redirect != null) return redirect;

            ValidateCeremonyParticipations(ceremonyParticipations, ModelState);
            AddCeremonyParticipations(registration, ceremonyParticipations, ModelState);
            AddRegistrationPetitions(registration, ceremonyParticipations, ModelState);

            registration.TransferValidationMessagesTo(ModelState);

            if (!registrationModel.AgreeToDisclaimer) ModelState.AddModelError("agreeToDisclaimer", StaticValues.Student_agree_to_disclaimer);

            if (ModelState.IsValid)
            {
                if (registration.RegistrationParticipations.Count > 0 || registration.RegistrationPetitions.Count > 0)
                {
                    // save registration
                    _registrationRepository.EnsurePersistent(registration);

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

                    // put message up for student
                    Message += StaticValues.Student_Register_Successful;
                }

                return this.RedirectToAction(a => a.DisplayRegistration());
            }

            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student: student, ceremonyParticipations: ceremonyParticipations, registration: registration);
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

        // getting removed
        //[PageTrackingFilter]
        ////public ActionResult RegistrationConfirmation(int id /* registration id */)
        //public ActionResult RegistrationConfirmation(Guid id /* Student's Id */)
        //{
        //    var registration = _registrationRepository.GetNullableById();
            

        //    var petitions = new List<RegistrationPetition>();
        //    foreach (var a in petitionIds)
        //    {
        //        var p = _registrationPetitionRepository.GetNullableById(a);
        //        if (p != null) petitions.Add(p);
        //    }

        //    if (registration == null && petitions.Count <= 0) return this.RedirectToAction(x => x.Index());
        //    if (registration.Student != _studentService.GetCurrentStudent(CurrentUser)) return this.RedirectToAction<ErrorController>(a => a.UnauthorizedAccess());

        //    return View(new RegistrationConfirmationViewModel{Registration = registration, RegistrationPetitions = petitions});
        //}

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
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            }

            //Get student info and create registration model
            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList(), student.TotalUnits), student: student, registration: registration, edit: true);            
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditRegistration(int id /* Registration Id */, RegistrationPostModel registrationPostModel)
        {
            //var registration = _registrationRepository.GetNullableById(id);
            //var student = GetCurrentStudent();

            //var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: _ceremonyService.StudentEligibility(student.Majors.ToList()), student: student, ceremonyParticipations:registrationPostModel.CeremonyParticipations, edit: true);
            //return View(viewModel);

            ///// testing stuff above

            var registrationToEdit = _registrationRepository.GetNullableById(id);
            var student = GetCurrentStudent();

            if (registrationToEdit == null || registrationToEdit.Student != student)
            {
                Message = StaticValues.Student_No_Registration_Found;
                return this.RedirectToAction(a => a.Index());
            }
            if (!registrationToEdit.RegistrationParticipations.Any(a => a.Ceremony.CanRegister()))
            {
                return this.RedirectToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            }

            CopyHelper.CopyRegistrationValues(registrationPostModel.Registration, registrationToEdit);
            NullOutBlankFields(registrationToEdit);
            UpdateCeremonyParticipations(registrationToEdit, registrationPostModel.CeremonyParticipations, ModelState);
            AddRegistrationPetitions(registrationToEdit, registrationPostModel.CeremonyParticipations, ModelState);

            registrationToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                //Save the registration
                _registrationRepository.EnsurePersistent(registrationToEdit);

                Message = StaticValues.Student_Register_Edit_Successful;

                try
                {
                    _emailService.SendRegistrationConfirmation(registrationToEdit);
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
        /// Validates that the ceremonies the student has decided upon are all valid to be registered for together
        /// </summary>
        /// <remarks>
        /// Rules are:
        ///     Student can register once per ceremony
        ///     Student can register once per college
        ///     Student can register for multiple ceremonies for different colleges
        /// </remarks>
        /// <param name="registration"></param>
        /// <param name="ceremonyParticipations"></param>
        private void ValidateCeremonyParticipations(List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState)
        {
            // count distinct ceremonies that student has selected
            var ceremonyCount = ceremonyParticipations.Where(a => a.Participate || a.Cancel).Select(a => a.Ceremony).Distinct();
            var petitionCount = ceremonyParticipations.Where(a => a.Petition).Select(a => a.Ceremony).Distinct();

            if (ceremonyCount.Count() == 0 && petitionCount.Count() == 0)
            {
                modelState.AddModelError("Participate", "You have to select one or more ceremonies to participate.");
            }

            // if # participating != distinct, then we have someone registering more than once for the same ceremony
            if (ceremonyParticipations.Where(a => a.Participate || a.Petition).Count() > ceremonyCount.Count() + petitionCount.Count())
            {
                modelState.AddModelError("Participate", "You cannot register for two majors within the same ceremony.");
            }

            // count distinct colleges
            var collegeCount = ceremonyParticipations.Where(a => a.Participate || a.Petition).Select(a => a.Major.MajorCollege).Distinct();
            // if # participating != distinct, then we have someone registering for more than on ceremony in one college
            if (ceremonyParticipations.Where(a => a.Participate || a.Petition).Count() > collegeCount.Count())
            {
                modelState.AddModelError("Participate", "You cannot register for two ceremonies within the same college.");
            }
        }

        private void AddCeremonyParticipations(Registration registration, List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState)
        {
            ValidateCeremonyParticipations(ceremonyParticipations, modelState);

            foreach (var a in ceremonyParticipations)
            {
                if (a.Participate) registration.AddParticipation(a.Major, a.Ceremony, a.Tickets);
            }
        }

        private void UpdateCeremonyParticipations(Registration registration, List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState)
        {
            ValidateCeremonyParticipations(ceremonyParticipations, modelState);

            foreach (var a in ceremonyParticipations)
            {
                var rp = registration.RegistrationParticipations.Where(b => b.Major == a.Major && b.Ceremony == a.Ceremony).FirstOrDefault();
                if (rp == null && a.Participate && !a.Cancel) registration.AddParticipation(a.Major, a.Ceremony, a.Tickets);
                else
                {
                    rp.Cancelled = a.Participate ? !a.Participate : a.Cancel;
                    rp.DateUpdated = DateTime.Now;
                    rp.NumberTickets = a.Tickets;
                }
            }
        }

        private List<SpecialNeed> LoadSpecialNeeds(List<string> specialNeeds)
        {
            if (specialNeeds == null) return new List<SpecialNeed>();

            var needs = new List<int>();
            foreach(var a in specialNeeds) needs.Add(Convert.ToInt32(a));
            return Repository.OfType<SpecialNeed>().Queryable.Where(a => needs.Contains(a.Id)).ToList();
        }

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
                return this.RedirectToAction<ErrorController>(a => a.RegisterOtherSystem());
            }

            // see if registration is open for at least one ceremony
            if (!eligibleCeremonies.Where(a => a.RegistrationBegin < DateTime.Now).Any())
            {
                return this.RedirectToAction<ErrorController>(a => a.NotOpen());
            }

            return null;
        }

        private void NullOutBlankFields(Registration registration)
        {
            registration.Address2 = registration.Address2.IsNullOrEmpty(true) ? null : registration.Address2;
            registration.Email = registration.Email.IsNullOrEmpty(true) ? null : registration.Email;
        }

        private Student GetCurrentStudent()
        {
            var currentStudent = _studentService.GetCurrentStudent(CurrentUser);

            Check.Require(currentStudent != null, "Current user is not a student or student not found.");
            
            return currentStudent;
        }

        private void AddRegistrationPetitions(Registration registration, List<CeremonyParticipation> ceremonyParticipations, ModelStateDictionary modelState)
        {
            //var petitions = new List<RegistrationPetition>();

            foreach (var a in ceremonyParticipations.Where(a=>a.Petition))
            {
                // check for existing petition
                var existingPetition = !_registrationPetitionRepository.Queryable.Any(b => b.Ceremony == a.Ceremony && b.Registration.Student == registration.Student);
                // check for existing registration
                var existingParticipation = !_participationRepository.Queryable.Any(b => b.Ceremony == a.Ceremony && b.Registration.Student == registration.Student);

                if (existingPetition && existingParticipation)
                {
                    var petition = new RegistrationPetition(registration, a.Major, a.Ceremony, a.PetitionReason, a.CompletionTerm, a.Tickets);
                    petition.TransferUnitsFrom = string.IsNullOrEmpty(a.TransferCollege) ? null : a.TransferCollege;
                    petition.TransferUnits = string.IsNullOrEmpty(a.TransferUnits) ? null : a.TransferUnits;

                    registration.AddPetition(petition);
                }
            }

            //return petitions;
        }
        #endregion

        [PageTrackingFilter]
        public ActionResult NoCeremony()
        {
            return View();
        }
    }

    public class RegistrationPostModel
    {
        public Registration Registration { get; set; }
        public List<CeremonyParticipation> CeremonyParticipations { get; set; }
        public List<string> SpecialNeeds { get; set; }
        public bool AgreeToDisclaimer { get; set; }
        public bool GradTrack { get; set; }
    }

    public class RegistrationConfirmationViewModel
    {
        public Registration Registration { get; set; }
        public List<RegistrationPetition> RegistrationPetitions { get; set; }
    }
}
