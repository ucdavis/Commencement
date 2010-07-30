using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Registration : DomainObject
    {
        [NotNull]
        public virtual Student Student { get; set; }
        [NotNull]
        public virtual MajorCode Major { get; set; }

        [Required]
        [Length(200)]
        public virtual string Address1 { get; set; }
        [Length(200)]
        public virtual string Address2 { get; set; }
        [Length(200)]
        public virtual string Address3 { get; set; }
        [Required]
        [Length(100)]
        public virtual string City { get; set; }
        [NotNull]
        public virtual State State { get; set; }
        [Required]
        [Length(15)]
        public virtual string Zip { get; set; }

        [Length(100)]
        [Email]
        public virtual string Email { get; set; }
        
        [Min(1)]
        public virtual int NumberTickets { get; set; }
        
        public virtual bool MailTickets { get; set; }

        [Length(1000, Message = "Please enter less than 1,000 characters")]
        public virtual string Comments { get; set; }
        
        [NotNull]
        public virtual Ceremony Ceremony { get; set; }

        public virtual string TicketDistributionMethod { 
            get {
                return MailTickets ? "Mail tickets to provided address" : "Pickup tickets at Arc Ticket Office";
            }
        }

        // total number of tickets given to student, includes count from extra ticket petition
        public virtual int TotalTickets { get { return NumberTickets; } }
    }
}
