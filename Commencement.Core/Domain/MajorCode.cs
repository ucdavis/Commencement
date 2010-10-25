using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class MajorCode : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
        public virtual string DisciplineCode { get; set; }
        
        public virtual College College { get; set; }
    }

    public class MajorCodeMap : ClassMap<MajorCode>
    {
        public MajorCodeMap()
        {
            Table("Majors");

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.DisciplineCode);

            References(x => x.College).Column("CollegeCode");
        }
    }

}
