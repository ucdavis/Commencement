using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Filters;

namespace Commencement.Mvc.Controllers
{
    [AnyoneWithRole]
    public class RetroactiveController : ApplicationController
    {
        //
        // GET: /Retroactive/

        public ActionResult Index()
        {
            return View(new List<RegistrationParticipation>());
        }

        [HttpPost]
        public ActionResult Index(int? id, string firstname, string lastname)
        {
            var sid = id.ToString();

            ViewData["Id"] = sid;
            ViewData["FirstName"] = firstname;
            ViewData["LastName"] = lastname;

            if (!string.IsNullOrEmpty(sid))
            {
                var participations = Repository.OfType<RegistrationParticipation>().Queryable.Where(a => a.Registration.Student.StudentId == sid);
                return View(participations.ToList());
            }

            if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname))
            {
                var participations = Repository.OfType<RegistrationParticipation>().Queryable
                                               .Where(a =>
                                                      a.Registration.Student.FirstName.Contains(firstname)
                                                      &&
                                                      a.Registration.Student.LastName.Contains(lastname));
                return View(participations.ToList());
            }

            Message = "Please provide a search parameter either studentid or firstname and lastname.";
            return View(new List<RegistrationParticipation>());
        }

        public ActionResult Details(int id)
        {
            var reg = Repository.OfType<RegistrationParticipation>().GetById(id);
            return View(reg);
        }

        [HttpPost]
        public ActionResult Details(int id, string nothing)
        {
            var reg = Repository.OfType<RegistrationParticipation>().GetById(id);

            reg.Cancelled = true;
            Repository.OfType<RegistrationParticipation>().EnsurePersistent(reg);

            Message = string.Format("Registration for {0} has been cancelled.", reg.Registration.Student.FullName);
            return RedirectToAction("Index");
        }
    }
}
