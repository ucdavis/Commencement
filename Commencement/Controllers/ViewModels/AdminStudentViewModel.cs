using System.Collections.Generic;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AdminStudentViewModel
    {
        public IEnumerable<Student> Students { get; set; }

        public static AdminStudentViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AdminStudentViewModel() {};

            return viewModel;
        }

    }
}