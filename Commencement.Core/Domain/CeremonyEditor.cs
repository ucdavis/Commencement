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

        public CeremonyEditor(string loginId, bool owner)
        {
            LoginId = loginId;
            Owner = owner;
        }

        [Required]
        [Length(50)]
        public virtual string LoginId { get; set; }
        
        public virtual bool Owner { get; set; }
        [NotNull]
        public virtual Ceremony Ceremony { get; set; }
    }

    public class CeremonyEditorMap : ClassMap<CeremonyEditor>
    {
        public CeremonyEditorMap()
        {
            Id(x => x.Id);
            Map(x => x.LoginId);
            Map(x => x.Owner);

            References(x => x.Ceremony);
        }
    }
}