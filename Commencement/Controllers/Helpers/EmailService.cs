using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Commencement.Core.Domain;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public interface IEmailService
    {
        void SendEmail(string[] to, string body);
        void SendRegistrationConfirmation(IRepository repository, Registration registration);
        void SendAddPermission(IRepository repository, Student student);
        void SendExtraTicketPetitionConfirmation(IRepository repository, ExtraTicketPetition extraTicketPetition);
        void SendRegistrationPetitionConfirmation(IRepository repository, RegistrationPetition registrationPetition);
    }

    public class EmailService : IEmailService
    {
        SmtpClient client = new SmtpClient();
        LetterGenerator letterGenerator = new LetterGenerator();

        public void SendEmail(string[] to, string body)
        {
            var message = new MailMessage();
            message.IsBodyHtml = true;
            foreach(var address in to) message.To.Add(address);
            //message.To.Add("anlai@ucdavis.edu");
            message.Body = body;

            client.Send(message);
        }

        public void SendRegistrationConfirmation(IRepository repository, Registration registration)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");

            var message = InitializeMessage();
            message.Subject = registration.Ceremony.Name + " Registration";

            // add who to mail the message to
            message.To.Add(registration.Student.Email);
            if (registration.Email != null) message.To.Add(registration.Email);

            // get the latest registration confirmation template
            var template = repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation).OrderByDescending(a => a.Id).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            // process the template text
            message.Body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

            client.Send(message);
        }

        public void SendAddPermission(IRepository repository, Student student)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(student != null, "Student is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration";
            message.To.Add(student.Email);

            var template = repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved).OrderByDescending(a => a.Id).FirstOrDefault();
            Check.Require(template != null, "No template is available");

            message.Body = letterGenerator.GenerateAddPermission(student, template);

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
            message.Bcc.Add("automatedemail@caes.ucdavis.edu");

            return message;
        }
    }

    public class DevEmailService : IEmailService
    {
        SmtpClient client = new SmtpClient();
        LetterGenerator letterGenerator =  new LetterGenerator();

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
            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");

            var message = InitializeMessage();
            message.Subject = registration.Ceremony.Name + " Registration";

            // add who to mail the message to
            message.To.Add("anlai@ucdavis.edu");
            message.To.Add("srkirkland@ucdavis.edu");

            // get the latest registration confirmation template
            var template = repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation).OrderByDescending(a => a.Id).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            // process the template text
            message.Body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

            client.Send(message);
        }

        public void SendAddPermission(IRepository repository, Student student)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(student != null, "Student is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration";

            var template = repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved).OrderByDescending(a => a.Id).FirstOrDefault();
            Check.Require(template != null, "No template is available");

            message.Body = letterGenerator.GenerateAddPermission(student, template);

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
            message.Bcc.Add("automatedemail@caes.ucdavis.edu");

            return message;
        }
    }
}
