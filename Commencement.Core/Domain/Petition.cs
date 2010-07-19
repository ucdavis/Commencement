using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;
using NHibernate.Validator.Constraints;

namespace Commencement.Core.Domain
{
    /// <summary>
    /// Baseclass for petition types
    /// </summary>
    public abstract class Petition : DomainObject
    {
        [Required]
        [Length(8)]
        public virtual string Pidm { get; set; }
        
        [Length(200)]
        public virtual string Address1 { get; set; }
        [Length(200)]
        public virtual string Address2 { get; set; }
        [Length(200)]
        public virtual string Address3 { get; set; }
        [Length(100)]
        public virtual string City { get; set; }
        [Length(2)]
        public virtual string State { get; set; }
        [Length(15)]
        public virtual string Zip { get; set; }

        [Length(100)]
        [Email]
        public virtual string Email { get; set; }

        [Min(1)]
        public virtual int NumberTickets { get; set; }

        public virtual bool MailTickets { get; set; }
        
        public virtual bool Pending { get; set; }
        public virtual bool Approved { get; set; }
    }
}
