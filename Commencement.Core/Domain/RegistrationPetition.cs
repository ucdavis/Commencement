using NHibernate.Validator.Constraints;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class RegistrationPetition : Petition
    {
        [Length(9)]
        public virtual string StudentId { get; set; }
        [Length(50)]
        public virtual string FirstName { get; set; }
        [Length(50)]
        public virtual string LastName { get; set; }

        [Required]
        [Length(4)]
        public virtual string MajorCode { get; set; }
        
        public virtual double Units { get; set; }

        [Required]
        [Length(1000)]
        public virtual string ExceptionReason { get; set; }

        [Required]
        [Length(6)]
        public virtual string CompletionTerm { get; set; }
    }
}