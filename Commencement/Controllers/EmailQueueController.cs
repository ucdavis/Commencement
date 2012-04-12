using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;

namespace Commencement.Controllers
{
    public class EmailQueueController : SuperController
    {
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        private readonly ICeremonyService _ceremonyService;
        private readonly ILetterGenerator _letterGenerator;

        public EmailQueueController(IRepository<EmailQueue> emailQueueRepository, ICeremonyService ceremonyService, ILetterGenerator letterGenerator)
        {
            _emailQueueRepository = emailQueueRepository;
            _ceremonyService = ceremonyService;
            _letterGenerator = letterGenerator;
        }

        //
        // GET: /EmailQueue/

        public ActionResult Index(bool showAll = false)
        {
            var ceremonies = _ceremonyService.GetCeremonies(CurrentUser.Identity.Name);
            var queue = _emailQueueRepository.Queryable.Where(a => (ceremonies.Contains(a.RegistrationParticipation.Ceremony) 
                                                                || ceremonies.Contains(a.RegistrationPetition.Ceremony)));

            if (!showAll) queue = queue.Where(a => a.Pending);

            return View(queue);
        }

        [HttpPost]
        public JsonResult Cancel(int id)
        {
            var emailQueue = _emailQueueRepository.GetNullableById(id);
            if (emailQueue == null) return Json(false);

            try
            {
                emailQueue.Pending = false;
                _emailQueueRepository.EnsurePersistent(emailQueue);
            }
            catch 
            {
#if debug
                throw;
#else
                return Json(false);
#endif

            }

            return Json(true);
        }

        public ActionResult EmailStudents()
        {
            var viewModel = EmailStudentsViewModel.Create(Repository, _ceremonyService, CurrentUser.Identity.Name);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult EmailStudents(Ceremony ceremony, string subject, string body)
        public ActionResult EmailStudents(EmailStudentsViewModel emailStudents)
        {
            // get the template type
            var templateType = Repository.OfType<TemplateType>().Queryable.Where(a => a.Name == StaticValues.Template_EmailAllStudents).FirstOrDefault();

            if (templateType == null)
            {
                Message = "Invalid template type, please have the database checked.";
                return RedirectToAction("Index");
            }

            if (emailStudents.Ceremony == null)
            {
                ModelState.AddModelError("Ceremony", "Ceremony is required.");
            }
            if (string.IsNullOrWhiteSpace(emailStudents.Subject))
            {
                ModelState.AddModelError("Subject", "Subject is required");
            }
            if (string.IsNullOrWhiteSpace(emailStudents.Body))
            {
                ModelState.AddModelError("Body", "Body is required.");
            }

            if (ModelState.IsValid)
            {
                foreach (var participation in emailStudents.Ceremony.RegistrationParticipations)
                {
                    var bodyText = _letterGenerator.GenerateEmailAllStudents(participation, emailStudents.Body, templateType);

                    var eq = new EmailQueue(participation.Registration.Student, null, emailStudents.Subject, bodyText, false);
                    eq.Registration = participation.Registration;
                    eq.RegistrationParticipation = participation;
                    Repository.OfType<EmailQueue>().EnsurePersistent(eq);
                }

                Message = "Emails have been queued.";
                return RedirectToAction("Index");    
            }

            var viewModel = EmailStudentsViewModel.Create(Repository, _ceremonyService, CurrentUser.Identity.Name);
            viewModel.Ceremony = emailStudents.Ceremony;
            viewModel.Subject = emailStudents.Subject;
            viewModel.Body = emailStudents.Body;
            return View(viewModel);
        }

    }
}
