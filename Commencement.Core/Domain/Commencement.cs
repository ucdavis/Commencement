using System;
using UCDArch.Core.NHibernateValidator.Extensions;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class Commencement : DomainObject
    {
        [Required]
        [Length(200)]
        public virtual string Location { get; set; }
        
        [NotNull]
        public virtual DateTime DateTime { get; set; }
        
        [NotNull]
        [Min(1)]
        public virtual int TotalTickets { get; set; }
        
        [NotNull]
        public virtual DateTime RegistrationDeadline { get; set; }
    }
}
