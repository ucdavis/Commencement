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

        public ExtraTicketPetition(int numberTickets, int numberTicketsStreaming = 0)
        {
            NumberTicketsRequested = numberTickets;
            NumberTicketsRequestedStreaming = numberTicketsStreaming;

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
        public virtual int NumberTicketsRequested { get; set; }
        public virtual int NumberTicketsRequestedStreaming { get; set; }
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

        public virtual int TotalTicketsRequested
        {
            get { return NumberTicketsRequested + NumberTicketsRequestedStreaming; }
        }

        public virtual int TotalTickets
        {
            get { return (NumberTickets.HasValue ? NumberTickets.Value: 0) + (NumberTicketsStreaming.HasValue ? NumberTicketsStreaming.Value : 0); }
        }
    }

    public class ExtraTicketPetitionMap : ClassMap<ExtraTicketPetition>
    {
        public ExtraTicketPetitionMap()
        {
            Id(x => x.Id);

            Map(x => x.NumberTicketsRequested);
            Map(x => x.NumberTicketsRequestedStreaming);
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