using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure;
using SparkPost;

namespace Commencement.Controllers.Services
{
    public interface IErrorService
    {
        void ReportError(Exception ex);
    }

    public class ErrorService : IErrorService
    {
        private delegate void BeginReportErrorHandlerDelegate(Exception ex);

        public void ReportError(Exception ex)
        {
            //var deligate = new BeginReportErrorHandlerDelegate(BeginReportErrorHandler);
            //var callback = new AsyncCallback(EndReportErrorHandler);

            //deligate.BeginInvoke(ex, callback, null);

            try
            {
                var body = ex.Message;
                if (ex.InnerException != null)
                {
                    body = string.Format("{0}{1}{2}", body, "<br/><hr><br/>", ex.InnerException.Message);
                }
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
                        Subject = "Commencement Error",
                        Html = body
                    }
                };
                emailTransmission.Recipients.Add(new Recipient
                {
                    Address = new Address {Email = "jsylvestre@ucdavis.edu"}
                });

                var client = new Client(CloudConfigurationManager.GetSetting("SparkPostApiKey"));
                Task.Run(() => client.Transmissions.Send(emailTransmission));
            }
            catch (Exception exs)
            {
                //Meh
            }
        }

        public static void BeginReportErrorHandler(Exception ex)
        {
            throw ex;
        }

        public static void EndReportErrorHandler(IAsyncResult ar) {}
    }
}