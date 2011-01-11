using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class TemplateToken : DomainObject
    {
        [NotNull]
        public virtual TemplateType TemplateType { get; set; }
        [Required]
        [Length(50)]
        public virtual string Name { get; set; }

        // grabs the name and removes the spaces
        public virtual string Token { 
            get { return "{" + Name.Replace(" ", string.Empty) + "}"; }
        }
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
