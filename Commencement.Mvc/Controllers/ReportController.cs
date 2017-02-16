using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Commencement.Mvc;
using Commencement.Mvc.ReportDataSets;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_MajorCountByCeremonyReportTableAdapters;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_RegistrarsReportTableAdapters;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_RegistrationMajorMismatchReportTableAdapters;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_SpecialNeedsReportTableAdapters;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_SummaryReportTableAdapters;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_TotalRegisteredByMajorReportTableAdapters;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_TotalRegistrationReportTableAdapters;
using Microsoft.Reporting.WebForms;
using Microsoft.WindowsAzure;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.ActionResults;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class ReportController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<TermCode, string> _termRepository;
        private readonly IUserService _userService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IMajorService _majorService;
        private readonly IRepository<RegistrationParticipation> _registrationParticipationRepository;
        private readonly string _serverLocation = CloudConfigurationManager.GetSetting("ReportServer");

        public ReportController(IRepositoryWithTypedId<TermCode, string> termRepository, IUserService userService, ICeremonyService ceremonyService, IMajorService majorService, IRepository<RegistrationParticipation> registrationParticipationRepository)
        {
            _termRepository = termRepository;
            _userService = userService;
            _ceremonyService = ceremonyService;
            _majorService = majorService;
            _registrationParticipationRepository = registrationParticipationRepository;
        }

        //
        // GET: /Report/

        public ActionResult Index()
        {
            var viewModel = ReportViewModel.Create(Repository, _ceremonyService, CurrentUser.Identity.Name);
            viewModel.MajorCodes = GetMajorsForTerm(TermService.GetCurrent().Id);
            viewModel.Ceremonies = GetCeremoniesForTerm(TermService.GetCurrent().Id);

            return View(viewModel);
        }

        #region Microsoft Report Server Reports
        public FileResult GetReport(Report report, string termCode, string majorCode, string ceremony)
        {
            Check.Require(!string.IsNullOrEmpty(termCode), "Term code is required.");

            var name = string.Empty;
            var parameters = new Dictionary<string, string>();

            parameters.Add("term", termCode);
            parameters.Add("userId", _userService.GetCurrentUser(CurrentUser).Id.ToString());
            DataTable data = null;
            ReportDataSource rs = null;
            switch (report)
            {
                case Report.TotalRegisteredStudents:
                    name = "TotalRegistrationReport";
                    data = new usp_TotalRegisteredStudentsTableAdapter().GetData(parameters["term"], Convert.ToInt32(parameters["userId"]));
                    rs = new ReportDataSource("TotalRegisteredStudents", data);
                    
                    break;
                case Report.TotalRegisteredByMajor:
                    name = "TotalRegistrationByMajorReport";
                    parameters.Add("major", majorCode);
                    data = new usp_TotalRegisteredByMajorTableAdapter().GetData(parameters["term"], Convert.ToInt32(parameters["userId"]), parameters["major"]);
                    rs = new ReportDataSource("TotalByMajorReport", data);
                    
                    break;
                //case Report.TotalRegistrationPetitions:
                //    name = "TotalRegistrationPetitions";
                //    break;
                case Report.SumOfAllTickets:
                    name = "SummaryReport";
                    data = new usp_SummaryReportTableAdapter().GetData(parameters["term"], Convert.ToInt32(parameters["userId"]));
                    rs = new ReportDataSource("SumOfAllTickets", data);
                    
                    break;
                case Report.SpecialNeedsRequest:
                    name = "SpecialNeedsRequest";
                    data = new usp_SpecialNeedsReportTableAdapter().GetData(parameters["term"], Convert.ToInt32(parameters["userId"]));
                    rs = new ReportDataSource("SpecialNeeds", data);
                    
                    break;
                case Report.RegistrarsReport:
                    name = "RegistrarReport";
                    data = new usp_RegistrarReportTableAdapter().GetData(parameters["term"], Convert.ToInt32(parameters["userId"]));
                    rs = new ReportDataSource("RegistrarsReport", data);
                    break;
                case Report.TicketSignOutSheet:
                    throw new NotImplementedException();
                    name = "TicketSignOutSheet";
                    break;
                case Report.MajorCountByCeremony:
                    name = "MajorCountByCeremony";
                    parameters.Remove("term");
                    parameters.Add("ceremonyId", ceremony);
                    data = new usp_MajorCountByCeremonyTableAdapter().GetData(Convert.ToInt32(parameters["ceremonyId"]), Convert.ToInt32(parameters["userId"]));
                    rs = new ReportDataSource("MajorCountByCeremony", data);
                    break;
                case Report.RegistartionMajorMismatch:
                    name = "RegistrationMajorMismatch";
                    data = new usp_RegistrationMajorMismatchTableAdapter().GetData(parameters["term"], Convert.ToInt32(parameters["userId"]));
                    rs = new ReportDataSource("RegistrationMajorMismatch", data);
                    break;
            };

            return File(GetLocalReport(rs, name, parameters), "application/excel", string.Format("{0}.xls", name));
            //return File(GetReport(string.Format("/commencement/{0}", name), parameters), "application/excel", string.Format("{0}.xls", name));
        }


        private byte[] GetLocalReport(ReportDataSource rs, string reportName, Dictionary<string, string> parameters)
        {


            var rview = new ReportViewer();
            
            rview.LocalReport.ReportPath = string.Format("{0}{1}.rdlc", HostingEnvironment.MapPath("~/Reports/"),reportName);
            
            rview.ProcessingMode = ProcessingMode.Local;
            rview.LocalReport.DataSources.Clear();
            rview.LocalReport.DataSources.Add(rs);

            var paramList = new List<ReportParameter>();

            if (parameters.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    paramList.Add(new ReportParameter(kvp.Key, kvp.Value));
                }
            }

            rview.LocalReport.SetParameters(paramList);

            string mimeType, encoding, extension;
            string[] streamids;
            Warning[] warnings;

            string format = "Excel";



            byte[] bytes = rview.LocalReport.Render(format, null, out mimeType, out encoding, out extension, out streamids, out warnings);

            return bytes;
        }
        [Obsolete]
        private byte[] GetReport(string ReportName, Dictionary<string, string> parameters)
        {
            string reportServer = _serverLocation;

            var rview = new ReportViewer();
            rview.ServerReport.ReportServerUrl = new Uri(reportServer);
            rview.ServerReport.ReportPath = ReportName;

            var paramList = new List<ReportParameter>();

            if (parameters.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    paramList.Add(new ReportParameter(kvp.Key, kvp.Value));
                }
            }

            rview.ServerReport.SetParameters(paramList);

            string mimeType, encoding, extension, deviceInfo;
            string[] streamids;
            Warning[] warnings;

            string format = "Excel";

            deviceInfo =
            "<DeviceInfo>" +
            "<SimplePageHeaders>True</SimplePageHeaders>" +
            "<HumanReadablePDF>True</HumanReadablePDF>" +   // this line disables the compression done by SSRS 2008 so that it can be merged.
            "</DeviceInfo>";

            byte[] bytes = rview.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            return bytes;
        }

        public enum Report { TotalRegisteredStudents=0, TotalRegistrationPetitions
                           , SumOfAllTickets, SpecialNeedsRequest, RegistrarsReport
                           , TicketSignOutSheet, TotalRegisteredByMajor, MajorCountByCeremony, RegistartionMajorMismatch
                           , TotalRegStudentsForTerm 
                           }
        #endregion

        #region Label Generator
        public ActionResult GenerateAveryLabels(string termCode, bool printMailing, bool printAll)
        {
            var term = _termRepository.GetNullableById(termCode);

            var query = from a in _registrationParticipationRepository.Queryable
                        where _ceremonyService.GetCeremonies(CurrentUser.Identity.Name, term).Contains(a.Ceremony)
                              && !a.Registration.Student.SjaBlock && a.Registration.MailTickets == printMailing
                        select a;

            if (!printAll)
            {
                query = (IOrderedQueryable<RegistrationParticipation>)
                    query.Where(
                        a =>
                        !a.LabelPrinted ||
                        (a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsApproved && !a.ExtraTicketPetition.IsPending &&
                         !a.ExtraTicketPetition.LabelPrinted));
            }

            query = (IOrderedQueryable<RegistrationParticipation>)
                query.OrderBy(a => a.Registration.Student.LastName);

            var registrations = query.ToList();
            var doc = GenerateLabelDoc(registrations, printAll);

            foreach(var r in registrations)
            {
                r.LabelPrinted = true;
                if (r.ExtraTicketPetition != null) r.ExtraTicketPetition.LabelPrinted = true;
                Repository.OfType<RegistrationParticipation>().EnsurePersistent(r);
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(doc);

            return File(bytes, "application/word", "labels.doc");
        }

        private string GenerateLabelDoc(List<RegistrationParticipation> registrations, bool printAll)
        {
            var labels = new StringBuilder();
            var rows = GenerateRows(registrations, printAll);
            foreach (var r in rows)
            {
                // put all 3 cells into a row
                labels.Append(string.Format(Labels.Avery5160_LabelRow, r.GetCell1, r.GetCell2, r.GetCell3));
            }

            var doc = string.Format(Labels.Avery5160_Doc, labels);
            doc = doc.Replace("&", "&amp;");

            return doc;
        }

        // create a list of the data broken down into rows
        private List<LabelRow> GenerateRows(List<RegistrationParticipation> registrations, bool printAll)
        {
            var rows = new List<LabelRow>();
            var row = new LabelRow();

            foreach (var rp in registrations)
            {
                // if no space is avaible add it to the list
                if (!row.HasSpace())
                {
                    rows.Add(row);
                    row = new LabelRow();
                }

                // calculate the number of tickets
                var tickets = !rp.LabelPrinted || printAll ? rp.NumberTickets : 0;  // figure out if we need to print original ticket count
                // calculate any extra tickets
                tickets += rp.ExtraTicketPetition != null && !rp.ExtraTicketPetition.IsPending && rp.ExtraTicketPetition.IsApproved 
                    && (!rp.ExtraTicketPetition.LabelPrinted || printAll) ? rp.ExtraTicketPetition.NumberTickets.Value : 0;

                // calculate streaming
                var streaming = rp.ExtraTicketPetition != null && rp.ExtraTicketPetition.IsApprovedCompletely
                                    ? rp.ExtraTicketPetition.NumberTicketsStreaming
                                    : 0;

                var ticketString = string.Format("{0}-{1}", tickets, streaming);

                if (tickets > 0)
                {
                    string cell = string.Empty;


                    if (rp.TicketDistributionMethod != null && rp.TicketDistributionMethod.Id == StaticIndexes.Td_Mail)
                    {
                        var address2 = string.IsNullOrEmpty(rp.Registration.Address2) ? string.Empty : string.Format(Labels.Avergy5160_Mail_Address2, rp.Registration.Address2);
                        cell = string.Format(Labels.Avery5160_MailCell, rp.Registration.Student.FullName, rp.Registration.Address1,
                                                 address2, rp.Registration.City + ", " + rp.Registration.State.Id + " " + rp.Registration.Zip
                                                 , rp.Registration.RegistrationParticipations[0].Ceremony.DateTime.ToString("t") + "-" + ticketString);
                    }
                    else if(rp.TicketDistributionMethod != null && rp.TicketDistributionMethod.Id == StaticIndexes.Td_Pickup)
                    {
                        cell = string.Format(Labels.Avery5160_PickupCell, rp.Registration.Student.FullName,
                                             rp.Registration.Student.StudentId, string.Format("{0} ({1})", rp.Registration.RegistrationParticipations[0].Major.Id, rp.Registration.RegistrationParticipations[0].Major.College.Id)
                                             , rp.Ceremony.DateTime.ToString("t") + "-" + ticketString);
                    }

                    row.AddCell(cell);
                }
            }

            rows.Add(row);

            return rows;
        }
        #endregion

        public ActionResult RegistrationData()
        {
            var viewModel = RegistrationDataViewModel.Create(Repository, _ceremonyService, CurrentUser.Identity.Name, TermService.GetCurrent());
            return View(viewModel);
        }

        public ActionResult Honors()
        {
            var viewModel = new HonorsPostModel();
            viewModel.TermCode = TermService.GetCurrent().Id;
            viewModel.Colleges = Repository.OfType<College>().Queryable.Where(a => a.Display).ToList();
            viewModel.HonorsReports = Repository.OfType<HonorsReport>().Queryable.Where(a => a.User.LoginId == User.Identity.Name).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Honors(HonorsPostModel honorsPostModel)
        {
            if (honorsPostModel.Validate())
            {
                ReportRequestHandler.ExecuteReportRequest(Repository, honorsPostModel, User.Identity.Name);

                //Message = "Request has been submitted, you will receive an email when it is ready.";
                Message = "Request has been submitted, Check back in a few minutes to see if it is ready.";
            }

            honorsPostModel.Colleges = Repository.OfType<College>().Queryable.Where(a => a.Display).ToList();
            honorsPostModel.HonorsReports = Repository.OfType<HonorsReport>().Queryable.Where(a => a.User.LoginId == User.Identity.Name).ToList();

            return View(honorsPostModel);
        }

        public FileResult DownloadHonors(int id)
        {
            var hr = Repository.OfType<HonorsReport>().GetNullableById(id);

            return File(hr.Contents, "application/excel", string.Format("{0}-Honors{1}.xls", hr.TermCode, hr.College.Id));
        }

        public JsonNetResult LoadMajorsForTerm(string term)
        {
            var majors = GetMajorsForTerm(term);

            return new JsonNetResult(majors.Select(a => new { a.Id, Name = a.MajorName }));
        }

        private List<MajorCode> GetMajorsForTerm(string term)
        {
            var termCode = _termRepository.GetNullableById(term);
            var ceremonies = _ceremonyService.GetCeremonies(CurrentUser.Identity.Name, termCode);
            var majors = ceremonies.SelectMany(a => a.Majors).Where(a => a.ConsolidationMajor == null).ToList();

            return majors;
        }

        public JsonNetResult LoadCeremoniesForTerm(string term)
        {
            var ceremonies = GetCeremoniesForTerm(term);

            return new JsonNetResult(ceremonies.Select(a => new {a.Id, Name=a.CeremonyName}).ToList());
        }

        private List<Ceremony> GetCeremoniesForTerm(string term)
        {
            var termCode = _termRepository.GetNullableById(term);
            var ceremonies = _ceremonyService.GetCeremonies(CurrentUser.Identity.Name, termCode);

            return ceremonies;
        }

        public JsonNetResult GetCutOffs(int term, string college)
        {
            var cutoffs =
                Repository.OfType<HonorsCutoff>()
                          .Queryable.Where(a => a.College == college && (a.StartTerm <= term && a.EndTerm >= term))
                          .OrderBy(a => a.MinUnits)
                          .ToList();

            var result = new HonorsCutoffModel();

            if (cutoffs.Count != 3) return new JsonNetResult(result);

            if (college == "LS")
            {
                result.Tier1Honors = cutoffs[0].HonorsGpa;
                result.Tier2Honors = cutoffs[1].HonorsGpa;
                result.Tier3Honors = cutoffs[2].HonorsGpa;
            }
            else
            {
                result.Tier1Honors = cutoffs[0].HonorsGpa;
                result.Tier1HighHonors = cutoffs[0].HighHonorsGpa;
                result.Tier1HighestHonors = cutoffs[0].HighestHonorsGpa;
                result.Tier2Honors = cutoffs[1].HonorsGpa;
                result.Tier2HighHonors = cutoffs[1].HighHonorsGpa;
                result.Tier2HighestHonors = cutoffs[1].HighestHonorsGpa;
                result.Tier3Honors = cutoffs[2].HonorsGpa;
                result.Tier3HighHonors = cutoffs[2].HighHonorsGpa;
                result.Tier3HighestHonors = cutoffs[2].HighestHonorsGpa;
            }

            return new JsonNetResult(result);
        }
    }

    public class LabelRow
    {
        public LabelRow()
        {
            Cell1 = string.Empty;
            Cell2 = string.Empty;
            Cell3 = string.Empty;
        }

        public bool HasSpace()
        {
            return string.IsNullOrEmpty(Cell1) || string.IsNullOrEmpty(Cell2) || string.IsNullOrEmpty(Cell3);
        }
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Cell1) && string.IsNullOrEmpty(Cell2) && string.IsNullOrEmpty(Cell3);
        }
        public bool AddCell(string contents)
        {
            if (string.IsNullOrEmpty(Cell1)) {
                Cell1 = contents;
                return true;
            }
            if (string.IsNullOrEmpty(Cell2)) {
                Cell2 = contents;
                return true;
            }
            if (string.IsNullOrEmpty(Cell3))
            {
                Cell3 = contents;
                return true;
            }

            return false;
        }

        public string GetCell1 { get { return string.IsNullOrEmpty(Cell1) ? Labels.Avery5160_EmptyCell : Cell1; }}
        public string GetCell2 { get { return string.IsNullOrEmpty(Cell2) ? Labels.Avery5160_EmptyCell : Cell2; } }
        public string GetCell3 { get { return string.IsNullOrEmpty(Cell3) ? Labels.Avery5160_EmptyCell : Cell3; } }

        public string Cell1 { get; set; }
        public string Cell2 { get; set; }
        public string Cell3 { get; set; }
    }

    public class HonorsPostModel
    {
        public HonorsPostModel()
        {
            HonorsReports = new List<HonorsReport>();
        }

        public College College { get; set; }
        public string TermCode { get; set; }
        public decimal Honors4590 { get; set; }
        public decimal? HighHonors4590 { get; set; }
        public decimal? HighestHonors4590 { get; set; }

        public decimal Honors90135 { get; set; }
        public decimal? HighHonors90135 { get; set; }
        public decimal? HighestHonors90135 { get; set; }

        public decimal Honors135 { get; set; }
        public decimal? HighHonors135 { get; set; }
        public decimal? HighestHonors135 { get; set; }

        public IEnumerable<HonorsReport> HonorsReports { get; set; }
        public IEnumerable<College> Colleges { get; set; }

        public bool Validate()
        {
            if (College == null) return false;

            if (Honors4590 == 0 || Honors90135 == 0 || Honors135 == 0) return false;

            if (College.Id != "LS")
            {
                return HighHonors4590.HasValue && HighestHonors4590.HasValue && HighHonors90135.HasValue &&
                       HighestHonors90135.HasValue
                       && HighHonors135.HasValue && HighestHonors135.HasValue;
            }

            return true;
        }

        public HonorsReport Convert()
        {
            var hr = new HonorsReport();

            hr.College = College;
            hr.TermCode = TermCode;

            hr.Honors4590 = Honors4590;
            hr.HighHonors4590 = HighHonors4590;
            hr.HighestHonors4590 = HighestHonors4590;

            hr.Honors90135 = Honors90135;
            hr.HighHonors90135 = HighHonors90135;
            hr.HighestHonors90135 = HighestHonors90135;

            hr.Honors135 = Honors135;
            hr.HighHonors135 = HighHonors135;
            hr.HighestHonors135 = HighestHonors135;

            return hr;
        }

    }

    public class HonorsCutoffModel
    {
        public decimal Tier1Honors { get; set; }
        public decimal Tier1HighHonors { get; set; }
        public decimal Tier1HighestHonors { get; set; }

        public decimal Tier2Honors { get; set; }
        public decimal Tier2HighHonors { get; set; }
        public decimal Tier2HighestHonors { get; set; }

        public decimal Tier3Honors { get; set; }
        public decimal Tier3HighHonors { get; set; }
        public decimal Tier3HighestHonors { get; set; }
    }
}
