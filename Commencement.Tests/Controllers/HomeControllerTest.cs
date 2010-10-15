using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers;
//using Commencement.Controllers.Filter;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : ControllerTestBase<HomeController>
    {
        private readonly Type _controllerClass = typeof(HomeController);
        private IRepositoryWithTypedId<Student, Guid> _studentRepository;

        #region Init


        protected override void SetupController()
        {
            _studentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();
            Controller = new TestControllerBuilder().CreateController<HomeController>(_studentRepository);
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
        /// Tests the index mapping.
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Home/Index".ShouldMapTo<HomeController>(a => a.Index());
        }

        /// <summary>
        /// Tests the about mapping.
        /// </summary>
        [TestMethod]
        public void TestAboutMapping()
        {
            "~/Home/About".ShouldMapTo<HomeController>(a => a.About());
        }

        #endregion Mapping Tests

        [TestMethod]
        public void Index()
        {
            //// Arrange
            //HomeController controller = new HomeController();

            //// Act
            //ViewResult result = controller.Index() as ViewResult;

            //// Assert
            //ViewDataDictionary viewData = result.ViewData;
            //Assert.AreEqual("Welcome to ASP.NET MVC!", viewData["Message"]);
        }

        [TestMethod]
        public void About()
        {
            //// Arrange
            //HomeController controller = new HomeController(null);

            //// Act
            //ViewResult result = controller.About() as ViewResult;

            //// Assert
            //Assert.IsNotNull(result);
        }

        #region Reflection Tests

        

        #endregion Reflection Tests
    }
}
