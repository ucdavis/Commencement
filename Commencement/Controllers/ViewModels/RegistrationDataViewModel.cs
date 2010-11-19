using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationDataViewModel
    {
        public IList<RegistrationData> RegistrationData { get; set; }

        public static RegistrationDataViewModel Create(IRepository repository, ICeremonyService ceremonyService, string userId, TermCode termCode)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new RegistrationDataViewModel();
            viewModel.RegistrationData = new List<RegistrationData>();

            var ceremonies = ceremonyService.GetCeremonies(userId, termCode);

            viewModel.RegistrationData = ceremonies.Select(a => new RegistrationData()
                                                  {
                                                      TermCode = a.TermCode,
                                                      Ceremony = a,
                                                      Registrants = a.Registrations.Count(),
                                                      CancelledRegistrants = a.Registrations.Count(),
                                                      RegistrationPetitionsSubmitted = a.RegistrationPetitions.Count,
                                                      RegistrationPetitionsApproved =
                                                          a.RegistrationPetitions.Where(
                                                              b => b.IsApproved && !b.IsPending).Count(),
                                                      TicketsRequested = a.RequestedTickets,
                                                      ExtraTicketsRequested = a.ExtraRequestedtickets,
                                                      TotalTickets = a.TotalRequestedTickets
                                                  }).ToList();

            return viewModel;
        }
    }

    public class RegistrationData
    {
        public TermCode TermCode { get; set; }
        public Ceremony Ceremony { get; set; }
        /// <summary>
        /// # of registrants
        /// </summary>
        public int Registrants { get; set; }
        /// <summary>
        /// # of cancelled registrants
        /// </summary>
        public int CancelledRegistrants { get; set; }
        /// <summary>
        /// # of registration petitions submitted
        /// </summary>
        public int RegistrationPetitionsSubmitted { get; set; }
        /// <summary>
        /// # of registration petitions approved
        /// </summary>
        public int RegistrationPetitionsApproved { get; set; }
        /// <summary>
        /// # of tickets requested by original request
        /// </summary>
        public int TicketsRequested { get; set; }
        /// <summary>
        /// # of tickets requested by extra request
        /// </summary>
        public int ExtraTicketsRequested { get; set; }
        /// <summary>
        /// # of total tickets approved
        /// </summary>
        public int TotalTickets { get; set; }
    }
}