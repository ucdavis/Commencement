using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.Attributes;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;
using System.Web.Mvc;

//using Microsoft.Practices.ServiceLocation;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    [TestClass]
    public partial class AdminControllerTests : ControllerTestBase<AdminController>
    {
        protected readonly Type ControllerClass = typeof(AdminController);
        protected IRepositoryWithTypedId<Student, Guid> StudentRepository;
        protected IRepositoryWithTypedId<MajorCode, string> MajorRepository;
        protected IStudentService StudentService;
        protected IEmailService EmailService;
        protected IMajorService MajorService;

        protected IRepository<Student> StudentRepository2;
        protected IRepository<Registration> RegistrationRepository;
        protected IRepository<State> StateRepository;
        protected IRepository<Ceremony> CeremonyRepository;

        public IRepository<TermCode> TermCodeRepository;
        #region Init

        public AdminControllerTests()
        {
            StudentRepository2 = FakeRepository<Student>();
            Controller.Repository.Expect(a => a.OfType<Student>()).Return(StudentRepository2).Repeat.Any();

            RegistrationRepository = FakeRepository<Registration>();
            Controller.Repository.Expect(a => a.OfType<Registration>()).Return(RegistrationRepository).Repeat.Any();

            StateRepository = FakeRepository<State>();
            Controller.Repository.Expect(a => a.OfType<State>()).Return(StateRepository).Repeat.Any();

            CeremonyRepository = FakeRepository<Ceremony>();
            Controller.Repository.Expect(a => a.OfType<Ceremony>()).Return(CeremonyRepository).Repeat.Any();
        }

        protected override void SetupController()
        {
            StudentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();
            MajorRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<MajorCode, string>>();
            StudentService = MockRepository.GenerateStub<IStudentService>();
            EmailService = MockRepository.GenerateStub<IEmailService>();
            MajorService = MockRepository.GenerateStub<IMajorService>();

            Controller = new TestControllerBuilder().CreateController<AdminController>(StudentRepository, MajorRepository, StudentService, EmailService, MajorService);
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

        #region Reflection
        #region Controller Class Tests
        /// <summary>
        /// Tests the controller inherits from super controller.
        /// </summary>
        [TestMethod]
        public void TestControllerInheritsFromApplicationControllerThenSuperController()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            #endregion Arrange

            #region Act
            Assert.IsNotNull(controllerClass.BaseType);
            Assert.IsNotNull(controllerClass.BaseType.BaseType);
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
            var controllerClass = ControllerClass;
            #endregion Arrange

            #region Act
            Assert.IsNotNull(controllerClass.BaseType);
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
            var controllerClass = ControllerClass;
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
            var controllerClass = ControllerClass;
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
            var controllerClass = ControllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<UseAntiForgeryTokenOnPostByDefault>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "UseAntiForgeryTokenOnPostByDefault not found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has Anyone With Role Attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasAnyoneWithRoleAttribute()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<AnyoneWithRoleAttribute>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "AnyoneWithRoleAttribute not found.");
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
            var controllerClass = ControllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetMethods().Where(a => a.DeclaringType == controllerClass);
            #endregion Act

            #region Assert
            Assert.AreEqual(7, result.Count(), "It looks like a method was added or removed from the controller.");
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
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("Index");
            #endregion Arrange

            #region Act
            //var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
           // Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method students contains expected attributes.
        /// #2
        /// </summary>
        [TestMethod]
        public void TestControllerMethodStudentsContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("Students");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method student details contains expected attributes.
        /// #3
        /// </summary>
        [TestMethod]
        public void TestControllerMethodStudentDetailsContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("StudentDetails");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method add student contains expected attributes.
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodAddStudentContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("AddStudent");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method add student confirm get contains expected attributes.
        /// #5
        /// </summary>
        [TestMethod]
        public void TestControllerMethodAddStudentConfirmGetContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "AddStudentConfirm");
            #endregion Arrange

            #region Act
            //var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
            var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            //Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method add student confirm post contains expected attributes.
        /// #6
        /// </summary>
        [TestMethod]
        public void TestControllerMethodAddStudentConfirmPostContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "AddStudentConfirm");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<HttpPostAttribute>();
            var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method change major get contains expected attributes.
        /// </summary>
        [TestMethod]
        public void TestControllerMethodChangeMajorGetContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "ChangeMajor");
            int getElement = 0;
            if (controllerMethod.ElementAt(0).GetParameters().Count() != 1)
            {
                getElement = 1;
            }
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.ElementAt(getElement).GetCustomAttributes(true);
            

            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        #endregion Controller Method Tests

        #endregion Reflection

        #region Helpers

        protected void LoadTermCodes(string termCode)
        {
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;            
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);
            termCodes[0].SetIdTo(termCode);
        }

        #endregion Helpers
    }
}
