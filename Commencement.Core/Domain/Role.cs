using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

namespace Commencement.Core.Domain
{
    /// <summary>
    /// A list of available roles. Will manually add to the DB. Base of current Catbert Roles for Commencement.
    /// </summary>
    public class Role : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
        public virtual IList<User> Users { get; set; }
    }

    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name);
            HasManyToMany(x => x.Users).Table("Permissions").ParentKeyColumn("RoleID").ChildKeyColumn("UserID");
        }
    }
}
