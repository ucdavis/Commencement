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
            Assert.AreEqual(2, result.Count(), "Still need to test methods.");
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

        #endregion Controller Method Tests

        #endregion Reflection
    }
}
