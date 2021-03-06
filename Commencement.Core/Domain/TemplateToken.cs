﻿using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class TemplateToken : DomainObject
    {
        [Required]
        public virtual TemplateType TemplateType { get; set; }
        [Required]
        [StringLength(50)]
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
