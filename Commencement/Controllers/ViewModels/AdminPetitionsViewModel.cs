using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminPetitionsViewModel
    {
        public IEnumerable<Registration> PendingExtraTicket { get; set; }
        public IEnumerable<RegistrationPetition> PendingRegistrationPetitions { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }

        public static AdminPetitionsViewModel Create(IRepository repository, ICeremonyService ceremonyService, IPetitionService petitionService, string userId, TermCode termCode)
        {
            Check.Require(repository!= null, "Repository is required.");

            //var viewModel = new AdminPetitionsViewModel()
            //                    {
            //                        PendingExtraTicket = repository.OfType<Registration>().Queryable.Where(a => a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsPending).ToList(),
            //                        Ceremonies = repository.OfType<Ceremony>().Queryable.Where(a=>a.TermCode == TermService.GetCurrent()),
            //                        PendingRegistrationPetitions = repository.OfType<RegistrationPetition>().Queryable.Where(
            //                                                        a=>a.TermCode == TermService.GetCurrent() && a.IsPending).ToList()
            //                    };

            var ceremonies = ceremonyService.GetCeremonies(userId, termCode);
            var ceremonyIds = ceremonies.Select(a => a.Id).ToList();

            var viewModel = new AdminPetitionsViewModel()
                                {
                                    //PendingExtraTicket = petitionService.GetPendingExtraTicket(userId, termCode, ceremonyIds),
                                    PendingRegistrationPetitions = petitionService.GetPendingRegistration(userId, termCode, ceremonyIds),
                                    Ceremonies = ceremonies
                                };

            return viewModel;
        }
    }
}