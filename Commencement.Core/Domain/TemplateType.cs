using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class TemplateType : DomainObject
    {
        public TemplateType()
        {
            TemplateTokens = new List<TemplateToken>();
        }

        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        [StringLength(2)]
        public virtual string Code { get; set; }
        [Required]
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
