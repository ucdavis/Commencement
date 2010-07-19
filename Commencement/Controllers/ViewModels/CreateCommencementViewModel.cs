using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class CreateCommencementViewModel
    {

        public static CreateCommencementViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new CreateCommencementViewModel() {};

            return viewModel;
        }

    }
}