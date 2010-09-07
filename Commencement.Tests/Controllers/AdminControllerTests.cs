using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Castle.MicroKernel.Registration;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.Attributes;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;
using UCDArch.Web.Attributes;
using Castle.Windsor;
using UCDArch.Web.IoC;

//using Microsoft.Practices.ServiceLocation;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests : ControllerTestBase<AdminController>
    {
        private readonly Type _controllerClass = typeof(AdminController);
        private IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private IRepositoryWithTypedId<MajorCode, string> _majorRepository;
        private IStudentService _studentService;
        private IEmailService _emailService;
        private IMajorService _majorService;

        protected IRepository<Student> StudentRepository2;
        protected IRepository<Registration> RegistrationRepository;

        public IRepository<TermCode> TermCodeRepository;
        #region Init

        public AdminControllerTests()
        {
            StudentRepository2 = FakeRepository<Student>();
            Controller.Repository.Expect(a => a.OfType<Student>()).Return(StudentRepository2).Repeat.Any();

            RegistrationRepository = FakeRepository<Registration>();
            Controller.Repository.Expect(a => a.OfType<Registration>()).Return(RegistrationRepository).Repeat.Any();
        }

        protected override void SetupController()
        {
            _studentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();
            _majorRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<MajorCode, string>>();
            _studentService = MockRepository.GenerateStub<IStudentService>();
            _emailService = MockRepository.GenerateStub<IEmailService>();
            _majorService = MockRepository.GenerateStub<IMajorService>();

            Controller = new TestControllerBuilder().CreateController<AdminController>(_studentRepository, _majorRepository, _studentService, _emailService, _majorService);
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

        #region Mapping Tests

        /// <summary>
        /// Tests the index mapping.
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Admin/Index".ShouldMapTo<AdminController>(a => a.Index());
        }

        /// <summary>
        /// Tests the students mapping.
        /// </summary>
        [TestMethod]
        public void TestStudentsMapping()
        {
            "~/Admin/Students/".ShouldMapTo<AdminController>(a => a.Students("1", null,null,null), true);
        }
        

        #endregion Mapping Tests

        #region Index Tests

        /// <summary>
        /// Tests the index returns view.
        /// </summary>
        [TestMethod]
        public void TestIndexReturnsView()
        {
            #region Arrange

            #endregion Arrange

            #region Act
            Controller.Index()
                .AssertViewRendered();
            #endregion Act

            #region Assert

            #endregion Assert		
        }
        #endregion Index Tests

        #region Students Tests

        /// <summary>
        /// Tests the students returns view.
        /// </summary>
        [TestMethod]
        public void TestStudentsReturnsView()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[1].TermCode = termCodes[0];
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);

            #endregion Arrange

            #region Act
            var result = Controller.Students("1", null, null, null)
                .AssertViewRendered();
            #endregion Act

            #region Assert

            #endregion Assert		
        }

        #endregion Students Tests

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
            var controllerClass = _controllerClass;
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
        /// Tests the controller has Anyone With Role Attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasAnyoneWithRoleAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
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
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetMethods().Where(a => a.DeclaringType == controllerClass);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, result.Count(), "It looks like a method was added or removed from the controller.");
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
            //var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
           // Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        ///// <summary>
        ///// Tests the controller method choose ceremony contains expected attributes.
        ///// #2
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodChooseCeremonyContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("ChooseCeremony");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method display registration contains expected attributes.
        ///// #3
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodDisplayRegistrationContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("DisplayRegistration");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method registration confirmation contains expected attributes.
        ///// #4
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegistrationConfirmationContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("RegistrationConfirmation");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method register get contains expected attributes.
        ///// #5
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegisterGetContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(0).GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method register post contains expected attributes.
        ///// #6
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegisterPostContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
        //    var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method edit registration get contains expected attributes.
        ///// #7
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodEditRegistrationGetContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "EditRegistration");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(0).GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method edit registration post contains expected attributes.
        ///// #8
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodEditRegistrationPostContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "EditRegistration");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
        //    var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method no ceremony contains expected attributes.
        ///// #9 Note: this one is not being used.
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodNoCeremonyContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("NoCeremony");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        #endregion Controller Method Tests

        #endregion Reflection
    }
}
