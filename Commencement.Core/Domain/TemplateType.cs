using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class TemplateType : DomainObject
    {
        [Required]
        [Length(50)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
    }
}
