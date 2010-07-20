using System.Collections.Generic;
using System.Linq;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class Student : DomainObjectWithTypedId<string>
    {
        public Student()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Majors = new List<MajorCode>();
        }

        [Length(9)]
        [Required]
        public virtual string StudentId { get; set; }
        [Length(50)]
        public virtual string FirstName { get; set; }
        [Length(50)]
        public virtual string LastName { get; set; }

        public virtual string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public virtual decimal Units { get; set; }
        [Length(100)]
        public virtual string Email { get; set; }
        [Length(50)]
        public virtual string Login { get; set; }

        public virtual IList<MajorCode> Majors { get; set; }

        public virtual string StrMajors
        {
            get
            {
                return string.Join(",", Majors.Select(x=>x.Name).ToArray());
            }
        }
    }
}
