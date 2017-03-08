using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.MVC.Controllers.Services
{
    public interface IUserService
    {
        vUser GetCurrentUser(IPrincipal currentUser);
    }

    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        public vUser GetCurrentUser(IPrincipal currentUser)
        {
            return _repository.OfType<vUser>().Queryable.Where(a => a.LoginId == currentUser.Identity.Name).FirstOrDefault();
        }
    }
}