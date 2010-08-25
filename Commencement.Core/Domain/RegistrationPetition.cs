﻿using System;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class RegistrationPetition : DomainObject
    {
        public RegistrationPetition()
        {
            IsPending = true;
            IsApproved = false;

            DateSubmitted = DateTime.Now;
            DateDecision = null;
        }

        [Required]
        [Length(8)]
        public virtual string Pidm { get; set; }

        [Required]
        [Length(9)]
        public virtual string StudentId { get; set; }
        [Required]
        [Length(50)]
        public virtual string FirstName { get; set; }
        [Required]
        [Length(50)]
        public virtual string LastName { get; set; }

        [Required]
        [Length(50)]
        public virtual string Email { get; set; }
        [Required]
        [Length(50)]
        public virtual string Login { get; set; }

        //[Required]
        //[Length(4)]
        [NotNull]
        public virtual MajorCode MajorCode { get; set; }
        
        public virtual decimal Units { get; set; }

        [Required]
        [Length(1000)]
        public virtual string ExceptionReason { get; set; }

        [Required]
        [Length(6)]
        public virtual string CompletionTerm { get; set; }

        [Length(100)]
        public virtual string TransferUnitsFrom { get; set; }
        public virtual double? TransferUnits { get; set; }
        [Length(50)]
        public virtual string CurrentlyRegistered { get; set; }

        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }

        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }

        [NotNull]
        public virtual TermCode TermCode { get; set; }


        public virtual string FullName { get { return FirstName + " " + LastName; } }

        public virtual void SetDecision(bool isApproved)
        {
            IsPending = false;
            IsApproved = isApproved;
            DateDecision = DateTime.Now;
        }
    }
}