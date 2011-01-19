using System.Collections.Generic;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AddMajorCodeViewModel
    {
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public MajorCode MajorCode { get; set; }
        public bool NewMajor { get; set; }

        public static AddMajorCodeViewModel Create(IRepositoryWithTypedId<MajorCode, string> majorRepository, MajorCode majorCode)
        {
            Check.Require(majorRepository != null, "Repository is required.");

            var viewModel = new AddMajorCodeViewModel() {MajorCodes = majorRepository.GetAll("Id", true), MajorCode = majorCode, NewMajor = false};

            return viewModel;
        }
    }
}