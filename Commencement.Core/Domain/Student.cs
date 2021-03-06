﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Commencement.Core.Helpers;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Student : DomainObjectWithTypedId<Guid>
    {
        #region Constructors
        public Student(string pidm, string studentId, string firstName, string mi, string lastName, decimal currentUnits, decimal earnedUnits, string email, string login, TermCode termCode)
        {
            Id = Guid.NewGuid();
            
            Pidm = pidm;
            StudentId = studentId;
            FirstName = firstName;
            MI = mi;
            LastName = lastName;
            CurrentUnits = currentUnits;
            EarnedUnits = earnedUnits;
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
            Id = Guid.NewGuid();

            Majors = new List<MajorCode>();

            DateAdded = DateTime.UtcNow.ToPacificTime();
            DateUpdated = DateTime.UtcNow.ToPacificTime();
        }
        #endregion

        #region Mapped Fields
        [StringLength(8)]
        [Required]
        public virtual string Pidm { get; set; }
        [StringLength(9)]
        [Required]
        public virtual string StudentId { get; set; }
        [StringLength(50)]
        public virtual string FirstName { get; set; }
        [StringLength(50)]
        public virtual string MI { get; set; }
        [StringLength(50)]
        public virtual string LastName { get; set; }
        public virtual decimal EarnedUnits { get; set; }
        public virtual decimal CurrentUnits { get; set; }
        [StringLength(100)]
        public virtual string Email { get; set; }
        [StringLength(50)]
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

        public virtual string AddedBy { get; set; }

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

        /// <summary>
        /// Comma seperated list of majors (ignoring consolidation codes)
        /// </summary>
        public virtual string ActualStrMajors { 
            get { return string.Join(",", Majors.Select(x => x.MajorName).ToArray()); } 
        }
        /// <summary>
        /// Comma seperated list of majors (including consolidation codes)
        /// </summary>
        public virtual string StrMajors
        {
            get
            {
                return string.Join(",", Majors.Select(x=>x.MajorName).ToArray());
            }
        }

        /// <summary>
        /// Comma seperated list of major ids (including consolidation codes)
        /// </summary>
        public virtual string StrMajorCodes
        {
            get
            {
                return string.Join(",", Majors.Select(x => x.MajorId).ToArray());
            }
        }

        public virtual string StrColleges
        {
            get
            {
                return string.Join(",", Majors.Where(a => a.College != null && a.IsActive).Select(a => a.College.Id).Distinct());
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
            Map(x => x.AddedBy);

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
