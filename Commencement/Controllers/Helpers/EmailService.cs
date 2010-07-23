using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public interface IEmailService
    {
        void SendEmail(string[] to, string body);
        void SendRegistrationConfirmation(IRepository repository, Registration registration);
        void SendExtraTicketPetitionConfirmation(IRepository repository, ExtraTicketPetition extraTicketPetition);
        void SendRegistrationPetitionConfirmation(IRepository repository, RegistrationPetition registrationPetition);
    }

    public class EmailService : IEmailService
    {
        public void SendEmail(string[] to, string body)
        {
            throw new NotImplementedException();
        }

        public void SendRegistrationConfirmation(IRepository repository, Registration registration)
        {
            throw new NotImplementedException();
        }

        public void SendExtraTicketPetitionConfirmation(IRepository repository, ExtraTicketPetition extraTicketPetition)
        {
            throw new NotImplementedException();
        }

        public void SendRegistrationPetitionConfirmation(IRepository repository, RegistrationPetition registrationPetition)
        {
            throw new NotImplementedException();
        }
    }

    public class DevEmailService : IEmailService
    {
        SmtpClient client = new SmtpClient();

        public void SendEmail(string[] to, string body)
        {
            var message = new MailMessage();
            message.IsBodyHtml = true;
            //foreach(var address in to) message.To.Add(address);
            message.To.Add("anlai@ucdavis.edu");
            message.Body = body;

            client.Send(message);
        }

        public void SendRegistrationConfirmation(IRepository repository, Registration registration)
        {
            Check.Require(repository != null, "Reposity is required.");
            Check.Require(registration != null, "Registration is required.");

            var message = InitializeMessage();
            message.Subject = registration.Ceremony.Name + " Registration";

            // add who to mail the message to
            //message.To.Add(registration.Student.Email);
            //if (!string.IsNullOrEmpty(registration.Email)) message.To.Add(registration.Email);
            message.To.Add("anlai@ucdavis.edu");

            // get the latest registration confirmation template
            var template = repository.OfType<Template>().Queryable.Where(a => a.RegistrationConfirmation).OrderByDescending(a => a.Id).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            // process the template text

            var body = string.Empty;
            message.Body = template.BodyText;

            client.Send(message);
        }

        public void SendExtraTicketPetitionConfirmation(IRepository repository, ExtraTicketPetition extraTicketPetition)
        {
            throw new NotImplementedException();
        }

        public void SendRegistrationPetitionConfirmation(IRepository repository, RegistrationPetition registrationPetition)
        {
            throw new NotImplementedException();
        }

        private MailMessage InitializeMessage()
        {
            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.Bcc.Add("automatedemail@ucdavis.edu");

            return message;
        }
    }
}
