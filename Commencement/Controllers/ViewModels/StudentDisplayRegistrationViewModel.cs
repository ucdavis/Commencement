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
        //public DateTime EarliestExtraTicket { get; set; }
        //public DateTime LatestExtraTicket { get; set; }
        //public DateTime LatestRegDeadline { get; set; }

        public bool CanPetitionForExtraTickets { get; set; }
        public bool CanEditRegistration { get; set; }

        public StudentDisplayRegistrationViewModel()
        {
            CanPetitionForExtraTickets = false;
            CanEditRegistration = false;
        }

        public static StudentDisplayRegistrationViewModel Create(IRepository repository, Registration registration)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new StudentDisplayRegistrationViewModel() {Registration = registration};
            
            var participations = repository.OfType<RegistrationParticipation>().Queryable.Where(a=>a.Registration == registration).ToList();
            var ceremonies = participations.Select(a => a.Ceremony).ToList();
            var earliestExtraTicket = ceremonies.Min(a => a.ExtraTicketBegin);
            var latestExtraTicket = ceremonies.Max(a => a.ExtraTicketDeadline);
            var hasAvailableExtraTicketPetition = participations.Any(a=>a.ExtraTicketPetition== null);
            var latestRegDeadline = ceremonies.Max(a => a.RegistrationDeadline);

            if (DateTime.Now >= earliestExtraTicket && DateTime.Now <= latestExtraTicket && hasAvailableExtraTicketPetition) viewModel.CanPetitionForExtraTickets = true;
            if (DateTime.Now <= latestRegDeadline) viewModel.CanEditRegistration = true;

            return viewModel;
        }
    }
}