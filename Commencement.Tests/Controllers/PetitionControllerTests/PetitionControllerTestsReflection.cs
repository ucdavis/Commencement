using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Web.Attributes;


namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests
    {
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
            Assert.IsNotNull(controllerClass);
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
            Assert.IsNotNull(controllerClass);
            Assert.IsNotNull(controllerClass.BaseType);
            var result = controllerClass.BaseType.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("ApplicationController", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has two attributes.
        /// </summary>
        [TestMethod]
        public void TestControllerHasTwottributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(2, result.Count());
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
            Assert.Inconclusive("Still Working on these tests");
            Assert.AreEqual(4, result.Count(), "It looks like a method was added or removed from the controller.");
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
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<AnyoneWithRoleAttribute>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "AnyoneWithRoleAttribute not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// #2
        /// </summary>
        [TestMethod]
        public void TestControllerMethodExtraTicketPetitionsContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("ExtraTicketPetitions");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<AnyoneWithRoleAttribute>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "AnyoneWithRoleAttribute not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// #3
        /// </summary>
        [TestMethod]
        public void TestControllerMethodDecideExtraTicketPetitionContainsExpectedAttributes1()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("DecideExtraTicketPetition");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<AnyoneWithRoleAttribute>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "AnyoneWithRoleAttribute not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// #3
        /// </summary>
        [TestMethod]
        public void TestControllerMethodDecideExtraTicketPetitionContainsExpectedAttributes2()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("DecideExtraTicketPetition");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<HttpPostAttribute>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "HttpPostAttribute not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodUpdateTicketAmountContainsExpectedAttributes1()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("UpdateTicketAmount");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<AnyoneWithRoleAttribute>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "AnyoneWithRoleAttribute not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodUpdateTicketAmountContainsExpectedAttributes2()
        {
            #region Arrange
            var controllerClass = ControllerClass;
            var controllerMethod = controllerClass.GetMethod("UpdateTicketAmount");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<HttpPostAttribute>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "HttpPostAttribute not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        ///// <summary>
        ///// Tests the controller method index contains expected attributes.
        ///// #1
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodIndexContainsExpectedAttributes2()
        //{
        //    #region Arrange
        //    var controllerClass = ControllerClass;
        //    var controllerMethod = controllerClass.GetMethod("Index");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<IgnoreStudentsOnly>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "IgnoreStudentsOnly not found");
        //    Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// #2
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegistrationRoutingContainsExpectedAttributes1()
        //{
        //    #region Arrange
        //    var controllerClass = ControllerClass;
        //    var controllerMethod = controllerClass.GetMethod("RegistrationRouting");
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
        ///// #3
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegisterGetContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = ControllerClass;
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
        ///// #4
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegisterPostContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = ControllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<HttpPostAttribute>();
        //    var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "HttpPostAttribute not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// #5
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodDisplayRegistrationContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = ControllerClass;
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
        ///// #6
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodEditRegistrationGetContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = ControllerClass;
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
        ///// #7
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodEditRegistrationPostContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = ControllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "EditRegistration");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<HttpPostAttribute>();
        //    var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
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
