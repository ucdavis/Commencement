using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class MajorCode : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
        public virtual string DepartmentCode { get; set; }
        public virtual string DisciplineCode { get; set; }

        public virtual College College { get; set; }
    }

    public class MajorCodeMap : ClassMap<MajorCode>
    {
        public MajorCodeMap()
        {
            ReadOnly();
            Where("IsMajor=1");
            Table("vMajors");

            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.DepartmentCode);
            Map(x => x.DisciplineCode);

            References(x => x.College);
        }
    }

}
