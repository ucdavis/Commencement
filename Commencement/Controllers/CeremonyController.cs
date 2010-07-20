using System.Web.Mvc;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib.Attributes;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Commencement.Controllers
{
    public class CeremonyController : ApplicationController
    {
        //
        // GET: /Commencement/

        public ActionResult Index()
        {
            var viewModel = CommencementViewModel.Create(Repository);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = CreateCommencementViewModel.Create(Repository);

            return View(viewModel);
        }

        [AcceptPost]
        public ActionResult Create(Ceremony ceremony)
        {
            ceremony.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                // save
                Repository.OfType<Core.Domain.Ceremony>().EnsurePersistent(ceremony);

                // redirect to the list
                return this.RedirectToAction(a => a.Index());
            }

            // redirect back to the page
            var viewModel = CreateCommencementViewModel.Create(Repository);
            viewModel.Ceremony = ceremony;

            return View(viewModel);
        }
    }
}
