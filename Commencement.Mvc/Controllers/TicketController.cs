using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using Microsoft.Reporting.WebForms;
using UCDArch.Core.Utils;

namespace Commencement.Controllers
{
    [Ticketing]
    public class TicketController : ApplicationController
    {
        private readonly string _serverLocation = ConfigurationManager.AppSettings["ReportServer"];

        public ActionResult Index()
        {
            var term = TermService.GetCurrent();
            //var xxx = term.Ceremonies.ToList(); //Scott, this is generating a Lazy Load error
            var ceremonies = Repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode.Id == term.Id).ToList();
            //var ceremonies = term.Ceremonies;

            return View(new TicketViewModel(){Ceremonies = ceremonies});
        }

        public FileResult GetReport(int? ceremonyId)
        {
            var name = string.Empty;
            var parameters = new Dictionary<string, string>();

            if (ceremonyId.HasValue)
            {
                name = "TicketingByCeremony";
                parameters.Add("cid", ceremonyId.Value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                name = "TicketingAllCeremonies";
                parameters.Add("term", TermService.GetCurrent().Id);
            }

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
