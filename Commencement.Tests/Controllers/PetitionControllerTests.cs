using System;
using Commencement.Controllers;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
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
        private ICeremonyService _ceremonyService;
        private IPetitionService _petitionService;

        #region Init
        /*
        private readonly IStudentService _studentService;
        private readonly IEmailService _emailService;
        private readonly IRepositoryWithTypedId<MajorCode, string> _majorService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IPetitionService _petitionService;


        public PetitionController(IStudentService studentService, IEmailService emailService, IRepositoryWithTypedId<MajorCode, string> majorService, ICeremonyService ceremonyService, IPetitionService petitionService)
        {
            _studentService = studentService;
            _emailService = emailService;
            _majorService = majorService;
            _ceremonyService = ceremonyService;
            _petitionService = petitionService;
        
        }
         */
        protected override void SetupController()
        {
            _studentService = MockRepository.GenerateStub<IStudentService>();
            _emailService = MockRepository.GenerateStub<IEmailService>();
            _majorService = MockRepository.GenerateStub<IRepositoryWithTypedId<MajorCode, string>>();
            _ceremonyService = MockRepository.GenerateStub<ICeremonyService>();
            _petitionService = MockRepository.GenerateStub<IPetitionService>();
            Controller = new TestControllerBuilder().CreateController<PetitionController>(_studentService,_emailService, _majorService, _ceremonyService, _petitionService);
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
        [TestMethod, Ignore]
        public void TestIndexMapping()
        {
            "~/Petition/Index".ShouldMapTo<PetitionController>(a => a.Index());
        }

        /// <summary>
        /// Test the DecideExtraTicketPetition mapping.
        /// </summary>
        [TestMethod, Ignore]
        public void TestDecideExtraTicketPetitionMapping()
        {
            "~/Petition/DecideExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.DecideExtraTicketPetition(5, true), true);
        }

        /// <summary>
        /// Tests the Register mapping.
        /// </summary>
        [TestMethod, Ignore]
        public void TestRegisterMapping()
        {
            "~/Petition/Register".ShouldMapTo<PetitionController>(a => a.Register());
        }


        /// <summary>
        /// Tests the ExtraTicketPetition get mapping.
        /// </summary>
        [TestMethod, Ignore]
        public void TestExtraTicketPetitionGetMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5));
        }

        /// <summary>
        /// Tests the ExtraTicketPetition put mapping.
        /// </summary>
        [TestMethod, Ignore]
        public void TestExtraTicketPetitionPutMapping()
        {
            "~/Petition/ExtraTicketPetition/5".ShouldMapTo<PetitionController>(a => a.ExtraTicketPetition(5));
        }

        #endregion Mapping Tests

    }
}
