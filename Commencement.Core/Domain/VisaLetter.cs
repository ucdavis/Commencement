using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            IsDenied = false;
            IsCanceled = false;
            DateCreated = DateTime.Now;
            ReferenceGuid = Guid.NewGuid();
            HardCopy = "N";
        }

        [NotNull]
        public virtual Student Student { get; set; } //Might want Pidm, or at least to get the latest student where the pidm matches this student

        [Required]
        [Length(50)]
        public virtual string StudentFirstName { get; set; }

        [Required]
        [Length(50)]
        public virtual string StudentLastName { get; set; }

        public virtual DateTime DateCreated { get; set; }

        [Required]
        public virtual string Gender { get; set; }
        public virtual char? Ceremony { get; set; }  // If they are not registered for a ceremony, indicate which one they will apply for. (I think)

        [Required]
        [Length(5)]
        public virtual string RelativeTitle { get; set; }

        [Required]
        [Length(100)]
        public virtual string RelativeFirstName { get; set; }

        [Required]
        [Length(100)]
        public virtual string RelativeLastName { get; set; }

        [Required]
        [Length(100)]
        public virtual string RelationshipToStudent { get; set; }

        [Required]
        [Length(500)]
        public virtual string RelativeMailingAddress { get; set; }

        public virtual bool IsApproved { get; set; }
        public virtual bool IsDenied { get; set; }


        //TODO: Who approved it so we can get the signature block and other info.

        public virtual DateTime? DateDecided { get; set; }

        public virtual bool IsPending { get; set; }

        public virtual bool IsCanceled { get; set; }

        public virtual string CollegeName { get; set; } //Drop down list for student, try to pick for student

        public virtual string CollegeCode { get; set; } //Drop down list for student, try to pick for student
        public virtual string MajorName { get; set; } // try to fill out for student

        public virtual string Degree { get; set; } //TODO: Try and get this from banner or a dropdown? or don't include it in the letter?

        public virtual DateTime? CeremonyDateTime { get; set; }
        public virtual DateTime? LastUpdateDateTime { get; set; } 

        public virtual string ApprovedBy { get; set; } //Or maybe edited by, etc.

        public virtual Guid ReferenceGuid { get; set; }

        [Required]
        public virtual string HardCopy { get; set; }

        //Not a DB field
        public virtual string Decide
        {
            get
            {
                if (IsApproved)
                {
                    return "A";
                }
                if (IsDenied)
                {
                    return "D";
                }
                return "N";

            }
        }

        public virtual string Status
        {
            get
            {
                if (IsCanceled)
                {
                    return "Canceled";
                }
                if (IsPending)
                {
                    return "Pending";
                }
                if (IsApproved)
                {
                    return "Approved";
                }
                if (IsDenied)
                {
                    return "Denied";
                }
                return string.Empty;
            }
        }
    }

    public class VisaLetterMap : ClassMap<VisaLetter>
    {
        public VisaLetterMap()
        {
            Id(x => x.Id);

            References(x => x.Student).Column("Student_Id").Fetch.Join();

            Map(x => x.StudentFirstName);
            Map(x => x.StudentLastName);

            Map(x => x.DateCreated);
            Map(x => x.Gender);
            Map(x => x.Ceremony);
            Map(x => x.RelativeTitle);
            Map(x => x.RelativeFirstName);
            Map(x => x.RelativeLastName);
            Map(x => x.RelationshipToStudent);
            Map(x => x.RelativeMailingAddress);
            Map(x => x.IsApproved);
            Map(x => x.IsDenied);
            Map(x => x.DateDecided);
            Map(x => x.IsPending);
            Map(x => x.IsCanceled);
            Map(x => x.CollegeName);
            Map(x => x.CollegeCode);
            Map(x => x.CeremonyDateTime);
            Map(x => x.ApprovedBy);
            Map(x => x.MajorName);
            Map(x => x.LastUpdateDateTime);
            Map(x => x.Degree);
            Map(x => x.ReferenceGuid);
            Map(x => x.HardCopy);
        }
    }
}
