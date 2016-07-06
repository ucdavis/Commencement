using Commencement.Mvc.Controllers.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Misc
{
    [TestClass]
    public class RolesFiltersTests
    {
        [TestMethod]
        public void TestAdminOnlyReturnsAdminRole()
        {
            #region Arrange
            var attribute = new AdminOnlyAttribute();
            #endregion Arrange

            #region Assert
            Assert.AreEqual("Admin", attribute.Roles);
            #endregion Assert		
        }
        [TestMethod]
        public void TestUserOnlyReturnsAdminRole()
        {
            #region Arrange
            var attribute = new UserOnlyAttribute();
            #endregion Arrange

            #region Assert
            Assert.AreEqual("User", attribute.Roles);
            #endregion Assert
        }
        [TestMethod]
        public void TestAnyoneWithRoleReturnsAdminRole()
        {
            #region Arrange
            var attribute = new AnyoneWithRoleAttribute();
            #endregion Arrange

            #region Assert
            Assert.AreEqual("Admin,User", attribute.Roles);
            #endregion Assert
        }
        [TestMethod]
        public void TestEmulationUserOnlyReturnsAdminRole()
        {
            #region Arrange
            var attribute = new EmulationUserOnlyAttribute();
            #endregion Arrange

            #region Assert
            Assert.AreEqual("EmulationUser", attribute.Roles);
            #endregion Assert
        }
    }
}
