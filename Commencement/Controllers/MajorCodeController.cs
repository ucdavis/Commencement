using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using MvcContrib;

namespace Commencement.Controllers
{
    [AdminOnly]
    public class MajorCodeController : SuperController
    {
        private readonly IRepository<MajorCode> _majorRepository;

        public MajorCodeController(IRepository<MajorCode> majorRepository)
        {
            _majorRepository = majorRepository;
        }

        //
        // GET: /MajorCode/

        public ActionResult Index()
        {
            var majors = _majorRepository.GetAll("Id", true);

            return View(majors);
        }

    }
}
