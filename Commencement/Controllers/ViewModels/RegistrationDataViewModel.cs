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

            //var viewModel = new RegistrationDataViewModel();
            //viewModel.RegistrationData = new List<RegistrationData>();

            //var ceremonies = ceremonyService.GetCeremonies(userId, termCode);

            //viewModel.RegistrationData = ceremonies.Select(a => new RegistrationData()
            //                                      {
            //                                          TermCode = a.TermCode,
            //                                          Ceremony = a,
            //                                          Registrants = a.RegistrationParticipations.Count(),
            //                                          CancelledRegistrants = a.RegistrationParticipations.Count(),
            //                                          RegistrationPetitionsSubmitted = a.RegistrationPetitions.Count,
            //                                          RegistrationPetitionsApproved =
            //                                              a.RegistrationPetitions.Where(
            //                                                  b => b.IsApproved && !b.IsPending).Count(),
            //                                          TicketsRequested = a.ProjectedTicketCount,
            //                                          ExtraTicketsRequested = a.ProjectedTicketCount,
            //                                          TotalTickets = a.TotalTickets
            //                                      }).ToList();

            //return viewModel;


            var viewModel = new RegistrationDataViewModel();
            
            // load all ceremonies that a user has access to
            var ceremonies = ceremonyService.GetCeremonies(userId);

            viewModel.RegistrationData = (from a in ceremonies
                       select new RegistrationData()
                                  {
                                      TermCode = a.TermCode,
                                      Ceremony = a,
                                      Registrants = a.RegistrationParticipations.Where(b => !b.Cancelled).Count(),
                                      CancelledRegistrants = a.RegistrationParticipations.Where(b => b.Cancelled).Count(),
                                      RegistrationPetitionsSubmitted = a.RegistrationPetitions.Count,
                                      RegistrationPetitionsApproved = a.RegistrationPetitions.Where(b => b.IsApproved).Count(),
                                      ExtraTicketPetitionsSubmitted = a.RegistrationParticipations.Where(b => b.ExtraTicketPetition != null).Count(),
                                      ExtraTicketPetitionsApproved = a.RegistrationParticipations.Where(b => b.ExtraTicketPetition != null && b.ExtraTicketPetition.IsApproved).Count(),
                                      TicketsPavilion = a.TicketCount,
                                      TicketsBallroom = a.TicketStreamingCount,
                                      TicketsByPetition = a.RegistrationParticipations.Where(b => b.ExtraTicketPetition != null && b.ExtraTicketPetition.IsApprovedCompletely).Sum(b => b.ExtraTicketPetition.TotalTickets)
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
        /// # of extra ticket petitions submitted
        /// </summary>
        public int ExtraTicketPetitionsSubmitted { get; set; }
        /// <summary>
        /// # of extra ticket petitions approved
        /// </summary>
        public int ExtraTicketPetitionsApproved { get; set; }
        /// <summary>
        /// # of tickets to the Pavilion
        /// </summary>
        public int TicketsPavilion { get; set; }
        /// <summary>
        /// # of tickets to the ballroom
        /// </summary>
        public int? TicketsBallroom { get; set; }
        /// <summary>
        /// # of tickets to either pavilion or ballroom by petition
        /// </summary>
        public int TicketsByPetition { get; set; }

        ///// <summary>
        ///// # of tickets requested by original request
        ///// </summary>
        //public int TicketsRequested { get; set; }
        ///// <summary>
        ///// # of tickets requested by extra request
        ///// </summary>
        //public int ExtraTicketsRequested { get; set; }
        ///// <summary>
        ///// # of total tickets approved
        ///// </summary>
        //public int TotalTickets { get; set; }
    }
}