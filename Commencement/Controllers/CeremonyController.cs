using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using MvcContrib.Attributes;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Commencement.Controllers
{
    [AdminOnly]
    public class CeremonyController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<TermCode, string> _termRepository;
        private readonly IRepositoryWithTypedId<vTermCode, string> _vTermRepository;
        private readonly IRepositoryWithTypedId<College, string> _collegeRepository;
        private readonly IMajorService _majorService;
        private readonly ICeremonyService _ceremonyService;

        public CeremonyController(IRepositoryWithTypedId<TermCode, string> termRepository, IRepositoryWithTypedId<vTermCode, string> vTermRepository, IRepositoryWithTypedId<College, string> collegeRepository, IMajorService majorService, ICeremonyService ceremonyService)
        {
            _termRepository = termRepository;
            _vTermRepository = vTermRepository;
            _collegeRepository = collegeRepository;
            _majorService = majorService;
            _ceremonyService = ceremonyService;
        }

        //
        // GET: /Commencement/
        public ActionResult Index()
        {
            var viewModel = CommencementViewModel.Create(Repository, _ceremonyService, User.Identity.Name);

            return View(viewModel);
        }
        public ActionResult Edit(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);

            if (ceremony == null) return this.RedirectToAction(a => a.Index());
            if (!ceremony.IsEditor(User.Identity.Name))
            {
                Message = "You do not have permission to edit selected ceremony.";
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = CeremonyViewModel.Create(Repository, _majorService, ceremony);
            
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Edit(CeremonyEditModel ceremonyEditModel)
        {
            Check.Require(ceremonyEditModel.Ceremony != null, "Ceremony cannot be null.");

            var destCeremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyEditModel.Id);
            if (ceremonyEditModel.Ceremony == null) return this.RedirectToAction(a => a.Index());

            if (!destCeremony.IsEditor(User.Identity.Name))
            {
                Message = "You do not have permission to edit selected ceremony.";
                return this.RedirectToAction(a => a.Index());
            }

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

            var viewModel = CeremonyViewModel.Create(Repository, _majorService, destCeremony);

            return View(viewModel);
        }
        public ActionResult Create()
        {
            var viewModel = CeremonyViewModel.Create(Repository, _majorService, new Ceremony());

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(CeremonyEditModel ceremonyEditModel)
        {
            if (string.IsNullOrEmpty(ceremonyEditModel.Term))
            {
                ModelState.AddModelError("Term Code", "Term code must be selected.");
            }

            var termCode = _termRepository.GetNullableById(ceremonyEditModel.Term);

            if (termCode == null && !string.IsNullOrEmpty(ceremonyEditModel.Term))
            {
                // term code doesn't exist, create a new one
                var vTermCode = _vTermRepository.GetNullableById(ceremonyEditModel.Term);

                termCode = new TermCode(vTermCode);
            }

            Ceremony ceremony = new Ceremony();
            CopyCeremony(ceremony, ceremonyEditModel.Ceremony, ceremonyEditModel.CeremonyMajors);
            ceremony.TermCode = termCode;
            ceremony.AddEditor(User.Identity.Name, true);

            ceremony.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                // save
                //_termRepository.EnsurePersistent(termCode, true);
                //Repository.OfType<Ceremony>().EnsurePersistent(ceremony);

                // redirect to the list
                return this.RedirectToAction(a => a.Index());
            }

            // redirect back to the page
            var viewModel = CeremonyViewModel.Create(Repository, _majorService, ceremony);
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
            destCeremony.ExtraTicketDeadline = srcCeremony.ExtraTicketDeadline;
            destCeremony.ExtraTicketPerStudent = srcCeremony.ExtraTicketPerStudent;
            destCeremony.PrintingDeadline = srcCeremony.PrintingDeadline;

            MergeCeremonyMajors(destCeremony.Majors, srcMajors);
        }

        public JsonNetResult GetMajors(string[] colleges)
        {
             if (colleges == null) return new JsonNetResult();

            var colls = new List<College>();

            // get the colleges
            foreach (var college in colleges)
            {
                colls.Add(_collegeRepository.GetById(college));
            }

            return new JsonNetResult(_majorService.GetByCollege(colls).Select(a=> new {Id=a.Id, Name=a.Name}));
        }
    }

    public class CeremonyEditModel
    {
        public int Id { get; set; }
        public Ceremony Ceremony { get; set; }
        public string Term { get; set; }
        public IList<MajorCode> CeremonyMajors { get; set; }
        public IList<College> Colleges { get; set; }

        public CeremonyEditModel()
        {
            CeremonyMajors = new List<MajorCode>();
        }
    }

}
