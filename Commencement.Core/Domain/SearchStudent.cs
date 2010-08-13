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
        public virtual string LastName { get; set; }

        public virtual decimal HoursEarned { get; set; }

        public virtual string Email { get; set; }

        public virtual string MajorCode { get; set; }
        public virtual string CollegeCode { get; set; }
        public virtual string DegreeCode { get; set; }

        public virtual string LoginId { get; set; }

        public virtual string Astd { get; set; }
    }
}
