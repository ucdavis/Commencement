using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class TicketDistributionMethod : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
    }

    public class TicketDistributionMethodMap : ClassMap<TicketDistributionMethod>
    {
        public TicketDistributionMethodMap()
        {
            ReadOnly();

            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
        }
    }
}
