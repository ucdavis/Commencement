﻿using System.Linq;
using System.Net.Mail;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using Queryable = System.Linq.Queryable;

namespace Commencement.Controllers.Services
{
    public interface IEmailService
    {
        void SendEmail(string[] to, string body);
        void SendRegistrationConfirmation(IRepository repository, Registration registration);
        void SendAddPermission(IRepository repository, Student student, Ceremony ceremony);
        void SendExtraTicketPetitionDecision(IRepository repository, Registration registration);
        void SendExtraTicketPetitionConfirmation(IRepository repository, Registration registration);
        void SendRegistrationPetitionConfirmation(IRepository repository, RegistrationPetition registrationPetition);
        void SendRegistrationPetitionApproved(IRepository repository, RegistrationPetition registrationPetition);
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
            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            // process the template text
            message.Body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

            client.Send(message);
        }

        public void SendAddPermission(IRepository repository, Student student, Ceremony ceremony)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(student != null, "Student is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration";
            message.To.Add(student.Email);

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available");

            message.Body = letterGenerator.GenerateAddPermission(student, template, ceremony);

            client.Send(message);
        }

        public void SendExtraTicketPetitionDecision(IRepository repository, Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";
            message.To.Add(registration.Student.Email);
            if (registration.Email != null) message.To.Add(registration.Email);

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            client.Send(message);
        }

        public void SendExtraTicketPetitionConfirmation(IRepository repository, Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";
            message.To.Add(registration.Student.Email);
            if (registration.Email != null) message.To.Add(registration.Email);

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            client.Send(message);
        }

        public void SendRegistrationPetitionConfirmation(IRepository repository, RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";
            message.To.Add(registrationPetition.Email);

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            client.Send(message);
        }

        public void SendRegistrationPetitionApproved(IRepository repository, RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";
            message.To.Add(registrationPetition.Email);

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            client.Send(message);
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
            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            // process the template text
            message.Body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

            client.Send(message);
        }

        public void SendAddPermission(IRepository repository, Student student, Ceremony ceremony)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(student != null, "Student is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration";

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available");

            message.Body = letterGenerator.GenerateAddPermission(student, template, ceremony);

            client.Send(message);
        }

        public void SendExtraTicketPetitionDecision(IRepository repository, Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";
            message.To.Add("anlai@ucdavis.edu");

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            client.Send(message);
        }

        public void SendExtraTicketPetitionConfirmation(IRepository repository, Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";
            message.To.Add("anlai@ucdavis.edu");

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            client.Send(message);
        }

        public void SendRegistrationPetitionConfirmation(IRepository repository, RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";
            message.To.Add("anlai@ucdavis.edu");

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            client.Send(message);
        }

        public void SendRegistrationPetitionApproved(IRepository repository, RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(repository != null, "Repository is required.");
            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";
            message.To.Add("anlai@ucdavis.edu");

            var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            client.Send(message);
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