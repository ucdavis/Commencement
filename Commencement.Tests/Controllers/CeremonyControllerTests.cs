using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers;

using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;
using Commencement.Tests.Core.Extensions;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class CeremonyControllerTests : ControllerTestBase<CeremonyController>
    {
        private  IRepositoryWithTypedId<TermCode, string> _termRepository;
        private  IRepositoryWithTypedId<vTermCode, string> _vTermRepository;
        private  IRepositoryWithTypedId<College, string> _collegeRepository;
        private  IMajorService _majorService;
        private ICeremonyService _ceremonyService;
        private IUserService _userService;
        #region Init
        /*
        private readonly IRepositoryWithTypedId<TermCode, string> _termRepository;
        private readonly IRepositoryWithTypedId<vTermCode, string> _vTermRepository;
        private readonly IRepositoryWithTypedId<College, string> _collegeRepository;
        private readonly IMajorService _majorService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IUserService _userService;

        public CeremonyController(IRepositoryWithTypedId<TermCode, string> termRepository, IRepositoryWithTypedId<vTermCode, string> vTermRepository, IRepositoryWithTypedId<College, string> collegeRepository, IMajorService majorService, ICeremonyService ceremonyService, IUserService userService)
        {
            _termRepository = termRepository;
            _vTermRepository = vTermRepository;
            _collegeRepository = collegeRepository;
            _majorService = majorService;
            _ceremonyService = ceremonyService;
            _userService = userService;
        }
         */

        protected override void SetupController()
        {
            _termRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<TermCode, string>>();
            _vTermRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<vTermCode, string>>();
            _majorService = MockRepository.GenerateStub<IMajorService>();
            _collegeRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<College, string>>();
            _ceremonyService = MockRepository.GenerateStub<ICeremonyService>();
            _userService = MockRepository.GenerateStub<IUserService>();

            Controller = new TestControllerBuilder().CreateController<CeremonyController>(_termRepository,_vTermRepository, _collegeRepository, _majorService, _ceremonyService, _userService);
        }
        /// <summary>
        /// Registers the routes.
        /// </summary>
        protected override void RegisterRoutes()
        {
            new RouteConfigurator().RegisterRoutes();
        }
        #endregion Init

        #region Mapping Tests
        /// <summary>
        /// Tests the index mapping.
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Ceremony/Index".ShouldMapTo<CeremonyController>(a => a.Index());
        }

        /// <summary>
        /// Tests the Edit mapping.
        /// </summary>
        [TestMethod]
        public void TestEditGetMapping()
        {
            "~/Ceremony/Edit/5".ShouldMapTo<CeremonyController>(a => a.Edit(5));
        }

        /// <summary>
        /// Tests the Edit mapping.
        /// </summary>
        [TestMethod]
        public void TestEditPutMapping()
        {
            "~/Ceremony/Edit/5".ShouldMapTo<CeremonyController>(a => a.Edit(new CeremonyEditModel()), true);
        }

        /// <summary>
        /// Tests the Create mapping.
        /// </summary>
        [TestMethod]
        public void TestCreateGetMapping()
        {
            "~/Ceremony/Create".ShouldMapTo<CeremonyController>(a => a.Create());
        }


        /// <summary>
        /// Tests the Create mapping.
        /// </summary>
        [TestMethod]
        public void TestCreatePutMapping()
        {
            "~/Ceremony/Create".ShouldMapTo<CeremonyController>(a => a.Create(new CeremonyEditModel()), true);
        }



        #endregion Mapping Tests
    }
}

