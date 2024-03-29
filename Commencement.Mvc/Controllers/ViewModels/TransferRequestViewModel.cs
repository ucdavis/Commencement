﻿using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.ViewModels
{
    public class TransferRequestViewModel
    {
        public TransferRequest TransferRequest { get; set; }
        public RegistrationParticipation RegistrationParticipation { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }

        public static TransferRequestViewModel Create(IRepository repository, RegistrationParticipation registrationParticipation, string username, TransferRequest transferRequest = null)
        {
            var viewModel = new TransferRequestViewModel()
                {
                    Ceremonies = repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode == TermService.GetCurrent()),
                    RegistrationParticipation = registrationParticipation,
                    TransferRequest = transferRequest ?? new TransferRequest()
                };

            return viewModel;
        }
    }
}