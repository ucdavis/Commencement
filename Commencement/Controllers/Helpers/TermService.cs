using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Helpers
{
    public static class TermService
    {
        //private readonly IRepository<TermCode> _termCodeRepository;

        //public TermService(IRepository<TermCode> termCodeRepository)
        //{
        //    _termCodeRepository = termCodeRepository;
        //}

        public static TermCode GetCurrent()
        {
            var repository = SmartServiceLocator<IRepository<TermCode>>.GetService();
            var term = repository.Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();
            Check.Require(term != null, "Unable to find valid term.");

            return term;
        }
    }


}