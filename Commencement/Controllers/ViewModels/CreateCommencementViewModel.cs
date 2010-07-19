using System.Collections.Generic;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class CreateCommencementViewModel
    {
        // list of majors
        public IEnumerable<MajorCode> MajorCodes { get; set; }

        public static CreateCommencementViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new CreateCommencementViewModel()
                                {
                                    MajorCodes = repository.OfType<MajorCode>().GetAll()
                                };

            return viewModel;
        }

    }
}