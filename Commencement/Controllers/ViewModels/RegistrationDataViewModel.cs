using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class RegistrationDataViewModel
    {
        public IList<RegistrationData> RegistrationData { get; set; }

        public static RegistrationDataViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new RegistrationDataViewModel();
            viewModel.RegistrationData = new List<RegistrationData>();

            var termCodes = repository.OfType<TermCode>().GetAll();
            foreach (var tc in termCodes)
            {
                foreach (var ceremony in tc.Ceremonies)
                {
                    var registrationData = new RegistrationData();
                    registrationData.TermCode = tc;
                    registrationData.Ceremony = ceremony;
                    registrationData.Registrants = ceremony.Registrations.Count;
                    registrationData.RegistrationPetitionsSubmitted = ceremony.RegistrationPetitions.Count;
                    registrationData.RegistrationPetitionsApproved = ceremony.RegistrationPetitions.Where(a => a.IsApproved && !a.IsPending).Count();
                    registrationData.TicketsRequested = ceremony.RequestedTickets;
                    registrationData.ExtraTicketsRequested = ceremony.ExtraRequestedtickets;
                    registrationData.TotalTickets = ceremony.TotalTickets;

                    viewModel.RegistrationData.Add(registrationData);
                }
            }

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