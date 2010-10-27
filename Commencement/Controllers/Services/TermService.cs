using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Commencement.Controllers.Services
{     
    public static class TermService
    {
        private static readonly string TermKey = "CurrentTerm";

        private static TermCode TermCode
        {
            get
            {
                var repository = SmartServiceLocator<IRepository<TermCode>>.GetService();
                var term = (TermCode)System.Web.HttpContext.Current.Cache[TermKey];

                if (term == null)
                {
                    term = repository.Queryable.Where(a => a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();
                    System.Web.HttpContext.Current.Cache[TermKey] = term;
                }

                return term;
            }
            set { System.Web.HttpContext.Current.Cache[TermKey] = value; }
        }

        public static TermCode GetCurrent()
        {
            return TermCode;
        }

        public static void UpdateCurrent(TermCode termCode)
        {
            TermCode = termCode;
        }

    }


}