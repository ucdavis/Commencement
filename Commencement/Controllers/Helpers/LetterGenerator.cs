﻿using System;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public class LetterGenerator
    {
        private Ceremony _ceremony;
        public Student Student { get; set; }
        //public Registration Registration { get; set; }  // probably not needed
        public RegistrationPetition RegistrationPetition { get; set; }
        public ExtraTicketPetition ExtraTicketPetition { get; set; }

        public RegistrationParticipation RegistrationParticipation { get; set; }

        public string GenerateRegistrationConfirmation(RegistrationParticipation registrationParticipation, Template template)
        {
            Check.Require(registrationParticipation != null, "registrationParticipation is required.");
            Check.Require(template != null, "template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_RegistrationConfirmation, "Template mismatch.");

            _ceremony = registrationParticipation.Ceremony;
            Student = registrationParticipation.Registration.Student;
            RegistrationParticipation = registrationParticipation;

            return HandleBody(template.BodyText);
        }

        public string GenerateExtraTicketRequestPetitionConfirmation(Registration registration, Template template)
        {
            Check.Require(registration != null, "Registration is required.");
            Check.Require(template != null, "Template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_TicketPetition);

            //Registration = registration;
            _ceremony = registration.RegistrationParticipations[0].Ceremony;
            Student = registration.Student;
            //ExtraTicketPetition = registration.ExtraTicketPetition;

            return HandleBody(template.BodyText);
        }

        public string GenerateExtraTicketRequestPetitionDecision(Registration registration, Template template)
        {
            Check.Require(registration != null, "Registration is required.");
            Check.Require(template != null, "Template is required.");
            Check.Require(template.TemplateType.Name == StaticValues.Template_TicketPetition_Decision);

            //Registration = registration;
            _ceremony = registration.RegistrationParticipations[0].Ceremony;
            Student = registration.Student;
            //ExtraTicketPetition = registration.ExtraTicketPetition;

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
                    if (RegistrationPetition != null)
                        return string.Format("{0} {1}", RegistrationPetition.FirstName, RegistrationPetition.LastName);

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
                case "numberoftickets": // only number tickets from original registration
                    if (RegistrationParticipation != null) return RegistrationParticipation.NumberTickets.ToString();

                    throw new ArgumentException("No valid object was provided.");
                case "petitiondecision":
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.IsApproved ? "Approved" : "Denied";
                    if (RegistrationPetition != null) return RegistrationPetition.IsApproved ? "Approved" : "Denied";

                    throw new ArgumentException("No valid object was provided.");
                case "numberofextratickets":
                    if (ExtraTicketPetition != null) return ExtraTicketPetition.NumberTickets.ToString();

                    throw new ArgumentException("Extra Ticket Petition was missing");
                case "addressline1":

                    if (RegistrationParticipation != null) return RegistrationParticipation.Registration.Address1;

                    throw new ArgumentException("No valid object was provided.");
                case "addressline2":

                    if (RegistrationParticipation != null) return RegistrationParticipation.Registration.Address2;

                    throw new ArgumentException("No valid object was provided.");
                case "city":

                    if (RegistrationParticipation != null) return RegistrationParticipation.Registration.City;

                    throw new ArgumentException("No valid object was provided.");
                case "state":

                    if (RegistrationParticipation != null) return RegistrationParticipation.Registration.State.Id;

                    throw new ArgumentException("No valid object was provided.");
                case "zip":

                    if (RegistrationParticipation != null) return RegistrationParticipation.Registration.Zip;

                    throw new ArgumentException("No valid object was provided.");
                case "distributionmethod":
                    Check.Require(RegistrationParticipation != null, "Registration participation is required.");

                    return RegistrationParticipation.Registration.TicketDistributionMethod;
                case "specialneeds":
                    Check.Require(RegistrationParticipation != null, "Registration participation is required.");

                    return string.Join(", ", RegistrationParticipation.Registration.SpecialNeeds);
                case "major":
                    Check.Require(RegistrationParticipation != null, "Registration participation is required.");

                    return RegistrationParticipation.Major.MajorName;
                case "exceptionreason":
                    Check.Require(RegistrationPetition != null, "Registration petition is required.");

                    return RegistrationPetition.ExceptionReason;
                case "completionterm":
                    Check.Require(RegistrationPetition != null, "Registration petition is required.");

                    return RegistrationPetition.CompletionTerm;
                case "status":
                    if (RegistrationParticipation != null) { return RegistrationParticipation.Cancelled ? "Cancelled" : "Registered"; }

                    throw new ArgumentException("No valid object was provided.");

                default: return "";
            }

            throw new ArgumentException("Parameter does not exist(" + parameter + ")");
        }
        #endregion
    }
}
