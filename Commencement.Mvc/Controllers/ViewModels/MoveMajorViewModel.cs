using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class MoveMajorViewModel
    {
        public IEnumerable<Ceremony> Ceremonies { get; set; }
        public IEnumerable<MajorCode> MajorCodes { get; set; }

        public static MoveMajorViewModel Create(IRepository repository, IPrincipal currentUser, ICeremonyService ceremonyService)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new MoveMajorViewModel() {Ceremonies = ceremonyService.GetCeremonies(currentUser.Identity.Name, TermService.GetCurrent()), MajorCodes = new List<MajorCode>()};
            //viewModel.MajorCodes = viewModel.Ceremonies.Select(a => a.Majors).ToList();

            var majorCodes = new List<MajorCode>();
            foreach (var a in viewModel.Ceremonies)
            {
                majorCodes.AddRange(a.Majors);
            }

            viewModel.MajorCodes = majorCodes.OrderBy(a=>a.Name).Distinct().ToList();

            return viewModel;
        }
    }
}