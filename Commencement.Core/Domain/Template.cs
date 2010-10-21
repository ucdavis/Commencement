using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Template : DomainObject
    {
        public Template(string bodyText, TemplateType templateType)
        {
            BodyText = bodyText;
            TemplateType = templateType;
        }

        public Template()
        {
        }

        [Required]
        public virtual string BodyText { get; set; }

        [NotNull]
        public virtual TemplateType TemplateType { get; set; }

        [NotNull]
        public virtual Ceremony Ceremony { get; set; }

        public virtual bool IsActive { get; set; }
    }

    public class TemplateMap : ClassMap<Template>
    {
        public TemplateMap()
        {
            Id(x => x.Id);

            Map(x => x.BodyText);
            Map(x => x.IsActive);

            References(x => x.TemplateType);
            References(x => x.Ceremony);
        }
    }
}
