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
        List<RegistrationParticipation> GetPendingExtraTicket(string userId, int ceremonyId, TermCode termCode = null);
        List<RegistrationPetition> GetPendingRegistration(string userId, TermCode termCode = null, List<int> ceremonyIds = null);
    }

    public class PetitionService : IPetitionService
    {
        private readonly IRepository<Registration> _registrationRepository;
        private readonly IRepository<RegistrationParticipation> _registrationParticipationRepository;
        private readonly IRepository<ExtraTicketPetition> _extraTicketRepository;
        private readonly IRepository<RegistrationPetition> _registrationPetitionRepository;
        private readonly ICeremonyService _ceremonyService;

        public PetitionService(IRepository<Registration> registrationRepository, IRepository<RegistrationParticipation> registrationParticipationRepository, IRepository<ExtraTicketPetition> extraTicketRepository, IRepository<RegistrationPetition> registrationPetitionRepository, ICeremonyService ceremonyService)
        {
            _registrationRepository = registrationRepository;
            _registrationParticipationRepository = registrationParticipationRepository;
            _extraTicketRepository = extraTicketRepository;
            _registrationPetitionRepository = registrationPetitionRepository;
            _ceremonyService = ceremonyService;
        }

        /// <summary>
        /// Gets a list of user's pending extra ticket petitions.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="termCode"></param>
        /// <returns>Return registration so user has access to name and what not</returns>
        public List<RegistrationParticipation> GetPendingExtraTicket(string userId, int ceremonyId, TermCode termCode = null)
        {
            // get the list of my valid ceremonies
            var participations = _registrationParticipationRepository.Queryable.Where(a => a.Ceremony.Id == ceremonyId
                                                                                        && !a.Cancelled 
                                                                                        && a.ExtraTicketPetition != null 
                                                                                        && a.ExtraTicketPetition.IsPending);
            return participations.ToList();
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