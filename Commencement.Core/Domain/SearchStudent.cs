using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    /// <summary>
    /// Loaded by the SearchStudents named query
    /// </summary>
    public class SearchStudent
    {
        public virtual string Id { get; set; }
        public virtual string Pidm { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MI { get; set; }
        public virtual string LastName { get; set; }

        public virtual decimal HoursEarned { get; set; }

        public virtual string Email { get; set; }

        public virtual string MajorCode { get; set; }
        public virtual string CollegeCode { get; set; }
        public virtual string DegreeCode { get; set; }

        public virtual string LoginId { get; set; }

        public virtual string Astd { get; set; }

        public virtual string FullName
        {
            get
            {
                return string.Format("{0}{1} {2}", FirstName, string.IsNullOrEmpty(MI) ? string.Empty : " " + MI, LastName);
            }
        }

        public virtual int? CeremonyId { get; set; }
    }

    public class SearchStudentMap : ClassMap<SearchStudent>
    {
        public SearchStudentMap()
        {
            Id(x => x.Id).Column("Spriden_Id");

            Map(x => x.Pidm).Column("Spriden_Pidm");
            Map(x => x.FirstName).Column("Spriden_First_Name");
            Map(x => x.MI).Column("Spriden_MI");
            Map(x => x.LastName).Column("Spriden_Last_Name");
            Map(x => x.HoursEarned).Column("Shrlgpa_Hours_Earned");
            Map(x => x.Email).Column("Goremal_Email_Address");
            Map(x => x.MajorCode).Column("Zgvlcfs_Majr_Code");
            Map(x => x.CollegeCode).Column("Zgvlcfs_Coll_Code");
            Map(x => x.DegreeCode).Column("shrdgmr_degs_code");
            Map(x => x.LoginId).Column("loginid");
            Map(x => x.Astd).Column("shrttrm_astd_code_end_of_term");
        }
    }
}
