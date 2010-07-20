using System.Web.Mvc;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib.Attributes;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Commencement.Controllers
{
    public class CeremonyController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<TermCode, string> _termRepository;
        private readonly IRepositoryWithTypedId<vTermCode, string> _vTermRepository;

        public CeremonyController(IRepositoryWithTypedId<TermCode, string> termRepository, IRepositoryWithTypedId<vTermCode, string> vTermRepository)
        {
            _termRepository = termRepository;
            _vTermRepository = vTermRepository;
        }

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
        public ActionResult Create(Ceremony ceremony, string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                ModelState.AddModelError("Term Code", "Term code must be selected.");
            }

            var termCode = _termRepository.GetNullableById(term);

            if (termCode == null && !string.IsNullOrEmpty(term))
            {
                // term code doesn't exist, create a new one
                var vTermCode = _vTermRepository.GetNullableById(term);

                termCode = new TermCode(vTermCode);
            }

            ceremony.TermCode = termCode;

            ceremony.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                // save
                _termRepository.EnsurePersistent(termCode, true);
                Repository.OfType<Ceremony>().EnsurePersistent(ceremony);

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
