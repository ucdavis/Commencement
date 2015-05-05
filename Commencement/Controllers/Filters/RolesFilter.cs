using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Commencement.Core.Domain;
using UCDArch.Data.NHibernate;

namespace Commencement.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Roles = Role.Codes.Admin;    //Set the roles prop to a comma delimited string of allowed roles
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

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //public class AnyoneWithRoleAttribute : AuthorizeAttribute
    //{
    //    public AnyoneWithRoleAttribute()
    //    {
    //        Roles = RoleNames.RoleAdmin + "," + RoleNames.RoleUser;    //Set the roles prop to a comma delimited string of allowed roles
    //    }
    //}

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AnyoneWithRoleAttribute : AuthorizeAttribute
    {
        public AnyoneWithRoleAttribute()
        {
            Roles = Role.Codes.Admin + "," + Role.Codes.User; //Set the roles prop to a comma delimited string of allowed roles
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TicketingAttribute : AuthorizeAttribute
    {
        public TicketingAttribute()
        {
            Roles = RoleNames.RoleTicketing;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EmulationUserOnlyAttribute : AuthorizeAttribute
    {
        public EmulationUserOnlyAttribute()
        {
            Roles = RoleNames.RoleEmulationUser;
        }
    }

    public static class RoleNames
    {
        public static readonly string RoleAdmin = "Admin";
        public static readonly string RoleUser = "User";
        public static readonly string RoleEmulationUser = "EmulationUser";

        public static readonly string RoleTicketing = "Ticketing";
    }
}
