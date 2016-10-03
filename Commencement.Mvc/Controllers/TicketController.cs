using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_TicketingByCeremonyReportTableAdapters;
using Commencement.Mvc.ReportDataSets.CommencementDataSet_TicketingByTermReportTableAdapters;
using Microsoft.Reporting.WebForms;
using Microsoft.WindowsAzure;
using UCDArch.Core.Utils;

namespace Commencement.Controllers
{
    [Ticketing]
    public class TicketController : ApplicationController
    {
        private readonly ICeremonyService _ceremonyService;
        private readonly string _serverLocation = CloudConfigurationManager.GetSetting("ReportServer");

        public TicketController(ICeremonyService ceremonyService)
        {
            _ceremonyService = ceremonyService;
        }

        public ActionResult Index()
        {
            //var xxx = term.Ceremonies.ToList(); //Scott, this is generating a Lazy Load error
            var ceremonies = _ceremonyService.GetCeremonies(User.Identity.Name, TermService.GetCurrent()); //This is the correct way

            return View(new TicketViewModel(){Ceremonies = ceremonies});
        }

        public FileResult GetReport(int? ceremonyId)
        {
            var name = string.Empty;
            var parameters = new Dictionary<string, string>();
            DataTable data = null;
            ReportDataSource rs = null;

            if (ceremonyId.HasValue)
            {
                parameters.Add("cid", ceremonyId.Value.ToString(CultureInfo.InvariantCulture));

                name = "TicketingByCeremony";
                data = new usp_TicketingByCeremonyTableAdapter().GetData(Convert.ToInt32(parameters["cid"]));
                rs = new ReportDataSource("TicketingByCeremony", data);
                
            }
            else 
            {

                name = "TicketingByTerm";
                parameters.Add("term", TermService.GetCurrent().Id);
                data = new usp_TicketingByTermTableAdapter().GetData(parameters["term"]);
                rs = new ReportDataSource("TicketingByTerm", data);

                
            }

            return File(GetLocalReport(rs, name, parameters), "application/excel", string.Format("{0}.xls", name));

            //return File(GetReport(string.Format("/commencement/{0}", name), parameters), "application/excel", string.Format("{0}.xls", name));
        }

        private byte[] GetLocalReport(ReportDataSource rs, string reportName, Dictionary<string, string> parameters)
        {


            var rview = new ReportViewer();

            rview.LocalReport.ReportPath = string.Format("{0}{1}.rdlc", HostingEnvironment.MapPath("~/Reports/"), reportName);

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
                paramList.AddRange(parameters.Select(kvp => new ReportParameter(kvp.Key, kvp.Value)));
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

    }

    public class TicketViewModel
    {
        public IEnumerable<Ceremony> Ceremonies { get; set; }
    }
}
