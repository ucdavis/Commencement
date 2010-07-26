using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using UCDArch.Data.NHibernate;

namespace Commencement.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Roles = RoleNames.RoleAdmin;    //Set the roles prop to a comma delimited string of allowed roles
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserOnlyAttribute : AuthorizeAttribute
    {
        public UserOnlyAttribute()
        {
            Roles = RoleNames.RoleUser;    //Set the roles prop to a comma delimited string of allowed roles
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AnyoneWithRoleAttribute : AuthorizeAttribute
    {
        public AnyoneWithRoleAttribute()
        {
            Roles = RoleNames.RoleAdmin + "," + RoleNames.RoleUser;    //Set the roles prop to a comma delimited string of allowed roles
        }
    }

    public static class RoleNames
    {
        public static readonly string RoleAdmin = "Admin";
        public static readonly string RoleUser = "User";
    }
}
