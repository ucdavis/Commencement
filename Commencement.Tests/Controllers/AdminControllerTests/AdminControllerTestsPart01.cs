using System;
using Commencement.Controllers;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    public partial class AdminControllerTests
    {
        #region Mapping Tests

        /// <summary>
        /// Tests the index mapping.
        /// #1
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Admin/Index".ShouldMapTo<AdminController>(a => a.Index());
        }

        [TestMethod]
        public void TestAdminLandingMapping()
        {
            "~/Admin/AdminLanding".ShouldMapTo<AdminController>(a => a.AdminLanding());
        }

        /// <summary>
        /// Tests the students mapping.
        /// #2
        /// </summary>
        [TestMethod]
        public void TestStudentsMapping()
        {
            "~/Admin/Students/".ShouldMapTo<AdminController>(a => a.Students("1", null, null, null), true);
        }

        [TestMethod]
        public void TestRegistrationsMapping()
        {
            "~/Admin/Registrations/".ShouldMapTo<AdminController>(a => a.Registrations("studentId", "LastName", "FirstName", "MajorCode", null, "CollegeCode"), true);
        }

        /// <summary>
        /// Tests the student details mapping.
        /// </summary>
        [TestMethod]
        public void TestStudentDetailsMapping()
        {
            "~/Admin/StudentDetails/".ShouldMapTo<AdminController>(a => a.StudentDetails(Guid.Empty, true), true);
        }

        [TestMethod]
        public void TestBlockMapping()
        {
            "~/Admin/Block/".ShouldMapTo<AdminController>(a => a.Block(Guid.Empty), true);
        }
        [TestMethod]
        public void TestBlockWithReasonMapping()
        {
            "~/Admin/Block/".ShouldMapTo<AdminController>(a => a.Block(Guid.Empty, true, "because"), true);
        }

        [TestMethod]
        public void TestRegisterForStudentMapping()
        {
            "~/Admin/RegisterForStudent/".ShouldMapTo<AdminController>(a => a.RegisterForStudent(Guid.Empty), true);
        }

        [TestMethod]
        public void TestRegisterForStudent2Mapping()
        {
            "~/Admin/RegisterForStudent/".ShouldMapTo<AdminController>(a => a.RegisterForStudent(Guid.Empty, null), true);
        }

        [TestMethod]
        public void TestEditStudentMapping()
        {
            "~/Admin/EditStudent/".ShouldMapTo<AdminController>(a => a.EditStudent(Guid.Empty), true);
        }

        [TestMethod]
        public void TestEditStudent2Mapping()
        {
            "~/Admin/EditStudent/".ShouldMapTo<AdminController>(a => a.EditStudent(Guid.Empty, null), true);
        }

        /// <summary>
        /// Tests the add student mapping.
        /// </summary>
        [TestMethod]
        public void TestAddStudentMapping()
        {
            "~/Admin/AddStudent/".ShouldMapTo<AdminController>(a => a.AddStudent(null), true);
        }

        [TestMethod]
        public void TestMajorsMapping()
        {
            "~/Admin/Majors/".ShouldMapTo<AdminController>(a => a.Majors());
        }


        #endregion Mapping Tests
    }
}
