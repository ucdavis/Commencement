using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class CommencementViewModel
    {
        public IEnumerable<Core.Domain.Ceremony> Ceremonies { get; set; }

        public static CommencementViewModel Create(IRepository repository, ICeremonyService ceremonyService, string userId)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(!string.IsNullOrEmpty(userId), "User Id is required.");

            var viewModel = new CommencementViewModel()
                                {
                                    Ceremonies = ceremonyService.GetCeremonies(userId)
                                };

            return viewModel;
        }

    }
}