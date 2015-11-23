using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class vCancelledRegistrations : DomainObjectWithTypedId<Guid>
    {
        public virtual string StudentId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MI { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual int CeremonyId { get; set; }
        public virtual string MajorCode { get; set; }
        public virtual int NumberTickets { get; set; }
        public virtual DateTime DateRegistered { get; set; }
        public virtual DateTime DateUpdated { get; set; }
    }

    public class vCancelledRegistrationsMap : ClassMap<vCancelledRegistrations>
    {
        public vCancelledRegistrationsMap() 
        {
            Table("vCancelledRegistrations");
            ReadOnly();
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.StudentId);
            Map(x => x.FirstName);
            Map(x => x.MI);
            Map(x => x.LastName);
            Map(x => x.Email);
            Map(x => x.CeremonyId);
            Map(x => x.MajorCode);
            Map(x => x.NumberTickets);
            Map(x => x.DateRegistered);
            Map(x => x.DateUpdated);
        }
        

    }
}
