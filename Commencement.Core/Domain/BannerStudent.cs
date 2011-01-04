using FluentNHibernate.Mapping;

namespace Commencement.Core.Domain
{
    public class BannerStudent
    {
        /// <summary>
        /// Pidm
        /// </summary>
        public virtual string Pidm { get; set; }  
        public virtual string StudentId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Mi { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual decimal EarnedUnits { get; set; }
        public virtual decimal CurrentUnits { get; set; }
        public virtual string Major { get; set; }
        public virtual string LastTerm { get; set; }
        public virtual string Astd { get; set; }
        public virtual string LoginId { get; set; }
    }

    public class BannerStudentMap : ClassMap<BannerStudent>
    {
        public BannerStudentMap()
        {
            Id(x => x.Pidm).Column("Pidm");

            Map(x => x.StudentId);
            Map(x => x.FirstName);
            Map(x => x.Mi);
            Map(x => x.LastName);
            Map(x => x.Email);
            Map(x => x.EarnedUnits);
            Map(x => x.CurrentUnits);
            Map(x => x.Major);
            Map(x => x.LastTerm);
            Map(x => x.Astd);
            Map(x => x.LoginId);
        }
    }
}
