using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Commencement.Core.Domain;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

namespace Commencement.Core
{
    public class User : DomainObjectWithTypedId<string>
    {
        public User()
        {
            Roles = new List<Role>();
        }
        public User(string id) : this()
        {
            Id = id == null ? null : id.ToLower();
            IsActive = true;
        }

        public override string Id { get; protected set; }

        public virtual string Email { get; set; }
        public virtual string UserEmail { get; set; } //Actual user's email. We have changed the Email above for a couple people in catbert to a generic Account.
        public virtual string Phone { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int? CatbertUserId { get; set; } //Maybe don't use. We just need to link this to the Ceremony Editors

        public virtual IList<Role> Roles { get; set; }
    }
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            
            Map(x => x.Email);
            Map(x => x.UserEmail);
            Map(x => x.Phone);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.IsActive);
            Map(x => x.CatbertUserId);
            HasManyToMany(x => x.Roles).Table("Permissions").ChildKeyColumn("RoleID").ParentKeyColumn("UserID");
        }
    }
}
