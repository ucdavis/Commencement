using System.Collections.Generic;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Mvc.Controllers.ViewModels
{
    public class MajorCodeViewModel
    {
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public IEnumerable<College> Colleges { get; set; }

        public static MajorCodeViewModel Create(IRepositoryWithTypedId<MajorCode, string> majorRepository, IRepositoryWithTypedId<College, string> collegeRepository)
        {
            Check.Require(majorRepository != null, "majorRepository is required.");
            Check.Require(collegeRepository != null, "collegeRepository is required.");

            var viewModel = new MajorCodeViewModel() {MajorCodes = majorRepository.GetAll("Id", true), Colleges = collegeRepository.GetAll()};

            return viewModel;
        }
    }
}