using System;
using Commencement.Core.Domain;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public class LetterGenerator
    {
        public Student Student { get; set; }
        public Registration Registration { get; set; }
        public RegistrationPetition RegistrationPetition { get; set; }
        public ExtraTicketPetition ExtraTicketPetition { get; set; }

        public string GenerateRegistrationConfirmation(Registration registration, Template template)
        {
            Check.Require(registration != null, "Registration is required.");
            Check.Require(template != null, "Template is required.");
            Check.Require(template.RegistrationConfirmation, "Incorrect template supplied.");
            Check.Require(template.OnlyOneBool, "Template has more than one purpose supplied.");
            Check.Require(registration.Student != null, "Student is required.");

            Registration = registration;
            Student = registration.Student;

            return HandleBody(template.BodyText);
        }

        public string GenerateRegistrationPetitionConfirmation(RegistrationPetition registrationPetition, Template template)
        {
            throw new NotImplementedException();
        }

        public string GenerateExtraTicketRequestPetitionConfirmation(ExtraTicketPetition extraTicketPetition, Template template)
        {
            throw new NotImplementedException();
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
            Ceremony ceremony = null;

            if (Registration != null) ceremony = Registration.Ceremony;
            else if (RegistrationPetition != null) ceremony = RegistrationPetition.Ceremony;
            else if (ExtraTicketPetition != null) ceremony = ExtraTicketPetition.Ceremony;

            // Trim the {}
            int length = parameter.Length;
            parameter = parameter.Substring(1, length - 2);

            switch (parameter.ToLower())
            {
                case "StudentId":
                    return Student.StudentId;
                    break;
                case "StudentName":
                    return Student.FullName;
                    break;
                case "CeremonyName":
                    if (ceremony == null) throw new ArgumentException("No valid object was provided.");

                    return ceremony.Name;
                    break;
                case "CeremonyTime":
                    if (ceremony == null) throw new ArgumentException("No valid object was provided.");

                    return ceremony.DateTime.ToString();
                    break;
                case "CeremonyLocation":
                    if (ceremony == null) throw new ArgumentException("No valid object was provided.");

                    return ceremony.Location;
                    break;
                case "NumberOfTickets":

                    if (Registration != null) return Registration.NumberTickets.ToString();
                    if (RegistrationPetition != null) return RegistrationPetition.NumberTickets.ToString();
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.NumberTickets.ToString();

                    throw new ArgumentException("No valid object was provided.");

                    break;
                case "AddressLine1":

                    if (Registration != null) return Registration.Address1;
                    if (RegistrationPetition != null) return RegistrationPetition.Address1;
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.Address1;

                    throw new ArgumentException("No valid object was provided.");

                    break;
                case "AddressLine2":

                    if (Registration != null) return Registration.Address2;
                    if (RegistrationPetition != null) return RegistrationPetition.Address2;
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.Address2;

                    throw new ArgumentException("No valid object was provided.");

                    break;
                case "City":

                    if (Registration != null) return Registration.City;
                    if (RegistrationPetition != null) return RegistrationPetition.City;
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.City;

                    throw new ArgumentException("No valid object was provided.");

                    break;
                case "State":

                    if (Registration != null) return Registration.State.Id;
                    if (RegistrationPetition != null) return RegistrationPetition.State.Id;
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.State.Id;

                    throw new ArgumentException("No valid object was provided.");

                    break;
                case "Zip":

                    if (Registration != null) return Registration.Zip;
                    if (RegistrationPetition != null) return RegistrationPetition.Zip;
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.Zip;

                    throw new ArgumentException("No valid object was provided.");

                    break;
                case "TicketReceiveMethod":

                    var mailTickets = false;

                    if (Registration != null) mailTickets = Registration.MailTickets;
                    else if (RegistrationPetition != null) mailTickets = RegistrationPetition.MailTickets;
                    else if (ExtraTicketPetition != null) mailTickets = ExtraTicketPetition.MailTickets;
                    else throw new ArgumentException("No valid object was provided.");

                    return mailTickets ? "Mail" : "Pick-Up";
                    break;
                case "SpecialNeeds":
                    Check.Require(Registration != null, "Registration is required.");

                    return Registration.Comments;
                    break;
                case "ExceptionReason":
                    Check.Require(RegistrationPetition != null, "Registration petition is required.");

                    return RegistrationPetition.ExceptionReason;
                    break;
                case "CompletionTerm":
                    Check.Require(RegistrationPetition != null, "Registration petition is required.");

                    return RegistrationPetition.CompletionTerm;
                    break;
                default: return "";
            }

            throw new ArgumentException("Parameter does not exist(" + parameter + ")");
        }
        #endregion
    }
}
