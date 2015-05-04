using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;
using UCDArch.Core.DomainModel;

namespace Commencement.Core
{
    public class User : DomainObject
    {

        public virtual string LoginID { get; set; }
        public virtual string Email { get; set; }
        public virtual string UserEmail { get; set; } //Actual user's email. We have changed the Email above for a couple people in catbert to a generic Account.
        public virtual string Phone { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int? CatbertUserId { get; set; } //Maybe don't use. We just need to link this to the Ceremony Editors
    }
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            
            Map(x => x.LoginID);
            Map(x => x.Email);
            Map(x => x.UserEmail);
            Map(x => x.Phone);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.IsActive);
            Map(x => x.CatbertUserId);
        }
    }
}
