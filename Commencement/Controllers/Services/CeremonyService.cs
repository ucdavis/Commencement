using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Services
{
    public interface ICeremonyService
    {
        List<Ceremony> GetCeremonies(string userId, TermCode termCode = null);
        List<int> GetCeremonyIds(string userId, TermCode termCode = null);
    }

    public class CeremonyService : ICeremonyService
    {
        private readonly IRepository _repository;

        private readonly string UserCeremoniesKey = "UserCeremoniesKey";
        private readonly string UserCeremonyIdsKey = "UserCeremonyIdsKey";
        private List<Ceremony> UserCeremonies { 
            get { return (List<Ceremony>)System.Web.HttpContext.Current.Session[UserCeremoniesKey]; }
            set { System.Web.HttpContext.Current.Session[UserCeremoniesKey] = value; }
        }
        private List<int> UserCeremonyIds
        {
            get { return (List<int>)System.Web.HttpContext.Current.Session[UserCeremonyIdsKey]; }
            set { System.Web.HttpContext.Current.Session[UserCeremonyIdsKey] = value; }
        }

        public CeremonyService(IRepository repository)
        {
            _repository = repository;
        }

        public virtual List<Ceremony> GetCeremonies (string userId, TermCode termCode = null)
        {
            if (UserCeremonies == null)
            {

                var ceremonyIds = GetCeremonyIds(userId, termCode);

                // build the query for getting the available ceremonies
                var query = from a in _repository.OfType<Ceremony>().Queryable
                            where ceremonyIds.Contains(a.Id)
                            select a;

                UserCeremonies = query.ToList();
            }

            return UserCeremonies;
        }

        public virtual List<int> GetCeremonyIds (string userId, TermCode termCode = null)
        {
            if (UserCeremonyIds == null)
            {
                // get a list of ceremonies that the user has access to
                var query = from a in _repository.OfType<CeremonyEditor>().Queryable
                            where a.User.LoginId == userId
                            select a;

                if (termCode != null)
                {
                    query = query.Where(a => a.Ceremony.TermCode == termCode);
                }

                UserCeremonyIds = query.Select(a => a.Ceremony.Id).ToList();
            }

            return UserCeremonyIds;
        }

        public virtual bool HasAccess(int id, string userId)
        {
            var ceremony = _repository.OfType<Ceremony>().GetNullableById(id);
            Check.Require(ceremony != null, "ceremony is required.");

            return ceremony.IsEditor(userId);
        }
    }
}