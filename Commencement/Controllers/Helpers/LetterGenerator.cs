using System;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public class LetterGenerator
    {
        private Ceremony _ceremony;
        public Student Student { get; set; }
        public Registration Registration { get; set; }
        public RegistrationPetition RegistrationPetition { get; set; }
        public ExtraTicketPetition ExtraTicketPetition { get; set; }

        public string GenerateRegistrationConfirmation(Registration registration, Template template)
        {
            Check.Require(registration != null, "Registration is required.");
            Check.Require(template != null, "Template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationConfirmation);
            Check.Require(registration.Student != null, "Student is required.");

            _ceremony = registration.Ceremony;
            Registration = registration;
            Student = registration.Student;

            return HandleBody(template.BodyText);
        }

        public string GenerateAddPermission(Student student, Template template, Ceremony ceremony)
        {
            Check.Require(student != null, "Student is required.");
            Check.Require(template != null, "Template is required");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved);

            _ceremony = ceremony;
            Student = student;

            return HandleBody(template.BodyText);
        }

        public string GenerateRegistrationPetitionConfirmation(RegistrationPetition registrationPetition, Template template)
        {
            Check.Require(registrationPetition != null, "Registration Petition is required.");
            Check.Require(template != null, "Template is required");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationPetition);

            RegistrationPetition = registrationPetition;

            return HandleBody(template.BodyText);
        }

        public string GenerateRegistrationPetitionApproved(RegistrationPetition registrationPetition, Template template)
        {
            Check.Require(registrationPetition != null, "Registration Petition is required.");
            Check.Require(template != null, "Template is required");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationPetition_Approved);

            RegistrationPetition = registrationPetition;

            return HandleBody(template.BodyText);
        }

        public string GenerateExtraTicketRequestPetitionConfirmation(Registration registration, Template template)
        {
            Check.Require(registration != null, "Registration is required.");
            Check.Require(template != null, "Template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_TicketPetition);

            Registration = registration;
            _ceremony = registration.Ceremony;
            Student = registration.Student;
            ExtraTicketPetition = registration.ExtraTicketPetition;

            return HandleBody(template.BodyText);
        }

        public string GenerateExtraTicketRequestPetitionDecision(Registration registration, Template template)
        {
            Check.Require(registration != null, "Registration is required.");
            Check.Require(template != null, "Template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_TicketPetition_Decision);

            Registration = registration;
            _ceremony = registration.Ceremony;
            Student = registration.Student;
            ExtraTicketPetition = registration.ExtraTicketPetition;

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
            // Begin actual processing function
            string tempbody = "";
            string parameter;

            // Find the beginning of a replacement string
            int begindex = body.IndexOf("{");
            int endindex;
            while (begindex > 0)
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
            Ceremony ceremony = _ceremony;

            //if (Registration != null) ceremony = Registration.Ceremony;
            //else if (RegistrationPetition != null) ceremony = RegistrationPetition.Ceremony;
            //else if (ExtraTicketPetition != null) ceremony = ExtraTicketPetition.Registration.Ceremony;

            // Trim the {}
            int length = parameter.Length;
            parameter = parameter.Substring(1, length - 2);

            switch (parameter.ToLower())
            {
                case "studentid":
                    if (Student != null) return Student.StudentId;
                    if (RegistrationPetition != null) return RegistrationPetition.StudentId;

                    throw new ArgumentException("No valid object was provided.");
                case "studentname":
                    if (Student != null) return Student.FullName;
                    if (RegistrationPetition != null) return RegistrationPetition.FirstName + " " + RegistrationPetition.LastName;

                    throw new ArgumentException("No valid object was provided.");
                case "ceremonyname":
                    if (ceremony == null) throw new ArgumentException("No valid object was provided.");

                    return ceremony.Name;
                case "ceremonytime":
                    if (ceremony == null) throw new ArgumentException("No valid object was provided.");

                    return ceremony.DateTime.ToString("f");
                case "ceremonylocation":
                    if (ceremony == null) throw new ArgumentException("No valid object was provided.");

                    return ceremony.Location;
                case "numberoftickets":

                    if (Registration != null) return Registration.TotalTickets.ToString();

                    throw new ArgumentException("No valid object was provided.");
                case "petitiondecision":

                    if (ExtraTicketPetition != null) return ExtraTicketPetition.IsApproved ? "Approved" : "Denied";
                    if (RegistrationPetition != null) return RegistrationPetition.IsApproved ? "Approved" : "Denied";

                    throw new ArgumentException("No valid object was provided.");
                case "numberofextratickets":
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.NumberTickets.ToString();

                    throw new ArgumentException("Extra Ticket Petition was missing");
                case "addressline1":

                    if (Registration != null) return Registration.Address1;

                    throw new ArgumentException("No valid object was provided.");
                case "addressline2":

                    if (Registration != null) return Registration.Address2;

                    throw new ArgumentException("No valid object was provided.");
                case "city":

                    if (Registration != null) return Registration.City;

                    throw new ArgumentException("No valid object was provided.");
                case "state":

                    if (Registration != null) return Registration.State.Id;

                    throw new ArgumentException("No valid object was provided.");
                case "zip":

                    if (Registration != null) return Registration.Zip;

                    throw new ArgumentException("No valid object was provided.");
                case "distributionmethod":
                    Check.Require(Registration != null, "Registration is required.");

                    return Registration.TicketDistributionMethod;
                case "specialneeds":
                    Check.Require(Registration != null, "Registration is required.");

                    return Registration.Comments;
                case "major":
                    Check.Require(Registration != null, "Registration is required.");

                    return Registration.Major.Name;
                case "exceptionreason":
                    Check.Require(RegistrationPetition != null, "Registration petition is required.");

                    return RegistrationPetition.ExceptionReason;
                case "completionterm":
                    Check.Require(RegistrationPetition != null, "Registration petition is required.");

                    return RegistrationPetition.CompletionTerm;
                default: return "";
            }

            throw new ArgumentException("Parameter does not exist(" + parameter + ")");
        }
        #endregion
    }
}
