using System;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class StudentDisplayRegistrationViewModel
    {
        public Registration Registration { get; set; }
        public DateTime EarliestExtraTicket { get; set; }
        public DateTime LatestExtraTicket { get; set; }
        public DateTime LatestRegDeadline { get; set; }

        public static StudentDisplayRegistrationViewModel Create(IRepository repository, Registration registration)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new StudentDisplayRegistrationViewModel() {Registration = registration};
            
            var participations = repository.OfType<RegistrationParticipation>().Queryable.Where(a=>a.Registration == registration).ToList();
            var ceremonies = participations.Select(a => a.Ceremony).ToList();
            viewModel.EarliestExtraTicket = ceremonies.Min(a => a.ExtraTicketBegin);
            viewModel.LatestExtraTicket = ceremonies.Max(a => a.ExtraTicketDeadline);
            viewModel.LatestRegDeadline = ceremonies.Max(a => a.RegistrationDeadline);

            return viewModel;
        }
    }
}