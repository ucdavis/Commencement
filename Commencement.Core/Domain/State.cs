using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class State : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
