using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class SpecialNeed : DomainObject
    {
        [Required]
        [StringLength(100)]
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
