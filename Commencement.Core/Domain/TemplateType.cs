using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class TemplateType : DomainObject
    {
        public TemplateType()
        {
            TemplateTokens = new List<TemplateToken>();
        }

        [Required]
        [Length(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        [Length(2)]
        public virtual string Code { get; set; }
        [NotNull]
        public virtual IList<TemplateToken> TemplateTokens { get; set; }
    }

    public class TemplateTypeMap : ClassMap<TemplateType>
    {
        public TemplateTypeMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.Code);

            HasMany(x => x.TemplateTokens).Cascade.None().Inverse();
        }
    }
}
