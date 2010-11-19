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
        public virtual Registration Registration { get; set; }
        public virtual MajorCode Major { get; set; }
        public virtual Ceremony Ceremony { get; set; }
        public virtual int NumberTickets { get; set; }
    }

    public class RegistrationParticipationMap : ClassMap<RegistrationParticipation>
    {
        public RegistrationParticipationMap()
        {
            Id(x => x.Id);

            References(x => x.Registration).Cascade.None().Fetch.Join();
            References(x => x.Major).Column("MajorCode").Cascade.None().Fetch.Join();
            References(x => x.Ceremony).Cascade.None().Fetch.Join();
            Map(x => x.NumberTickets);
        }
    }
}
