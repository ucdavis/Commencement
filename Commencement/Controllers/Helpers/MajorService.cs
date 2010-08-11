using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Controllers.Helpers
{
    public interface IMajorService
    {
        IEnumerable<MajorCode> GetAESMajors();
    }

    public class MajorService
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

        private IEnumerable<MajorCode> GetAESMajors()
        {
            return _majorRepository.Queryable.Where(a => a.Id.StartsWith("A")).ToList();
        }
    }
}
