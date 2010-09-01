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
            TotalTickets = totalTickets;
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
            RegistrationPetitions = new List<RegistrationPetition>();
            DateTime = DateTime.Now;
            RegistrationDeadline = DateTime.Now;
            ExtraTicketDeadline = DateTime.Now;
            PrintingDeadline = DateTime.Now;
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
        public virtual DateTime ExtraTicketDeadline { get; set; }

        public virtual int ExtraTicketPerStudent { get; set; }

        [NotNull]
        public virtual TermCode TermCode { get; set; }

        [NotNull]
        public virtual IList<Registration> Registrations { get; set; }
        [NotNull]
        public virtual IList<MajorCode> Majors { get; set; }
        [NotNull]
        public virtual IList<RegistrationPetition> RegistrationPetitions { get; set; }


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

        /// <summary>
        /// # available tickets
        /// </summary>
        public virtual int AvailableTickets { 
            get
            {
                return TotalTickets - Registrations.Where(a=>!a.SjaBlock).Sum(a => a.TotalTickets);
            } 
        }

        /// <summary>
        /// # of tickets requested by original registration
        /// </summary>
        public virtual int RequestedTickets
        {
            get { return Registrations.Where(a => !a.SjaBlock).Sum(a => a.NumberTickets); }
        }

        /// <summary>
        /// # of tickets requested by extra ticket petitions
        /// </summary>
        public virtual int ExtraRequestedtickets
        {
            get
            {
                return Registrations.Where(a => a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsApproved && !a.ExtraTicketPetition.IsPending && !a.SjaBlock)
                                    .Sum(a => a.ExtraTicketPetition.NumberTickets);
            }
        }

        public virtual int TotalRequestedTickets
        {
            get
            {
                return Registrations.Where(a => !a.SjaBlock).Sum(a => a.TotalTickets);
            }
        }
    }
}
