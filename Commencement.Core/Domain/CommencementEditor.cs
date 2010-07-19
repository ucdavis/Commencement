using NHibernate.Validator.Constraints;
using UCDArch.Core.NHibernateValidator.Extensions;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class CommencementEditor : DomainObject
    {
        [Required]
        [Length(50)]
        public virtual string LoginId { get; set; }
        
        public virtual bool Owner { get; set; }

        public virtual Commencement Commencement { get; set; }
    }
}