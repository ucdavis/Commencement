using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.Services
{
    public interface IMajorService
    {
        IEnumerable<MajorCode> GetAESMajors();
        IEnumerable<MajorCode> GetMajors();
        IEnumerable<MajorCode> GetByCollege(List<College> colleges);
    }

    public class MajorService : IMajorService
    {
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorRepository;

        public MajorService(IRepositoryWithTypedId<MajorCode, string> majorRepository)
        {
            _majorRepository = majorRepository;
        }

        public IEnumerable<MajorCode> GetMajors()
        {
            return GetAESMajors();
        }

        public IEnumerable<MajorCode> GetAESMajors()
        {
            return _majorRepository.Queryable.Where(a => a.Id.StartsWith("A")).ToList();
        }

        public IEnumerable<MajorCode> GetByCollege(List<College> colleges)
        {
            return _majorRepository.Queryable.Where(a => colleges.Contains(a.College)).ToList();
        }
    }
}
