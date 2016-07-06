using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class AdminExtraTicketPetitionViewModel
    {
        public IEnumerable<RegistrationParticipation> RegistrationParticipations { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public Ceremony Ceremony { get; set; }
        public bool ViewAll { get; set; }

        public static AdminExtraTicketPetitionViewModel Create(IRepository repository, ICeremonyService ceremonyService, IPetitionService petitionService, IPrincipal currentUser, TermCode termCode, int? ceremonyId, bool? viewAll)
        {
            Check.Require(repository != null, "Repository is required.");

            // set the default to false
            viewAll = viewAll ?? false;

            var ceremonies = ceremonyService.GetCeremonies(currentUser.Identity.Name, termCode);
            //var ceremonyIds = ceremonies.Select(a => a.Id).ToList();

            var viewModel = new AdminExtraTicketPetitionViewModel()
                                {
                                    Ceremonies = ceremonies,
                                    Ceremony = ceremonyId.HasValue ? repository.OfType<Ceremony>().GetNullableById(ceremonyId.Value) : null,
                                    ViewAll = viewAll.Value
                                };

            // has a ceremony been selected and does the current user have access
            if (ceremonyId.HasValue && ceremonyService.HasAccess(ceremonyId.Value, currentUser.Identity.Name))
            {
                if (viewAll.Value)
                {
                    viewModel.RegistrationParticipations = viewModel.Ceremony.RegistrationParticipations.Where(a => a.ExtraTicketPetition != null).ToList();
                }
                else
                {
                    viewModel.RegistrationParticipations = petitionService.GetPendingExtraTicket(currentUser.Identity.Name, ceremonyId.Value, termCode);    
                }
                
            }

            return viewModel;
        }
    }
}