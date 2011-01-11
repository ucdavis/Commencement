using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    public partial class AdminControllerTests
    {

        #region Misc Tests        
        [TestMethod]
        public void TestIndexReturnsView()
        {
            #region Assert
            Controller.Index().AssertViewRendered();
            #endregion Assert		
        }

        [TestMethod]
        public void TestAdminLandingReturnsView()
        {
            #region Assert
            Controller.AdminLanding().AssertViewRendered();
            #endregion Assert
        }
        #endregion Misc Tests

        #region Students Tests (List)


        [TestMethod]
        public void TestStudentsReturnsViewModelWithExpectedData()
        {
            #region Arrange
            LoadTermCodes("201003");


            

           
            

            #endregion Arrange

            #region Act
            var result = Controller.Students("123456789", null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();

            #endregion Act

            #region Assert

            #endregion Assert		
        }

        #endregion Students Tests (List)


    }
}
