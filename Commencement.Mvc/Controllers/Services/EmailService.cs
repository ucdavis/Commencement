using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Commencement.Mvc.Controllers.Helpers;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.Services
{
    public interface IEmailService
    {
        void QueueRegistrationConfirmation(Registration registration, string additionalMessage = null);
        void QueueExtraTicketPetition(RegistrationParticipation participation);
        void QueueExtraTicketPetitionDecision(RegistrationParticipation participation);
        void QueueRegistrationPetition(Registration registration);
        void QueueRegistrationPetitionDecision(RegistrationPetition registration);
        void QueueMajorMove(Registration registration, RegistrationParticipation participation);
        void QueueVisaLetterDecision(VisaLetter visaLetter, Ceremony ceremony, HttpRequestBase request, UrlHelper url);
    }

    public class EmailService : IEmailService
    {
        private readonly IRepository<Template> _templateRepository;
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        private readonly ILetterGenerator _letterGenerator;

        public EmailService(IRepository<Template> templateRepository, IRepository<EmailQueue> emailQueueRepository, ILetterGenerator letterGenerator)
        {
            _templateRepository = templateRepository;
            _emailQueueRepository = emailQueueRepository;
            _letterGenerator = letterGenerator;
        }

        /// <summary>
        /// Queue's up each individual email for each registration participation
        /// </summary>
        /// <param name="registration"></param>
        public void QueueRegistrationConfirmation(Registration registration, string additionalMessage = null)
        {
            Check.Require(registration != null, "Registration is required.");

            // fix for task 237
            foreach (var a in registration.RegistrationParticipations)
            {
                var template = a.Cancelled ? 
                    a.Ceremony.Templates.FirstOrDefault(b => b.TemplateType.Name == StaticValues.Template_Cancellation && b.IsActive)
                    :
                    a.Ceremony.Templates.FirstOrDefault(b => b.TemplateType.Name == StaticValues.Template_RegistrationConfirmation && b.IsActive);

                if (template != null)
                {
                    var subject = template.Subject;
                    var body = _letterGenerator.GenerateRegistrationConfirmation(a, template);

                    if (!string.IsNullOrEmpty(additionalMessage))
                    {
                        body += additionalMessage;
                    }

                    var emailQueue = new EmailQueue(a.Registration.Student, template, subject, body, true);
                    emailQueue.Registration = registration;
                    emailQueue.RegistrationParticipation = a;

                    _emailQueueRepository.EnsurePersistent(emailQueue);                    
                }
            }
        }

        public void QueueExtraTicketPetition(RegistrationParticipation participation)
        {
            Check.Require(participation != null, "participation is required.");
            
            var template = participation.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition && a.IsActive).FirstOrDefault();

            if (template != null)
            {
                var subject = template.Subject;
                var body = _letterGenerator.GenerateExtraTicketRequestPetitionConfirmation(participation, template);

                var emailQueue = new EmailQueue(participation.Registration.Student, template, subject, body, false);
                emailQueue.Registration = participation.Registration;
                emailQueue.RegistrationParticipation = participation;
                emailQueue.ExtraTicketPetition = participation.ExtraTicketPetition;

                _emailQueueRepository.EnsurePersistent(emailQueue);    
            }
        }

        public void QueueExtraTicketPetitionDecision(RegistrationParticipation participation)
        {
            Check.Require(participation != null, "participation is required.");

            var template = participation.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision && a.IsActive).FirstOrDefault();

            if (template != null)
            {
                var subject = template.Subject;
                var body = _letterGenerator.GenerateExtraTicketRequestPetitionDecision(participation, template, null,  null, null);

                var emailQueue = new EmailQueue(participation.Registration.Student, template, subject, body, false);
                emailQueue.Registration = participation.Registration;
                emailQueue.RegistrationParticipation = participation;
                emailQueue.ExtraTicketPetition = participation.ExtraTicketPetition;

                _emailQueueRepository.EnsurePersistent(emailQueue);                
            }
        }

        public void QueueRegistrationPetition(Registration registration)
        {
            Check.Require(registration != null, "registration is required.");

            #region Old Code to remove

            //var template = registration.RegistrationPetitions.First().Ceremony.Templates.Where(b => b.TemplateType.Name == StaticValues.Template_RegistrationPetition
            //                    && b.IsActive).FirstOrDefault();

            //Check.Require(template != null, "No template is available.");

            //foreach (var a in registration.RegistrationPetitions)
            //{
            //    var subject = template.Subject;
            //    var body = _letterGenerator.GenerateRegistrationPetitionConfirmation(a, template);

            //    var emailQueue = new EmailQueue(a.Registration.Student, template, subject, body, false);
            //    emailQueue.Registration = registration;
            //    emailQueue.RegistrationPetition = a;

            //    _emailQueueRepository.EnsurePersistent(emailQueue);
            //}
            #endregion Old Code to remove

            // fix for problem similar to task 237
            foreach (var a in registration.RegistrationPetitions)
            {
                var template =
                    a.Ceremony.Templates.Where(b => b.TemplateType.Name == StaticValues.Template_RegistrationPetition
                             && b.IsActive).FirstOrDefault();
                if(template != null)
                {
                    var subject = template.Subject;
                    var body = _letterGenerator.GenerateRegistrationPetitionConfirmation(a, template);

                    var emailQueue = new EmailQueue(a.Registration.Student, template, subject, body, false);
                    emailQueue.Registration = registration;
                    emailQueue.RegistrationPetition = a;

                    _emailQueueRepository.EnsurePersistent(emailQueue);
                }
            }
        }

        public void QueueRegistrationPetitionDecision(RegistrationPetition registrationPetition)
        {
            Check.Require(registrationPetition != null, "registration is required.");

            var template =
                registrationPetition.Ceremony.Templates.Where(
                    a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved && a.IsActive).
                    FirstOrDefault();

            if (template != null)
            {
                var subject = template.Subject;
                var body = _letterGenerator.GenerateRegistrationPetitionApproved(registrationPetition, template);

                var emailQueue = new EmailQueue(registrationPetition.Registration.Student, template, subject, body);
                emailQueue.Registration = registrationPetition.Registration;
                emailQueue.RegistrationPetition = registrationPetition;

                _emailQueueRepository.EnsurePersistent(emailQueue);
            }
        }

        public void QueueMajorMove(Registration registration, RegistrationParticipation participation)
        {
            Check.Require(registration != null, "registration is required.");

            var template = participation.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_MoveMajor && a.IsActive).FirstOrDefault();

            if (template != null)
            {
                var subject = template.Subject;
                var body = _letterGenerator.GenerateMoveMajor(participation, template);

                var emailQueue = new EmailQueue(registration.Student, template, subject, body, true);
                emailQueue.Registration = registration;
                emailQueue.RegistrationParticipation = participation;

                _emailQueueRepository.EnsurePersistent(emailQueue);
            }
        }

        public void QueueVisaLetterDecision(VisaLetter visaLetter, Ceremony ceremony, HttpRequestBase request, UrlHelper url)
        {
            Check.Require(visaLetter != null, "Visa Letter Request Required.");
            var template = ceremony.Templates.FirstOrDefault(a => a.TemplateType.Name == StaticValues.Template_VisaLetterDecision && a.IsActive);
            if (template != null)
            {
                var subject = template.Subject;
                var body = _letterGenerator.GenerateVisaLetterRequestDecision(visaLetter, template, request, url);
                var emailQueue = new EmailQueue(visaLetter.Student, template, subject, body);
                _emailQueueRepository.EnsurePersistent(emailQueue);
            }
        }


    }

    //public class DevEmailService : IEmailService
    //{
    //    private readonly IRepository<Template> _templateRepository;
    //    private readonly IRepository<EmailQueue> _emailQueueRepository;
    //    SmtpClient client = new SmtpClient();
    //    LetterGenerator letterGenerator = new LetterGenerator();

    //    public DevEmailService(IRepository<Template> templateRepository, IRepository<EmailQueue> emailQueueRepository)
    //    {
    //        _templateRepository = templateRepository;
    //        _emailQueueRepository = emailQueueRepository;
    //    }

    //    private readonly string emailAddr = "jsylvestre@ucdavis.edu";

    //    public void SendRegistrationConfirmation(Registration registration)
    //    {
    //        Check.Require(registration != null, "Registration is required.");

    //        var message = InitializeMessage();
    //        message.Subject = registration.RegistrationParticipations[0].Ceremony.Name + " Registration";

    //        // get the latest registration confirmation template
    //        var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation && a.IsActive).FirstOrDefault();
    //        Check.Require(template != null, "No template is available.");

    //        // process the template text
    //       // message.Body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

    //        Send(message);
    //    }

    //    public void QueueRegistrationConfirmation(Registration registration)
    //    {
    //        Check.Require(registration != null, "Registration is required.");
    //        Check.Require(registration.Id > 0, "Completed registration is required.");

    //        // get the latest registration confirmation template
    //        var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationConfirmation && a.IsActive).FirstOrDefault();
    //        Check.Require(template != null, "No template is available.");

    //        var body = letterGenerator.GenerateRegistrationConfirmation(registration, template);

    //        var emailQueue = new EmailQueue(registration.Student, template, template.Subject, body, true);
    //        emailQueue.Registration = registration;
    //        _emailQueueRepository.EnsurePersistent(emailQueue);
    //    }

    //    public void QueueExtraTicketPetitionDecision(RegistrationParticipation participation)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void SendAddPermission(Student student, Ceremony ceremony)
    //    {
    //        var term = TermService.GetCurrent();

    //        Check.Require(student != null, "Student is required.");
    //        Check.Require(term != null, "Unable to get current term.");

    //        var message = InitializeMessage();
    //        message.Subject = term.Name + " Commencement Registration";

    //        var template = ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved && a.IsActive).FirstOrDefault();
    //        Check.Require(template != null, "No template is available");

    //        message.Body = letterGenerator.GenerateAddPermission(student, template, ceremony);

    //        Send(message);
    //    }

    //    public void SendExtraTicketPetitionDecision(Registration registration)
    //    {
    //        var term = TermService.GetCurrent();

    //        Check.Require(registration != null, "Registration is required.");
    //        Check.Require(term != null, "Unable to get current term.");

    //        var message = InitializeMessage();
    //        message.Subject = term.Name + " Commencement Extra Ticket Petition";

    //        //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision).OrderByDescending(a => a.Id));
    //        var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition_Decision && a.IsActive).FirstOrDefault();
    //        Check.Require(template != null, "No template is available.");

    //        message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

    //        Send(message);
    //    }

    //    public void SendExtraTicketPetitionConfirmation(Registration registration)
    //    {
    //        var term = TermService.GetCurrent();

    //        Check.Require(registration != null, "Registration is required.");
    //        Check.Require(term != null, "Unable to get current term.");

    //        var message = InitializeMessage();
    //        message.Subject = term.Name + " Commencement Extra Ticket Petition";

    //        //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition).OrderByDescending(a => a.Id));
    //        var template = registration.RegistrationParticipations[0].Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_TicketPetition && a.IsActive).FirstOrDefault();
    //        Check.Require(template != null, "No template is available.");

    //        message.Body = letterGenerator.GenerateExtraTicketRequestPetitionDecision(registration, template);

    //        Send(message);
    //    }

    //    public void SendRegistrationPetitionConfirmation(RegistrationPetition registrationPetition)
    //    {
    //        var term = TermService.GetCurrent();

    //        Check.Require(registrationPetition != null, "Registration Petition is required.");

    //        var message = InitializeMessage();
    //        message.Subject = term.Name + " Commencement Registration Petition";
            
    //        var template = registrationPetition.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition && a.IsActive).FirstOrDefault();
    //        Check.Require(template != null, "No template is available.");

    //        message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

    //        Send(message);
    //    }

    //    public void SendRegistrationPetitionApproved(RegistrationPetition registrationPetition)
    //    {
    //        var term = TermService.GetCurrent();

    //        Check.Require(registrationPetition != null, "Registration Petition is required.");

    //        var message = InitializeMessage();
    //        message.Subject = term.Name + " Commencement Registration Petition";

    //        //var template = Queryable.FirstOrDefault<Template>(repository.OfType<Template>().Queryable.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition).OrderByDescending(a => a.Id));
    //        var template = registrationPetition.Ceremony.Templates.Where(a => a.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved && a.IsActive).FirstOrDefault();
    //        Check.Require(template != null, "No template is available.");

    //        message.Body = letterGenerator.GenerateRegistrationPetitionConfirmation(registrationPetition, template);

    //        Send(message);
    //    }

    //    private MailMessage InitializeMessage()
    //    {
    //        var message = new MailMessage();
    //        message.IsBodyHtml = true;
    //        message.Bcc.Add("automatedemail@caes.ucdavis.edu");

    //        return message;
    //    }

    //    private void Send(MailMessage message)
    //    {
    //        // clear out the message to
    //        message.To.Clear();
    //        message.To.Add(emailAddr);

    //        // add the disclaimer
    //        message.Body = "** This email was generated by an automated system.<br/>Please do not respond to this email address.<br/><br/>" + message.Body;

    //        // send
    //        client.Send(message);
    //    }
    //}
}
