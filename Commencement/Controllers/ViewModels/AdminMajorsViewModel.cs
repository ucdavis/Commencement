using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminMajorsViewModel
    {
        public IList<CeremonyCounts> CeremonyCounts { get; set; }

        public static AdminMajorsViewModel Create(IRepository repository, ICeremonyService ceremonyService, IRegistrationService registrationService, IPrincipal user)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(ceremonyService != null, "ceremonyService is required.");
            Check.Require(user != null, "user is required.");

            // get the current user's ceremonies
            var ceremonies = ceremonyService.GetCeremonies(user.Identity.Name, TermService.GetCurrent());
            var participations = repository.OfType<RegistrationParticipation>().Queryable.Where(a => ceremonies.Contains(a.Ceremony)).ToList();
            participations = participations.Where(a => a.IsValidForTickets).ToList();

            var viewModel = new AdminMajorsViewModel() {CeremonyCounts = new List<CeremonyCounts>()};

            // go through all the ceremonies
            foreach (var a in ceremonies)
            {
                var ceremonyCount = new CeremonyCounts() {Ceremony = a, MajorCounts = new List<MajorCount>()};

                // go through each of the majors
                foreach (var b in a.Majors)
                {
                    var majorCount = new MajorCount() {Major = b};
                    majorCount.TotalTickets = participations.Where(c => c.Major == b).Sum(c => c.TotalTickets);
                    majorCount.TotalStreaming = participations.Where(c => c.Major == b).Sum(c => c.TotalStreamingTickets);
                    majorCount.ProjectedTickets = participations.Where(c => c.Major == b).Sum(c => c.ProjectedTickets);
                    majorCount.ProjectedStreamingTickets = participations.Where(c => c.Major == b).Sum(c => c.ProjectedStreamingTickets);

                    if (majorCount.TotalTickets > 0 || majorCount.TotalStreaming > 0 || majorCount.ProjectedTickets > 0 || majorCount.ProjectedStreamingTickets > 0)
                    {
                        ceremonyCount.MajorCounts.Add(majorCount);
                    }
                }

                viewModel.CeremonyCounts.Add(ceremonyCount);
            }

            return viewModel;
        }
    }

    public class CeremonyCounts
    {
        public Ceremony Ceremony { get; set; }
        public IList<MajorCount> MajorCounts { get; set; }
    }

    public class MajorCount
    {
        public MajorCode Major { get; set; }
        public int TotalTickets { get; set; }
        public int TotalStreaming { get; set; }
        public int ProjectedTickets { get; set; }
        public int ProjectedStreamingTickets { get; set; }
    }
}