using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class CeremonyEditor : DomainObject
    {
        public CeremonyEditor()
        {
            
        }

        public CeremonyEditor(vUser user, bool owner)
        {
            User = user;
            Owner = owner;
        }

        public virtual bool Owner { get; set; }
        [Required]
        public virtual Ceremony Ceremony { get; set; }
        [Required]
        public virtual vUser User { get; set; }
    }

    public class CeremonyEditorMap : ClassMap<CeremonyEditor>
    {
        public CeremonyEditorMap()
        {
            Id(x => x.Id);
            Map(x => x.Owner);

            References(x => x.Ceremony);
            References(x => x.User).Column("UserId").Fetch.Join();
        }
    }
}