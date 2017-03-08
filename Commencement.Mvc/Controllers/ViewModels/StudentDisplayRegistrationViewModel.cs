using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.MVC.Controllers.ViewModels
{
    public class StudentDisplayRegistrationViewModel
    {
        public Registration Registration { get; set; }
        //public IEnumerable<RegistrationPetition> Petitions { get; set; }
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
            Check.Require(registration != null, "registration is required.");

            var viewModel = new StudentDisplayRegistrationViewModel() {Registration = registration};
            
            var participations = repository.OfType<RegistrationParticipation>().Queryable.Where(a=>a.Registration == registration).ToList();
            var ceremonies = participations.Select(a => a.Ceremony).ToList();
            var hasAvailableExtraTicketPetition = participations.Any(a=>a.ExtraTicketPetition== null);

            viewModel.CanPetitionForExtraTickets = (ceremonies.Any(a => a.CanSubmitExtraTicket()) && hasAvailableExtraTicketPetition);
            viewModel.CanEditRegistration = ceremonies.Any(a => a.CanRegister(true));

            return viewModel;
        }
    }
}