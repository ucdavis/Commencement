using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;
using UCDArch.Core.NHibernateValidator.Extensions;

namespace Commencement.Core.Domain
{
    public class SpecialNeed : DomainObject
    {
        [Required]
        [Length(100)]
        public virtual string Name { get; set; }
        public virtual string Tip { get; set; }
        public virtual bool IsActive { get; set; }
    }

    public class SpecialNeedMap : ClassMap<SpecialNeed>
    {
        public SpecialNeedMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Tip);
            Map(x => x.IsActive);
        }
    }

}
