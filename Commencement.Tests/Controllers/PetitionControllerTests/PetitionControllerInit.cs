using System;
using Castle.Windsor;
using Commencement.Controllers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
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

        public IRepository<TermCode> TermCodeRepository;
        public IRepository<Ceremony> CeremonyRepository;

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

        public PetitionControllerTests()
        {
            CeremonyRepository = FakeRepository<Ceremony>();
            Controller.Repository.Expect(a => a.OfType<Ceremony>()).Return(CeremonyRepository).Repeat.Any();
        }

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

        /// <summary>
        /// Need to do this because the call to the static class TermService.
        /// </summary>
        /// <param name="container"></param>
        protected override void RegisterAdditionalServices(IWindsorContainer container)
        {
            TermCodeRepository = MockRepository.GenerateStub<IRepository<TermCode>>();
            container.Kernel.AddComponentInstance<IRepository<TermCode>>(TermCodeRepository);
            base.RegisterAdditionalServices(container);
        }
        
        #endregion Init

       

    }
}
