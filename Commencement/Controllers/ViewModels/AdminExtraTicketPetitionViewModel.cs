using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminExtraTicketPetitionViewModel
    {
        public IEnumerable<RegistrationParticipation> RegistrationParticipations { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public Ceremony Ceremony { get; set; }

        public static AdminExtraTicketPetitionViewModel Create(IRepository repository, ICeremonyService ceremonyService, IPetitionService petitionService, IPrincipal currentUser, TermCode termCode, int? ceremonyId)
        {
            Check.Require(repository != null, "Repository is required.");

            var ceremonies = ceremonyService.GetCeremonies(currentUser.Identity.Name, termCode);
            //var ceremonyIds = ceremonies.Select(a => a.Id).ToList();

            var viewModel = new AdminExtraTicketPetitionViewModel()
                                {
                                    Ceremonies = ceremonies,
                                    Ceremony = ceremonyId.HasValue ? repository.OfType<Ceremony>().GetNullableById(ceremonyId.Value) : null
                                };

            // has a ceremony been selected and does the current user have access
            if (ceremonyId.HasValue && ceremonyService.HasAccess(ceremonyId.Value, currentUser.Identity.Name))
            {
                viewModel.RegistrationParticipations = petitionService.GetPendingExtraTicket(currentUser.Identity.Name, ceremonyId.Value, termCode);
            }

            return viewModel;
        }
    }
}