using NHibernate.Validator.Constraints;

namespace Commencement.Core.Domain
{
    public class ExtraTicketPetition : Petition
    {
        [NotNull]
        public virtual Student Student { get; set; }
    }
}