using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class School : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
    }
}
