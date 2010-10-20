using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.NHibernateValidator.Extensions;
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
        [NotNull]
        public virtual Ceremony Ceremony { get; set; }

        public virtual vUser User { get; set; }
    }

    public class CeremonyEditorMap : ClassMap<CeremonyEditor>
    {
        public CeremonyEditorMap()
        {
            Id(x => x.Id);
            Map(x => x.Owner);

            References(x => x.Ceremony);
            References(x => x.User).Column("UserId");
        }
    }
}