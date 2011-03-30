using System;
using System.Linq;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using UCDArch.Testing;
using UCDArch.Web.Attributes;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests : ControllerTestBase<AccountController>
    {
        private readonly Type _controllerClass = typeof(AccountController);

        #region Init

        protected override void SetupController()
        {
            Controller = new TestControllerBuilder().CreateController<AccountController>();
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
        /// Tests the log on mapping.
        /// </summary>
        [TestMethod]
        public void TestLogOnMapping()
        {
            "~/Account/LogOn/5".ShouldMapTo<AccountController>(a => a.LogOn("Test"), true);
        }

        /// <summary>
        /// Tests the log out mapping.
        /// </summary>
        [TestMethod]
        public void TestLogOutMapping()
        {
            "~/Account/LogOut".ShouldMapTo<AccountController>(a => a.LogOut());
        }

        /// <summary>
        /// Tests the not CAES student mapping.
        /// </summary>
        [TestMethod]
// ReSharper disable InconsistentNaming
        public void TestNotCAESStudentMapping()
// ReSharper restore InconsistentNaming
        {
            "~/Account/NotCAESStudent".ShouldMapTo<AccountController>(a => a.NotCAESStudent());
        }

        /// <summary>
        /// Tests the emulate mapping.
        /// </summary>
        [TestMethod]
        public void TestEmulateMapping()
        {
            "~/Account/Emulate/5".ShouldMapTo<AccountController>(a => a.Emulate("test"), true);
        }

        /// <summary>
        /// Tests the end emulate mapping.
        /// </summary>
        [TestMethod]
        public void TestEndEmulateMapping()
        {
            "~/Account/EndEmulate/".ShouldMapTo<AccountController>(a => a.EndEmulate());
        }
        #endregion Mapping Tests

        #region NotCAESStudent Tests

        [TestMethod]
// ReSharper disable InconsistentNaming
        public void TestNotCAESStudentRedirectsToErrorUnauthorizedAccess()
// ReSharper restore InconsistentNaming
        {
            #region Arrange

            #endregion Arrange

            #region Act/Assert
            var result = Controller.NotCAESStudent()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.UnauthorizedAccess());
            #endregion Act/Assert

            #region Assert
            Assert.IsNotNull(result);
            
            #endregion Assert	
        }

        #endregion NotCAESStudent Tests

        //Note, other methods are not tested because it would require too much mocking...

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
        /// Tests the controller has four attributes.
        /// </summary>
        [TestMethod]
        public void TestControllerHasFourAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(4, result.Count());
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
        /// Tests the controller has handle transactions manually attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasHandleTransactionsManuallyAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<HandleTransactionsManuallyAttribute>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "HandleTransactionsManuallyAttribute not found.");
            #endregion Assert
        }

        [TestMethod]
        public void TestControllerHasLoadDisplayDataAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<LoadDisplayDataAttribute>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "LoadDisplayDataAttribute not found.");
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
            Assert.AreEqual(5, result.Count(), "It looks like a method was added or removed from the controller.");
            #endregion Assert
        }


        /// <summary>
        /// Tests the controller method log on contains expected attributes.
        /// #1
        /// </summary>
        [TestMethod]
        public void TestControllerMethodLogOnContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("LogOn");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method log out contains expected attributes.
        /// #2
        /// </summary>
        [TestMethod]
        public void TestControllerMethodLogOutContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("LogOut");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method not CAES student contains expected attributes.
        /// #3
        /// </summary>
        [TestMethod]
// ReSharper disable InconsistentNaming
        public void TestControllerMethodNotCAESStudentContainsExpectedAttributes()
// ReSharper restore InconsistentNaming
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("NotCAESStudent");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method emulate contains expected attributes.
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodEmulateContainsExpectedAttributes1()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("Emulate");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<EmulationUserOnlyAttribute>();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            Assert.AreEqual(1, expectedAttribute.Count(), "EmulationUserOnlyAttribute not found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method emulate contains expected attributes.
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodEmulateContainsExpectedAttributes2()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("Emulate");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method end emulate contains expected attributes.
        /// #5
        /// </summary>
        [TestMethod]
        public void TestControllerMethodEndEmulateContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("EndEmulate");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found.");
            #endregion Assert
        }
        #endregion Controller Method Tests

        #endregion Reflection
    }
}
