using System.Collections.Generic;
using NHibernate.Validator.Constraints;
using Spring.Objects.Factory.Attributes;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class TermCode : DomainObjectWithTypedId<string>
    {
        public TermCode()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Commencements = new List<Commencement>();
        }

        [Required]
        [Length(50)]
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual IList<Commencement> Commencements { get; set; }
    }
}
