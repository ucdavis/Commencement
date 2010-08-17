using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers;

using Commencement.Controllers.Helpers;
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
        private  IMajorService _majorService;

        #region Init


        protected override void SetupController()
        {
            _termRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<TermCode, string>>();
            _vTermRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<vTermCode, string>>();
            _majorService = MockRepository.GenerateStub<IMajorService>();

            Controller = new TestControllerBuilder().CreateController<CeremonyController>(_termRepository,_vTermRepository,_majorService);
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

        

        #endregion Mapping Tests
    }
}

