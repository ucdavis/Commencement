using NHibernate.Validator.Constraints;
using Spring.Objects.Factory.Attributes;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class TermCode : DomainObjectWithTypedId<string>
    {
        [Required]
        [Length(50)]
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
