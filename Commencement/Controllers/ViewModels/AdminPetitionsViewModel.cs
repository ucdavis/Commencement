using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminPetitionsViewModel
    {
        public IEnumerable<Registration> PendingExtraTicket { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }

        public static AdminPetitionsViewModel Create(IRepository repository)
        {
            Check.Require(repository!= null, "Repository is required.");

            var viewModel = new AdminPetitionsViewModel()
                                {
                                    PendingExtraTicket = repository.OfType<Registration>().Queryable.Where(a => a.ExtraTicketPetition != null && a.ExtraTicketPetition.IsPending).ToList(),
                                    Ceremonies = repository.OfType<Ceremony>().Queryable.Where(a=>a.TermCode == TermService.GetCurrent())
                                };

            return viewModel;
        }
    }
}