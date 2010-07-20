using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Registration : DomainObject
    {
        [NotNull]
        public virtual Student Student { get; set; }
        [NotNull]
        public virtual MajorCode Major { get; set; }
        
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string City { get; set; }
        public virtual State State { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Email { get; set; }
        public virtual int NumberTickets { get; set; }
        public virtual bool MailTickets { get; set; }
        [NotNull]
        public virtual vTermCode TermCode { get; set; }
        [NotNull]
        public virtual Ceremony Ceremony { get; set; }
    }
}
