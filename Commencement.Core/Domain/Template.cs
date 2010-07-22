using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Template : DomainObject
    {
        public Template(string bodyText, bool registrationConfirmation, bool registrationPetition, bool extraTicketPetition)
        {
            BodyText = bodyText;
            RegistrationConfirmation = registrationConfirmation;
            RegistrationPetition = registrationPetition;
            ExtraTicketPetition = extraTicketPetition;
        }

        public Template()
        {
            RegistrationConfirmation = false;
            RegistrationPetition = false;
            ExtraTicketPetition = false;
        }

        [Required]
        public virtual string BodyText { get; set; }

        public virtual bool RegistrationConfirmation { get; set; }
        public virtual bool RegistrationPetition { get; set; }
        public virtual bool ExtraTicketPetition { get; set; }

        [AssertTrue(Message="Only one template type can be selected.")]
        public virtual bool OnlyOneBool
        {
            get
            {
                return RegistrationConfirmation != RegistrationPetition != ExtraTicketPetition;
            }
        }
    }
}
