using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Helpers;
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
    public class ExtraTicketPetitionRepositoryTests : AbstractRepositoryTests<ExtraTicketPetition, int>
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
            return ExtraTicketPetitionRepository.Queryable.Where(a => a.NumberTickets == numberAtEnd);
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(ExtraTicketPetition entity, int counter)
        {
            Assert.AreEqual(counter, entity.NumberTickets);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(ExtraTicketPetition entity, ARTAction action)
        {
            const int updateValue = 999;
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.NumberTickets);
                    break;
                case ARTAction.Restore:
                    entity.NumberTickets = IntRestoreValue;
                    break;
                case ARTAction.Update:
                    IntRestoreValue = entity.NumberTickets;
                    entity.NumberTickets = updateValue;
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
        
        #region NumberTickets Tests

        #region Invalid Tests

        /// <summary>
        /// Tests the NumberTickets with A value of 0 does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNumberTicketsWithAValueOf0DoesNotSave()
        {
            ExtraTicketPetition extraTicketPetition = null;
            try
            {
                #region Arrange
                extraTicketPetition = GetValid(9);
                extraTicketPetition.NumberTickets = 0;
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
                Assert.AreEqual(extraTicketPetition.NumberTickets, 0);
                var results = extraTicketPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("NumberTickets: must be greater than or equal to 1");
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
        /// Tests the number tickets with value of one saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithValueOfOneSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 1;
            #endregion Arrange

            #region Act
            ExtraTicketPetitionRepository.DbContext.BeginTransaction();
            ExtraTicketPetitionRepository.EnsurePersistent(record);
            ExtraTicketPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion NumberTickets Tests

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
        public void TestConstructorWithParametersSetsExpectedValues()
        {
            #region Arrange
            var record = new ExtraTicketPetition(5);
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsNull(record.DateDecision);
            Assert.AreEqual(5, record.NumberTickets);
            Assert.IsFalse(record.LabelPrinted);
            #endregion Assert
        }
        #endregion Constructor Tests


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
            expectedFields.Add(new NameAndType("DateDecision", "System.Nullable`1[System.DateTime]", new List<string>()));
            expectedFields.Add(new NameAndType("DateSubmitted", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IsApproved", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("IsPending", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("LabelPrinted", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("NumberTickets", "System.Int32", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)1)]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(ExtraTicketPetition));

        }

        #endregion Reflection of Database.	
		
		
    }
}