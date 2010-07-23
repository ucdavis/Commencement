using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Commencement.Controllers.Helpers
{
    public interface IEmailService
    {
        void SendEmail(string[] to, string body);
    }

    public class EmailService : IEmailService
    {
        public void SendEmail(string[] to, string body)
        {
            throw new NotImplementedException();
        }
    }

    public class DevEmailService : IEmailService
    {
        public void SendEmail(string[] to, string body)
        {
            var client = new SmtpClient();
            var message = new MailMessage();
            message.IsBodyHtml = true;
            //foreach(var address in to) message.To.Add(address);
            message.To.Add("anlai@ucdavis.edu");
            message.Body = body;

            client.Send(message);
        }
    }
}
