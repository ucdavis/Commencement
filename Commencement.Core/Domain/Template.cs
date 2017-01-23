using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Commencement.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

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

        [Required]
        public virtual TemplateType TemplateType { get; set; }

        [Required]
        public virtual Ceremony Ceremony { get; set; }

        public virtual bool IsActive { get; set; }

        [StringLength(100)]
        public virtual string Subject { get; set; }       
    }

    public class TemplateMap : ClassMap<Template>
    {
        public TemplateMap()
        {
            Id(x => x.Id);

            Map(x => x.BodyText).StringMaxLength();
            Map(x => x.IsActive);
            Map(x => x.Subject);

            References(x => x.TemplateType);
            References(x => x.Ceremony);
        }
    }
}
