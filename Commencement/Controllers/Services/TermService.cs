using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Services
{     
    public static class TermService
    {
        public static TermCode GetCurrent()
        {
            var repository = SmartServiceLocator<IRepository<TermCode>>.GetService();
            var term = repository.Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();
            Check.Require(term != null, "Unable to find valid term.");

            return term;
        }
    }


}