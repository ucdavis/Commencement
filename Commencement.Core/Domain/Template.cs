using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Template : DomainObject
    {
        public Template(string bodyText, TemplateType templateType, Ceremony ceremony)
        {
            BodyText = bodyText;
            TemplateType = templateType;
            Ceremony = ceremony;

            SetDefaults();
        }

        public Template()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            IsActive = true;
        }

        [Required]
        public virtual string BodyText { get; set; }

        [NotNull]
        public virtual TemplateType TemplateType { get; set; }

        [NotNull]
        public virtual Ceremony Ceremony { get; set; }

        public virtual bool IsActive { get; set; }

        [Length(100)]
        public virtual string Subject { get; set; }       
    }

    public class TemplateMap : ClassMap<Template>
    {
        public TemplateMap()
        {
            Id(x => x.Id);

            Map(x => x.BodyText);
            Map(x => x.IsActive);
            Map(x => x.Subject);

            References(x => x.TemplateType);
            References(x => x.Ceremony);
        }
    }
}
