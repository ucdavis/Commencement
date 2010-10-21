using System.Collections.Generic;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class College : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
        public virtual bool Display { get; set; }

        public virtual IList<MajorCode> Majors { get; set; }
        public virtual IList<Registration> Registrations  { get; set; }
    }

    public class CollegeMap : ClassMap<College>
    {
        public CollegeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.Display);
            HasMany(x => x.Majors).KeyColumn("CollegeCode").Inverse();
            HasMany(x => x.Registrations).KeyColumn("CollegeCode").Inverse();
        }
    }
}
