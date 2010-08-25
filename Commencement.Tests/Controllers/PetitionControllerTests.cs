using System;
using Commencement.Controllers;
using Commencement.Controllers.Helpers;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class PetitionControllerTests : ControllerTestBase<PetitionController>
    {
        private readonly Type _controllerClass = typeof(PetitionController);
        private IStudentService _studentService;
        private IEmailService _emailService;
        private IRepositoryWithTypedId<MajorCode, string> _majorService;

        #region Init

        protected override void SetupController()
        {
            _studentService = MockRepository.GenerateStub<IStudentService>();
            _emailService = MockRepository.GenerateStub<IEmailService>();
            _majorService = MockRepository.GenerateStub<IRepositoryWithTypedId<MajorCode, string>>();
            Controller = new TestControllerBuilder().CreateController<PetitionController>(_studentService,_emailService, _majorService);
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
            "~/Petition/Index".ShouldMapTo<PetitionController>(a => a.Index());
        }

        /// <summary>
        /// Test the DecideExtraTicketPetition mapping.
        /// </summary>
        [TestMethod]
        public void TestDecideExtraTicketPetitionMapping()
        {
            "~/Petition/DecideExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.DecideExtraTicketPetition(5, true), true);
        }

        /// <summary>
        /// Tests the Register mapping.
        /// </summary>
        [TestMethod]
        public void TestRegisterMapping()
        {
            "~/Petition/Register".ShouldMapTo<PetitionController>(a => a.Register());
        }


        /// <summary>
        /// Tests the ExtraTicketPetition get mapping.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionGetMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5));
        }

        /// <summary>
        /// Tests the ExtraTicketPetition put mapping.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionPutMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5));
        }

        #endregion Mapping Tests

    }
}
