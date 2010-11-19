using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Student : DomainObjectWithTypedId<Guid>
    {
        #region Constructors
        public Student(string pidm, string studentId, string firstName, string mi, string lastName, decimal currentUnits, string email, string login, TermCode termCode)
        {
            Id = Guid.NewGuid();
            
            Pidm = pidm;
            StudentId = studentId;
            FirstName = firstName;
            MI = mi;
            LastName = lastName;
            CurrentUnits = CurrentUnits;
            Email = email;
            Login = login;
            TermCode = termCode;

            SetDefaults();
        }

        public Student()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Majors = new List<MajorCode>();

            DateAdded = DateTime.Now;
            DateUpdated = DateTime.Now;
        }
        #endregion

        #region Mapped Fields
        [Length(8)]
        [Required]
        public virtual string Pidm { get; set; }
        [Length(9)]
        [Required]
        public virtual string StudentId { get; set; }
        [Length(50)]
        public virtual string FirstName { get; set; }
        [Length(50)]
        public virtual string MI { get; set; }
        [Length(50)]
        public virtual string LastName { get; set; }
        public virtual decimal EarnedUnits { get; set; }
        public virtual decimal CurrentUnits { get; set; }
        [Length(100)]
        public virtual string Email { get; set; }
        [Length(50)]
        public virtual string Login { get; set; }

        public virtual DateTime DateAdded { get; set; }
        public virtual DateTime DateUpdated { get; set; }
        public virtual TermCode TermCode { get; set; }
        /// <summary>
        /// If this is not null, then this is an override to give a student this specific ceremony.
        /// </summary>
        public virtual Ceremony Ceremony { get; set; }

        public virtual bool SjaBlock { get; set; }
        public virtual bool Blocked { get; set; }

        public virtual IList<MajorCode> Majors { get; set; }
        #endregion

        #region Extended Properties
        public virtual string FullName
        {
            get
            {
                return string.Format("{0}{1} {2}", FirstName, string.IsNullOrEmpty(MI) || MI.Trim() == string.Empty ? string.Empty : " " + MI , LastName);
            }
        }
        public virtual string StrMajors
        {
            get
            {
                return string.Join(",", Majors.Select(x=>x.MajorName).ToArray());
            }
        }
        public virtual string StrMajorCodes
        {
            get
            {
                return string.Join(",", Majors.Select(x => x.Id).ToArray());
            }
        }

        public virtual decimal TotalUnits { 
            get { return EarnedUnits + CurrentUnits; } 
        }
        #endregion
    }

    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Pidm);
            Map(x => x.StudentId);
            Map(x => x.FirstName);
            Map(x => x.MI);
            Map(x => x.LastName);
            Map(x => x.EarnedUnits);
            Map(x => x.CurrentUnits);
            Map(x => x.Email);
            Map(x => x.Login);
            Map(x => x.DateAdded);
            Map(x => x.DateUpdated);
            Map(x => x.SjaBlock);
            Map(x => x.Blocked);

            References(x => x.TermCode).Column("TermCode");
            References(x => x.Ceremony);

            HasManyToMany(x => x.Majors)
                .ParentKeyColumn("Student_Id")
                .ChildKeyColumn("MajorCode")
                .Table("StudentMajors")
                .Cascade.SaveUpdate()
                .Fetch.Subselect();
        }
    }
}
