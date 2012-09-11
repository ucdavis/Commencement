using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public interface ILetterGenerator
    {
        string GenerateRegistrationConfirmation(RegistrationParticipation registrationParticipation, Template template);
        string GenerateExtraTicketRequestPetitionConfirmation(RegistrationParticipation registrationParticipation, Template template);
        string GenerateExtraTicketRequestPetitionDecision(RegistrationParticipation registrationParticipation, Template template);
        //string GenerateExtraTicketRequestPetitionDecision(RegistrationParticipation registrationParticipation, Template template);
        //string GenerateAddPermission(Student student, Template template, Ceremony ceremony);
        string GenerateRegistrationPetitionConfirmation(RegistrationPetition registrationPetition, Template template);
        string GenerateRegistrationPetitionApproved(RegistrationPetition registrationPetition, Template template);
        string GenerateMoveMajor(RegistrationParticipation registrationParticipation, Template template);
        string GenerateEmailAllStudents(Ceremony ceremony, Student student, string body, TemplateType templateType);
        bool ValidateTemplate(Template template, List<string> invalidTokens);
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

        public string GenerateRegistrationConfirmation(RegistrationParticipation registrationParticipation, Template template)
        {
            Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(registrationParticipation.Registration.Student != null, "registrationParticipation.Registration.Student is required.");
            Check.Require(template != null, "template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationConfirmation, "Template mismatch.");
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

        public string GenerateExtraTicketRequestPetitionDecision(RegistrationParticipation registrationParticipation, Template template)
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
            _extraTicketPetition = registrationParticipation.ExtraTicketPetition;

            return HandleBody(template.BodyText);            
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

        public string GenerateEmailAllStudents(Ceremony ceremony, Student student, string body, TemplateType templateType)
        {
            //Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(ceremony != null, "ceremony is required");
            Check.Require(student != null, "student is required");
            Check.Require(!string.IsNullOrWhiteSpace(body), "body is required.");

            _ceremony = ceremony;
            _student = student;
            //_registrationParticipation = registrationParticipation;
            //_registration = registrationParticipation.Registration;
            _registration = new Registration();
            _template = new Template(){TemplateType = templateType};

            return HandleBody(body);
        }

        public bool ValidateTemplate(Template template, List<string> invalidTokens)
        {
            return ValidateBody(template.BodyText, template.TemplateType, invalidTokens);
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
                case "distributionmethod":  return _registration.TicketDistributionMethod;
                case "specialneeds":        return string.Join(", ", _registration.SpecialNeeds);
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
                }
            }
            else if (templateName == StaticValues.Template_RegistrationPetition || templateName == StaticValues.Template_RegistrationPetition_Approved)
            {
                Check.Require(_registrationPetition != null, "_registrationPetition is required.");

                switch(parameter.ToLower())
                {
                    case "exceptionreason":         return _registrationPetition.ExceptionReason;
                    case "completionterm":          return _registrationPetition.TermCodeComplete.Description;
                    case "status":                  return _registrationPetition.IsPending ? "Pending" : (_registrationPetition.IsApproved ? "Approved" : "Denied");
                    case "major":                   return _registrationPetition.MajorCode.Name;
                    case "numberoftickets":         return _registrationPetition.NumberTickets.ToString();
                }

            }

            throw new ArgumentException("Invalid parameter was passed.");
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
    }
}
