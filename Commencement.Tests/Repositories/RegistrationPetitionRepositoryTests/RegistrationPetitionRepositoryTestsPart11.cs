using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
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
        #region CompletionTerm Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the CompletionTerm with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                //registrationPetition.CompletionTerm = null;
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
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the CompletionTerm with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.CompletionTerm = string.Empty;
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
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the CompletionTerm with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.CompletionTerm = " ";
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
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        ///// <summary>
        ///// Tests the CompletionTerm with too long value does not save.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void TestCompletionTermWithTooLongValueDoesNotSave()
        //{
        //    RegistrationPetition registrationPetition = null;
        //    try
        //    {
        //        #region Arrange
        //        registrationPetition = GetValid(9);
        //        registrationPetition.CompletionTerm = "x".RepeatTimes((6 + 1));
        //        #endregion Arrange

        //        #region Act
        //        RegistrationPetitionRepository.DbContext.BeginTransaction();
        //        RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
        //        RegistrationPetitionRepository.DbContext.CommitTransaction();
        //        #endregion Act
        //    }
        //    catch (Exception)
        //    {
        //        Assert.IsNotNull(registrationPetition);
        //        Assert.AreEqual(6 + 1, registrationPetition.CompletionTerm.Length);
        //        var results = registrationPetition.ValidationResults().AsMessageList();
        //        results.AssertErrorsAre("CompletionTerm: length must be between 6 and 6");
        //        Assert.IsTrue(registrationPetition.IsTransient());
        //        Assert.IsFalse(registrationPetition.IsValid());
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// Tests the completion term with too short value does not save.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void TestCompletionTermWithTooShortValueDoesNotSave()
        //{
        //    RegistrationPetition registrationPetition = null;
        //    try
        //    {
        //        #region Arrange
        //        registrationPetition = GetValid(9);
        //        registrationPetition.CompletionTerm = "x".RepeatTimes((6 - 1));
        //        #endregion Arrange

        //        #region Act
        //        RegistrationPetitionRepository.DbContext.BeginTransaction();
        //        RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
        //        RegistrationPetitionRepository.DbContext.CommitTransaction();
        //        #endregion Act
        //    }
        //    catch (Exception)
        //    {
        //        Assert.IsNotNull(registrationPetition);
        //        Assert.AreEqual(6 - 1, registrationPetition.CompletionTerm.Length);
        //        var results = registrationPetition.ValidationResults().AsMessageList();
        //        results.AssertErrorsAre("CompletionTerm: length must be between 6 and 6");
        //        Assert.IsTrue(registrationPetition.IsTransient());
        //        Assert.IsFalse(registrationPetition.IsValid());
        //        throw;
        //    }
        //}
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the CompletionTerm with long value saves.
        /// </summary>
        [TestMethod]
        public void TestCompletionTermWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.CompletionTerm = "x".RepeatTimes(6);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(6, registrationPetition.CompletionTerm.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the completion term with too long value saves.
        /// </summary>
        [TestMethod]
        public void TestCompletionTermWithTooLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.CompletionTerm = "x".RepeatTimes(600);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(600, registrationPetition.CompletionTerm.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the completion term with too short value saves.
        /// </summary>
        [TestMethod]
        public void TestCompletionTermWithTooShortValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.CompletionTerm = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, registrationPetition.CompletionTerm.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion CompletionTerm Tests
    }
}
