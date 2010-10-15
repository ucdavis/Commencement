using System;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class vTermCode : DomainObjectWithTypedId<string>
    {
        public virtual string Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
    }

    public class vTermCodeMap : ClassMap<vTermCode>
    {
        public vTermCodeMap()
        {
            ReadOnly();
            Where("TypeCode='Q' and (id like '%10' or id like '%03')");

            Id(x => x.Id);
            Map(x => x.Description);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
        }
    }
}
