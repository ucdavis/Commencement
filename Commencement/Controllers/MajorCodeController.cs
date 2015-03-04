using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using MvcContrib;

namespace Commencement.Controllers
{
    [AdminOnly]
    public class MajorCodeController : SuperController
    {
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorRepository;
        private readonly IRepositoryWithTypedId<College, string> _collegeRepository;

        public MajorCodeController(IRepositoryWithTypedId<MajorCode, string> majorRepository, IRepositoryWithTypedId<College, string> collegeRepository)
        {
            _majorRepository = majorRepository;
            _collegeRepository = collegeRepository;
        }

        //
        // GET: /MajorCode/

        public ActionResult Index()
        {
            var viewModel = MajorCodeViewModel.Create(_majorRepository, _collegeRepository);
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Deactivate(string majorCode)
        {
            var major = _majorRepository.GetNullableById(majorCode);
            if (major == null) return Json("Major not found.");

            major.IsActive = false;
            _majorRepository.EnsurePersistent(major);

            return Json(string.Empty);
        }

        [HttpPost]
        public JsonResult SetConsolidation(string majorCode, string consolidationCode)
        {
            var major = _majorRepository.GetNullableById(majorCode);
            var consolidation = _majorRepository.GetNullableById(consolidationCode);

            if (major == null)
            {
                return Json("Major not found.");
            }

            major.ConsolidationMajor = consolidation;
            _majorRepository.EnsurePersistent(major);

            return Json(string.Empty);
        }

        [HttpPost]
        public JsonResult SetCollege(string majorCode, string collegeCode)
        {
            var major = _majorRepository.GetNullableById(majorCode);
            var college = _collegeRepository.GetNullableById(collegeCode);

            if (major == null || college == null)
            {
                return Json("Major or college was not found.");
            }

            major.College = college;
            _majorRepository.EnsurePersistent(major);

            return Json(string.Empty);
        }

        public ActionResult Add()
        {
            var viewModel = AddMajorCodeViewModel.Create(_majorRepository, null);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(string majorId, string majorCode, string majorName)
        {
            var major = _majorRepository.GetNullableById(!string.IsNullOrEmpty(majorId) ? majorId : majorCode);

            if (major == null && !string.IsNullOrEmpty(majorCode) && !string.IsNullOrEmpty(majorName))
            {
                major = new MajorCode(majorCode, majorName);
            }
            
            if (major == null)
            {
                ModelState.AddModelError("Major", "Invalid major code or missing information.");
            }
            if (major != null && !string.IsNullOrWhiteSpace(majorName))
            {
                major.Name = majorName;
            }

            //ModelState.AddModelError("Testing", "Always fail validation.");

            if (ModelState.IsValid)
            {
                major.IsActive = true;
                _majorRepository.EnsurePersistent(major);
                Message = "Major has been added/activated";
                return this.RedirectToAction<MajorCodeController>(a => a.Index());
            }

            var viewModel = AddMajorCodeViewModel.Create(_majorRepository, major);
            viewModel.NewMajor = string.IsNullOrEmpty(majorId);
            return View(viewModel);
        }

    }
}
