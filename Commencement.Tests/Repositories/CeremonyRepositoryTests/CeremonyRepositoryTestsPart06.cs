using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region ExtraTicketPerStudent Tests

        /// <summary>
        /// Tests the ExtraTicketPerStudent with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPerStudentWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.ExtraTicketPerStudent = int.MaxValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.ExtraTicketPerStudent);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ExtraTicketPerStudent with min int value saves.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPerStudentWithMinIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.ExtraTicketPerStudent = int.MinValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MinValue, record.ExtraTicketPerStudent);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion ExtraTicketPerStudent Tests

        #region MinUnits Tests


        /// <summary>
        /// Tests the MinUnits with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestMinUnitsWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.MinUnits = int.MaxValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.MinUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the MinUnits with min int value saves.
        /// </summary>
        [TestMethod]
        public void TestMinUnitsWithMinIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.MinUnits = int.MinValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MinValue, record.MinUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion MinUnits Tests
        #region PetitionThreshold Tests

        /// <summary>
        /// Tests the PetitionThreshold with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestPetitionThresholdWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.PetitionThreshold = int.MaxValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.PetitionThreshold);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the PetitionThreshold with min int value saves.
        /// </summary>
        [TestMethod]
        public void TestPetitionThresholdWithMinIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.PetitionThreshold = int.MinValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MinValue, record.PetitionThreshold);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion PetitionThreshold Tests

        #region HasStreamingTickets Tests

        /// <summary>
        /// Tests the HasStreamingTickets is false saves.
        /// </summary>
        [TestMethod]
        public void TestHasStreamingTicketsIsFalseSaves()
        {
            #region Arrange

            Ceremony ceremony = GetValid(9);
            ceremony.HasStreamingTickets = false;

            #endregion Arrange

            #region Act

            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(ceremony.HasStreamingTickets);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the HasStreamingTickets is true saves.
        /// </summary>
        [TestMethod]
        public void TestHasStreamingTicketsIsTrueSaves()
        {
            #region Arrange

            var ceremony = GetValid(9);
            ceremony.HasStreamingTickets = true;

            #endregion Arrange

            #region Act

            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(ceremony.HasStreamingTickets);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());

            #endregion Assert
        }

        #endregion HasStreamingTickets Tests


        #region TermCode Tests

        #region Invalid Tests

        /// <summary>
        /// Tests the term code with new value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestTermCodeWithNewValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.TermCode = new TermCode();
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ceremony);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.TermCode, Entity: Commencement.Core.Domain.TermCode", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Tests the term code with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTermCodeWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.TermCode = null;
                #endregion Arrange

                #region Act
                CeremonyRepository.DbContext.BeginTransaction();
                CeremonyRepository.EnsurePersistent(ceremony);
                CeremonyRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(ceremony);
                Assert.IsNull(ceremony.TermCode);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TermCode: may not be null");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        [TestMethod]
        public void TestTermCodeWithSavedValuesSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.ExtraTicketPerStudent = int.MaxValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.ExtraTicketPerStudent);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the term code changed to existing value updates.
        /// </summary>
        [TestMethod]
        public void TestTermCodeChangedToExistingValueUpdates()
        {
            #region Arrange
            var record = CeremonyRepository.GetById(2);
            Assert.AreEqual("Name2", record.TermCode.Name);
            record.TermCode = TermCodeRepository.GetById("3");
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual("Name3", record.TermCode.Name);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests

        #region Cascade Tests

        [TestMethod]
        public void TestCascadeDeleteDoesNotRemoveTermCode()
        {
            #region Arrange
            var termCodeCount = Repository.OfType<TermCode>().GetAll().Count;
            var ceremony = CeremonyRepository.GetById(2);
            Assert.IsNotNull(ceremony.TermCode);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(Repository.OfType<TermCode>().GetAll().Count, termCodeCount);
            #endregion Assert
        }

        #endregion Cascade Tests
        #endregion TermCode Tests
    }
}
