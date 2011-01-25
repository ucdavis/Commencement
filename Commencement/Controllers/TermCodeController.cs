using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib;
using UCDArch.Web.Helpers;

namespace Commencement.Controllers
{
    [AdminOnly]
    public class TermCodeController : ApplicationController
    {
        //
        // GET: /TermCode/

        public ActionResult Index()
        {
            var viewModel = TermcodeViewModel.Create(Repository);
            return View(viewModel.AllTermCodes);
        }

        public ActionResult Details(string termCodeId)
        {
            var termCode = Repository.OfType<TermCode>().Queryable.Where(a => a.Id == termCodeId).Single();
            return View(termCode);
        }

        /// <summary>
        /// Adds the vTermCode to TermCodes, then it should redirect to the edit.
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(string termCodeId)
        {
            var vTermCode = Repository.OfType<vTermCode>().Queryable.Where(a => a.Id == termCodeId).Single();
            var termCode = new TermCode(vTermCode);
            termCode.IsActive = false;
            Repository.OfType<TermCode>().EnsurePersistent(termCode);

            return this.RedirectToAction(a => a.Edit(termCode.Id));
        }

        /// <summary>
        /// Activates the current TermCode, deactivates all others.
        /// </summary>
        /// <returns></returns>
        public ActionResult Activate(string termCodeId)
        {
            var termCode = Repository.OfType<TermCode>().Queryable.Where(a => a.Id == termCodeId).Single();
            var termCodes = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive);
            foreach (var code in termCodes)
            {
                code.IsActive = false;
                Repository.OfType<TermCode>().EnsurePersistent(code);
            }
            termCode.IsActive = true;
            Repository.OfType<TermCode>().EnsurePersistent(termCode);
            return this.RedirectToAction(a => a.Index());
        }


        //
        // GET: /TermCode/Edit/5
        /// <summary>
        /// Allow editing of some fields
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string termCodeId)
        {
            var termCode = Repository.OfType<TermCode>().Queryable.Where(a => a.Id == termCodeId).Single();

            return View(termCode);
        }

        //
        // POST: /TermCode/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, TermCode termCode)
        {
            var termCodeToUpdate = Repository.OfType<TermCode>().Queryable.Where(a => a.Id == id).Single();
            termCodeToUpdate.LandingText = termCode.LandingText;
            termCodeToUpdate.RegistrationWelcome = termCode.RegistrationWelcome;

            termCodeToUpdate.TransferValidationMessagesTo(ModelState);
            if (ModelState.IsValid)
            {
                Repository.OfType<TermCode>().EnsurePersistent(termCodeToUpdate);
                Message = id + " Term Code Saved.";
                return this.RedirectToAction(a => a.Index());
            }
            Message = id + " Unable to save.";
            return View(termCode);
        }

    }
}
