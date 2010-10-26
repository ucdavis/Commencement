using FluentNHibernate.Mapping;
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

    public class StateMap : ClassMap<State>
    {
        public StateMap()
        {
            ReadOnly();

            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);
        }
    }
}
