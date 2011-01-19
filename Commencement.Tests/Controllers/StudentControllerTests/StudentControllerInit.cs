using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Castle.Windsor;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;

namespace Commencement.Tests.Controllers.StudentControllerTests
{
    [TestClass]
    public partial class StudentControllerTests : ControllerTestBase<StudentController>
    {
        protected readonly Type ControllerClass = typeof(StudentController);
        protected IRepositoryWithTypedId<Student, Guid> StudentRepository;
        protected IRepository<Ceremony> CeremonyRepository;
        protected IRepository<Registration> RegistrationRepository;
        protected IStudentService StudentService;
        protected IEmailService EmailService;
        protected readonly IRepository<State> StateRepository;
        protected IErrorService ErrorService;
        protected ICeremonyService CeremonyService;
        protected IRepository<RegistrationPetition> RegistrationPetitionRepository;
        protected IRepository<RegistrationParticipation> ParticipationRepository;
        protected IRegistrationPopulator RegistrationPopulator;

        public IRepository<TermCode> TermCodeRepository;
        public IRepository<vTermCode> VTermCodeRepository;
        public IRepository<SpecialNeed> SpecialNeedRepository;

        #region Init

        public StudentControllerTests()
        {
            StateRepository = FakeRepository<State>();
            Controller.Repository.Expect(a => a.OfType<State>()).Return(StateRepository).Repeat.Any();

            VTermCodeRepository = FakeRepository<vTermCode>();
            Controller.Repository.Expect(a => a.OfType<vTermCode>()).Return(VTermCodeRepository).Repeat.Any();

            SpecialNeedRepository = FakeRepository<SpecialNeed>();
            Controller.Repository.Expect(a => a.OfType<SpecialNeed>()).Return(SpecialNeedRepository).Repeat.Any();

            //ParticipationRepository = FakeRepository<RegistrationParticipation>();
            Controller.Repository.Expect(a => a.OfType<RegistrationParticipation>()).Return(ParticipationRepository).Repeat.Any();

            Controller.Repository.Expect(a => a.OfType<Ceremony>()).Return(CeremonyRepository).Repeat.Any();
        }

        protected override void SetupController()
        {

            /*
        public StudentController(IStudentService studentService, 
            IEmailService emailService,
            IRepositoryWithTypedId<Student, Guid> studentRepository, 
            IRepository<Ceremony> ceremonyRepository, 
            IRepository<Registration> registrationRepository,
            IErrorService errorService,
            ICeremonyService ceremonyService, 
             IRepository<RegistrationPetition> registrationPetitionRepository,
            IRepository<RegistrationParticipation> participationRepository, 
             IRegistrationPopulator registrationPopulator)
        {
            StudentRepository = studentRepository;
            CeremonyRepository = ceremonyRepository;
            RegistrationRepository = registrationRepository;
            ErrorService = errorService;
            CeremonyService = ceremonyService;
            RegistrationPetitionRepository = registrationPetitionRepository;
            ParticipationRepository = participationRepository;
            RegistrationPopulator = registrationPopulator;
            StudentService = studentService;
            EmailService = emailService;
        }
             */
            StudentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();
            CeremonyRepository = MockRepository.GenerateStub<IRepository<Ceremony>>();
            RegistrationRepository = MockRepository.GenerateStub<IRepository<Registration>>();
            ErrorService = MockRepository.GenerateStub<IErrorService>();
            CeremonyService = MockRepository.GenerateStub<ICeremonyService>();
            RegistrationPetitionRepository = FakeRepository<RegistrationPetition>();
            ParticipationRepository = FakeRepository<RegistrationParticipation>();
            RegistrationPopulator = MockRepository.GenerateStub<IRegistrationPopulator>();
            StudentService = MockRepository.GenerateStub<IStudentService>();
            EmailService = MockRepository.GenerateStub<IEmailService>();
            

            Controller = new TestControllerBuilder().CreateController<StudentController>
                (StudentService,
                EmailService,
                StudentRepository,
                CeremonyRepository,
                RegistrationRepository,
                ErrorService,
                CeremonyService,
                RegistrationPetitionRepository,
                ParticipationRepository,
                RegistrationPopulator
                );
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
