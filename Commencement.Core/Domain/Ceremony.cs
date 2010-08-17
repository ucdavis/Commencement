using System;
using System.Collections.Generic;
using System.Text;
using UCDArch.Core.NHibernateValidator.Extensions;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using System.Linq;

namespace Commencement.Core.Domain
{
    public class Ceremony : DomainObject
    {
        public Ceremony(string location, DateTime dateTime, int ticketsPerStudent, int totalTickets, DateTime printingDeadline, DateTime registrationDeadline, TermCode termCode)
        {
            SetDefaults();

            Location = location;
            DateTime = dateTime;
            TicketsPerStudent = ticketsPerStudent;
            TicketsPerStudent = totalTickets;
            PrintingDeadline = printingDeadline;
            RegistrationDeadline = registrationDeadline;
            TermCode = termCode;
        }

        public Ceremony()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Registrations = new List<Registration>();
            Majors = new List<MajorCode>();
            DateTime = DateTime.Now;
            RegistrationDeadline = DateTime.Now;
        }

        [Required]
        [Length(200)]
        public virtual string Location { get; set; }
        
        [NotNull]
        [Future]
        public virtual DateTime DateTime { get; set; }

        [NotNull]
        [Min(1)]
        public virtual int TicketsPerStudent { get; set; }

        [NotNull]
        [Min(1)]
        public virtual int TotalTickets { get; set; }

        [NotNull]
        public virtual DateTime PrintingDeadline { get; set; }
        [NotNull]
        public virtual DateTime RegistrationDeadline { get; set; }

        [NotNull]
        public virtual TermCode TermCode { get; set; }

        public virtual IList<Registration> Registrations { get; set; }
        public virtual IList<MajorCode> Majors { get; set; }

        public virtual string Name { 
            get
            {
                var sb = new StringBuilder("CA&ES");

                if (this.TermCode.Id.EndsWith("03")) sb.Append(" Spring");
                else if (this.TermCode.Id.EndsWith("10")) sb.Append(" Fall");

                sb.Append(" Commencement");
                sb.Append(" " + this.TermCode.Id.Substring(0, 4));

                return sb.ToString();
            }
        }

        public virtual int AvailableTickets { 
            get
            {
                return TotalTickets - Registrations.Sum(a => a.TotalTickets);
            } 
        }
    }
}
