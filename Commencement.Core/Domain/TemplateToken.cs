using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class TemplateToken : DomainObject
    {
        public virtual TemplateType TemplateType { get; set; }
        public virtual string Name { get; set; }
    }

    public class TemplateTokenMap : ClassMap<TemplateToken>
    {
        public TemplateTokenMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            References(x => x.TemplateType);
        }
    }
}
