using System;
using Commencement.Core.Domain;
using System.Collections.Generic;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class ExtraTicketPetitionModel
    {
        public ExtraTicketPetition ExtraTicketPetition { get; set; }
        public Registration Registration { get; set; }
        public DateTime DisclaimerStartDate { get; set; }

        public static ExtraTicketPetitionModel Create(IRepository repository, Registration registration)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(registration != null, "Registration is required.");

            var viewModel = new ExtraTicketPetitionModel
                                {
                                    Registration = registration, 
                                    ExtraTicketPetition = new ExtraTicketPetition(),
                                    DisclaimerStartDate = registration.Ceremony.RegistrationDeadline.AddDays(7)
                                };
            return viewModel;
        }
    }
}
