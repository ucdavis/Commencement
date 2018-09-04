using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class AddEditorViewModel
    {
        public IEnumerable<vUser> Users { get; set; }
        public Ceremony Ceremony { get; set; }
        public vUser User { get; set; }

        public static AddEditorViewModel Create(IRepository repository, Ceremony ceremony)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(ceremony != null, "ceremony is required.");

            var viewModel = new AddEditorViewModel()
                                {
                                    Ceremony = ceremony,
                                    Users = repository.OfType<vUser>().Queryable.Where(w => w.IsActive).OrderBy(a => a.LastName).ToList()
                                };

            return viewModel;
        }
    }
}