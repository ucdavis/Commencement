using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib.Attributes;
using Telerik.Web.Mvc.Extensions;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
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

        public ActionResult Edit(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);

            if (ceremony == null) return this.RedirectToAction(a => a.Index());

            var viewModel = CeremonyViewModel.Create(Repository, ceremony);
            
            return View(viewModel);
        }

        [AcceptPost]
        public ActionResult Edit(int id, Ceremony ceremony, string term, IEnumerable<MajorCode> ceremonyMajors)
        {
            Check.Require(ceremony != null, "Ceremony cannot be null.");

            var destCeremony = Repository.OfType<Ceremony>().GetNullableById(id);
            if (ceremony == null) return this.RedirectToAction(a => a.Index());

            // update the term
            var termCode = _termRepository.GetNullableById(term);
            destCeremony.TermCode = termCode;

            // copy all the fields
            destCeremony.DateTime = ceremony.DateTime;
            destCeremony.Location = ceremony.Location;
            destCeremony.TicketsPerStudent = ceremony.TicketsPerStudent;
            destCeremony.TotalTickets = ceremony.TotalTickets;
            destCeremony.RegistrationDeadline = ceremony.RegistrationDeadline;
            
            MergeCeremonyMajors(destCeremony.Majors, ceremony.Majors);

            // validate the majors
            destCeremony.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Ceremony>().EnsurePersistent(destCeremony);

                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = CeremonyViewModel.Create(Repository, destCeremony);

            return View(viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = CeremonyViewModel.Create(Repository, new Ceremony());

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
            var viewModel = CeremonyViewModel.Create(Repository, ceremony);
            viewModel.Ceremony = ceremony;

            return View(viewModel);
        }

        private void MergeCeremonyMajors (IList<MajorCode> destMajors, IList<MajorCode> srcMajors)
        {
            destMajors.Clear();
            foreach (var m in srcMajors) destMajors.Add(m);
        }
    }
}
