using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Student : DomainObjectWithTypedId<Guid>
    {
        public Student(string pidm, string studentId, string firstName, string mi, string lastName, decimal units, string email, string login, TermCode termCode)
        {
            Id = Guid.NewGuid();
            
            Pidm = pidm;
            StudentId = studentId;
            FirstName = firstName;
            MI = mi;
            LastName = lastName;
            Units = units;
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
        public virtual decimal Units { get; set; }
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

        public virtual IList<MajorCode> Majors { get; set; }

        #region Extended Properties
        public virtual string FullName
        {
            get
            {
                return string.Format("{0}{1} {2}", FirstName, string.IsNullOrEmpty(MI) ? string.Empty : " " + MI , LastName);
            }
        }
        public virtual string StrMajors
        {
            get
            {
                return string.Join(",", Majors.Select(x=>x.Name).ToArray());
            }
        }
        public virtual string StrMajorCodes
        {
            get
            {
                return string.Join(",", Majors.Select(x => x.Id).ToArray());
            }
        }
        #endregion
    }
}
