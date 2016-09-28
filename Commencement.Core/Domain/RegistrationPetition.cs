using System;
using System.ComponentModel.DataAnnotations;
using Commencement.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class RegistrationPetition : DomainObject
    {
        #region Constructors
        public RegistrationPetition()
        {
            SetDefaults();
        }

        public RegistrationPetition(Registration registration, MajorCode major, Ceremony ceremony, string exceptionReason, vTermCode completionTerm, int numberTickets)
        {
            Registration = registration;
            MajorCode = major;
            Ceremony = ceremony;
            ExceptionReason = exceptionReason;
            //TermCodeComplete = completionTerm;
            NumberTickets = numberTickets;
            
            SetDefaults();
        }

        private void SetDefaults()
        {
            DateSubmitted = DateTime.UtcNow.ToPacificTime();

            IsPending = true;
            IsApproved = false;
            ExitSurvey = false;
        }
        #endregion

        #region Mapped Fields
        [Required]
        public virtual Registration Registration { get; set; }
        [Required]
        public virtual MajorCode MajorCode { get; set; }
        [Required]
        [StringLength(500)]
        public virtual string ExceptionReason { get; set; }
        //public virtual vTermCode TermCodeComplete{ get; set; }
        [StringLength(100)]
        public virtual string TransferUnitsFrom { get; set; }
        [StringLength(5)]
        public virtual string TransferUnits { get; set; }
        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }
        public virtual Ceremony Ceremony { get; set; }
        public virtual int NumberTickets { get; set; }

        public virtual bool ExitSurvey { get; set; }

        public virtual TicketDistributionMethod TicketDistributionMethod { get; set; }

        #endregion

        #region Methods
        public virtual void SetDecision(bool isApproved)
        {
            IsPending = false;
            IsApproved = isApproved;
            DateDecision = DateTime.UtcNow.ToPacificTime();
        }

        public virtual string Status
        {
            get { return IsPending ? "Pending" : (IsApproved ? "Approved" : "Denied"); }
        }
        #endregion
    }

    public class RegistrationPetitionMap : ClassMap<RegistrationPetition>
    {
        public RegistrationPetitionMap()
        {
            Id(x => x.Id);
            References(x => x.Registration);
            References(x => x.MajorCode).Column("MajorCode");
            Map(x => x.ExceptionReason);
            //References(x => x.TermCodeComplete).Column("CompletionTerm");
            Map(x => x.TransferUnitsFrom);
            Map(x => x.TransferUnits);
            Map(x => x.IsPending);
            Map(x => x.IsApproved);
            Map(x => x.DateSubmitted);
            Map(x => x.DateDecision);
            References(x => x.Ceremony);
            Map(x => x.NumberTickets);
            Map(x => x.ExitSurvey);

            References(x => x.TicketDistributionMethod);
        }
    }
}