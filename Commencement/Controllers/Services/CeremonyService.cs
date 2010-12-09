using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Services
{
    public interface ICeremonyService
    {
        List<Ceremony> GetCeremonies(string userId, TermCode termCode = null);
        List<int> GetCeremonyIds(string userId, TermCode termCode = null);
        void ResetUserCeremonies();
        bool HasAccess(int id, string userId);

        List<Ceremony> StudentEligibility(List<MajorCode> majors, decimal totalUnits, TermCode termCode = null);
    }

    public class CeremonyService : ICeremonyService
    {
        private readonly IRepository _repository;

        private List<Ceremony> UserCeremonies { 
            get { return (List<Ceremony>)System.Web.HttpContext.Current.Session[StaticIndexes.UserCeremoniesKey]; }
            set { System.Web.HttpContext.Current.Session[StaticIndexes.UserCeremoniesKey] = value; }
        }
        private List<int> UserCeremonyIds
        {
            get { return (List<int>)System.Web.HttpContext.Current.Session[StaticIndexes.UserCeremonyIdsKey]; }
            set { System.Web.HttpContext.Current.Session[StaticIndexes.UserCeremonyIdsKey] = value; }
        }

        public CeremonyService(IRepository repository)
        {
            _repository = repository;
        }

        public virtual List<Ceremony> GetCeremonies (string userId, TermCode termCode = null)
        {
            if (UserCeremonies == null || ((List<Ceremony>)UserCeremonies).Count <= 0)
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

        public virtual void ResetUserCeremonies()
        {
            UserCeremonies = null;
            UserCeremonyIds = null;
        }

        public virtual bool HasAccess(int id, string userId)
        {
            var ceremony = _repository.OfType<Ceremony>().GetNullableById(id);
            Check.Require(ceremony != null, "ceremony is required.");

            return ceremony.IsEditor(userId);
        }

        /// <summary>
        /// Returns ceremonies that this student is eligible for or are eligible to petition for
        /// </summary>
        /// <param name="majors"></param>
        /// <returns>List of ceremonies, if empty, student not eligible for ceremony is system.</returns>
        public virtual List<Ceremony> StudentEligibility(List<MajorCode> majors, decimal totalUnits, TermCode termCode = null)
        {
            // get term code if we don't have one
            if (termCode == null) termCode = TermService.GetCurrent();

            // load all valid ceremonies for current term
            var ceremonies = _repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode == termCode).ToList();

            var eligibleCeremonies = new List<Ceremony>();

            // find ceremonies, student is eligible for
            foreach (var a in ceremonies)
            {
                // make sure units are enough
                if (totalUnits >= a.PetitionThreshold)
                {
                    // go through each of the student's major
                    foreach (var b in majors)
                    {
                        // if major is in ceremony
                        if (a.Majors.Contains(b))
                        {
                            // add to the list of valid
                            eligibleCeremonies.Add(a);
                        }
                    }
                }
            }

            // return distinct list
            return eligibleCeremonies.Distinct().ToList();
        }
    }
}