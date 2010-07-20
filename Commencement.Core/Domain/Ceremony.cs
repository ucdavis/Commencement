using System;
using System.Collections.Generic;
using UCDArch.Core.NHibernateValidator.Extensions;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Ceremony : DomainObject
    {
        public Ceremony()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Registrations = new List<Registration>();
            Majors = new List<MajorCode>();
        }

        [Required]
        [Length(200)]
        public virtual string Location { get; set; }
        
        [NotNull]
        public virtual DateTime DateTime { get; set; }

        [NotNull]
        [Min(1)]
        public virtual int TicketsPerStudent { get; set; }

        [NotNull]
        [Min(1)]
        public virtual int TotalTickets { get; set; }
        
        [NotNull]
        public virtual DateTime RegistrationDeadline { get; set; }

        [NotNull]
        public virtual TermCode TermCode { get; set; }

        public virtual IList<Registration> Registrations { get; set; }
        public virtual IList<MajorCode> Majors { get; set; }
    }
}
