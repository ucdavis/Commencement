using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.Helpers
{
    public class CeremonyMajorCheck
    {
        public List<MajorCode> Check(Ceremony ceremony, List<MajorCode> majorCodes, IRepository<Ceremony> ceremonyRepository)
        {
            var foundMajors = new List<MajorCode>();
            foreach (var majorCode in majorCodes)
            {
                var code = majorCode;
                if(ceremonyRepository.Queryable.Where(a => a.TermCode == ceremony.TermCode && a.Id != ceremony.Id && a.Majors.Contains(code)).Any())
                {
                    foundMajors.Add(code);
                }
            }
            return foundMajors.Count > 0 ? foundMajors : null;
        }
    }
}