using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class RegistrationParticipation : DomainObject
    {
        public RegistrationParticipation()
        {
            Cancelled = false;
            LabelPrinted = false;
            DateRegistered = DateTime.Now;
            DateUpdated = DateTime.Now;
        }

        public virtual Registration Registration { get; set; }
        public virtual MajorCode Major { get; set; }
        public virtual Ceremony Ceremony { get; set; }
        public virtual ExtraTicketPetition ExtraTicketPetition { get; set; }

        public virtual int NumberTickets { get; set; }
        public virtual bool Cancelled { get; set; }
        public virtual bool LabelPrinted { get; set; }
        public virtual DateTime DateRegistered { get; set; }
        public virtual DateTime DateUpdated { get; set; }

        public virtual string TicketDistribution
        {
            get
            {
                var message = string.Empty;
                if (DateRegistered > Ceremony.PrintingDeadline)
                    message = "Pickup Tickets in person as specified in web site faq.";
                else if (Registration.MailTickets)
                    message = "Mail tickets to provided address.";
                else message = "Pickup tickets at arc ticket off";

                return message;
            }
        }
    }

    public class RegistrationParticipationMap : ClassMap<RegistrationParticipation>
    {
        public RegistrationParticipationMap()
        {
            Id(x => x.Id);

            References(x => x.Registration).Cascade.None().Fetch.Join();
            References(x => x.Major).Column("MajorCode").Cascade.None().Fetch.Join();
            References(x => x.Ceremony).Cascade.None().Fetch.Join();
            References(x => x.ExtraTicketPetition);

            Map(x => x.NumberTickets);
            Map(x => x.Cancelled);
            Map(x => x.LabelPrinted);
            Map(x => x.DateRegistered);
            Map(x => x.DateUpdated);
        }
    }
}
