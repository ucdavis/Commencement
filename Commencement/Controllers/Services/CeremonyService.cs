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

        public CeremonyService(IRepository repository)
        {
            _repository = repository;
        }

        public virtual List<Ceremony> GetCeremonies (string userId, TermCode termCode = null)
        {
            var ceremonyIds = GetCeremonyIds(userId, termCode);

            // build the query for getting the available ceremonies
            var query = from a in _repository.OfType<Ceremony>().Queryable
                        where ceremonyIds.Contains(a.Id)
                        select a;

            // add the restriction on term if needed
            if (termCode != null)
            {
                query = query.Where(a => a.TermCode == termCode);
            }

            return query.ToList();
        }

        public virtual List<int> GetCeremonyIds (string userId, TermCode termCode = null)
        {
            // get a list of ceremonies that the user has access to
            return (from a in _repository.OfType<CeremonyEditor>().Queryable
                    where a.User.LoginId == userId
                    select a.Ceremony.Id).ToList();
        }

        public virtual bool HasAccess(int id, string userId)
        {
            var ceremony = _repository.OfType<Ceremony>().GetNullableById(id);
            Check.Require(ceremony != null, "ceremony is required.");

            return ceremony.IsEditor(userId);
        }
    }
}