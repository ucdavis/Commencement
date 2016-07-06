using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Mvc.Controllers.Services
{
    public interface IMajorService
    {
        IEnumerable<MajorCode> GetAESMajors();
        IEnumerable<MajorCode> GetMajors();
        IEnumerable<MajorCode> GetByCollege(List<College> colleges);
        IEnumerable<MajorCode> GetByCeremonies(string userId, List<Ceremony> ceremonies = null);
    }

    public class MajorService : IMajorService
    {
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorRepository;
        private readonly ICeremonyService _ceremonyService;

        public MajorService(IRepositoryWithTypedId<MajorCode, string> majorRepository, ICeremonyService ceremonyService)
        {
            _majorRepository = majorRepository;
            _ceremonyService = ceremonyService;
        }

        public IEnumerable<MajorCode> GetMajors()
        {
            return _majorRepository.GetAll("Name", true);
        }

        /// <summary>
        /// probably not used any more
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MajorCode> GetAESMajors()
        {
            return _majorRepository.Queryable.Where(a => a.Id.StartsWith("A")).ToList();
        }

        /// <summary>
        /// returns majors by college(s)
        /// </summary>
        /// <param name="colleges"></param>
        /// <returns></returns>
        public IEnumerable<MajorCode> GetByCollege(List<College> colleges)
        {
            return _majorRepository.Queryable.Where(a => colleges.Contains(a.College) && a.IsActive).ToList();
        }

        /// <summary>
        /// returns list of majors based on a list of ceremonies
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ceremonies"></param>
        /// <returns></returns>
         public IEnumerable<MajorCode> GetByCeremonies(string userId, List<Ceremony> ceremonies = null)
         {
             if (ceremonies == null) ceremonies = _ceremonyService.GetCeremonies(userId, TermService.GetCurrent());

             var majors = new List<MajorCode>();
             foreach (var a in ceremonies)
             {
                 foreach (var b in a.Majors) majors.Add(b);
             }

             return majors.Distinct();
         }
    }
}
