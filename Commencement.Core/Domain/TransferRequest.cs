using System;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class TransferRequest : DomainObject
    {
        public TransferRequest()
        {
            DateRequested = DateTime.Now;
            Pending = true;
        }

        [Required]
        public virtual RegistrationParticipation RegistrationParticipation { get; set; }
        [Required]
        public virtual Ceremony Ceremony { get; set; }
        [Required]
        [StringLength(200)]
        public virtual string Reason { get; set; }
        public virtual DateTime DateRequested { get; set; }
        [Required]
        public virtual vUser User { get; set; }

        public virtual bool Pending { get; set; }
    }

    public class TransferRequestMap : ClassMap<TransferRequest>
    {
        public TransferRequestMap()
        {
            Id(x => x.Id);

            Map(x => x.Reason);
            Map(x => x.DateRequested);
            Map(x => x.Pending);

            References(x => x.RegistrationParticipation);
            References(x => x.Ceremony);
            References(x => x.User).Column("UserId").Fetch.Join();
        }
    }
}
