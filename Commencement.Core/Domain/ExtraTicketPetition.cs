using System;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class ExtraTicketPetition : DomainObject
    {
        public ExtraTicketPetition()
        {
            SetDefaults();
        }

        public ExtraTicketPetition(int numberTickets)
        {
            NumberTickets = numberTickets;

            SetDefaults();
        }

        private void SetDefaults()
        {
            IsPending = true;
            IsApproved = false;
            LabelPrinted = false;

            DateSubmitted = DateTime.Now;
            DateDecision = null;
        }

        /// <summary>
        /// Number tickets requested
        /// </summary>
        [Min(1)]
        public virtual int NumberTicketsRequested { get; set; }
        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }

        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }

        public virtual bool LabelPrinted { get; set; }

        /// <summary>
        /// Number tickets approved
        /// </summary>
        public virtual int? NumberTickets { get; set; }
        /// <summary>
        /// Number of streaming tickets approved
        /// </summary>
        public virtual int? NumberTicketsStreaming { get; set; }
        [Required]
        [Length(100)]
        public virtual string Reason { get; set; }
    }

    public class ExtraTicketPetitionMap : ClassMap<ExtraTicketPetition>
    {
        public ExtraTicketPetitionMap()
        {
            Id(x => x.Id);

            Map(x => x.NumberTicketsRequested);
            Map(x => x.IsPending);
            Map(x => x.IsApproved);
            Map(x => x.DateSubmitted);
            Map(x => x.DateDecision);
            Map(x => x.LabelPrinted);

            Map(x => x.NumberTickets);
            Map(x => x.NumberTicketsStreaming);
            Map(x => x.Reason);
        }
    }
}