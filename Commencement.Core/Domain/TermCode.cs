﻿using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class TermCode : DomainObjectWithTypedId<string>
    {
        public TermCode(vTermCode vTermCode)
        {
            Id = vTermCode.Id;
            Name = vTermCode.Description;

            SetDefaults();
        }

        public TermCode()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            IsActive = true;
            Ceremonies = new List<Ceremony>();
        }

        [Required]
        [Length(50)]
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual string LandingText { get; set; }
        public virtual string RegistrationWelcome { get; set; }

        public virtual DateTime CapAndGownDeadline { get; set; }
        public virtual DateTime FileToGraduateDeadline { get; set; }

        public virtual IList<Ceremony> Ceremonies { get; set; }
    }

    public class TermCodeMap : ClassMap<TermCode>
    {
        public TermCodeMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
            Map(x => x.LandingText);
            Map(x => x.RegistrationWelcome);
            Map(x => x.CapAndGownDeadline);
            Map(x => x.FileToGraduateDeadline);

            HasMany(x => x.Ceremonies).KeyColumn("TermCode").Cascade.None().Inverse();
        }
    }

}
