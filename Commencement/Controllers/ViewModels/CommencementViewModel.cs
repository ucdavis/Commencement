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
            Check.Require(!string.IsNullOrEmpty(userId), "User Id is required.");

            var ceremonyIds = (from a in repository.OfType<CeremonyEditor>().Queryable
                               where a.LoginId == userId
                               select a.Ceremony.Id).ToList();

            var viewModel = new CommencementViewModel()
                                {
                                    TermCodes = repository.OfType<TermCode>().GetAll(),
                                    Ceremonies = repository.OfType<Ceremony>().Queryable.Where(a=>ceremonyIds.Contains(a.Id)).ToList()
                                };

            return viewModel;
        }

    }
}