using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using Commencement.Controllers;
//using Commencement.Controllers.Filter;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.Attributes;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;
using Commencement.Tests.Core.Extensions;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTests : ControllerTestBase<StudentController>
    {
        private readonly Type _controllerClass = typeof(StudentController);
        private IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private IRepository<Ceremony> _ceremonyRepository;
        private IRepository<Registration> _registrationRepository;
        private IStudentService _studentService;
        private IEmailService _emailService;
        private IRepository<State> _stateRepository;

        #region Init

        public StudentControllerTests()
        {
            _stateRepository = FakeRepository<State>();
            Controller.Repository.Expect(a => a.OfType<State>()).Return(_stateRepository).Repeat.Any();
        }

        protected override void SetupController()
        {

            _studentService = MockRepository.GenerateStub<IStudentService>();
            _emailService = MockRepository.GenerateStub<IEmailService>();
            _ceremonyRepository = MockRepository.GenerateStub<IRepository<Ceremony>>();
            _registrationRepository = MockRepository.GenerateStub<IRepository<Registration>>();
            _studentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();

            Controller = new TestControllerBuilder().CreateController<StudentController>
                (_studentService,
                _emailService, 
                _studentRepository,
                _ceremonyRepository,
                _registrationRepository);
        }
        /// <summary>
        /// Registers the routes.
        /// </summary>
        protected override void RegisterRoutes()
        {
            new RouteConfigurator().RegisterRoutes();
        }
        #endregion Init

        #region Mapping

        /// <summary>
        /// Tests the index mapping.
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Student/Index".ShouldMapTo<StudentController>(a => a.Index());
        }

        /// <summary>
        /// Tests the choose ceremony mapping.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyMapping()
        {
            "~/Student/ChooseCeremony".ShouldMapTo<StudentController>(a => a.ChooseCeremony());
        }

        /// <summary>
        /// Tests the display registration mapping.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationMapping()
        {
            "~/Student/DisplayRegistration/5".ShouldMapTo<StudentController>(a => a.DisplayRegistration(5));
        }

        /// <summary>
        /// Tests the registration confirmation mapping.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationMapping()
        {
            "~/Student/RegistrationConfirmation/5".ShouldMapTo<StudentController>(a => a.RegistrationConfirmation(5));
        }

        [TestMethod]
        public void TestRegisterGetMapping()
        {
            //"~/Student/Register/?id=5&major=AABB1".ShouldMapTo<StudentController>(a => a.Register(5, "AABB1"));
            "~/Student/Register/5".ShouldMapTo<StudentController>(a => a.Register(5, "AABB1"),true);
        }

        [TestMethod]
        public void TestRegisterPutMapping()
        {
            //"~/Student/Register/?id=5&major=AABB1".ShouldMapTo<StudentController>(a => a.Register(5, "AABB1"));
            "~/Student/Register/5".ShouldMapTo<StudentController>(a => a.Register(5, new Registration(), true), true);
        }

        [TestMethod]
        public void TestEditRegistrationGetMapping()
        {
            "~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5));
        }
        [TestMethod]
        public void TestEditRegistrationPutMapping()
        {
            "~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5, new Registration(), true), true);
        }

        [TestMethod]
        public void TestNoCeremonyMapping()
        {
            "~/Student/NoCeremony".ShouldMapTo<StudentController>(a => a.NoCeremony());
        }

        
        #endregion Mapping

        #region Index Tests

        /// <summary>
        /// Tests the index redirects to choose ceremony when prior registration is null.
        /// </summary>
        [TestMethod]
        public void TestIndexRedirectsToChooseCeremonyWhenPriorRegistrationIsNull()
        {
            #region Arrange
            //ControllerRecordFakes.FakeStudent(3, _studentRepository)
            var student = CreateValidEntities.Student(1);
            _studentService.Expect(a => a.GetPriorRegistration(Arg<Student>.Is.Anything)).Return(null).Repeat.Any();
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act/Assert
            Controller.Index().AssertActionRedirect().ToAction<StudentController>(a => a.ChooseCeremony());
            #endregion Act/Assert

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        /// <summary>
        /// Tests the index redirects to display registration when prior registration is not null.
        /// </summary>
        [TestMethod]
        public void TestIndexRedirectsToDisplayRegistrationWhenPriorRegistrationIsNotNull()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            var student = CreateValidEntities.Student(1);
            _studentService.Expect(a => a.GetPriorRegistration(Arg<Student>.Is.Anything)).Return(registration).Repeat.Any();
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act/Assert
            Controller.Index()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration(registration.Id));
            #endregion Act/Assert
        }

        #endregion Index Tests

        #region GetCurrentStudent Tests


        /// <summary>
        /// Tests the get student should redirect to petition work flow when student not found.
        /// </summary>
        [TestMethod]
        public void TestGetStudentShouldRedirectToPetitionWorkFlowWhenStudentNotFound()
        {
            #region Arrange
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();            
            #endregion Arrange

            #region Act
            //TODO: Once this works, check it is redirecting correctly
            Controller.Index();
            Controller.Index()
                .AssertActionRedirect()
                .ToAction<PetitionController>(a => a.Index());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert	
        }

        #endregion GetCurrentStudent Tests

        #region ChooseCeremony Tests

        /// <summary>
        /// Tests the choose ceremony with no ceremony with major redirects to error controller.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyWithNoCeremonyWithMajorRedirectsToErrorController()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonyWithMajors = new List<CeremonyWithMajor>();
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _studentService.Expect(a => a.GetMajorsAndCeremoniesForStudent(student)).Return(ceremonyWithMajors).Repeat.Any();
            #endregion Arrange

            #region Act/Assert
            Controller.ChooseCeremony()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.NoCeremony));
            #endregion Act/Assert

        }

        /// <summary>
        /// Tests the choose ceremony with only one ceremony with major redirects to register.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyWithOnlyOneCeremonyWithMajorRedirectsToRegister()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonyWithMajors = new List<CeremonyWithMajor>();
            var ceremonyWithMajor = CreateValidEntities.CeremonyWithMajor(1);
            ceremonyWithMajors.Add(ceremonyWithMajor);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _studentService.Expect(a => a.GetMajorsAndCeremoniesForStudent(student)).Return(ceremonyWithMajors).Repeat.Any();            
            #endregion Arrange

            #region Act
            Controller.ChooseCeremony()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Register(1, string.Empty));
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        /// <summary>
        /// Tests the choose ceremony with more than one ceremony with major returns view.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyWithMoreThanOneCeremonyWithMajorReturnsView()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonyWithMajors = new List<CeremonyWithMajor>();
            ceremonyWithMajors.Add(CreateValidEntities.CeremonyWithMajor(1));
            ceremonyWithMajors.Add(CreateValidEntities.CeremonyWithMajor(2));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _studentService.Expect(a => a.GetMajorsAndCeremoniesForStudent(student)).Return(ceremonyWithMajors).Repeat.Any();  
            #endregion Arrange

            #region Act
            var result = Controller.ChooseCeremony()
                .AssertViewRendered()
                .WithViewData<List<CeremonyWithMajor>>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            #endregion Assert		
        }

        #endregion ChooseCeremony Tests

        #region DisplayRegistration Tests

        /// <summary>
        /// Tests the display registration redirects to index when registration is null.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationRedirectsToIndexWhenRegistrationIsNull()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNull(_registrationRepository.GetNullableById(4));
            #endregion Arrange

            #region Act/Assert
            Controller.DisplayRegistration(4)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert
        }


        /// <summary>
        /// Tests the display registration redirects to index if the registration student does not match the current student.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationRedirectsToIndexIfTheRegistrationStudentDoesNotMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student2).Repeat.Any();
            #endregion Arrange

            #region Act/Assert
            Controller.DisplayRegistration(1)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert		
        }

        /// <summary>
        /// Tests the display registration returns view if the registration student does match the current student.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationReturnsViewIfTheRegistrationStudentDoesMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.DisplayRegistration(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            #endregion Assert
        }

        /// <summary>
        /// Tests the display registration returns view with edit setting.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationReturnsViewWithEditSetting()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.DisplayRegistration(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            Assert.IsTrue((bool)Controller.ViewData["CanEditRegistration"]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the display registration returns view without edit setting.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationReturnsViewWithoutEditSetting()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.DisplayRegistration(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            Assert.IsFalse((bool)Controller.ViewData["CanEditRegistration"]);
            #endregion Assert
        }
        #endregion DisplayRegistration Tests

        #region RegistrationConfirmation Tests

        /// <summary>
        /// Tests the registration confirmation redirects to index when registration is null.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationRedirectsToIndexWhenRegistrationIsNull()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNull(_registrationRepository.GetNullableById(4));
            #endregion Arrange

            #region Act/Assert
            Controller.RegistrationConfirmation(4)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert
        }

        /// <summary>
        /// Tests the registration confirmation redirects to index if the registration student does not match the current student.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationRedirectsToIndexIfTheRegistrationStudentDoesNotMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student2).Repeat.Any();
            #endregion Arrange

            #region Act/Assert
            Controller.RegistrationConfirmation(1)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert
        }

        /// <summary>
        /// Tests the registration confirmation returns view if the registration student does match the current student.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationReturnsViewIfTheRegistrationStudentDoesMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.RegistrationConfirmation(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            #endregion Assert
        }       

        #endregion RegistrationConfirmation Tests

        #region Register Tests
        #region Register Get Tests

        /// <summary>
        /// Tests the register get redirects to index if ceremony not found.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetRedirectsToIndexIfCeremonyNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeCeremony(1, _ceremonyRepository);
            Assert.IsNull(_ceremonyRepository.GetNullableById(2));
            #endregion Arrange

            #region Act
            Controller.Register(2, string.Empty)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());                
            #endregion Act

            #region Assert
            Assert.AreEqual("No matching ceremony found.  Please try your registration again.", Controller.Message);
            #endregion Assert		
        }


        /// <summary>
        /// Tests the register get redirects to index if ceremony deadline passed.
        /// </summary>
        [TestMethod, Ignore] //This has been changed to redirect to the ErrorPage.
        public void TestRegisterGetRedirectsToIndexIfCeremonyDeadlinePassed()
        {
            #region Arrange
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            #endregion Arrange

            #region Act
            Controller.Register(1, string.Empty)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());  
            #endregion Act

            #region Assert
            Assert.AreEqual("The deadline to register for the ceremony has passed.", Controller.Message);
            #endregion Assert		
        }

        [TestMethod]
        public void TestRegisterGetRedirectsToErrorIfCeremonyDeadlinePassed()
        {
            #region Arrange
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            #endregion Arrange

            #region Act
            Controller.Register(1, string.Empty)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            #endregion Act

            #region Assert
            //Assert.AreEqual("The deadline to register for the ceremony has passed.", Controller.Message);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get redirects to index if student major is empty and major not passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetRedirectsToIndexIfStudentMajorIsEmptyAndMajorNotPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            Controller.Register(1, string.Empty)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Student has multiple majors but did not supply a major code.", Controller.Message);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the register get redirects to index if student major has multiple values and major not passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetRedirectsToIndexIfStudentMajorHasMultipleValuesAndMajorNotPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(CreateValidEntities.MajorCode(1));
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            Controller.Register(1, string.Empty)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Student has multiple majors but did not supply a major code.", Controller.Message);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get returns view if student has only one major and major not passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetReturnsViewIfStudentHasOnlyOneMajorAndMajorNotPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();            
            student.Majors.Add(CreateValidEntities.MajorCode(9));
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, string.Empty)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNull(Controller.Message);
            Assert.IsNotNull(result);
            Assert.AreEqual("Name9", result.Registration.Major.Name);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreSame(ceremonies[0], result.Ceremony);
            Assert.AreSame(student, result.Student);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get returns view if student has multiple majors and major is passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetReturnsViewIfStudentHasMultipleMajorsAndMajorIsPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(CreateValidEntities.MajorCode(1));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.Majors.Add(CreateValidEntities.MajorCode(9));
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, "3")
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNull(Controller.Message);
            Assert.IsNotNull(result);
            Assert.AreEqual("Name3", result.Registration.Major.Name);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreSame(ceremonies[0], result.Ceremony);
            Assert.AreSame(student, result.Student);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get throws exception if student has multiple majors and wrong major is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegisterGetThrowsExceptionIfStudentHasMultipleMajorsAndWrongMajorIsPassed()
        {
            try
            {
                #region Arrange
                var student = CreateValidEntities.Student(1);
                student.Majors = new List<MajorCode>();
                student.Majors.Add(CreateValidEntities.MajorCode(1));
                student.Majors.Add(CreateValidEntities.MajorCode(3));
                student.Majors.Add(CreateValidEntities.MajorCode(9));
                var ceremonies = new List<Ceremony>();
                ceremonies.Add(CreateValidEntities.Ceremony(1));
                ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
                ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
                Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
                _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
                ControllerRecordFakes.FakeState(3, Controller.Repository);
                #endregion Arrange

                #region Act
                Controller.Register(1, "2");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                throw;
                #endregion Assert
            }
        }

        /// <summary>
        /// Tests the register get throws exception if student has multiple majors with duplicates and major is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegisterGetThrowsExceptionIfStudentHasMultipleMajorsWithDuplicatesAndMajorIsPassed()
        {
            try
            {
                #region Arrange
                var student = CreateValidEntities.Student(1);
                student.Majors = new List<MajorCode>();
                student.Majors.Add(CreateValidEntities.MajorCode(1));
                student.Majors.Add(CreateValidEntities.MajorCode(3));
                student.Majors.Add(CreateValidEntities.MajorCode(3));
                student.Majors.Add(CreateValidEntities.MajorCode(9));
                var ceremonies = new List<Ceremony>();
                ceremonies.Add(CreateValidEntities.Ceremony(1));
                ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
                ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
                Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
                _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
                ControllerRecordFakes.FakeState(3, Controller.Repository);
                #endregion Arrange

                #region Act
                Controller.Register(1, "3");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains more than one element", ex.Message);
                throw;
                #endregion Assert
            }
        }
        #endregion Register Get Tests
        #region Register Post Tests

        /// <summary>
        /// Tests the register post returns view if agree to disclaimer is false.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostReturnsViewIfAgreeToDisclaimerIsFalse()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);            
            var registration = CreateValidEntities.Registration(1);
            registration.Student = null;
            registration.Ceremony = null;

            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, registration, false)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreSame(ceremonies[0], result.Registration.Ceremony);
            Assert.AreSame(student, result.Registration.Student);
            Controller.ModelState.AssertErrorsAre("You must agree to the disclaimer");
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            #endregion Assert		
        }


        /// <summary>
        /// Tests the register post redirects to index if ceremony deadline passed.
        /// </summary>
        [TestMethod, Ignore] //This has been changed to redirect to the error page
        public void TestRegisterPostRedirectsToIndexIfCeremonyDeadlinePassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            var registration = CreateValidEntities.Registration(1);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.Register(1, registration, true)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("The deadline to register for the ceremony has passed.", Controller.Message);
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            #endregion Assert
        }
        /// <summary>
        /// Tests the register post redirects to error if ceremony deadline passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostRedirectsToErrorIfCeremonyDeadlinePassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            var registration = CreateValidEntities.Registration(1);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.Register(1, registration, true)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            #endregion Act

            #region Assert
            //Assert.AreEqual("The deadline to register for the ceremony has passed.", Controller.Message);
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            #endregion Assert
        }

        /// <summary>
        /// Tests the register post returns view if ceremony not found.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostReturnsViewIfCeremonyNotFound()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            var registration = CreateValidEntities.Registration(1);
            registration.Student = null;
            registration.Ceremony = null;

            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(2, registration, true)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreNotSame(ceremonies[0], result.Registration.Ceremony);
            Assert.AreSame(student, result.Registration.Student);
            Controller.ModelState.AssertErrorsAre("Ceremony: may not be empty");
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            #endregion Assert
        }


        /// <summary>
        /// Tests the register post with valid data saves.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostWithValidDataSaves()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            var registration = CreateValidEntities.Registration(1);
            registration.Student = null;
            registration.Ceremony = null;

            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.Register(1, registration, true)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            _registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            _emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the register post with valid data saves but email fails.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostWithValidDataSavesButEmailFails()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            var registration = CreateValidEntities.Registration(1);
            registration.Student = null;
            registration.Ceremony = null;

            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any().Throw(new Exception("Faked Exception"));
            #endregion Arrange

            #region Act
            Controller.Register(1, registration, true)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            _registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            _emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            Assert.AreEqual("You have successfully registered for commencement. There was a problem sending you an email.  Please print this page for your records.", Controller.Message);
            #endregion Assert
        }

        #endregion Register Post Tests
        #endregion Register Tests

        #region EditRegistration Tests
        #region EditRegistration Get Tests

        /// <summary>
        /// Tests the edit registration get redirects to index if registration is null.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationGetRedirectsToIndexIfRegistrationIsNull()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            ControllerRecordFakes.FakeRegistration(1, _registrationRepository);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.EditRegistration(2)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            #endregion Assert		
        }
        #endregion EditRegistration Get Tests
        #region EditRegistration Post Tests               
        //TODO:
        #endregion EditRegistration Post Tests
        #endregion EditRegistration Tests

        #region Reflection
        #region Controller Class Tests
        /// <summary>
        /// Tests the controller inherits from super controller.
        /// </summary>
        [TestMethod]
        public void TestControllerInheritsFromApplicationControllerThenSuperController()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.BaseType.BaseType.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("SuperController", result);
            #endregion Assert
        }
        /// <summary>
        /// Tests the controller inherits from super controller.
        /// </summary>
        [TestMethod]
        public void TestControllerInheritsFromApplicationController()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.BaseType.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("ApplicationController", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has only three attributes.
        /// </summary>
        [TestMethod]
        public void TestControllerHasOnlyThreeAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(3, result.Count());
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has transaction attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasTransactionAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<UseTransactionsByDefaultAttribute>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "UseTransactionsByDefaultAttribute not found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has anti forgery token attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasAntiForgeryTokenAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<UseAntiForgeryTokenOnPostByDefault>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "UseAntiForgeryTokenOnPostByDefault not found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has students only attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasStudentsOnlyAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<StudentsOnly>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "StudentsOnly not found.");
            #endregion Assert
        }

        #endregion Controller Class Tests

        #region Controller Method Tests

        /// <summary>
        /// Tests the controller contains expected number of public methods.
        /// </summary>
        [TestMethod]
        public void TestControllerContainsExpectedNumberOfPublicMethods()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetMethods().Where(a => a.DeclaringType == controllerClass);
            #endregion Act

            #region Assert
            Assert.AreEqual(4, result.Count(), "Still need to test methods.");
            //Assert.AreEqual(9, result.Count(), "It looks like a method was added or removed from the controller.");
            #endregion Assert
        }


        /// <summary>
        /// Tests the controller method index contains expected attributes.
        /// #1
        /// </summary>
        [TestMethod]
        public void TestControllerMethodIndexContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("Index");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method choose ceremony contains expected attributes.
        /// #2
        /// </summary>
        [TestMethod]
        public void TestControllerMethodChooseCeremonyContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("ChooseCeremony");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method display registration contains expected attributes.
        /// #3
        /// </summary>
        [TestMethod]
        public void TestControllerMethodDisplayRegistrationContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("DisplayRegistration");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method registration confirmation contains expected attributes.
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodRegistrationConfirmationContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("RegistrationConfirmation");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method register get contains expected attributes.
        /// #5
        /// </summary>
        [TestMethod]
        public void TestControllerMethodRegisterGetContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.ElementAt(0).GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method register post contains expected attributes.
        /// #6
        /// </summary>
        [TestMethod]
        public void TestControllerMethodRegisterPostContainsExpectedAttributes1()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
            var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method register post contains expected attributes.
        /// #6
        /// </summary>
        [TestMethod]
        public void TestControllerMethodRegisterPostContainsExpectedAttributes2()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<ValidateAntiForgeryTokenAttribute>();
            var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "ValidateAntiForgeryTokenAttribute not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }
        #endregion Controller Method Tests

        #endregion Reflection
    }
}
