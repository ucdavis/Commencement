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
        public IEnumerable<vTermCode> TermCodes { get; set; }
        public TermCode TermCode { get; set; }
        public IEnumerable<MajorCode> MajorCodes { get; set; }

        public static ReportViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");

            var existingTerms = repository.OfType<TermCode>().Queryable.Select(a=>a.Id).ToList();

            var viewModel = new ReportViewModel()
                                {
                                    TermCodes = Enumerable.ToList<vTermCode>(repository.OfType<vTermCode>().Queryable.Where(a => existingTerms.Contains(a.Id))),
                                    TermCode = TermService.GetCurrent()
                                };

            return viewModel;
        }
    }
}