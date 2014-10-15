using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

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
            IsPending = true;
            IsApproved = false;
            DateCreated = DateTime.Now;
        }

        [NotNull]
        public virtual Student Student { get; set; } //Might want Pidm, or at least to get the latest student where the pidm matches this student

        public virtual DateTime DateCreated { get; set; }
        public virtual char Gender { get; set; }
        public virtual char? Ceremony { get; set; }  // If they are not registered for a ceremony, indicate which one they will apply for. (I think)

        [NotNull]
        [Length(5)]
        public virtual string RelativeTitle { get; set; }

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


        //TODO: Who approved it so we can get the signature block and other info.

        public virtual DateTime? DateDecided { get; set; }

        public virtual bool IsPending { get; set; }

        public virtual string CollegeName { get; set; } //Drop down list for student, try to pick for student
        public virtual string MajorName { get; set; } //Drop down list, try to fill out for student

        public virtual DateTime? CeremonyDateTime { get; set; } //If ceremony is not created, allow admin to pick date.

        public virtual string ApprovedBy { get; set; } //Or maybe edited by, etc.
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
            Map(x => x.RelativeTitle);
            Map(x => x.RelativeFirstName);
            Map(x => x.RelativeLastName);
            Map(x => x.RelationshipToStudent);
            Map(x => x.RelativeMailingAddress);
            Map(x => x.IsApproved);
            Map(x => x.DateDecided);
            Map(x => x.IsPending);
            Map(x => x.CollegeName);
            Map(x => x.CeremonyDateTime);
            Map(x => x.ApprovedBy);
        }
    }
}
