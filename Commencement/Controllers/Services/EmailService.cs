using System;
using System.Linq;
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
        void SendRegistrationConfirmation(Registration registration);
        void SendAddPermission(Student student, Ceremony ceremony);
        void SendExtraTicketPetitionDecision(Registration registration);
        void SendExtraTicketPetitionConfirmation(Registration registration);
        void SendRegistrationPetitionConfirmation(RegistrationPetition registrationPetition);
        void SendRegistrationPetitionApproved(RegistrationPetition registrationPetition);

        void QueueRegistrationConfirmation(Registration registration);
    }

    public class EmailService : IEmailService
    {
        private readonly IRepository<Template> _templateRepository;
        SmtpClient client = new SmtpClient();
        LetterGenerator letterGenerator = new LetterGenerator();

        public EmailService(IRepository<Template> templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public void SendRegistrationConfirmation(Registration registration)
        {
            Check.Require(registration != null, "Registration is required.");

            var message = InitializeMessage();
            message.Subject = registration.RegistrationParticipations[0].Ceremony.Name + " Registration";

            // add who to mail the message to
            message.To.Add(registration.Student.Email);
            if (registration.Email != null) message.To.Add(registration.Email);

            // get the latest registration confirmation template
            var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation).OrderByDescending(a => a.Id));
            //Check.Require(template != null, "No template is available.");

            // process the template text
            message.Body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

            Send(message);
        }

        public void SendAddPermission(Student student, Ceremony ceremony)
        {
            var term = TermService.GetCurrent();

            Check.Require(student != null, "Student is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration";
            message.To.Add(student.Email);

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved).OrderByDescending(a => a.Id));
            var template = ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available");

            message.Body = letterGenerator.GenerateAddPermission(student, template, ceremony);

            Send(message);
        }

        public void SendExtraTicketPetitionDecision(Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";
            message.To.Add(registration.Student.Email);
            if (registration.Email != null) message.To.Add(registration.Email);

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision).OrderByDescending(a => a.Id));
            var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            Send(message);
        }

        public void SendExtraTicketPetitionConfirmation(Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";
            message.To.Add(registration.Student.Email);
            if (registration.Email != null) message.To.Add(registration.Email);

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition).OrderByDescending(a => a.Id));
            var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            Send(message);
        }

        public void SendRegistrationPetitionConfirmation(RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";
            message.To.Add(registrationPetition.Email);

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
            var template = registrationPetition.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            Send(message);
        }

        public void SendRegistrationPetitionApproved(RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";
            message.To.Add(registrationPetition.Email);

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
            var template = registrationPetition.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            Send(message);
        }

        public void QueueRegistrationConfirmation(Registration registration)
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

        private void Send(MailMessage message)
        {
            // add the disclaimer
            message.Body = "** This email was generated by an automated system.<br/>Please do not respond to this email address.<br/><br/>" + message.Body;

            // send
            client.Send(message);
        }
    }

    public class DevEmailService : IEmailService
    {
        private readonly IRepository<Template> _templateRepository;
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        SmtpClient client = new SmtpClient();
        LetterGenerator letterGenerator = new LetterGenerator();

        public DevEmailService(IRepository<Template> templateRepository, IRepository<EmailQueue> emailQueueRepository)
        {
            _templateRepository = templateRepository;
            _emailQueueRepository = emailQueueRepository;
        }

        private readonly string emailAddr = "anlai@ucdavis.edu";

        public void SendRegistrationConfirmation(Registration registration)
        {
            Check.Require(registration != null, "Registration is required.");

            var message = InitializeMessage();
            message.Subject = registration.RegistrationParticipations[0].Ceremony.Name + " Registration";

            // get the latest registration confirmation template
            var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            // process the template text
            message.Body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

            Send(message);
        }

        public void QueueRegistrationConfirmation(Registration registration)
        {
            Check.Require(registration != null, "Registration is required.");
            Check.Require(registration.Id > 0, "Completed registration is required.");

            // get the latest registration confirmation template
            var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            var body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

            var emailQueue = new EmailQueue(registration.Student, template, template.Subject, body, true);
            emailQueue.Registration = registration;
            _emailQueueRepository.EnsurePersistent(emailQueue);
        }

        public void SendAddPermission(Student student, Ceremony ceremony)
        {
            var term = TermService.GetCurrent();

            Check.Require(student != null, "Student is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration";

            var template = ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available");

            message.Body = letterGenerator.GenerateAddPermission(student, template, ceremony);

            Send(message);
        }

        public void SendExtraTicketPetitionDecision(Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision).OrderByDescending(a => a.Id));
            var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            Send(message);
        }

        public void SendExtraTicketPetitionConfirmation(Registration registration)
        {
            var term = TermService.GetCurrent();

            Check.Require(registration != null, "Registration is required.");
            Check.Require(term != null, "Unable to get current term.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Extra Ticket Petition";

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition).OrderByDescending(a => a.Id));
            var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

            Send(message);
        }

        public void SendRegistrationPetitionConfirmation(RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";
            
            var template = registrationPetition.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            Send(message);
        }

        public void SendRegistrationPetitionApproved(RegistrationPetition registrationPetition)
        {
            var term = TermService.GetCurrent();

            Check.Require(registrationPetition != null, "Registration Petition is required.");

            var message = InitializeMessage();
            message.Subject = term.Name + " Commencement Registration Petition";

            //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
            var template = registrationPetition.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved && a.IsActive).FirstOrDefault();
            Check.Require(template != null, "No template is available.");

            message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

            Send(message);
        }

        private MailMessage InitializeMessage()
        {
            var message = new MailMessage();
            message.IsBodyHtml = true;
            message.Bcc.Add("automatedemail@caes.ucdavis.edu");

            return message;
        }

        private void Send(MailMessage message)
        {
            // clear out the message to
            message.To.Clear();
            message.To.Add(emailAddr);

            // add the disclaimer
            message.Body = "** This email was generated by an automated system.<br/>Please do not respond to this email address.<br/><br/>" + message.Body;

            // send
            client.Send(message);
        }
    }
}
