using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

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

            var client = new SmtpClient("smtp.ucdavis.edu");

            var message = new MailMessage("automatedemail@caes.ucdavis.edu", "anlai@ucdavis.edu");
            message.Subject = "Commencement Error";
            message.Body = ex.Message;
            message.IsBodyHtml = true;

            if (ex.InnerException != null)
            {
                message.Body += "<br/><hr><br/>";
                message.Body += ex.InnerException.Message;
            }

            client.Send(message);
        }

        public static void BeginReportErrorHandler(Exception ex)
        {
            throw ex;
        }

        public static void EndReportErrorHandler(IAsyncResult ar) {}
    }
}