using System.Collections.Generic;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class College : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }

        public virtual IList<MajorCode> Majors { get; set; }
    }

    public class CollegeMap : ClassMap<College>
    {
        public CollegeMap()
        {
            Table("vColleges");
            ReadOnly();

            Id(x => x.Id);

            Map(x => x.Name);

            HasMany(x => x.Majors).KeyColumn("CollegeCode").Inverse();
        }
    }
}
