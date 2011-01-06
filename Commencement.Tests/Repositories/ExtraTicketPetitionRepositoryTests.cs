using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		ExtraTicketPetition
    /// LookupFieldName:	NumberTickets
    /// </summary>
    [TestClass]
    public class ExtraTicketPetitionRepositoryTests : AbstractRepositoryTests<ExtraTicketPetition, int, ExtraTicketPetitionMap>
    {
        /// <summary>
        /// Gets or sets the ExtraTicketPetition repository.
        /// </summary>
        /// <value>The ExtraTicketPetition repository.</value>
        public IRepository<ExtraTicketPetition> ExtraTicketPetitionRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtraTicketPetitionRepositoryTests"/> class.
        /// </summary>
        public ExtraTicketPetitionRepositoryTests()
        {
            ExtraTicketPetitionRepository = new Repository<ExtraTicketPetition>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override ExtraTicketPetition GetValid(int? counter)
        {
            var count = 1;
            if(counter != null)
            {
                count = (int)counter;
            }
            return CreateValidEntities.ExtraTicketPetition(count);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<ExtraTicketPetition> GetQuery(int numberAtEnd)
        {
            return ExtraTicketPetitionRepository.Queryable.Where(a => a.Reason.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(ExtraTicketPetition entity, int counter)
        {
            Assert.AreEqual("Reason" + counter, entity.Reason);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(ExtraTicketPetition entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Reason);
                    break;
                case ARTAction.Restore:
                    entity.Reason = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Reason;
                    entity.Reason = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region NumberTicketsRequested Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the NumberTicketsRequested with A value of -1 does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNumberTicketsRequestedWithAValueOfNegativeOneDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.NumberTicketsRequested = -1;
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                Assert.AreEqual(extraTicketPetition.NumberTicketsRequested, -1);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("NumberTicketsRequested: Can not enter negative numbers");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }	
        }
        #endregion Invalid Tests

        #region Valid Tests
        
        /// <summary>
        /// Tests the NumberTicketsRequested with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsRequestedWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTicketsRequested = int.MaxValue;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.NumberTicketsRequested);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the NumberTicketsRequested with zero value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsRequestedWithZeroValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTicketsRequested = 0;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.NumberTicketsRequested);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion NumberTicketsRequested Tests

        #region NumberTicketsRequestedStreaming Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the NumberTicketsRequested with A value of -1 does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNumberTicketsRequestedStreamingWithAValueOfNegativeOneDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.NumberTicketsRequestedStreaming = -1;
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                Assert.AreEqual(extraTicketPetition.NumberTicketsRequestedStreaming, -1);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("NumberTicketsRequestedStreaming: Can not enter negative numbers");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the NumberTicketsRequestedStreaming with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsRequestedStreamingWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTicketsRequestedStreaming = int.MaxValue;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.NumberTicketsRequestedStreaming);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the NumberTicketsRequestedStreaming with zero value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsRequestedStreamingWithZeroValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTicketsRequestedStreaming = 0;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.NumberTicketsRequestedStreaming);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion NumberTicketsRequestedStreaming Tests

        #region IsPending Tests

        /// <summary>
        /// Tests the IsPending is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsFalseSaves()
        {
            #region Arrange

            ExtraTicketPetition extraTicketPetition = GetValid(9);
            extraTicketPetition.IsPending = false;

            #endregion Arrange

            #region Act

            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(extraTicketPetition.IsPending);
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsPending is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsTrueSaves()
        {
            #region Arrange

            var extraTicketPetition = GetValid(9);
            extraTicketPetition.IsPending = true;

            #endregion Arrange

            #region Act

            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(extraTicketPetition.IsPending);
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());

            #endregion Assert
        }

        #endregion IsPending Tests

        #region IsApproved Tests

        /// <summary>
        /// Tests the IsApproved is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsApprovedIsFalseSaves()
        {
            #region Arrange

            ExtraTicketPetition extraTicketPetition = GetValid(9);
            extraTicketPetition.IsApproved = false;

            #endregion Arrange

            #region Act

            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(extraTicketPetition.IsApproved);
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsApproved is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsApprovedIsTrueSaves()
        {
            #region Arrange

            var extraTicketPetition = GetValid(9);
            extraTicketPetition.IsApproved = true;

            #endregion Arrange

            #region Act

            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(extraTicketPetition.IsApproved);
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());

            #endregion Assert
        }

        #endregion IsApproved Tests

        #region DateSubmitted Tests

        /// <summary>
        /// Tests the DateSubmitted with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            ExtraTicketPetition record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateSubmitted with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateSubmitted with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateSubmittedWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateSubmitted);
            #endregion Assert
        }
        #endregion DateSubmitted Tests

        #region DateDecision Tests
        /// <summary>
        /// Tests the DateDecision with null date saves.
        /// </summary>
        [TestMethod]
        public void TesDateDecisionWithNullDateSaves()
        {
            #region Arrange
            var record = GetValid(99);
            record.DateDecision = null;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.IsNull(record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            ExtraTicketPetition record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateDecision with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }
        #endregion DateDecision Tests

        #region LabelPrinted Tests

        /// <summary>
        /// Tests the LabelPrinted is false saves.
        /// </summary>
        [TestMethod]
        public void TestLabelPrintedIsFalseSaves()
        {
            #region Arrange

            ExtraTicketPetition extraTicketPetition = GetValid(9);
            extraTicketPetition.LabelPrinted = false;

            #endregion Arrange

            #region Act

            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(extraTicketPetition.LabelPrinted);
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the LabelPrinted is true saves.
        /// </summary>
        [TestMethod]
        public void TestLabelPrintedIsTrueSaves()
        {
            #region Arrange

            var extraTicketPetition = GetValid(9);
            extraTicketPetition.LabelPrinted = true;

            #endregion Arrange

            #region Act

            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(extraTicketPetition.LabelPrinted);
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());

            #endregion Assert
        }

        #endregion LabelPrinted Tests

        #region NumberTickets Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNumberTicketsWithAValueOfNegativeOneDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.NumberTickets = -1;
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                Assert.AreEqual(extraTicketPetition.NumberTickets, -1);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("NumberTickets: Can not enter negative numbers");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests
        
        /// <summary>
        /// Tests the NumberTickets with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = int.MaxValue;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the NumberTickets with Zero value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithZeroValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 0;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestNumberTicketsWithNullValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = null;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(null, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }


        #endregion Valid Tests
        #endregion NumberTickets Tests

        #region NumberTickets Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNumberTicketsStreamingWithAValueOfNegativeOneDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.NumberTicketsStreaming = -1;
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                Assert.AreEqual(extraTicketPetition.NumberTicketsStreaming, -1);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("NumberTicketsStreaming: Can not enter negative numbers");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the NumberTicketsStreaming with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsStreamingWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTicketsStreaming = int.MaxValue;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.NumberTicketsStreaming);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the NumberTicketsStreaming with Zero value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsStreamingWithZeroValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTicketsStreaming = 0;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.NumberTicketsStreaming);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestNumberTicketsStreamingWithNullValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTicketsStreaming = null;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(null, record.NumberTicketsStreaming);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }


        #endregion Valid Tests
        #endregion NumberTicketsStreaming Tests

        #region Reason Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Reason with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestReasonWithNullValueDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.Reason = null;
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Reason: may not be null or empty");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Reason with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestReasonWithEmptyStringDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.Reason = string.Empty;
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Reason: may not be null or empty");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Reason with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestReasonWithSpacesOnlyDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.Reason = " ";
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Reason: may not be null or empty");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Reason with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestReasonWithTooLongValueDoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.Reason = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                ExtraTicketPetitionRepository.DbContext.BeginTransaction();
                ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
                ExtraTicketPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(extraTicketPetition);
                Assert.AreEqual(100 + 1, extraTicketPetition.Reason.Length);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Reason: length must be between 0 and 100");
                Assert.IsTrue(extraTicketPetition.IsTransient());
                Assert.IsFalse(extraTicketPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Reason with one character saves.
        /// </summary>
        [TestMethod]
        public void TestReasonWithOneCharacterSaves()
        {
            #region Arrange
            var extraTicketPetition = GetValid(9);
            extraTicketPetition.Reason = "x";
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Reason with long value saves.
        /// </summary>
        [TestMethod]
        public void TestReasonWithLongValueSaves()
        {
            #region Arrange
            var extraTicketPetition = GetValid(9);
            extraTicketPetition.Reason = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(extraTicketPetition);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, extraTicketPetition.Reason.Length);
            Assert.IsFalse(extraTicketPetition.IsTransient());
            Assert.IsTrue(extraTicketPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Reason Tests

        #region  Extended/Calculated Fields
        #region TotalTicketsRequested Tests

        [TestMethod]
        public void TestTotalTicketsRequestedReturnsExpectedValue()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.NumberTicketsRequested = 10;
            record.NumberTicketsRequestedStreaming = 5;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(15, record.TotalTicketsRequested);
            #endregion Assert		
        }
        #endregion TotalTicketsRequested Tests

        #region TotalTickets Tests
        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.NumberTickets = 10;
            record.NumberTicketsStreaming = 5;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(15, record.TotalTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.NumberTickets = 0;
            record.NumberTicketsStreaming = 5;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(5, record.TotalTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.NumberTickets = null;
            record.NumberTicketsStreaming = 5;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(5, record.TotalTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.NumberTickets = 10;
            record.NumberTicketsStreaming = 0;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(10, record.TotalTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestTotalTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.NumberTickets = 10;
            record.NumberTicketsStreaming = null;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(10, record.TotalTickets);
            #endregion Assert
        }

        #endregion TotalTickets Tests

        #region MakeDecision Tests

        [TestMethod]
        public void TestMakeDecisionSetsExpectedValues1()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsPending = true;
            record.IsApproved = false;
            Assert.IsNull(record.DateDecision);
            #endregion Arrange

            #region Act
            record.MakeDecision(true);
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsPending);
            Assert.IsTrue(record.IsApproved);
            Assert.IsNotNull(record.DateDecision);
            var checkDate = (DateTime)record.DateDecision;
            Assert.AreEqual(DateTime.Now.Date, checkDate.Date);
            #endregion Assert		
        }

        [TestMethod]
        public void TestMakeDecisionSetsExpectedValues2()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsPending = true;
            record.IsApproved = false;
            Assert.IsNull(record.DateDecision);
            #endregion Arrange

            #region Act
            record.MakeDecision(false);
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.IsNotNull(record.DateDecision);
            var checkDate = (DateTime)record.DateDecision;
            Assert.AreEqual(DateTime.Now.Date, checkDate.Date);
            #endregion Assert
        }

        #endregion  MakeDecision Tests

        #region IsApprovedCompletely Tests

        [TestMethod]
        public void TestIsApprovedCompletelyReturnsExpectValue1()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = true;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            #endregion Assert		
        }

        [TestMethod]
        public void TestIsApprovedCompletelyReturnsExpectValue2()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsApprovedCompletely);
            #endregion Assert
        }
        [TestMethod]
        public void TestIsApprovedCompletelyReturnsExpectValue3()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = true;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            #endregion Assert
        }
        [TestMethod]
        public void TestIsApprovedCompletelyReturnsExpectValue4()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = false;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            #endregion Assert
        }

        #endregion IsApprovedCompletely Tests

        #region ApprovedTickets Tests

        [TestMethod]
        public void TestApprovedTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTickets = 4;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(4, record.ApprovedTickets);
            #endregion Assert		
        }

        [TestMethod]
        public void TestApprovedTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = false;
            record.NumberTickets = 4;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestApprovedTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = true;
            record.NumberTickets = 4;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestApprovedTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTickets = 0;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestApprovedTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTickets = null;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedTickets);
            #endregion Assert
        }
        #endregion ApprovedTickets Tests

        #region ApprovedStreamingTickets Tests

        [TestMethod]
        public void TestApprovedStreamingTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTicketsStreaming = 4;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(4, record.ApprovedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestApprovedStreamingTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = false;
            record.NumberTicketsStreaming = 4;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestApprovedStreamingTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = true;
            record.NumberTicketsStreaming = 4;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestApprovedStreamingTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTicketsStreaming = 0;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestApprovedStreamingTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTicketsStreaming = null;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(0, record.ApprovedStreamingTickets);
            #endregion Assert
        }
        #endregion ApprovedStreamingTickets Tests

        #region ProjectedTickets Tests

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTickets = 7;
            record.NumberTicketsRequested = 15;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsApprovedCompletely);
            Assert.AreEqual(7, record.ProjectedTickets);
            #endregion Assert		
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTickets = 0;
            record.NumberTicketsRequested = 15;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTickets = null;
            record.NumberTicketsRequested = 15;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = true;
            record.NumberTickets = 7;
            record.NumberTicketsRequested = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(7, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = true;
            record.NumberTickets = 0;
            record.NumberTicketsRequested = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue6()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = true;
            record.NumberTickets = null;
            record.NumberTicketsRequested = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(15, record.ProjectedTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedTicketsReturnsExpectedValue7()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = false;
            record.NumberTickets = 7;
            record.NumberTicketsRequested = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedTickets);
            #endregion Assert
        }

        #endregion ProjectedTickets Tests

        #region ProjectedStreamingTickets Tests

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue1()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTicketsStreaming = 7;
            record.NumberTicketsRequestedStreaming = 15;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsApprovedCompletely);
            Assert.AreEqual(7, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue2()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTicketsStreaming = 0;
            record.NumberTicketsRequestedStreaming = 15;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue3()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = true;
            record.IsPending = false;
            record.NumberTicketsStreaming = null;
            record.NumberTicketsRequestedStreaming = 15;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue4()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = true;
            record.NumberTicketsStreaming = 7;
            record.NumberTicketsRequestedStreaming = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(7, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue5()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = true;
            record.NumberTicketsStreaming = 0;
            record.NumberTicketsRequestedStreaming = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue6()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = true;
            record.NumberTicketsStreaming = null;
            record.NumberTicketsRequestedStreaming = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(15, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        [TestMethod]
        public void TestProjectedStreamingTicketsReturnsExpectedValue7()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            record.IsApproved = false;
            record.IsPending = false;
            record.NumberTicketsStreaming = 7;
            record.NumberTicketsRequestedStreaming = 15;
            #endregion Arrange

            #region Assert
            Assert.IsFalse(record.IsApprovedCompletely);
            Assert.AreEqual(0, record.ProjectedStreamingTickets);
            #endregion Assert
        }

        #endregion ProjectedStreamingTickets Tests
        #endregion  Extended/Calculated Fields
        
        #region Constructor Tests

        /// <summary>
        /// Tests the constructor with no parameters sets expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithNoParametersSetsExpectedValues()
        {
            #region Arrange
            var record = new ExtraTicketPetition();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsNull(record.DateDecision);
            Assert.IsFalse(record.LabelPrinted);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the constructor with parameters sets expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithParametersSetsExpectedValues1()
        {
            #region Arrange
            var record = new ExtraTicketPetition(5, "ReasonHere");
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsNull(record.DateDecision);
            Assert.IsFalse(record.LabelPrinted);
            Assert.AreEqual(5, record.NumberTicketsRequested);
            Assert.AreEqual(0, record.NumberTicketsRequestedStreaming);
            #endregion Assert
        }

        [TestMethod]
        public void TestConstructorWithParametersSetsExpectedValues2()
        {
            #region Arrange
            var record = new ExtraTicketPetition(5, "ReasonHere", 7);
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsNull(record.DateDecision);
            Assert.IsFalse(record.LabelPrinted);
            Assert.AreEqual(5, record.NumberTicketsRequested);
            Assert.AreEqual(7, record.NumberTicketsRequestedStreaming);
            #endregion Assert
        }
        #endregion Constructor Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapExtraTicketPetition1()
        {
            #region Arrange
            var id = ExtraTicketPetitionRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            var dateToCheck2 = new DateTime(2010, 01, 02);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<ExtraTicketPetition>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.NumberTicketsRequested, 6)
                .CheckProperty(c => c.NumberTicketsRequestedStreaming, 7)
                .CheckProperty(c => c.IsApproved, true) //Diff
                .CheckProperty(c => c.IsPending, true) //Diff
                .CheckProperty(c => c.DateSubmitted, dateToCheck2)
                .CheckProperty(c => c.DateDecision, dateToCheck1) //Diff
                .CheckProperty(c => c.LabelPrinted, false) //Diff
                .CheckProperty(c => c.NumberTickets, 12) //Diff
                .CheckProperty(c => c.NumberTicketsStreaming, 11) //Diff
                .CheckProperty(c => c.Reason, "Reason")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapExtraTicketPetition2()
        {
            #region Arrange
            var id = ExtraTicketPetitionRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            var dateToCheck2 = new DateTime(2010, 01, 02);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<ExtraTicketPetition>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.NumberTicketsRequested, 6)
                .CheckProperty(c => c.NumberTicketsRequestedStreaming, 7)
                .CheckProperty(c => c.IsApproved, false) //Diff
                .CheckProperty(c => c.IsPending, false) //Diff
                .CheckProperty(c => c.DateSubmitted, dateToCheck2)
                .CheckProperty(c => c.DateDecision, null) //Diff
                .CheckProperty(c => c.LabelPrinted, true) //Diff
                .CheckProperty(c => c.NumberTickets, null) //Diff
                .CheckProperty(c => c.NumberTicketsStreaming, null) //Diff
                .CheckProperty(c => c.Reason, "Reason")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        #endregion Fluent Mapping Tests

        #region Reflection of Database.

        /// <summary>
        /// Tests all fields in the database have been tested.
        /// If this fails and no other tests, it means that a field has been added which has not been tested above.
        /// </summary>
        [TestMethod]
        public void TestAllFieldsInTheDatabaseHaveBeenTested()
        {
            #region Arrange
            var expectedFields = new List<NameAndType>();            
            expectedFields.Add(new NameAndType("ApprovedStreamingTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("ApprovedTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("DateDecision", "System.Nullable`1[System.DateTime]", new List<string>()));
            expectedFields.Add(new NameAndType("DateSubmitted", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IsApproved", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("IsApprovedCompletely", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("IsPending", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("LabelPrinted", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("NumberTickets", "System.Nullable`1[System.Int32]", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)0, Message = \"Can not enter negative numbers\")]"
            }));
            expectedFields.Add(new NameAndType("NumberTicketsRequested", "System.Int32", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)0, Message = \"Can not enter negative numbers\")]"
            }));
            expectedFields.Add(new NameAndType("NumberTicketsRequestedStreaming", "System.Int32", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)0, Message = \"Can not enter negative numbers\")]"
            }));
            expectedFields.Add(new NameAndType("NumberTicketsStreaming", "System.Nullable`1[System.Int32]", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)0, Message = \"Can not enter negative numbers\")]"
            }));
            expectedFields.Add(new NameAndType("ProjectedStreamingTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("ProjectedTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Reason", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TotalTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("TotalTicketsRequested", "System.Int32", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(ExtraTicketPetition));

        }

        #endregion Reflection of Database.	
				
    }
}