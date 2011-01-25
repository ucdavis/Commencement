using System;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;

namespace Commencement.Controllers
{
    public class EmailQueueController : SuperController
    {
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        private readonly ICeremonyService _ceremonyService;


        public EmailQueueController(IRepository<EmailQueue> emailQueueRepository, ICeremonyService ceremonyService)
        {
            _emailQueueRepository = emailQueueRepository;
            _ceremonyService = ceremonyService;
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
            catch (Exception ex)
            {
#if debug
                throw;
#else
                return Json(false);
#endif

            }

            return Json(true);
        }

    }
}
