using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public interface ILetterGenerator
    {
        string GenerateRegistrationConfirmation(RegistrationParticipation registrationParticipation, Template template);
        string GenerateExtraTicketRequestPetitionConfirmation(RegistrationParticipation registrationParticipation, Template template);
        string GenerateExtraTicketRequestPetitionDecision(RegistrationParticipation registrationParticipation, Template template, Attachment attachment, HttpRequestBase request, UrlHelper url, string body = null);
        //string GenerateExtraTicketRequestPetitionDecision(RegistrationParticipation registrationParticipation, Template template);
        //string GenerateAddPermission(Student student, Template template, Ceremony ceremony);
        string GenerateRegistrationPetitionConfirmation(RegistrationPetition registrationPetition, Template template);
        string GenerateRegistrationPetitionApproved(RegistrationPetition registrationPetition, Template template);
        string GenerateMoveMajor(RegistrationParticipation registrationParticipation, Template template);
        string GenerateEmailAllStudents(Ceremony ceremony, Student student, string body, TemplateType templateType, Registration registration, Attachment attachment, HttpRequestBase request, UrlHelper url);
        bool ValidateTemplate(Template template, List<string> invalidTokens);

        string GenerateVisaLetterRequestDecision(VisaLetter visaLetter, Template template, HttpRequestBase request, UrlHelper url);
    }

    public class LetterGenerator : ILetterGenerator
    {
        private Ceremony _ceremony;
        private Student _student;
        private RegistrationPetition _registrationPetition;
        private RegistrationParticipation _registrationParticipation;
        private ExtraTicketPetition _extraTicketPetition;
        private Registration _registration;
        private Template _template;
        private Attachment _attachment;
        private HttpRequestBase _request;
        private UrlHelper _url;
        private VisaLetter _visaLetter;

        public string GenerateRegistrationConfirmation(RegistrationParticipation registrationParticipation, Template template)
        {
            Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(registrationParticipation.Registration.Student != null, "registrationParticipation.Registration.Student is required.");
            Check.Require(template != null, "template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationConfirmation || template.TemplateType.Name == StaticValues.Template_Cancellation, "Template mismatch.");
            Check.Require(registrationParticipation.Registration != null, "registrationParticipation.Registration is required.");

            _ceremony = registrationParticipation.Ceremony;
            _student = registrationParticipation.Registration.Student;
            _registrationParticipation = registrationParticipation;
            _registration = registrationParticipation.Registration;
            _template = template;

            return HandleBody(template.BodyText);
        }

        public string GenerateExtraTicketRequestPetitionConfirmation(RegistrationParticipation registrationParticipation, Template template)
        {
            Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(registrationParticipation.Registration.Student != null, "registrationParticipation.Registration.Student is required.");
            Check.Require(template != null, "template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_TicketPetition, "Template mismatch.");
            Check.Require(registrationParticipation.Registration != null, "registrationParticipation.Registration is required.");
            Check.Require(registrationParticipation.ExtraTicketPetition != null, "registrationParticipation.ExtraTicketPetition is required.");

            _ceremony = registrationParticipation.Ceremony;
            _student = registrationParticipation.Registration.Student;
            _registrationParticipation = registrationParticipation;
            _registration = registrationParticipation.Registration;
            _template = template;
            _extraTicketPetition = registrationParticipation.ExtraTicketPetition;
            
            return HandleBody(template.BodyText);
        }

        public string GenerateExtraTicketRequestPetitionDecision(RegistrationParticipation registrationParticipation, Template template, Attachment attachment, HttpRequestBase request, UrlHelper url, string body = null)
        {
            Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(registrationParticipation.Registration.Student != null, "registrationParticipation.Registration.Student is required.");
            Check.Require(template != null, "template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_TicketPetition_Decision, "Template mismatch.");
            Check.Require(registrationParticipation.Registration != null, "registrationParticipation.Registration is required.");
            Check.Require(registrationParticipation.ExtraTicketPetition != null, "registrationParticipation.ExtraTicketPetition is required.");

            _ceremony = registrationParticipation.Ceremony;
            _student = registrationParticipation.Registration.Student;
            _registrationParticipation = registrationParticipation;
            _registration = registrationParticipation.Registration;
            _template = template;
            _attachment = attachment;
            _request = request;
            _url = url;

            _extraTicketPetition = registrationParticipation.ExtraTicketPetition;
            if(string.IsNullOrWhiteSpace(body))
            {
                return HandleBody(template.BodyText);
            }
            else
            {
                return HandleBody(body);
            }
        }

        public string GenerateRegistrationPetitionConfirmation(RegistrationPetition registrationPetition, Template template)
        {
            Check.Require(registrationPetition != null, "registrationPetition is required.");
            Check.Require(registrationPetition.Registration != null, "registrationPetition.Registration is required.");
            Check.Require(template != null, "template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationPetition, "Template mismatch.");
            Check.Require(registrationPetition.Registration.Student != null, "registrationPetition.Registration.Student is required.");

            _ceremony = registrationPetition.Ceremony;
            _student = registrationPetition.Registration.Student;
            _registrationPetition = registrationPetition;
            _registration = registrationPetition.Registration;
            _template = template;

            return HandleBody(template.BodyText);
        }

        public string GenerateRegistrationPetitionApproved(RegistrationPetition registrationPetition, Template template)
        {
            Check.Require(registrationPetition != null, "Registration Petition is required.");
            Check.Require(template != null, "Template is required");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved, "template mismatch");

            _ceremony = registrationPetition.Ceremony;
            _student = registrationPetition.Registration.Student;
            _registrationPetition = registrationPetition;
            _registration = registrationPetition.Registration;
            _template = template;

            return HandleBody(template.BodyText);
        }

        public string GenerateMoveMajor(RegistrationParticipation registrationParticipation, Template template)
        {
            Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(template != null, "template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_MoveMajor, "template mismatch.");

            _ceremony = registrationParticipation.Ceremony;
            _student = registrationParticipation.Registration.Student;
            _registrationParticipation = registrationParticipation;
            _registration = registrationParticipation.Registration;
            _template = template;

            return HandleBody(template.BodyText);
        }

        public string GenerateEmailAllStudents(Ceremony ceremony, Student student, string body, TemplateType templateType, Registration registration, Attachment attachment, HttpRequestBase request, UrlHelper url)
        {
            //Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(ceremony != null, "ceremony is required");
            Check.Require(student != null, "student is required");
            Check.Require(!string.IsNullOrWhiteSpace(body), "body is required.");

            _request = request;
            _url = url;

            _ceremony = ceremony;
            _student = student;
            //_registrationParticipation = registrationParticipation;
            //_registration = registrationParticipation.Registration;
            _registration = registration ?? new Registration();
            _template = new Template(){TemplateType = templateType};
            _attachment = attachment;

            return HandleBody(body);
        }

        public bool ValidateTemplate(Template template, List<string> invalidTokens)
        {
            return ValidateBody(template.BodyText, template.TemplateType, invalidTokens);
        }

        public string GenerateVisaLetterRequestDecision(VisaLetter visaLetter, Template template, HttpRequestBase request, UrlHelper url)
        {
            _ceremony = new Ceremony(); //Don't care about ceremony
            _student = visaLetter.Student;
            _registration = new Registration(); //Don't Need
            _template = template;
            _visaLetter = visaLetter;

            _request = request;
            _url = url;

            return HandleBody(template.BodyText);
        }

        #region Main Processing Functions
        /// <summary>
        /// Takes the template text from the database and converts it to the finalized text
        /// </summary>
        /// <param name="body">Template text from the db</param>
        /// <returns>Completed text ready for exporting</returns>
        private string HandleBody(string body)
        {
            Check.Require(_ceremony != null, "_ceremony is required.");
            Check.Require(_student != null, "_student is required.");
            Check.Require(_template != null, "_template is required.");

            // Begin actual processing function
            string tempbody = "";
            string parameter;

            // Find the beginning of a replacement string
            int begindex = body.IndexOf("{");
            int endindex;
            while (begindex >= 0)
            {
                // Copy the text that comes before the replacement string to temp
                tempbody = tempbody + body.Substring(0, begindex);
                // Removes the first part from the string before the {
                body = body.Substring(begindex);

                // Find the end of a replacement string
                endindex = body.IndexOf("}");

                // Pulls the text between {}
                parameter = body.Substring(0, endindex + 1);
                // removes the parameter substring
                body = body.Substring(endindex + 1);

                tempbody = tempbody + replaceParameter(parameter);

                // Find the beginning of a replacement string
                begindex = body.IndexOf("{");
            }

            // Gets the remaining text from the template after the last tag
            tempbody = tempbody + body;

            return tempbody;
        }

        /// <summary>
        /// Returns the string data that should be replaced into the template text
        /// to create the final letter for the students.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        /// <returns>Value that should replace the parameter</returns>
        private string replaceParameter(string parameter)
        {
            // Trim the {}
            int length = parameter.Length;
            parameter = parameter.Substring(1, length - 2);

            Check.Require(_ceremony != null, "_ceremony is required.");
            Check.Require(_student != null, "_student is required.");
            Check.Require(_registration != null, "_registration is required.");

            // handle shared fields (ceremony, student and registration fields)
            switch(parameter.ToLower())
            {
                case "studentid":           return _student.StudentId;
                case "studentname":         return _student.FullName;
                case "ceremonyname":        return _ceremony.CeremonyName;
                case "ceremonytime":        return _ceremony.DateTime.ToString("f");
                case "ceremonylocation":    return _ceremony.Location;
                case "addressline1":        return _registration.Address1;
                case "addressline2":        return _registration.Address2;
                case "city":                return _registration.City;
                case "state":               return _registration.State.Id;
                case "zip":                 return _registration.Zip;
                case "specialneeds":        return string.Join(", ", _registration.SpecialNeeds);
                case "attachmentlink":      
                    if(_attachment != null)
                    {
                        return string.Format("<a href=\"{0}\">{1}</a>", GetAbsoluteUrl(_request, _url, string.Format("~/Download/Attachment/{0}", _attachment.PublicGuid)), _attachment.FileName);
                    }
                    else
                    {
                        return "Link for Attachment not found.";
                    }
            }

            if (_template == null || _template.TemplateType == null)
            {
                throw new ArgumentException(string.Format("Invalid parameter was passed. -- {0}", parameter));
            }

            // the bottom will handle fields that use either participation or petition values
            var templateName = _template.TemplateType.Name;
            if (templateName == StaticValues.Template_RegistrationConfirmation)
            {
                Check.Require(_registrationParticipation != null, "_registrationParticipation is required.");

                switch (parameter.ToLower())
                {
                    case "numberoftickets":     return _registrationParticipation.NumberTickets.ToString();
                    case "major":               return _registrationParticipation.Major.Name;
                    case "status":              return _registrationParticipation.Cancelled ? "Cancelled" : "Registered";
                    case "distributionmethod":  return _registrationParticipation.TicketDistributionMethod.Name;
                }
            }
            else if (templateName == StaticValues.Template_TicketPetition || templateName == StaticValues.Template_TicketPetition_Decision)
            {
                Check.Require(_registrationParticipation != null, "_registrationParticipation is required.");
                Check.Require(_extraTicketPetition != null, "_registrationParticipation.ExtraTicketPetition is required.");

                switch (parameter.ToLower())
                {
                    case "numberoftickets":         return _registrationParticipation.NumberTickets.ToString();
                    case "numberofextratickets":    return _extraTicketPetition.IsPending ? 
                                                            _extraTicketPetition.NumberTicketsRequested.ToString() 
                                                            : _extraTicketPetition.NumberTickets.Value.ToString();
                    case "numberstreamingtickets":  return _extraTicketPetition.IsPending ? 
                                                            _extraTicketPetition.NumberTicketsRequestedStreaming.ToString() 
                                                            : (_extraTicketPetition.NumberTicketsStreaming.HasValue ? _extraTicketPetition.NumberTicketsStreaming.Value.ToString() : "n/a");
                    case "status":                  return _extraTicketPetition.IsPending ? "Pending"
                                                            : (_extraTicketPetition.IsApproved ? "Approved" : "Denied");
                    case "major":                   return _registrationParticipation.Major.Name;
                    case "distributionmethod":      return _registrationParticipation.TicketDistributionMethod.Name;
                }
            }
            else if (templateName == StaticValues.Template_RegistrationPetition || templateName == StaticValues.Template_RegistrationPetition_Approved)
            {
                Check.Require(_registrationPetition != null, "_registrationPetition is required.");

                switch(parameter.ToLower())
                {
                    case "exceptionreason":         return _registrationPetition.ExceptionReason;
                    //case "completionterm":          return _registrationPetition.TermCodeComplete.Description;
                    case "status":                  return _registrationPetition.IsPending ? "Pending" : (_registrationPetition.IsApproved ? "Approved" : "Denied");
                    case "major":                   return _registrationPetition.MajorCode.Name;
                    case "numberoftickets":         return _registrationPetition.NumberTickets.ToString();
                    case "distributionmethod":      return _registrationPetition.TicketDistributionMethod.Name;
                }

            }
            else if (templateName == StaticValues.Template_ElectronicTicketDistribution)
            {
                switch (parameter.ToLower())
                {
                    case "ticketpassword": return _registration.TicketPassword != null ? _registration.TicketPassword : "Error Password Not Found";
                    case "login": return _student.Email;
                }
            }
            else if(templateName == StaticValues.Template_VisaLetterDecision)
            {
                Check.Require(_visaLetter != null, "_visaLetter is required.");
                switch (parameter.ToLower())
                {
                    case "linktoletter": return string.Format("<a href=\"{0}\">{1}</a>", GetAbsoluteUrl(_request, _url, string.Format("~/Student/VisaLetterPdf/{0}", _visaLetter.Id)), "VisaLetterRequest");
                    case "major": return _visaLetter.MajorName;
                    case "decision": return _visaLetter.Status;
                }
            }

            throw new ArgumentException(string.Format("Invalid parameter was passed. -- {0}", parameter));
        }

        /// <summary>
        /// Iterates through the body text and validates all tokens against list of tokens
        /// </summary>
        /// <param name="body"></param>
        /// <param name="templateType"></param>
        /// <param name="InvalidTokens"></param>
        /// <returns></returns>
        private bool ValidateBody(string body, TemplateType templateType, List<string> InvalidTokens)
        {
            // Begin actual processing function
            string tempbody = "";
            string parameter;

            // Find the beginning of a replacement string
            int begindex = body.IndexOf("{");
            int endindex;
            while (begindex >= 0)
            {
                // Copy the text that comes before the replacement string to temp
                tempbody = tempbody + body.Substring(0, begindex);
                // Removes the first part from the string before the {
                body = body.Substring(begindex);

                // Find the end of a replacement string
                endindex = body.IndexOf("}");

                // Pulls the text between {}
                parameter = body.Substring(0, endindex + 1);
                // removes the parameter substring
                body = body.Substring(endindex + 1);

                var exists = templateType.TemplateTokens.Any(a => a.Token == parameter);
                // validate the parameter value)
                if (!templateType.TemplateTokens.Any(a => a.Token == parameter))
                {
                    InvalidTokens.Add(parameter);
                }

                //tempbody = tempbody + replaceParameter(parameter);
                tempbody = tempbody + "token";

                // Find the beginning of a replacement string
                begindex = body.IndexOf("{");
            }

            return InvalidTokens.Count <= 0;
        }
        #endregion

        private string GetAbsoluteUrl(HttpRequestBase request, UrlHelper url, string relative)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Host, url.Content(relative));
        }
    }
}
