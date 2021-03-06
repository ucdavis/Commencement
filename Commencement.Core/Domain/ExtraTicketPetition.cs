﻿using System;
using System.ComponentModel.DataAnnotations;
using Commencement.Core.Helpers;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class ExtraTicketPetition : DomainObject
    {
        #region Constructors
        public ExtraTicketPetition()
        {
            SetDefaults();
        }

        public ExtraTicketPetition(int numberTickets, string reason, int numberTicketsStreaming = 0)
        {
            NumberTicketsRequested = numberTickets;
            NumberTicketsRequestedStreaming = numberTicketsStreaming;
            Reason = reason;

            SetDefaults();
        }

        private void SetDefaults()
        {
            IsPending = true;
            IsApproved = false;
            LabelPrinted = false;

            DateSubmitted = DateTime.UtcNow.ToPacificTime();
            DateDecision = null;
        }
        #endregion

        #region Mapped Fields
        /// <summary>
        /// Number tickets requested
        /// </summary>
        [Min(0, ErrorMessage = "Can not enter negative numbers")]
        public virtual int NumberTicketsRequested { get; set; }
        [Min(0, ErrorMessage = "Can not enter negative numbers")]
        public virtual int NumberTicketsRequestedStreaming { get; set; }
        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }

        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }

        public virtual bool LabelPrinted { get; set; }

        /// <summary>
        /// Number tickets approved
        /// </summary>
        [Min(0, ErrorMessage = "Can not enter negative numbers")]
        public virtual int? NumberTickets { get; set; }
        /// <summary>
        /// Number of streaming tickets approved
        /// </summary>
        [Min(0, ErrorMessage = "Can not enter negative numbers")]
        public virtual int? NumberTicketsStreaming { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string Reason { get; set; }
        #endregion

        #region  Extended/Calculated Fields
        /// <summary>
        /// Total # of tickets that were originally requested by the student
        /// </summary>
        public virtual int TotalTicketsRequested
        {
            get { return NumberTicketsRequested + NumberTicketsRequestedStreaming; }
        }

        /// <summary>
        /// Total # of tickets approved, if status is approved
        /// </summary>
        public virtual int TotalTickets
        {
            get { return (NumberTickets.HasValue ? NumberTickets.Value: 0) + (NumberTicketsStreaming.HasValue ? NumberTicketsStreaming.Value : 0); }
        }

        public virtual void MakeDecision(bool isApproved)
        {
            IsPending = false;
            LabelPrinted = false;
            IsApproved = isApproved;
            DateDecision = DateTime.UtcNow.ToPacificTime();

            if (!NumberTickets.HasValue) NumberTickets = NumberTicketsRequested;
            if (!NumberTicketsStreaming.HasValue) NumberTicketsStreaming = NumberTicketsRequestedStreaming;
        }

        public virtual bool IsApprovedCompletely
        {
            get { return IsApproved && !IsPending; }
        }

        public virtual int ApprovedTickets
        {
            get
            {
                if (IsApprovedCompletely && NumberTickets.HasValue)
                {
                    return NumberTickets.Value;
                }
                return 0;
            }
        }
        public virtual int ApprovedStreamingTickets
        {
            get
            {
                if (IsApprovedCompletely && NumberTicketsStreaming.HasValue)
                {
                    return NumberTicketsStreaming.Value;
                }
                return 0;
            }
        }

        public virtual int ProjectedTickets
        {
            get
            {
                //If it is approved and not pending return the number approved
                if (IsApprovedCompletely)
                {
                    return NumberTickets.HasValue ? NumberTickets.Value : 0;
                }
                //If it is still pending return the number that may be approved if it isn't null, or the number requested
                if (IsPending)
                {
                    if (NumberTickets.HasValue) 
                    {
                        return NumberTickets.Value;
                    }
                    else
                    {
                        return NumberTicketsRequested;
                    }
                }
                return 0;
            }
        }

        public virtual int ProjectedStreamingTickets
        {
            get
            {
                //If it is approved and not pending return the number approved
                if (IsApprovedCompletely)
                {
                    return NumberTicketsStreaming.HasValue ? NumberTicketsStreaming.Value : 0;
                }
                //If it is still pending return the number that may be approved if it isn't null, or the number requested
                if (IsPending)
                {
                    if (NumberTicketsStreaming.HasValue) 
                    {
                        return NumberTicketsStreaming.Value;
                    }
                    else
                    {
                        return NumberTicketsRequestedStreaming;
                    }
                }
                return 0;
            }
        }

        public virtual string Status { 
            get { return IsPending ? "Pending" : (IsApproved ? "Approved" : "Denied"); }
        }
        #endregion
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