using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.Services
{
    public interface IPetitionService
    {
        List<Registration> GetPendingExtraTicket(string userId, TermCode termCode = null, List<int> ceremonyIds = null);
        List<RegistrationPetition> GetPendingRegistration(string userId, TermCode termCode = null, List<int> ceremonyIds = null);
    }

    public class PetitionService : IPetitionService
    {
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepository<RegistrationPetition> _registrationPetitionRepository;
        private readonly ICeremonyService _ceremonyService;

        public PetitionService(IRepository<Registration> registrationRepository, IRepository<RegistrationPetition> registrationPetitionRepository, ICeremonyService ceremonyService)
        {
            _registrationRepository = registrationRepository;
            _registrationPetitionRepository = registrationPetitionRepository;
            _ceremonyService = ceremonyService;
        }

        /// <summary>
        /// Gets a list of user's pending extra ticket petitions.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="termCode"></param>
        /// <param name="ceremonyIds"></param>
        /// <returns>Return registration so user has access to name and what not</returns>
        public List<Registration> GetPendingExtraTicket(string userId, TermCode termCode = null, List<int> ceremonyIds = null)
        {
            // get the list of my valid ceremonies
            if (ceremonyIds == null) ceremonyIds = _ceremonyService.GetCeremonyIds(userId, termCode ?? TermService.GetCurrent());

            // filter the registrations to what we are looking for
            var registrations =
                _registrationRepository.Queryable.Where(
                    a =>
                    a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsPending &&
                    ceremonyIds.Contains(a.RegistrationParticipations[0].Ceremony.Id));


            return registrations.ToList();
        }

        public List<RegistrationPetition> GetPendingRegistration(string userId, TermCode termCode, List<int> ceremonyIds = null)
        {
            // get the list of my valid ceremonies
            if (ceremonyIds == null) ceremonyIds = _ceremonyService.GetCeremonyIds(userId, termCode ?? TermService.GetCurrent());

            var registration =
                _registrationPetitionRepository.Queryable.Where(a => a.IsPending && ceremonyIds.Contains(a.Ceremony.Id));

            return registration.ToList();
        }
    }
}