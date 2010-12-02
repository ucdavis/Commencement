using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminRegistrationPetitionViewModel
    {
        public static AdminRegistrationViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminRegistrationViewModel();

            return viewModel;
        }
    }
}