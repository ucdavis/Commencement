using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
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
        [AdminOnly]
        public ActionResult Index()
        {
            var viewModel = CommencementViewModel.Create(Repository);

            return View(viewModel);
        }
        [AdminOnly]
        public ActionResult Edit(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);

            if (ceremony == null) return this.RedirectToAction(a => a.Index());

            var viewModel = CeremonyViewModel.Create(Repository, ceremony);
            
            return View(viewModel);
        }
        [AdminOnly]
        [AcceptPost]
        public ActionResult Edit(CeremonyEditModel ceremonyEditModel)
        {
            Check.Require(ceremonyEditModel.Ceremony != null, "Ceremony cannot be null.");

            var destCeremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyEditModel.Id);
            if (ceremonyEditModel.Ceremony == null) return this.RedirectToAction(a => a.Index());

            // update the term
            var termCode = _termRepository.GetNullableById(ceremonyEditModel.Term);
            destCeremony.TermCode = termCode;

            // copy all the fields
            CopyCeremony(destCeremony, ceremonyEditModel.Ceremony, ceremonyEditModel.CeremonyMajors);

            // validate the ceremony
            destCeremony.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Ceremony>().EnsurePersistent(destCeremony);

                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = CeremonyViewModel.Create(Repository, destCeremony);

            return View(viewModel);
        }
        [AdminOnly]
        public ActionResult Create()
        {
            var viewModel = CeremonyViewModel.Create(Repository, new Ceremony());

            return View(viewModel);
        }
        [AdminOnly]
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

        private void CopyCeremony(Ceremony destCeremony, Ceremony srcCeremony, IList<MajorCode> srcMajors)
        {
            destCeremony.DateTime = srcCeremony.DateTime;
            destCeremony.Location = srcCeremony.Location;
            destCeremony.TicketsPerStudent = srcCeremony.TicketsPerStudent;
            destCeremony.TotalTickets = srcCeremony.TotalTickets;
            destCeremony.RegistrationDeadline = srcCeremony.RegistrationDeadline;

            MergeCeremonyMajors(destCeremony.Majors, srcMajors);
        }
    }

    public class CeremonyEditModel
    {
        public int Id { get; set; }
        public Ceremony Ceremony { get; set; }
        public string Term { get; set; }
        public IList<MajorCode> CeremonyMajors { get; set; }

        public CeremonyEditModel()
        {
            CeremonyMajors = new List<MajorCode>();
        }
    }

}
