using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Microsoft.Reporting.WebForms;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers
{
    [AnyoneWithRole]
    public class ReportController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<TermCode, string> _termRepository;
        private readonly IUserService _userService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IRepository<RegistrationParticipation> _registrationParticipationRepository;
        private readonly string _serverLocation = ConfigurationManager.AppSettings["ReportServer"];

        public ReportController(IRepositoryWithTypedId<TermCode, string> termRepository, IUserService userService, ICeremonyService ceremonyService, IRepository<RegistrationParticipation> registrationParticipationRepository)
        {
            _termRepository = termRepository;
            _userService = userService;
            _ceremonyService = ceremonyService;
            _registrationParticipationRepository = registrationParticipationRepository;
        }

        //
        // GET: /Report/

        public ActionResult Index()
        {
            var viewModel = ReportViewModel.Create(Repository);
            return View(viewModel);
        }

        #region Microsoft Report Server Reports
        public FileResult GetReport(Report report, string termCode)
        {
            Check.Require(!string.IsNullOrEmpty(termCode), "Term code is required.");

            var name = string.Empty;
            var parameters = new Dictionary<string, string>();

            parameters.Add("term", termCode);
            parameters.Add("userId", _userService.GetCurrentUser(CurrentUser).Id.ToString());

            switch(report)
            {
                case Report.TotalRegisteredStudents:
                    name = "TotalRegistrationReport";
                    break;
                case Report.TotalRegistrationPetitions:
                    name = "TotalRegistrationPetitions";
                    break;
                case Report.SumOfAllTickets:
                    name = "SummaryReport";
                    break;
                case Report.SpecialNeedsRequest:
                    name = "SpecialNeedsRequest";
                    break;
                case Report.RegistrarsReport:
                    name = "RegistrarReport";
                    break;
                case Report.TicketSignOutSheet:
                    name = "TicketSignOutSheet";
                    break;
            };

            return File(GetReport(string.Format("/commencement/{0}", name), parameters), "application/excel", string.Format("{0}.xls", name));
        }

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
                           , TicketSignOutSheet
                           }
        #endregion

        #region Label Generator
        public ActionResult GenerateAveryLabels(string termCode, bool printMailing, bool printAll)
        {
            var term = _termRepository.GetNullableById(termCode);

            var query = from a in _registrationParticipationRepository.Queryable
                        where _ceremonyService.GetCeremonies(CurrentUser.Identity.Name, term).Contains(a.Ceremony)
                              && !a.Registration.Student.SjaBlock && a.Registration.MailTickets == printMailing
                        orderby a.Registration.Student.LastName
                        select a;

            if (!printAll)
            {
                query = (IOrderedQueryable<RegistrationParticipation>)
                    query.Where(
                        a =>
                        !a.LabelPrinted ||
                        (a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsApprovedCompletely &&
                         !a.ExtraTicketPetition.LabelPrinted));
            }

            //var query = from a in Repository.OfType<Registration>().Queryable
            //            where _ceremonyService.GetCeremonies(CurrentUser.Identity.Name, term).Contains(a.RegistrationParticipations[0].Ceremony)
            //                //&& !a.SjaBlock && !a.Cancelled
            //                && a.MailTickets == printMailing
            //            orderby a.Student.LastName 
            //            select a;

            // filter to only pending labels
            //if (!printAll)
            //{
            //    query = (IOrderedQueryable<Registration>)query.Where(a =>
            //                                            //(!a.LabelPrinted)
            //                                            //||
            //                                            //(a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsApproved && !a.ExtraTicketPetition.LabelPrinted)
            //                                            );
            //}

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

                    if (rp.Registration.MailTickets)
                    {
                        var address2 = string.IsNullOrEmpty(rp.Registration.Address2) ? string.Empty : string.Format(Labels.Avergy5160_Mail_Address2, rp.Registration.Address2);
                        cell = string.Format(Labels.Avery5160_MailCell, rp.Registration.Student.FullName, rp.Registration.Address1,
                                                 address2, rp.Registration.City + ", " + rp.Registration.State.Id + " " + rp.Registration.Zip
                                                 , rp.Registration.RegistrationParticipations[0].Ceremony.DateTime.ToString("t") + "-" + ticketString);


                    }
                    else
                    {
                        cell = string.Format(Labels.Avery5160_PickupCell, rp.Registration.Student.FullName,
                                             rp.Registration.Student.StudentId, rp.Registration.RegistrationParticipations[0].Major.Name
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
}
