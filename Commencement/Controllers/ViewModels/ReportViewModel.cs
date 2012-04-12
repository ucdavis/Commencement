using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.ViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<TermCode> TermCodes { get; set; }
        public TermCode TermCode { get; set; }
        public IEnumerable<MajorCode> MajorCodes { get; set; }
        public IEnumerable<Ceremony> Ceremonies { get; set; }

        public static ReportViewModel Create(IRepository repository, ICeremonyService ceremonyService, string userId)
        {
            Check.Require(repository != null, "Repository is required.");

            var ceremonies = ceremonyService.GetCeremonies(userId);
            var terms = ceremonies.Select(a => a.TermCode).OrderByDescending(a => a.Id).Distinct();

            var existingTerms = repository.OfType<TermCode>().Queryable.Select(a=>a.Id).ToList();

            var viewModel = new ReportViewModel()
                                {
                                    TermCodes = terms,
                                    TermCode = TermService.GetCurrent(),
                                };

            return viewModel;
        }
    }
}