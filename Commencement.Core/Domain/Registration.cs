using System;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Registration : DomainObject
    {
        public Registration()
        {
            DateRegistered = DateTime.Now;
        }

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

        public virtual ExtraTicketPetition ExtraTicketPetition { get; set; }

        public virtual DateTime DateRegistered { get; set; }

        public virtual bool LabelPrinted { get; set; }

        public virtual string TicketDistributionMethod { 
            get {
                return MailTickets ? "Mail tickets to provided address" :
                    (Ceremony.PrintingDeadline > DateTime.Now ? "Pickup tickets at Arc Ticket Office" : "Pickup tickets in person as specified in web site FAQ");
            }
        }

        // total number of tickets given to student, includes count from extra ticket petition
        public virtual int TotalTickets
        {
            get
            {
                // sja blocked or cancelled no tickets given
                if (SjaBlock || Cancelled) return 0;
                
                var extraTickets = ExtraTicketPetition != null && !ExtraTicketPetition.IsPending &&
                                   ExtraTicketPetition.IsApproved
                                       ? ExtraTicketPetition.NumberTickets
                                       : 0;

                return NumberTickets + extraTickets; 
            }
        }

        public virtual void SetLabelPrinted()
        {
            LabelPrinted = true;
            if (ExtraTicketPetition != null) ExtraTicketPetition.LabelPrinted = true;
        }

        public virtual bool SjaBlock { get; set; }
        public virtual bool Cancelled { get; set; }
        public virtual College College { get; set; }
    }

    public class RegistrationMap : ClassMap<Registration>
    {
        public RegistrationMap()
        {
            Id(x => x.Id);

            References(x => x.Student).Column("Student_Id").Fetch.Join();
            References(x => x.Major).Column("MajorCode");
            References(x => x.State).Column("State");
            References(x => x.Ceremony);
            References(x => x.ExtraTicketPetition).Cascade.All();
            References(x => x.College).Column("CollegeCode");

            Map(x => x.Address1);
            Map(x => x.Address2);
            Map(x => x.Address3);
            Map(x => x.City);
            Map(x => x.Zip);
            Map(x => x.Email);
            Map(x => x.NumberTickets);
            Map(x => x.MailTickets);
            Map(x => x.Comments);
            Map(x => x.DateRegistered);
            Map(x => x.LabelPrinted);
            Map(x => x.SjaBlock);
            Map(x => x.Cancelled);
        }
    }
}
