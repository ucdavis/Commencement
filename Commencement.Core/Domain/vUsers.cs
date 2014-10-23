using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    public class vUser : DomainObject
    {
        public virtual string LoginId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }

        public virtual string Phone { get; set; }

        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

    public class vUserMap : ClassMap<vUser>
    {
        public vUserMap()
        {
            Table("vUsers");
            ReadOnly();

            Id(x => x.Id);

            Map(x => x.LoginId);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Email);
            Map(x => x.Phone);
        }
    }
}
