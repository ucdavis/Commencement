using System;
using Commencement.Controllers;
using Commencement.Controllers.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    [TestClass]
    public partial class PetitionControllerTests : ControllerTestBase<PetitionController>
    {
        protected readonly Type ControllerClass = typeof(PetitionController);
        protected IEmailService EmailService;
        protected ICeremonyService CeremonyService;
        protected IPetitionService PetitionService;
        protected IErrorService ErrorService;

        #region Init
        /*
        private readonly IEmailService _emailService;
        private readonly ICeremonyService _ceremonyService;
        private readonly IPetitionService _petitionService;
        private readonly IErrorService _errorService;


        public PetitionController(IEmailService emailService, ICeremonyService ceremonyService, IPetitionService petitionService, IErrorService errorService)
        {
            _emailService = emailService;
            _ceremonyService = ceremonyService;
            _petitionService = petitionService;
            _errorService = errorService;
        }
         */
        protected override void SetupController()
        {
            EmailService = MockRepository.GenerateStub<IEmailService>();
            CeremonyService = MockRepository.GenerateStub<ICeremonyService>();
            PetitionService = MockRepository.GenerateStub<IPetitionService>();
            ErrorService = MockRepository.GenerateStub<IErrorService>();
            Controller = new TestControllerBuilder()
                .CreateController<PetitionController>(EmailService, CeremonyService, PetitionService, ErrorService);
        }
        /// <summary>
        /// Registers the routes.
        /// </summary>
        protected override void RegisterRoutes()
        {
            new RouteConfigurator().RegisterRoutes();
        }
        
        #endregion Init

       

    }
}
