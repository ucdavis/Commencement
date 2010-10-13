using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class CommencementViewModel
    {
        public IEnumerable<TermCode> TermCodes { get; set; }
        public IEnumerable<Core.Domain.Ceremony> Ceremonies { get; set; }

        public static CommencementViewModel Create(IRepository repository, string userId)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new CommencementViewModel()
                                {
                                    TermCodes = repository.OfType<TermCode>().GetAll(),
                                    Ceremonies = repository.OfType<CeremonyEditor>().Queryable.Where(a => a.LoginId == userId).Select(a => a.Ceremony).Distinct().ToList()
                                };

            return viewModel;
        }

    }
}