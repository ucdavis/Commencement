using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using MvcContrib;
using UCDArch.Web.Validator;

namespace Commencement.Controllers
{
    [AdminOnly]
    public class SpecialNeedsController : ApplicationController
    {
        //
        // GET: /SpecialNeeds/

        public ActionResult Index()
        {
            var specialNeeds = Repository.OfType<SpecialNeed>().GetAll();
            return View(specialNeeds);
        }


        //
        // GET: /SpecialNeeds/Create

        public ActionResult Create()
        {
            return View(new SpecialNeed());
        } 

        //
        // POST: /SpecialNeeds/Create

        [HttpPost]
        public ActionResult Create(SpecialNeed specialNeed)
        {
            MvcValidationAdapter.TransferValidationMessagesTo(ModelState, specialNeed.ValidationResults());


            if (ModelState.IsValid)
            {
                Repository.OfType<SpecialNeed>().EnsurePersistent(specialNeed);
                Message = "Special Need Created";
                return this.RedirectToAction(a => a.Index());
            }
            Message = "Special Need Not Created";
            return View(specialNeed);
        }

        public ActionResult ToggleActive(int id)
        {
            var specialNeed = Repository.OfType<SpecialNeed>().GetById(id);
            specialNeed.IsActive = !specialNeed.IsActive;
            Repository.OfType<SpecialNeed>().EnsurePersistent(specialNeed);
            return this.RedirectToAction(a => a.Index());
        }
    }
}
