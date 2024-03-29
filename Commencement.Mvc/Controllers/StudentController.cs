using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Commencement.Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;
using Commencement.Controllers.Helpers;
using Commencement.Core.Helpers;
using MvcContrib;
using Serilog;

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
        private readonly IReportService _reportService;
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
            ICeremonyService ceremonyService, 
            IReportService reportService,
            IRepository<RegistrationPetition> registrationPetitionRepository,
            IRepository<RegistrationParticipation> participationRepository, IRegistrationPopulator registrationPopulator)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
            _errorService = errorService;
            _ceremonyService = ceremonyService;
            _reportService = reportService;
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
            ModelState.Clear();

            var student = GetCurrentStudent();

            if (student == null) return this.RedirectToAction<AdminController>(a => a.Students(null, null, null, null, null));

            // validate they can register, also checks for duplicate registrations
            var redirect = CheckStudentForRegistration();
            if (redirect != null) return redirect;

            var registration = _registrationPopulator.PopulateRegistration(registrationModel, student, ModelState);

            registration.TransferValidationMessagesTo(ModelState);

            if (!registrationModel.AgreeToDisclaimer) ModelState.AddModelError("agreeToDisclaimer", StaticValues.Student_agree_to_disclaimer);
            if (registration.RegistrationPetitions.Any(a=>string.IsNullOrWhiteSpace(a.ExceptionReason))) ModelState.AddModelError("Exception Reason", "Exception/Petition reason is required.");

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
                            Log.Error(ex, "Error creating email queue for Student Controller QueueRegistrationConfirmation: {ex}", ex);
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
                            Log.Error(ex, "Error creating email queue for Student Controller QueueRegistrationPetition: {ex}", ex);
                            _errorService.ReportError(ex); //This will generate an email to me which is good as this is serious...
                            Message += StaticValues.Student_Email_Problem;
                        }
                        Message += StaticValues.Student_RegistrationPetition_Successful;
                    }

                    // redirect to exit survey if needed
                    var surveyRedirect = SurveyRedirector(registration);
                    if (surveyRedirect != null) return surveyRedirect;
                }

                // exit survey not specified, just display the registration
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

            // redirect to exit survey if outstanding
            var surveyRedirect = SurveyRedirector(registration);
            if (surveyRedirect != null) return surveyRedirect;

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
            ModelState.Clear();
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

            //TODO: Should probably check if this is changed to a petition and if so, required fields are completed (Look at Register above)

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
                    Log.Error(ex, "Error creating email queue for Student Controller QueueRegistrationConfirmation: {ex}", ex);
                    _errorService.ReportError(ex);
                    Message += StaticValues.Student_Email_Problem;
                }

                //return this.RedirectToAction(x => x.RegistrationConfirmation(registrationToEdit.Id, null));
                return this.RedirectToAction(a => a.DisplayRegistration());
            }

            var viewModel = RegistrationModel.Create(repository: Repository, ceremonies: GetEligibleCeremonies(student), student: student, registration: registrationToEdit, edit: true);            
            return View(viewModel);
        }

        [PageTrackingFilter]
        public ActionResult CancelRegistrationPetition(int id)
        {
            var regPetition = Repository.OfType<RegistrationPetition>().GetNullableById(id);

            if (regPetition == null)
            {
                Message = "Unable to find registration petition, please try again.";
                return RedirectToAction("DisplayRegistration");
            }

            if (regPetition.Registration.Student.Login != User.Identity.Name)
            {
                return RedirectToAction("UnauthorizedAccess", "Error");
            }

            return View(regPetition);
        }
   
        [HttpPost]
        public ActionResult CancelRegistrationPetition(int id, bool? cancel)
        {
            var regPetition = Repository.OfType<RegistrationPetition>().GetNullableById(id);

            if (regPetition == null)
            {
                Message = "Unable to find registration petition, please try again.";
                return RedirectToAction("DisplayRegistration");
            }

            if (regPetition.Registration.Student.Login != User.Identity.Name)
            {
                return RedirectToAction("UnauthorizedAccess", "Error");
            }

            if (cancel.HasValue && cancel.Value)
            {
                var emailQueue = Repository.OfType<EmailQueue>().Queryable.Where(a => a.RegistrationPetition == regPetition).ToList();
                foreach (var queue in emailQueue)
                {
                    Repository.OfType<EmailQueue>().Remove(queue);
                }

                //Clear out any surveys they answered
                var registrationSurvey = Repository.OfType<RegistrationSurvey>().Queryable.Where(a => a.RegistrationPetition == regPetition).ToList();
                foreach (var survey in registrationSurvey)
                {
                    Repository.OfType<RegistrationSurvey>().Remove(survey);
                }

                Repository.OfType<RegistrationPetition>().Remove(regPetition);

                // check if we can delete the registration object
                if (!regPetition.Registration.RegistrationParticipations.Any() &&
                    !regPetition.Registration.RegistrationPetitions.Any())
                {
                    Repository.OfType<Registration>().Remove(regPetition.Registration);
                }

                Message = "Petition was successfully deleted.";
                return RedirectToAction("CancelRegistrationPetitionConfirm");
            }

            Message = "Petition was not deleted.";
            return RedirectToAction("DisplayRegistration");
        }

        [PageTrackingFilter]
        public ActionResult CancelRegistrationPetitionConfirm()
        {
            return View();
        }

        [PageTrackingFilter]
        public ActionResult VisaLetters()
        {
            // validate student is in our DB, otherwise we need to do a lookup
            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return this.RedirectToAction<ErrorController>(a => a.NotFound());

            var letters = Repository.OfType<VisaLetter>().Queryable.Where(a => a.Student.StudentId == student.StudentId && !a.IsCanceled).ToList();
            ViewBag.StudentName = student.FirstName;

            return View(letters);
        }

        [PageTrackingFilter]
        public ActionResult RequestVisaLetter()
        {
            // validate student is in our DB, otherwise we need to do a lookup
            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return this.RedirectToAction<ErrorController>(a => a.NotFound());

            var existingLetter = Repository.OfType<VisaLetter>().Queryable.FirstOrDefault(a => a.Student.StudentId == student.StudentId && !a.IsCanceled && !a.IsDenied);

            ViewBag.AllowChange = true;

            var letter = new VisaLetter();
            letter.Student = student;
            if (existingLetter != null)
            {
                letter.StudentFirstName = existingLetter.StudentFirstName;
                letter.StudentLastName = existingLetter.StudentLastName;
                letter.MajorName = existingLetter.MajorName;
                letter.CollegeCode = existingLetter.CollegeCode;
                letter.Gender = existingLetter.Gender;
                letter.CollegeName = existingLetter.CollegeName;
                letter.Degree = existingLetter.Degree;
                letter.HardCopy = existingLetter.HardCopy;
                letter.Ceremony = existingLetter.Ceremony;
                ViewBag.AllowChange = false;
                ViewBag.AllowCeremonyChange = false;
            }
            else
            {
                letter.StudentFirstName = student.FirstName;
                letter.StudentLastName = student.LastName;
                var major = student.Majors.FirstOrDefault();
                if (major != null)
                {
                    letter.MajorName = major.MajorName;
                    letter.CollegeCode = major.College.Id;
                }
            }


            var checkStudent = CheckStudentForVisaLetter();
            switch (checkStudent)
            {
                case "NotEligible":
                    return this.RedirectToAction<ErrorController>(a => a.NotEligible()); //Blocked
                case "SJA":
                    return this.RedirectToAction<ErrorController>(a => a.SJA());
                case "S":
                    letter.Ceremony = 'S';
                    ViewBag.AllowCeremonyChange = false;
                    break;
                case "F":
                    letter.Ceremony = 'F';
                    ViewBag.AllowCeremonyChange = false;
                    break;
                case "PreviouslyWalked":
                    return this.RedirectToAction<ErrorController>(a => a.PreviouslyWalked());
                case "CeremonyOver":
                    return this.RedirectToAction<ErrorController>(a => a.CeremonyOver());

            }            

            

            return View(letter);
        }
        
        [HttpPost]
        [PageTrackingFilter]
        public ActionResult RequestVisaLetter(VisaLetterPostModel model)
        {
            // validate student is in our DB, otherwise we need to do a lookup
            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return this.RedirectToAction<ErrorController>(a => a.NotFound());

            var existingLetter = Repository.OfType<VisaLetter>().Queryable.FirstOrDefault(a => a.Student.StudentId == student.StudentId && !a.IsCanceled && !a.IsDenied);
            if (existingLetter != null)
            {
                ViewBag.AllowChange = false;
            }
            else
            {
                ViewBag.AllowChange = true;
            }

            var visaLetter = new VisaLetter();
            visaLetter.Student = student;
            visaLetter.Ceremony = model.Ceremony;
            visaLetter.MajorName = model.MajorName;
            visaLetter.RelationshipToStudent = model.RelationshipToStudent;
            visaLetter.RelativeFirstName = model.RelativeFirstName;
            visaLetter.RelativeLastName = model.RelativeLastName;
            visaLetter.RelativeMailingAddress = model.RelativeMailingAddress;
            visaLetter.RelativeTitle = model.RelativeTitle;
            visaLetter.StudentFirstName = model.StudentFirstName;
            visaLetter.StudentLastName = model.StudentLastName;
            visaLetter.Gender = model.Gender;
            visaLetter.CollegeCode = model.CollegeCode;
            if (!string.IsNullOrWhiteSpace(visaLetter.CollegeCode))
            {
                visaLetter.CollegeName = SelectLists.CollegeNames.Single(a => a.Value == visaLetter.CollegeCode).Text;
            }            
            visaLetter.Degree = model.Degree;
            visaLetter.HardCopy = "N";

            var termCode = TermService.GetCurrent();
            var currentReg = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == student && a.TermCode.Id == termCode.Id);

            // has this student registered yet?
            if (currentReg != null)
            {
                // display previous registration
                var participation = currentReg.RegistrationParticipations.FirstOrDefault(a => !a.Cancelled && !a.Registration.Student.SjaBlock && !a.Registration.Student.Blocked);
                if (participation != null && participation.Ceremony.Colleges.Any(a => a.Id == visaLetter.CollegeCode))
                {
                    visaLetter.CeremonyDateTime = participation.Ceremony.DateTime;
                }

            }

            visaLetter.TransferValidationMessagesTo(ModelState);
            if (ModelState.IsValid)
            {
                //TODO: Try and pull to see if student is registered. If so, set values for which ceremony and which date.


                Repository.OfType<VisaLetter>().EnsurePersistent(visaLetter);
                Message = "Request Created";
              
                return this.RedirectToAction("VisaLetterReceipt");
            }

            Message = "Please correct errors and try again.";
            return View(visaLetter);
        }

        [PageTrackingFilter]
        public ActionResult CancelVisaLetterRequest(int id)
        {
            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return this.RedirectToAction<ErrorController>(a => a.NotFound());

            var letter = Repository.OfType<VisaLetter>().Queryable.SingleOrDefault(a => a.Id == id && a.Student.StudentId == student.StudentId && a.IsPending && !a.IsCanceled); //Only allow that student to print it.
            if (letter == null)
            {
                Message = "Pending Letter Not Found";
                return this.RedirectToAction<ErrorController>(a => a.NotFound());
            }

            return View(letter);
        }

        [HttpPost]
        [PageTrackingFilter]
        public ActionResult CancelVisaLetterRequest(int id, bool? cancel)
        {
            if (!cancel.HasValue || !cancel.Value)
            {
                Message = "Visa Letter Request Not Canceled";
                return this.RedirectToAction(a => a.VisaLetters());
            }

            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return this.RedirectToAction<ErrorController>(a => a.NotFound());

            var letter = Repository.OfType<VisaLetter>().Queryable.SingleOrDefault(a => a.Id == id && a.Student.StudentId == student.StudentId && a.IsPending && !a.IsCanceled); //Only allow that student to print it.
            if (letter == null)
            {
                Message = "Pending Letter Not Found";
                return this.RedirectToAction<ErrorController>(a => a.NotFound());
            }
            letter.IsCanceled = true;

            Repository.OfType<VisaLetter>().EnsurePersistent(letter);
            Message = "Visa Letter Request Canceled";
            return this.RedirectToAction(a => a.VisaLetters());
        }


        public ActionResult VisaLetterDetails(int id)
        {
            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return this.RedirectToAction<ErrorController>(a => a.NotFound());

            var letter = Repository.OfType<VisaLetter>().Queryable.SingleOrDefault(a => a.Id == id && a.Student.StudentId == student.StudentId); //Only allow that student to print it.
            if (letter == null)
            {
                Message = "Letter Not Found";
                return this.RedirectToAction<ErrorController>(a => a.NotFound());
            }

            return View(letter);
        }

        public ActionResult VisaLetterReceipt()
        {
            return View();
        }

        public FileResult VisaLetterPdf(int id)
        {
            // validate student is in our DB, otherwise we need to do a lookup
            var student = GetCurrentStudent();

            // we were just unable to find record
            if (student == null) return File(_reportService.WritePdfWithErrorMessage("Student Record Not Found"), "application/pdf");

            var letter = Repository.OfType<VisaLetter>().Queryable.Single(a => a.Id == id && a.Student.StudentId == student.StudentId); //Only allow that student to print it.

            if (!letter.IsApproved || letter.IsPending)
            {
                Message = "Approved Letter Not Found";
                return File(_reportService.WritePdfWithErrorMessage(Message), "application/pdf");
                //return this.RedirectToAction<ErrorController>(a => a.NotFound());
            }

            return File(_reportService.GenerateLetter(letter), "application/pdf");

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

            // student is blocked because of sja
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

            // no active term, or current term's reg is not open
            if (termCode == null || !termCode.CanRegister())
            {
                return this.RedirectToAction<ErrorController>(a => a.NotOpen());
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

        private string CheckStudentForVisaLetter()
        {
            var termCode = TermService.GetCurrent();
            var student = _studentService.GetCurrentStudent(CurrentUser);

            // unable to find record for some reason, either from download or banner lookup
            if (student == null || student.Blocked)
            {
                return "NotEligible";// this.RedirectToAction<ErrorController>(a => a.NotEligible());
            }

            // student is blocked becuase of sja
            if (student.SjaBlock)
            {
                return "SJA"; // this.RedirectToAction<ErrorController>(a => a.SJA());
            }

            // check for a current registration, there should only be one
            var currentReg = _registrationRepository.Queryable.SingleOrDefault(a => a.Student == student && a.TermCode.Id == termCode.Id);

            // has this student registered yet?
            if (currentReg != null)
            {
                // display previous registration
                var participation = currentReg.RegistrationParticipations.FirstOrDefault(a => !a.Cancelled && !a.Registration.Student.SjaBlock && !a.Registration.Student.Blocked);
                if (participation != null)
                {
                    if (participation.Ceremony.DateTime < DateTime.UtcNow.ToPacificTime())
                    {
                        return "CeremonyOver";
                    }
                    return termCode.Name[0].ToString();
                }
                
            }

            //// no active term, or current term's reg is not open
            //if (termCode == null || !termCode.CanRegister())
            //{
            //    return this.RedirectToAction<ErrorController>(a => a.NotOpen());
            //}

            // load all part participations that were never cancelled or blocked
            var participations = _participationRepository.Queryable.Where(a => a.Registration.Student == student && !a.Cancelled && !a.Registration.Student.SjaBlock && !a.Registration.Student.Blocked).ToList();

            // get the list of all colleges for the student, that the student has walked for
            var pastColleges = participations.Where(a => a.Registration.TermCode.Id != termCode.Id).Select(a => a.Major.College).Distinct().ToList();

            // all current colleges match those of previously walked
            if (student.Majors.All(a => pastColleges.Contains(a.College)) && pastColleges.Count > 0)
            {
                // redirect to past registration message
                return "PreviouslyWalked"; // this.RedirectToAction<ErrorController>(a => a.PreviouslyWalked());
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

        private ActionResult SurveyRedirector (Registration registration)
        {
            //Does the college have a survey for this student?
            var participations = registration.RegistrationParticipations.Where(a => !a.ExitSurvey && !a.Cancelled);
            foreach (var registrationParticipation in participations)
            {
                var college = registrationParticipation.Major.MajorCollege;
                var survey = registrationParticipation.Ceremony.CeremonySurveys.SingleOrDefault(a => a.College == college && (!string.IsNullOrWhiteSpace(a.SurveyUrl) || a.Survey != null));
                if (survey != null)
                {
                    if (!string.IsNullOrWhiteSpace(survey.SurveyUrl))
                    {
                        var url = survey.SurveyUrl;
                        registrationParticipation.ExitSurvey = true;
                        Repository.OfType<RegistrationParticipation>().EnsurePersistent(registrationParticipation);
                        return Redirect(url);
                    }
                    if (survey.Survey != null)
                    {
                        return RedirectToAction("Student", "Survey", new { id = survey.Survey.Id, participationId = registrationParticipation.Id });
                    }
                }
            }

            var petitions = registration.RegistrationPetitions.Where(a => !a.ExitSurvey && a.IsPending);
            foreach (var registrationPetition in petitions)
            {
                var college = registrationPetition.MajorCode.MajorCollege;
                var survey = registrationPetition.Ceremony.CeremonySurveys.SingleOrDefault(a => a.College == college && (!string.IsNullOrWhiteSpace(a.SurveyUrl) || a.Survey != null));
                if (survey != null)
                {
                    if (!string.IsNullOrWhiteSpace(survey.SurveyUrl))
                    {
                        var url = survey.SurveyUrl;
                        registrationPetition.ExitSurvey = true;
                        Repository.OfType<RegistrationPetition>().EnsurePersistent(registrationPetition);
                        return Redirect(url);
                    }

                    if (survey.Survey != null)
                    {
                        return RedirectToAction("Student", "Survey", new { id = survey.Survey.Id, petitionId = registrationPetition.Id });
                    }
                }
            }

            return null;

            //// participations where survey url or assigned survey is specified            
            //var participations = registration.RegistrationParticipations.Where(a => !a.ExitSurvey && (!string.IsNullOrEmpty(a.Ceremony.SurveyUrl) || a.Ceremony.Survey != null) && !a.Cancelled);

            //if (participations.Any())
            //{
            //    var p = participations.First();

            //    if (!string.IsNullOrEmpty(p.Ceremony.SurveyUrl))
            //    {
            //        var url = p.Ceremony.SurveyUrl;
            //        p.ExitSurvey = true;
            //        Repository.OfType<RegistrationParticipation>().EnsurePersistent(p);
            //        return Redirect(url);
            //    }
                
            //    if (p.Ceremony.Survey != null)
            //    {
            //        return RedirectToAction("Student", "Survey", new {id = p.Ceremony.Survey.Id, participationId = p.Id});
            //    }
            //}

            //return null;
        }
        #endregion
    }

    public class RegistrationConfirmationViewModel
    {
        public Registration Registration { get; set; }
        public List<RegistrationPetition> RegistrationPetitions { get; set; }
    }
}
