using System;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class vTermCode : DomainObjectWithTypedId<string>
    {
        public virtual string Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
    }
}
