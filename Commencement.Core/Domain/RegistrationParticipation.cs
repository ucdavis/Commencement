﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Commencement.Core.Helpers;
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
            DateRegistered = DateTime.UtcNow.ToPacificTime();
            DateUpdated = DateTime.UtcNow.ToPacificTime();
            ExitSurvey = false;

            TransferRequests = new List<TransferRequest>();
        }
        [Required]
        public virtual Registration Registration { get; set; }
        [Required]
        public virtual MajorCode Major { get; set; }
        [Required]
        public virtual Ceremony Ceremony { get; set; }
        public virtual ExtraTicketPetition ExtraTicketPetition { get; set; }

        public virtual int NumberTickets { get; set; }
        public virtual bool Cancelled { get; set; }
        public virtual bool LabelPrinted { get; set; }
        public virtual DateTime DateRegistered { get; set; }
        public virtual DateTime DateUpdated { get; set; }

        public virtual bool ExitSurvey { get; set; }

        public virtual TicketDistributionMethod TicketDistributionMethod { get; set; }

        public virtual IList<TransferRequest> TransferRequests { get; set; }

        #region Extended Fields / Methods
        public virtual string TicketDistribution
        {
            get
            {
                var message = string.Empty;
                if (DateRegistered > Ceremony.PrintingDeadline)
                    message = "Pickup Tickets in person as specified in web site faq.";
                else if (Registration.MailTickets)
                    message = "Mail tickets to provided address.";
                else message = "Pickup tickets at arc ticket office.";

                return message;
            }
        }

        /// <summary>
        /// Deteremines if there are any factors that would exclude this registration from ticket count
        /// </summary>
        public virtual bool IsValidForTickets
        { 
            get { return !Cancelled && !Registration.Student.SjaBlock && !Registration.Student.Blocked; }
        }

        /// <summary>
        /// Returns total number of tickets including extra ticket petition tickets if any
        /// </summary>
        public virtual int TotalTickets
        {
            get
            {
                if (IsValidForTickets)
                {
                    var ticketCount = NumberTickets;
                    //ticketCount += (ExtraTicketPetition != null && ExtraTicketPetition.IsApproved && !ExtraTicketPetition.IsPending &&
                    //                ExtraTicketPetition.NumberTickets.HasValue
                    //                    ? ExtraTicketPetition.NumberTickets.Value
                    //                    : 0);
                    ticketCount += (ExtraTicketPetition != null ? ExtraTicketPetition.ApprovedTickets : 0);
                    return ticketCount;
                }

                return 0;
            }
        }

        /// <summary>
        /// Returns total number tickets for streaming that are approved
        /// </summary>
        public virtual int TotalStreamingTickets
        {
            get 
            {
                if (Ceremony.HasStreamingTickets && IsValidForTickets)
                {                    
                    //return ExtraTicketPetition != null && ExtraTicketPetition.IsApproved && !ExtraTicketPetition.IsPending && ExtraTicketPetition.NumberTicketsStreaming.HasValue
                    //       ? ExtraTicketPetition.NumberTicketsStreaming.Value : 0;   

                    return ExtraTicketPetition != null ? ExtraTicketPetition.ApprovedStreamingTickets : 0;
                }

                return 0;
            }
        }

        /// <summary>
        /// Returns total number of projected tickets, includes all extra ticket petitions that have not been approved yet
        /// </summary>
        public virtual int ProjectedTickets
        {
            get
            {
                if (IsValidForTickets)
                {
                    var ticketCount = NumberTickets;

                    if (ExtraTicketPetition != null)
                    {
                        //ticketCount += ExtraTicketPetition.NumberTickets.HasValue
                        //                   ? ExtraTicketPetition.NumberTickets.Value
                        //                   : ExtraTicketPetition.NumberTicketsRequested;
                        ticketCount += ExtraTicketPetition.ProjectedTickets;
                    }
                    return ticketCount;
                }

                return 0;
            }
        }

        /// <summary>
        /// Returns total number of projected streaming tickets, includes all extra ticket petitions that have not been approved yet
        /// </summary>
        public virtual int ProjectedStreamingTickets
        {
            get
            {
                if (Ceremony.HasStreamingTickets && IsValidForTickets)
                {
                    //return (ExtraTicketPetition != null && ExtraTicketPetition.NumberTicketsStreaming.HasValue)
                    //            ? ExtraTicketPetition.NumberTicketsStreaming.Value
                    //            : 0;
                    return ExtraTicketPetition != null ? ExtraTicketPetition.ProjectedStreamingTickets : 0;
                }

                return 0;
            }
        }

        #endregion
    }

    public class RegistrationParticipationMap : ClassMap<RegistrationParticipation>
    {
        public RegistrationParticipationMap()
        {
            Id(x => x.Id);

            References(x => x.Registration).Cascade.None().Fetch.Join();
            References(x => x.Major).Column("MajorCode").Cascade.None().Fetch.Join();
            References(x => x.Ceremony).Cascade.None().Fetch.Join();
            References(x => x.ExtraTicketPetition).Cascade.All().Fetch.Join();

            Map(x => x.NumberTickets);
            Map(x => x.Cancelled);
            Map(x => x.LabelPrinted);
            Map(x => x.DateRegistered);
            Map(x => x.DateUpdated);
            Map(x => x.ExitSurvey);

            References(x => x.TicketDistributionMethod);

            HasMany(x => x.TransferRequests);
        }
    }
}
