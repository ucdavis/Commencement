using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class CeremonyController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<TermCode, string> _termRepository;
        private readonly IRepositoryWithTypedId<vTermCode, string> _vTermRepository;
        private readonly IRepositoryWithTypedId<College, string> _collegeRepository;
        private readonly IMajorService _majorService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IUserService _userService;

        public CeremonyController(IRepositoryWithTypedId<TermCode, string> termRepository, IRepositoryWithTypedId<vTermCode, string> vTermRepository, IRepositoryWithTypedId<College, string> collegeRepository, IMajorService majorService, ICeremonyService ceremonyService, IUserService userService)
        {
            _termRepository = termRepository;
            _vTermRepository = vTermRepository;
            _collegeRepository = collegeRepository;
            _majorService = majorService;
            _ceremonyService = ceremonyService;
            _userService = userService;
        }

        //
        // GET: /Commencement/
        public ActionResult Index()
        {
            var viewModel = CommencementViewModel.Create(Repository, _ceremonyService, User.Identity.Name);
            return View(viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = CeremonyViewModel.Create(Repository, User, _majorService, new Ceremony());

            return View(viewModel);
        }
        [HttpPost]
        [ValidateInput(false)]
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
            CopyCeremony(ceremony, ceremonyEditModel.Ceremony, ceremonyEditModel.CeremonyMajors, ceremonyEditModel.Colleges, ceremonyEditModel.TicketDistributionMethods);
            ceremony.TermCode = termCode;
            ceremony.AddEditor(_userService.GetCurrentUser(User), true);

            // fix the time on the end dates, so it ends on


            ceremony.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                // save
                _termRepository.EnsurePersistent(termCode, true);
                Repository.OfType<Ceremony>().EnsurePersistent(ceremony);
                TermService.UpdateCurrent(termCode);    // update the cache.

                // null out the current list of ceremonies the user has access to
                _ceremonyService.ResetUserCeremonies();

                // display a message
                Message = "Ceremony has been created.";

                // redirect to the list
                return this.RedirectToAction(a => a.Edit(ceremony.Id));
            }

            // redirect back to the page
            var viewModel = CeremonyViewModel.Create(Repository, User, _majorService, ceremony);
            viewModel.Ceremony = ceremony;

            return View(viewModel);
        }
        
        public ActionResult Edit(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);
            if (ceremony == null)
            {
                Message = "Unable to find ceremony.";
                return this.RedirectToAction(a => a.Index());
            }
            if (!ceremony.IsEditor(User.Identity.Name))
            {
                Message = "You do not have permission to edit selected ceremony.";
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = CeremonyViewModel.Create(Repository, User, _majorService, ceremony);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CeremonyEditModel ceremonyEditModel)
        {
            Check.Require(ceremonyEditModel.Ceremony != null, "Ceremony cannot be null.");
            //Check.Require(!ceremonyEditModel.id.HasValue, "Ceremony Id is required.");

            var destCeremony = Repository.OfType<Ceremony>().GetNullableById(ceremonyEditModel.id.Value);
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
            CopyCeremony(destCeremony, ceremonyEditModel.Ceremony, ceremonyEditModel.CeremonyMajors, ceremonyEditModel.Colleges, ceremonyEditModel.TicketDistributionMethods);

            // validate the ceremony
            destCeremony.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Repository.OfType<Ceremony>().EnsurePersistent(destCeremony);
                _ceremonyService.ResetUserCeremonies();
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = CeremonyViewModel.Create(Repository, User, _majorService, destCeremony);

            return View(viewModel);
        }

        public ActionResult CanceledRegistrations(int id)
        {
            var model = new CanceledRegistrationsViewModel();
            model.Ceremony = Repository.OfType<Ceremony>().GetNullableById(id);
            if (model.Ceremony == null)
            {
                Message = "Unable to find ceremony.";
                return this.RedirectToAction(a => a.Index());
            }

            model.CancelledRegistrations = Repository.OfType<vCancelledRegistrations>().Queryable.Where(a => a.CeremonyId == id).ToList();

            return View(model);
        }
        
        public ActionResult EditPermissions(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);
            if (ceremony == null) return this.RedirectToAction(a => a.Index());
            if (!ceremony.IsEditor(User.Identity.Name))
            {
                Message = "You do not have permission to edit selected ceremony.";
                return this.RedirectToAction(a => a.Index());
            }

            return View(ceremony);
        }
        [HttpPost]
        public ActionResult RemoveEditor(int id, int ceremonyEditorId)
        {
            var editor = Repository.OfType<CeremonyEditor>().GetNullableById(ceremonyEditorId);
            if (editor == null || editor.Owner)
            {
                Message = "Editor cannot be removed.";
            }
            else
            {
                Repository.OfType<CeremonyEditor>().Remove(editor);
            }

            return this.RedirectToAction(a => a.EditPermissions(id));
        }

        public ActionResult AddEditor(int id)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);
            if (ceremony == null) return this.RedirectToAction(a => a.Index());

            var viewModel = AddEditorViewModel.Create(Repository, ceremony);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddEditor(int id, int userId)
        {
            var ceremony = Repository.OfType<Ceremony>().GetNullableById(id);
            var user = Repository.OfType<vUser>().GetNullableById(userId);
            
            if (ceremony == null) return this.RedirectToAction(a => a.Index());

            if (user == null)
            {
                Message = "User is required.";
                return View(AddEditorViewModel.Create(Repository, ceremony));
            }

            if (ceremony.Editors.Where(a=>a.User == user).Any())
            {
                Message = "User is already an editor";
                return View(AddEditorViewModel.Create(Repository, ceremony));
            }

            ceremony.AddEditor(user, false);
            Repository.OfType<Ceremony>().EnsurePersistent(ceremony);

            return this.RedirectToAction(a => a.EditPermissions(id));
        }

        /// <summary>
        /// Ajax method for returning majors by college
        /// </summary>
        /// <param name="colleges"></param>
        /// <returns></returns>
        public JsonNetResult GetMajors(string[] colleges)
        {
            if (colleges == null) return new JsonNetResult();

            var colls = new List<College>();

            // get the colleges
            foreach (var college in colleges)
            {
                colls.Add(_collegeRepository.GetById(college));
            }

            return new JsonNetResult(_majorService.GetByCollege(colls).Select(a => new { Id = a.Id, Name = a.Name }));
        }

        #region Helper Methods
        private void MergeCeremonyMajors (IList<MajorCode> destMajors, IList<MajorCode> srcMajors, IList<College> srcColleges)
        {
            destMajors.Clear();
            foreach (var m in srcMajors.Where(a=>srcColleges.Contains(a.College))) destMajors.Add(m);
        }

        private void MergeTicketDistributionMethods(IList<TicketDistributionMethod> dest, IList<TicketDistributionMethod> src)
        {
            dest.Clear();
            foreach(var t in src) dest.Add(t);
        }

        private void CopyCeremony(Ceremony destCeremony, Ceremony srcCeremony, IList<MajorCode> srcMajors, IList<College> srcColleges, IList<TicketDistributionMethod> srcTicketDistributionMethods )
        {
            destCeremony.Name = srcCeremony.Name;
            destCeremony.DateTime = srcCeremony.DateTime;
            destCeremony.Location = srcCeremony.Location;
            destCeremony.TicketsPerStudent = srcCeremony.TicketsPerStudent;
            destCeremony.TotalTickets = srcCeremony.TotalTickets;
            destCeremony.ExtraTicketBegin = srcCeremony.ExtraTicketBegin;
            destCeremony.ExtraTicketDeadline = CreateDeadline(srcCeremony.ExtraTicketDeadline);
            destCeremony.ExtraTicketPerStudent = srcCeremony.ExtraTicketPerStudent;
            destCeremony.PrintingDeadline = srcCeremony.PrintingDeadline;
            destCeremony.MinUnits = srcCeremony.MinUnits;
            destCeremony.PetitionThreshold = srcCeremony.PetitionThreshold;
            destCeremony.Colleges = srcColleges;
            destCeremony.ConfirmationText = srcCeremony.ConfirmationText;

            destCeremony.WebsiteUrl = srcCeremony.WebsiteUrl;
            destCeremony.SurveyUrl = srcCeremony.SurveyUrl;
            destCeremony.Survey = srcCeremony.Survey;

            foreach (var ceremonySurvey in srcCeremony.CeremonySurveys)
            {
                var destCeremonySurvey = destCeremony.CeremonySurveys.SingleOrDefault(a => a.College.Id == ceremonySurvey.College.Id);
                if (destCeremonySurvey == null)
                {
                    var cSurvey = new CeremonySurvey();
                    cSurvey.Ceremony = destCeremony;
                    cSurvey.College = ceremonySurvey.College;
                    cSurvey.Survey = ceremonySurvey.Survey;
                    cSurvey.SurveyUrl = ceremonySurvey.SurveyUrl;
                    destCeremony.CeremonySurveys.Add(cSurvey);
                }
                else
                {
                    destCeremonySurvey.Survey = ceremonySurvey.Survey;
                    destCeremonySurvey.SurveyUrl = ceremonySurvey.SurveyUrl;
                }
            }

            MergeCeremonyMajors(destCeremony.Majors, srcMajors, srcColleges);
            MergeTicketDistributionMethods(destCeremony.TicketDistributionMethods, srcTicketDistributionMethods);
        }

        private DateTime CreateDeadline(DateTime src)
        {
            return new DateTime(src.Year, src.Month, src.Day, 23, 59, 59);
        }
        #endregion
    }

    public class CeremonyEditModel
    {
        public int? id { get; set; }
        
        public string Term { get; set; }
        public Ceremony Ceremony { get; set; }
        public IList<MajorCode> CeremonyMajors { get; set; }
        public IList<College> Colleges { get; set; }
        public IList<TicketDistributionMethod> TicketDistributionMethods { get; set; }

        public CeremonyEditModel()
        {
            CeremonyMajors = new List<MajorCode>();
            Colleges = new List<College>();
            TicketDistributionMethods = new List<TicketDistributionMethod>();
        }
    }
}
