using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationPetitionRepositoryTests
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    public partial class RegistrationPetitionRepositoryTests
    {
        #region FullName Tests

        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult1()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(99);
            //record.MI = null;
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("FirstName99 LastName99", record.FullName);
            #endregion Assert
        }
        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult2()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(99);
            record.MI = string.Empty;
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("FirstName99 LastName99", record.FullName);
            #endregion Assert
        }
        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult3()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(99);
            record.MI = "    ";
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("FirstName99 LastName99", record.FullName);
            #endregion Assert
        }
        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult4()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(99);
            record.MI = "xxx";
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("FirstName99 xxx LastName99", record.FullName);
            #endregion Assert
        }
        #endregion FullName Tests

        #region SetDecission Tests

        /// <summary>
        /// Tests the set decision sets expected values.
        /// </summary>
        [TestMethod]
        public void TestSetDecisionSetsExpectedValues1()
        {
            #region Arrange
            var record = new RegistrationPetition();
            record.DateDecision = null;
            record.IsApproved = false;
            record.IsPending = true;
            #endregion Arrange

            #region Act
            record.SetDecision(true);
            #endregion Act

            #region Assert
            Assert.IsNotNull(record.DateDecision);
            var compareDate = (DateTime)record.DateDecision;
            Assert.AreEqual(DateTime.Now.Date, compareDate.Date);
            Assert.IsTrue(record.IsApproved);
            Assert.IsFalse(record.IsPending);
            #endregion Assert
        }

        /// <summary>
        /// Tests the set decision sets expected values.
        /// </summary>
        [TestMethod]
        public void TestSetDecisionSetsExpectedValues2()
        {
            #region Arrange
            var record = new RegistrationPetition();
            record.DateDecision = null;
            record.IsApproved = false;
            record.IsPending = true;
            #endregion Arrange

            #region Act
            record.SetDecision(false);
            #endregion Act

            #region Assert
            Assert.IsNotNull(record.DateDecision);
            var compareDate = (DateTime)record.DateDecision;
            Assert.AreEqual(DateTime.Now.Date, compareDate.Date);
            Assert.IsFalse(record.IsApproved);
            Assert.IsFalse(record.IsPending);
            #endregion Assert
        }
        #endregion SetDecission Tests
    }
}
