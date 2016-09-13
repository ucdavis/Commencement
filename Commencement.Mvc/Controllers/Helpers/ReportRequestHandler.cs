using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Commencement.Core.Domain;
using Microsoft.Reporting.WebForms;
using Microsoft.WindowsAzure;
using SparkPost;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.Helpers
{
    public class ReportRequestHandler
    {
        private delegate void BeginReportRequestDelegate(IRepository repository, HonorsPostModel honorsModel, string userId);

        public static void ExecuteReportRequest(IRepository repository, HonorsPostModel honorsModel, string userId)
        {
            // setup the deligate and the callback
            var deligate = new BeginReportRequestDelegate(BeginReportRequest);
            var callback = new AsyncCallback(EndReportRequest);

            // begin the async call
            deligate.BeginInvoke(repository, honorsModel, userId, callback, null);
        }

        public static void BeginReportRequest(IRepository repository, HonorsPostModel honorsModel, string userId)
        {
            var parameters = new Dictionary<string, string>();
            var reportName = "/Commencement/Honors";

            // set the shared parameters
            parameters.Add("term", honorsModel.TermCode);
            parameters.Add("honors_4590", honorsModel.Honors4590.ToString());
            parameters.Add("honors_90135", honorsModel.Honors90135.ToString());
            parameters.Add("honors_135", honorsModel.Honors135.ToString());

            if (honorsModel.College.Id == "LS")
            {
                reportName = "/Commencement/HonorsLS";
            }
            else
            {
                parameters.Add("coll", honorsModel.College.Id);

                parameters.Add("highhonors_4590", honorsModel.HighHonors4590.ToString());
                parameters.Add("highhonors_90135", honorsModel.HighHonors90135.ToString());
                parameters.Add("highhonors_135", honorsModel.HighHonors135.ToString());

                parameters.Add("highesthonors_4590", honorsModel.HighestHonors4590.ToString());
                parameters.Add("highesthonors_90135", honorsModel.HighestHonors90135.ToString());
                parameters.Add("highesthonors_135", honorsModel.HighestHonors135.ToString());
            }

            var hr = honorsModel.Convert();
            hr.User = repository.OfType<vUser>().Queryable.FirstOrDefault(a => a.LoginId == userId);

            // start the transaction just so we can record the request
            using (var ts = new TransactionScope())
            {
                // create the history object, plus this will also set the request date time               
                repository.OfType<HonorsReport>().EnsurePersistent(hr);
                ts.CommitTransaction();
            }

            // make the call to get the report, then save it and email the user
            using (var ts = new TransactionScope())
            {
                // get the actual report itself
                hr.Contents = Get(reportName, parameters);

                // persist the object
                repository.OfType<HonorsReport>().EnsurePersistent(hr);
                ts.CommitTransaction();


                // email the user
                try
                {
                    var emailTransmission = new Transmission
                    {
                        Content = new Content
                        {
                            From =
                                new Address
                                {
                                    Email = "noreply@commencement-notify.ucdavis.edu",
                                    Name = "UCD Commencement Notification"
                                },
                            Subject = "Commencement - Honors Report Completed",
                            Html = "Your honors report request has completed."
                        }
                    };
                    emailTransmission.Recipients.Add(new Recipient {Address = new Address {Email = hr.User.Email} });

                    var client = new Client(CloudConfigurationManager.GetSetting("SparkPostApiKey"));
                    Task.Run(() => client.Transmissions.Send(emailTransmission)); 
                }
                catch (Exception ex)
                {
                    //Nothing...
                }
            }
        }

        private static void EndReportRequest(IAsyncResult ar)
        {
            // completed don't really need to do anything
        }

        /// <summary>
        /// Calls the sql report server and gets the byte array
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static byte[] Get(string ReportName, Dictionary<string, string> parameters)
        {
            string reportServer = CloudConfigurationManager.GetSetting("ReportServer");

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

            string format;

            format = "EXCEL";

            deviceInfo =
            "<DeviceInfo>" +
            "<SimplePageHeaders>True</SimplePageHeaders>" +
            "<HumanReadablePDF>True</HumanReadablePDF>" +   // this line disables the compression done by SSRS 2008 so that it can be merged.
            "</DeviceInfo>";

            byte[] bytes = rview.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            return bytes;
        }
    }
}