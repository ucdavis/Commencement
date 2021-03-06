﻿using Commencement.Controllers;
using Commencement.Controllers.Helpers;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;


namespace Commencement.Tests.Controllers.StudentControllerTests
{
    public partial class StudentControllerTests 
    {
        #region Mapping

        /// <summary>
        /// Tests the index mapping.
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Student/Index".ShouldMapTo<StudentController>(a => a.Index());
        }

        [TestMethod]
        public void TestRegistrationRoutingMapping()
        {
            "~/Student/RegistrationRouting".ShouldMapTo<StudentController>(a => a.RegistrationRouting());
        }

        [TestMethod]
        public void TestRegisterGetMapping()
        {
            "~/Student/Register".ShouldMapTo<StudentController>(a => a.Register());
        }

        [TestMethod]
        public void TestRegisterPostMapping()
        {
            "~/Student/Register".ShouldMapTo<StudentController>(a => a.Register(new RegistrationPostModel()), true);
        }

        [TestMethod]
        public void TestDisplayRegistrationMapping()
        {
            "~/Student/DisplayRegistration".ShouldMapTo<StudentController>(a => a.DisplayRegistration());
        }

        [TestMethod]
        public void TestEditRegistrationGetMapping()
        {
            "~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5));
        }

        [TestMethod]
        public void TestEditRegistrationPostMapping()
        {
            "~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5, new RegistrationPostModel()), true);
        }


        #endregion Mapping 
    }
}
