using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class TemplateType : DomainObject
    {
        [Required]
        [Length(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }

    public class TemplateTypeMap : ClassMap<TemplateType>
    {
        public TemplateTypeMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
        }
    }
}
