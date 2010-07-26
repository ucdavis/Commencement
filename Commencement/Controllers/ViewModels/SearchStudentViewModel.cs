using System.Collections.Generic;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class SearchStudentViewModel
    {
        public string StudentId { get; set; }
        public IList<SearchStudent> SearchStudents { get; set; }

        public static SearchStudentViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new SearchStudentViewModel();

            return viewModel;
        }
    }
}