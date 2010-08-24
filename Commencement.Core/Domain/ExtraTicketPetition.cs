using System;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

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

            DateSubmitted = DateTime.Now;
            DateDecision = null;
        }

        [Min(1)]
        public virtual int NumberTickets { get; set; }
        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }

        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }
    }
}