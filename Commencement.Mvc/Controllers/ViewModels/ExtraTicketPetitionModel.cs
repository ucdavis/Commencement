using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class ExtraTicketPetitionModel
    {
        public Registration Registration { get; set; }
        public DateTime DisclaimerStartDate { get; set; }
        public IEnumerable<int> AvailableParticipationIds { get; set; }
        public IEnumerable<ExtraTicketPetitionPostModel> ExtraTicketPetitionPostModels { get; set; }

        public static ExtraTicketPetitionModel Create(IRepository repository, Registration registration)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");

            var viewModel = new ExtraTicketPetitionModel
                                {
                                    Registration = registration, 
                                    ExtraTicketPetitionPostModels = new List<ExtraTicketPetitionPostModel>(),
                                    AvailableParticipationIds = registration.RegistrationParticipations.Where(
                                                                    a =>
                                                                    DateTime.Now >= a.Ceremony.ExtraTicketBegin && DateTime.Now < a.Ceremony.ExtraTicketDeadline.AddDays(1) &&
                                                                    a.ExtraTicketPetition == null && !a.Cancelled).Select(a => a.Id).ToList()
                                };
            return viewModel;
        }
    }

    public class ExtraTicketPetitionPostModel
    {
        public RegistrationParticipation RegistrationParticipation { get; set; }
        public Ceremony Ceremony { get; set; }
        public int NumberTickets { get; set; }
        public int NumberStreamingTickets { get; set; }
        public string Reason { get; set; }
    }
}
