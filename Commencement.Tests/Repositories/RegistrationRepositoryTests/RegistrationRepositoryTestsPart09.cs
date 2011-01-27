using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {

        #region TicketDistributionMethod Tests

        /// <summary>
        /// Tests the ticket distribution method returns expected result.
        /// </summary>
        [TestMethod]
        public void TestTicketDistributionMethodReturnsExpectedResult1()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            #endregion Arrange

            #region Act
            registration.MailTickets = true;
            #endregion Act

            #region Assert
            Assert.AreEqual("Mail tickets to provided address", registration.TicketDistributionMethod);
            #endregion Assert
        }

        /// <summary>
        /// Tests the ticket distribution method returns expected result.
        /// </summary>
        [TestMethod]
        public void TestTicketDistributionMethodReturnsExpectedResult2()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            #endregion Arrange

            #region Act
            registration.MailTickets = false;
            #endregion Act

            #region Assert
            Assert.AreEqual("Pickup tickets.  Refer to college FAQ", registration.TicketDistributionMethod);
            #endregion Assert
        }

        #endregion TicketDistributionMethod Tests
    }
}
