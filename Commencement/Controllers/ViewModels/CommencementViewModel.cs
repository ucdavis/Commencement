using System.Collections.Generic;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class CommencementViewModel
    {
        public IEnumerable<TermCode> TermCodes { get; set; }
        public IEnumerable<Core.Domain.Ceremony> Ceremonies { get; set; }

        public static CommencementViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new CommencementViewModel()
                                {
                                    TermCodes = repository.OfType<TermCode>().GetAll(),
                                    Ceremonies = repository.OfType<Core.Domain.Ceremony>().GetAll()
                                };

            return viewModel;
        }

    }
}