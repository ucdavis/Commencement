using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationPetitionRepositoryTests
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    public partial class RegistrationPetitionRepositoryTests
    {
        #region TermCode Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the TermCode with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTermCodeWithAValueOfNullNDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                //registrationPetition.TermCode = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                //Assert.AreEqual(registrationPetition.TermCode, null);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TermCode: may not be null");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the update to use A different TermCode saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentTermCodeSaves()
        {
            #region Arrange
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.AreNotSame(registrationPetition.TermCode, TermCodeRepository.GetById("2"));
            registrationPetition.TermCode = TermCodeRepository.GetById("2");
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registrationPetition.TermCode, TermCodeRepository.GetById("2"));
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion TermCode Tests

        #region Ceremony Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Ceremony with A new value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCeremonyWithANewValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Ceremony = new Ceremony();
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ceremony with null value saves.
        /// </summary>
        [TestMethod]
        public void TestCeremonyWithNullValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.IsNull(record.Ceremony);
            #endregion Assert
        }
        /// <summary>
        /// Tests the ceremony with new value saves.
        /// </summary>
        [TestMethod, Ignore]
        public void TestCeremonyWithExistingValueSaves()
        {
            #region Arrange
            LoadCeremony(1);
            var record = GetValid(9);
            record.Ceremony = Repository.OfType<Ceremony>().GetById(1);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.IsNotNull(record.Ceremony);
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Ceremony Tests
    }
}
