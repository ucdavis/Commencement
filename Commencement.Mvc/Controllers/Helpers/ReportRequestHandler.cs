using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using Commencement.Core.Domain;
using Microsoft.Reporting.WebForms;
using Microsoft.WindowsAzure;
using Newtonsoft.Json;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.MVC.Controllers.Helpers
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

        public static byte[] GetHonorsReport(Dictionary<string, string> parameters)
        {
            var token = CloudConfigurationManager.GetSetting("ReportToken");
            var reportUrl = CloudConfigurationManager.GetSetting("ReportApiUrl");
            var data = JsonConvert.SerializeObject(parameters);
            var client = new HttpClient();
            var result = client.PostAsync(string.Format("{0}?token={1}", reportUrl, token), new StringContent(data, Encoding.UTF8, "application/json")).Result;
            var contents = result.Content.ReadAsByteArrayAsync();

            return contents.Result;
        }

        public static void BeginReportRequest(IRepository repository, HonorsPostModel honorsModel, string userId)
        {
            var parameters = new Dictionary<string, string>();
            var reportName = "/Commencement/Honors";

            parameters.Add("CollegeId", honorsModel.College.Id);
            parameters.Add("TermCode", honorsModel.TermCode);
            parameters.Add("Honors4590", honorsModel.Honors4590.ToString());
            parameters.Add("HighHonors4590", honorsModel.HighHonors4590.ToString());
            parameters.Add("HighestHonors4590", honorsModel.HighestHonors4590.ToString());
            parameters.Add("Honors90135", honorsModel.Honors90135.ToString());
            parameters.Add("HighHonors90135", honorsModel.HighHonors90135.ToString());
            parameters.Add("HighestHonors90135", honorsModel.HighestHonors90135.ToString());
            parameters.Add("Honors135", honorsModel.Honors135.ToString());
            parameters.Add("HighHonors135", honorsModel.HighHonors135.ToString());
            parameters.Add("HighestHonors135", honorsModel.HighestHonors135.ToString());

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
                hr.Contents = GetHonorsReport(parameters); //Get(reportName, parameters);

                // persist the object
                repository.OfType<HonorsReport>().EnsurePersistent(hr);
                ts.CommitTransaction();

                // email the user
                //var message = new MailMessage();
                //message.To.Add(hr.User.Email);
                //message.Subject = "Commencement - Honors Report Completed";
                //message.Body = "Your honors report request has completed.";
                //message.IsBodyHtml = true;

                //// settings are set in the web.config
                //var client = new SmtpClient();
                //client.Send(message);
            }
        }

        private static void EndReportRequest(IAsyncResult ar)
        {
            // completed don't really need to do anything
        }

       
    }
}