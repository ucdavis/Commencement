using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationPetitionRepositoryTests
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    public partial class RegistrationPetitionRepositoryTests
    {
        #region Constructor Tests

        /// <summary>
        /// Tests the constructor sets expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorSetsExpectedValues()
        {
            #region Arrange
            var record = new RegistrationPetition();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsNull(record.DateDecision);
            #endregion Assert
        }

        #endregion Constructor Tests

        #region Cascade Tests
        /// <summary>
        /// Tests the delete registration petition does not delete major code.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationPetitionDoesNotDeleteMajorCode()
        {
            #region Arrange
            var majorCode = MajorCodeRepository.GetById("1");
            var registrationPetitionCount = RegistrationPetitionRepository.GetAll().Count;
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.AreSame(registrationPetition.MajorCode, majorCode);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationPetitionCount - 1, RegistrationPetitionRepository.GetAll().Count);
            var majorCodeCompare = MajorCodeRepository.GetById("1");
            Assert.AreSame(majorCode, majorCodeCompare);
            #endregion Assert
        }

        /// <summary>
        /// Tests the delete registration petition does not delete term code.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationPetitionDoesNotDeleteTermCode()
        {
            #region Arrange
            var termCodeCount = TermCodeRepository.GetAll().Count;
            var registrationPetitionCount = RegistrationPetitionRepository.GetAll().Count;
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.IsNotNull(registrationPetition.TermCode);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationPetitionCount - 1, RegistrationPetitionRepository.GetAll().Count);
            Assert.AreEqual(termCodeCount, TermCodeRepository.GetAll().Count);
            #endregion Assert
        }

        /// <summary>
        /// Tests the delete registration petition does not delete ceremony.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationPetitionDoesNotDeleteCeremony()
        {
            #region Arrange
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            LoadCeremony(3);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            var ceremonyCount = Repository.OfType<Ceremony>().GetAll().Count;
            var registrationPetition = GetValid(9);
            registrationPetition.Ceremony = Repository.OfType<Ceremony>().GetById(1);
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            var registrationPetitionCount = RegistrationPetitionRepository.GetAll().Count;
            Assert.IsNotNull(registrationPetition.Ceremony);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationPetitionCount - 1, RegistrationPetitionRepository.GetAll().Count);
            Assert.AreEqual(ceremonyCount, Repository.OfType<Ceremony>().GetAll().Count);
            #endregion Assert
        }
        #endregion Cascade Tests
    }
}
