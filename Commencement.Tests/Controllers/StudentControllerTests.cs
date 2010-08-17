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

        #region Init


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

            #region Act/Assert
            Controller.ChooseCeremony()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Register(1, string.Empty));
            #endregion Act/Assert
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
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            #endregion Assert
        }       

        #endregion RegistrationConfirmation Tests

        #region Register Tests
        #region Register Get Tests

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
            //Assert.AreEqual("", Controller.Message);
            #endregion Assert		
        }

        #endregion Register Get Tests
        #region Register Put Tests


        #endregion Register PutTests
        #endregion Register Tests

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

        #endregion Controller Method Tests

        #endregion Reflection
    }
}
