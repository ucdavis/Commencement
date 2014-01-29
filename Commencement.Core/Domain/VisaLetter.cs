using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;
using System.Linq;

namespace Commencement.Core.Domain
{
    public class VisaLetter : DomainObject
    {
        public VisaLetter()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            IsActive = true;
            IsApproved = false;
            DateCreated = DateTime.Now;
        }

        [NotNull]
        public virtual Student Student { get; set; } //Might want Pidm, or at least to get the latest student where the pidm matches this student

        public virtual DateTime DateCreated { get; set; }
        public virtual char Gender { get; set; }
        public virtual char? Ceremony { get; set; }  // If they are not registered for a ceremony, indicate which one they will apply for. (I think)

        [NotNull]
        [Length(100)]
        public virtual string RelativeFirstName { get; set; }

        [NotNull]
        [Length(100)]
        public virtual string RelativeLastName { get; set; }

        [NotNull]
        [Length(100)]
        public virtual string RelationshipToStudent { get; set; }

        [NotNull]
        [Length(500)]
        public virtual string RelativeMailingAddress { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual DateTime? DateApproved { get; set; }

        public virtual bool IsActive { get; set; }
    }

    public class VisaLetterMap : ClassMap<VisaLetter>
    {
        public VisaLetterMap()
        {
            Id(x => x.Id);

            References(x => x.Student).Column("Student_Id").Fetch.Join();

            Map(x => x.DateCreated);
            Map(x => x.Gender);
            Map(x => x.Ceremony);
            Map(x => x.RelativeFirstName);
            Map(x => x.RelativeLastName);
            Map(x => x.RelationshipToStudent);
            Map(x => x.RelativeMailingAddress);
            Map(x => x.IsApproved);
            Map(x => x.DateApproved);
            Map(x => x.IsActive);
        }
    }
}
