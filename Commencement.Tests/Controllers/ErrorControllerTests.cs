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
    public class ErrorControllerTests : ControllerTestBase<ErrorController>
    {
         private readonly Type _controllerClass = typeof(ErrorController);


        #region Init

        protected override void SetupController()
        {
            Controller = new TestControllerBuilder().CreateController<ErrorController>();
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
             "~/Error/Index".ShouldMapTo<ErrorController>(a => a.Index(null));
         }

        #endregion Mapping

    }
}
