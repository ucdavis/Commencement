using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class MajorCode : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
        public virtual string DepartmentCode { get; set; }
        public virtual string DisciplineCode { get; set; }
    }
}
