﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Commencement.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

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

            CapAndGownDeadline = DateTime.UtcNow.ToPacificTime();
            FileToGraduateDeadline = DateTime.UtcNow.ToPacificTime();

            RegistrationBegin = DateTime.UtcNow.ToPacificTime();
            RegistrationDeadline = DateTime.UtcNow.ToPacificTime();
        }

        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual string LandingText { get; set; }
        public virtual string RegistrationWelcome { get; set; }

        public virtual DateTime CapAndGownDeadline { get; set; }
        public virtual DateTime FileToGraduateDeadline { get; set; }
        [Required]
        public virtual IList<Ceremony> Ceremonies { get; set; }

        public virtual DateTime RegistrationBegin { get; set; }
        public virtual DateTime RegistrationDeadline { get; set; }
        public virtual DateTime? RegistrationPetitionDeadline { get; set; }

        public virtual string GetNiceTermName()
        {
            var split = this.Name.Split(' ');
            return string.Format("{0} {1}", split.FirstOrDefault(), split.LastOrDefault());
        }

        /// <summary>
        /// Determines if the system should be open for registration
        /// </summary>
        /// <param name="regular">Ignore the registration petition deadline</param>
        /// <returns></returns>
        public virtual bool CanRegister(bool regular = false)
        {
            // registration petition deadline is after the registration deadline
            if (!regular && RegistrationPetitionDeadline.HasValue && RegistrationPetitionDeadline.Value.Date > RegistrationDeadline.Date)
            {
                return DateTime.UtcNow.ToPacificTime().Date >= RegistrationBegin.Date && DateTime.UtcNow.ToPacificTime().Date <= RegistrationPetitionDeadline.Value.Date;
            }

            // no registration petition deadline, default to the standard deadlines
            return DateTime.UtcNow.ToPacificTime().Date >= RegistrationBegin.Date && DateTime.UtcNow.ToPacificTime().Date <= RegistrationDeadline.Date;
        }
    }

    public class TermCodeMap : ClassMap<TermCode>
    {
        public TermCodeMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
            Map(x => x.LandingText).StringMaxLength();
            Map(x => x.RegistrationWelcome).StringMaxLength();
            Map(x => x.CapAndGownDeadline);
            Map(x => x.FileToGraduateDeadline);

            Map(x => x.RegistrationBegin);
            Map(x => x.RegistrationDeadline);
            Map(x => x.RegistrationPetitionDeadline);

            HasMany(x => x.Ceremonies).KeyColumn("TermCode").Cascade.None().Inverse();
        }
    }

}
