using System;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    //public class RegistrationPetition : DomainObject
    //{
    //    public RegistrationPetition()
    //    {
    //        IsPending = true;
    //        IsApproved = false;

    //        DateSubmitted = DateTime.Now;
    //        DateDecision = null;
    //    }

    //    [Required]
    //    [Length(8)]
    //    public virtual string Pidm { get; set; }

    //    [Required]
    //    [Length(9)]
    //    public virtual string StudentId { get; set; }
    //    [Required]
    //    [Length(50)]
    //    public virtual string FirstName { get; set; }
    //    [Length(50)]
    //    public virtual string MI { get; set; }
    //    [Required]
    //    [Length(50)]
    //    public virtual string LastName { get; set; }

    //    [Required]
    //    [Length(50)]
    //    public virtual string Email { get; set; }
    //    [Required]
    //    [Length(50)]
    //    public virtual string Login { get; set; }

    //    [NotNull]
    //    public virtual MajorCode MajorCode { get; set; }
        
    //    public virtual decimal Units { get; set; }

    //    [Required]
    //    [Length(1000)]
    //    public virtual string ExceptionReason { get; set; }

    //    [Required]
    //    public virtual string CompletionTerm { get; set; }

    //    [Length(100)]
    //    public virtual string TransferUnitsFrom { get; set; }
    //    public virtual double? TransferUnits { get; set; }

    //    public virtual bool IsPending { get; set; }
    //    public virtual bool IsApproved { get; set; }

    //    public virtual DateTime DateSubmitted { get; set; }
    //    public virtual DateTime? DateDecision { get; set; }

    //    [NotNull]
    //    public virtual TermCode TermCode { get; set; }

    //    public virtual Ceremony Ceremony { get; set; }

    //    public virtual bool GradTrack { get; set; }


    //    public virtual string FullName
    //    {
    //        get
    //        {
    //            return string.Format("{0}{1} {2}", FirstName, string.IsNullOrEmpty(MI) || MI.Trim() == string.Empty ? string.Empty : " " + MI, LastName);
    //        }
    //    }

    //    public virtual void SetDecision(bool isApproved)
    //    {
    //        IsPending = false;
    //        IsApproved = isApproved;
    //        DateDecision = DateTime.Now;
    //    }

    //}

    //public class RegistrationPetitionMap : ClassMap<RegistrationPetition>
    //{
    //    public RegistrationPetitionMap()
    //    {
    //        Id(x => x.Id);

    //        Map(x => x.Pidm);
    //        Map(x => x.StudentId);
    //        Map(x => x.FirstName);
    //        Map(x => x.MI);
    //        Map(x => x.LastName);
    //        Map(x => x.Email);
    //        Map(x => x.Login);
    //        Map(x => x.Units);
    //        Map(x => x.ExceptionReason);
    //        Map(x => x.CompletionTerm);
    //        Map(x => x.TransferUnitsFrom);
    //        Map(x => x.TransferUnits);
    //        Map(x => x.IsPending);
    //        Map(x => x.IsApproved);
    //        Map(x => x.DateDecision);
    //        Map(x => x.DateSubmitted);
    //        Map(x => x.GradTrack);

    //        References(x => x.MajorCode).Column("MajorCode");
    //        References(x => x.TermCode).Column("TermCode");
    //        References(x => x.Ceremony);
    //    }
    //}
    public class RegistrationPetition : DomainObject
    {
        #region Constructors
        public RegistrationPetition()
        {
            SetDefaults();
        }

        public RegistrationPetition(Student student, MajorCode major, Ceremony ceremony, string exceptionReason, vTermCode completionTerm, int ticketsRequested)
        {
            Student = student;
            MajorCode = major;
            Ceremony = ceremony;
            ExceptionReason = exceptionReason;
            TermCodeComplete = completionTerm;
            TicketsRequested = ticketsRequested;

            SetDefaults();
        }

        private void SetDefaults()
        {
            DateSubmitted = DateTime.Now;

            IsPending = true;
            IsApproved = false;
            GradTrack = false;
        }
        #endregion

        #region Mapped Fields
        [NotNull]
        public virtual Student Student { get; set; }
        [NotNull]
        public virtual MajorCode MajorCode { get; set; }
        [Required]
        [Length(1000)]
        public virtual string ExceptionReason { get; set; }
        public virtual vTermCode TermCodeComplete{ get; set; }
        [Length(100)]
        public virtual string TransferUnitsFrom { get; set; }
        [Length(5)]
        public virtual string TransferUnits { get; set; }
        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }
        public virtual Ceremony Ceremony { get; set; }
        public virtual bool GradTrack { get; set; }
        public virtual string Email { get; set; }
        public virtual int TicketsRequested { get; set; }
        #endregion

        #region Methods
        public virtual void SetDecision(bool isApproved)
        {
            IsPending = false;
            IsApproved = isApproved;
            DateDecision = DateTime.Now;
        }
        #endregion
    }

    public class RegistrationPetitionMap : ClassMap<RegistrationPetition>
    {
        public RegistrationPetitionMap()
        {
            Id(x => x.Id);
            References(x => x.Student).Column("Student_Id");
            References(x => x.MajorCode).Column("MajorCode");
            Map(x => x.ExceptionReason);
            References(x => x.TermCodeComplete).Column("CompletionTerm");
            Map(x => x.TransferUnitsFrom);
            Map(x => x.TransferUnits);
            Map(x => x.IsPending);
            Map(x => x.IsApproved);
            Map(x => x.DateSubmitted);
            Map(x => x.DateDecision);
            References(x => x.Ceremony);
            Map(x => x.GradTrack);
            Map(x => x.Email);
            Map(x => x.TicketsRequested);
        }
    }
}