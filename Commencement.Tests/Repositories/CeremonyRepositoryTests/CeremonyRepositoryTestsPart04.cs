using System;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region TotalTickets Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the total tickets with zero value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTotalTicketsWithZeroValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.TotalTickets = 0;
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
                Assert.AreEqual(0, ceremony.TotalTickets);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TotalTickets: must be greater than or equal to 1");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests
        /// <summary>
        /// Tests the TotalTickets with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TotalTickets = int.MaxValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.TotalTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TotalTickets with min int value saves.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWithValueOfOneSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TotalTickets = 1;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, record.TotalTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion TotalTickets Tests

        #region TotalStreamingTickets Tests

        /// <summary>
        /// Tests the TotalStreamingTickets with null value saves.
        /// </summary>
        [TestMethod]
        public void TestTotalStreamingTicketsWithNullValueSaves()
        {
            #region Arrange
            Ceremony record = GetValid(9);
            record.TotalStreamingTickets = null;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(record.TotalStreamingTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert		
        }

        /// <summary>
        /// Tests the TotalStreamingTickets with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestTotalStreamingTicketsWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TotalStreamingTickets = int.MaxValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.TotalStreamingTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TotalStreamingTickets with min int value saves.
        /// </summary>
        [TestMethod]
        public void TestTotalStreamingTicketsWithMinIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TotalStreamingTickets = int.MinValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MinValue, record.TotalStreamingTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion TotalStreamingTickets Tests
        


        #region PrintingDeadline Tests

        /// <summary>
        /// Tests the PrintingDeadline with past date will save.
        /// </summary>
        [TestMethod]
        public void TestPrintingDeadlineWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Ceremony record = GetValid(99);
            record.PrintingDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.PrintingDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the PrintingDeadline with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestPrintingDeadlineWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.PrintingDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.PrintingDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the PrintingDeadline with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestPrintingDeadlineWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.PrintingDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.PrintingDeadline);
            #endregion Assert
        }
        #endregion PrintingDeadline Tests
    }
}
