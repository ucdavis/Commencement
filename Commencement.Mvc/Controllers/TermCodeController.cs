using System.Linq;
using System.Web.Mvc;
using Commencement.MVC.Controllers.Filters;
using Commencement.MVC.Controllers.Services;
using Commencement.MVC.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib;
using UCDArch.Web.Helpers;

namespace Commencement.MVC.Controllers
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
            var termCode = Repository.OfType<TermCode>().Queryable.Single(a => a.Id == termCodeId);
            return View(termCode);
        }

        /// <summary>
        /// Adds the vTermCode to TermCodes, then it should redirect to the edit.
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(string termCodeId)
        {
            var vTermCode = Repository.OfType<vTermCode>().Queryable.Single(a => a.Id == termCodeId);
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
            var termCode = Repository.OfType<TermCode>().Queryable.Single(a => a.Id == termCodeId);
            var termCodes = Repository.OfType<TermCode>().Queryable.Where(a => a.IsActive);
            foreach (var code in termCodes)
            {
                code.IsActive = false;
                Repository.OfType<TermCode>().EnsurePersistent(code);
            }
            termCode.IsActive = true;
            Repository.OfType<TermCode>().EnsurePersistent(termCode);

            // invalidate the cache
            TermService.UpdateCurrent(termCode);

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
            var termCode = Repository.OfType<TermCode>().Queryable.Single(a => a.Id == termCodeId);

            return View(termCode);
        }

        //
        // POST: /TermCode/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(string id, TermCode termCode)
        {
            ModelState.Clear();
            var termCodeToUpdate = Repository.OfType<TermCode>().Queryable.Single(a => a.Id == id);

            termCodeToUpdate.LandingText = termCode.LandingText;
            termCodeToUpdate.RegistrationWelcome = termCode.RegistrationWelcome;

            // copy the dates
            termCodeToUpdate.RegistrationBegin = termCode.RegistrationBegin;
            termCodeToUpdate.RegistrationDeadline = termCode.RegistrationDeadline;
            termCodeToUpdate.CapAndGownDeadline = termCode.CapAndGownDeadline;
            termCodeToUpdate.FileToGraduateDeadline = termCode.FileToGraduateDeadline;
            termCodeToUpdate.RegistrationPetitionDeadline = termCode.RegistrationPetitionDeadline;

            termCodeToUpdate.TransferValidationMessagesTo(ModelState);
            if (ModelState.IsValid)
            {
                Repository.OfType<TermCode>().EnsurePersistent(termCodeToUpdate);
                Message = id + " Term Code Saved.";
                TermService.UpdateCurrent(termCodeToUpdate);
                return this.RedirectToAction(a => a.Index());
            }
            Message = id + " Unable to save.";
            return View(termCode);
        }

    }
}
