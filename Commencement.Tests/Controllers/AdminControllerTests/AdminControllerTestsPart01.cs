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

        /// <summary>
        /// Tests the students mapping.
        /// #2
        /// </summary>
        [TestMethod]
        public void TestStudentsMapping()
        {
            "~/Admin/Students/".ShouldMapTo<AdminController>(a => a.Students("1", null, null, null), true);
        }

        /// <summary>
        /// Tests the student details mapping.
        /// </summary>
        [TestMethod]
        public void TestStudentDetailsMapping()
        {
            "~/Admin/StudentDetails/".ShouldMapTo<AdminController>(a => a.StudentDetails(Guid.Empty, true), true);
        }

        /// <summary>
        /// Tests the add student mapping.
        /// </summary>
        [TestMethod]
        public void TestAddStudentMapping()
        {
            "~/Admin/AddStudent/".ShouldMapTo<AdminController>(a => a.AddStudent(null), true);
        }

        /// <summary>
        /// Tests the add student confirm get mapping.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmGetMapping()
        {
            "~/Admin/AddStudentConfirm/".ShouldMapTo<AdminController>(a => a.AddStudentConfirm(null, null), true);
        }

        /// <summary>
        /// Tests the add student confirm post mapping.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostMapping()
        {
            "~/Admin/AddStudentConfirm/".ShouldMapTo<AdminController>(a => a.AddStudentConfirm(null, null, null), true);
        }

        /// <summary>
        /// Tests the change major get mapping.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorGetMapping()
        {
            "~/Admin/ChangeMajor/5".ShouldMapTo<AdminController>(a => a.ChangeMajor(5));
        }

        /// <summary>
        /// Tests the change major post mapping.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorPostMapping()
        {
            "~/Admin/ChangeMajor/5".ShouldMapTo<AdminController>(a => a.ChangeMajor(5, "123"), true);
        }
        #endregion Mapping Tests
    }
}
