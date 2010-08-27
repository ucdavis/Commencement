using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Core.Repositories
{
    /// <summary>
    /// Entity Name:		Ceremony
    /// LookupFieldName:	Location
    /// </summary>
    [TestClass]
    public class CeremonyRepositoryTests : AbstractRepositoryTests<Ceremony, int>
    {
        /// <summary>
        /// Gets or sets the Ceremony repository.
        /// </summary>
        /// <value>The Ceremony repository.</value>
        public IRepository<Ceremony> CeremonyRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string> TermCodeRepository { get; set; }
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
        public IRepositoryWithTypedId<State, string> StateRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="CeremonyRepositoryTests"/> class.
        /// </summary>
        public CeremonyRepositoryTests()
        {
            CeremonyRepository = new Repository<Ceremony>();
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            StateRepository = new RepositoryWithTypedId<State, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Ceremony GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.Ceremony(counter);
            var localCounter = "1";
            if (counter != null)
            {
                localCounter = counter.ToString();
            }

            rtValue.TermCode = TermCodeRepository.GetById(localCounter);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Ceremony> GetQuery(int numberAtEnd)
        {
            return CeremonyRepository.Queryable.Where(a => a.Location.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Ceremony entity, int counter)
        {
            Assert.AreEqual("Location" + counter, entity.Location);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Ceremony entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Location);
                    break;
                case ARTAction.Restore:
                    entity.Location = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Location;
                    entity.Location = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {         
            CeremonyRepository.DbContext.BeginTransaction();
            LoadTermCode(5);
            LoadRecords(5);
            CeremonyRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
                
        #region Location Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Location with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLocationWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Location = null;
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
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Location: may not be null or empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Location with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLocationWithEmptyStringDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Location = string.Empty;
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
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Location: may not be null or empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Location with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLocationWithSpacesOnlyDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Location = " ";
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
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Location: may not be null or empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Location with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLocationWithTooLongValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Location = "x".RepeatTimes((200 + 1));
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
                Assert.AreEqual(200 + 1, ceremony.Location.Length);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Location: length must be between 0 and 200");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Location with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithOneCharacterSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Location = "x";
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithLongValueSaves()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.Location = "x".RepeatTimes(200);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(200, ceremony.Location.Length);
            Assert.IsFalse(ceremony.IsTransient());
            Assert.IsTrue(ceremony.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Location Tests

        #region DateTime Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the date time with past date does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDateTimeWithPastDateDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.DateTime = DateTime.Now.AddDays(-1);
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
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("DateTime: must be a future date");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the date time with past date does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDateTimeWithCurrentDateDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.DateTime = DateTime.Now;
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
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("DateTime: must be a future date");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests


        /// <summary>
        /// Tests the DateTime with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateTimeWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateTime = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateTime);
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion DateTime Tests        

        #region TicketsPerStudent Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTicketsPerStudentWithZeroValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.TicketsPerStudent = 0;
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
                Assert.AreEqual(0, ceremony.TicketsPerStudent);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TicketsPerStudent: must be greater than or equal to 1");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests        
        /// <summary>
        /// Tests the TicketsPerStudent with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestTicketsPerStudentWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TicketsPerStudent = int.MaxValue;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.TicketsPerStudent);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TicketsPerStudent with min int value saves.
        /// </summary>
        [TestMethod]
        public void TestTicketsPerStudentWithValueOfOneSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TicketsPerStudent = 1;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, record.TicketsPerStudent);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion TicketsPerStudent Tests

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

        #region RegistrationDeadline Tests

        /// <summary>
        /// Tests the RegistrationDeadline with past date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationDeadlineWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Ceremony record = GetValid(99);
            record.RegistrationDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationDeadline);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the RegistrationDeadline with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationDeadlineWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.RegistrationDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the RegistrationDeadline with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestRegistrationDeadlineWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.RegistrationDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.RegistrationDeadline);
            #endregion Assert
        }
        #endregion RegistrationDeadline Tests

        #region ExtraTicketDeadline Tests

        /// <summary>
        /// Tests the ExtraTicketDeadline with past date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketDeadlineWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Ceremony record = GetValid(99);
            record.ExtraTicketDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketDeadline);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the ExtraTicketDeadline with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketDeadlineWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.ExtraTicketDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketDeadline);
            #endregion Assert
        }

        /// <summary>
        /// Tests the ExtraTicketDeadline with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketDeadlineWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.ExtraTicketDeadline = compareDate;
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.ExtraTicketDeadline);
            #endregion Assert
        }
        #endregion ExtraTicketDeadline Tests

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
                results.AssertErrorsAre("TermCode: may not be empty");
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
        #endregion TermCode Tests

        #region Registrations Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the registrations with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRegistrationsWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Registrations = null;
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
                Assert.IsNull(ceremony.Registrations);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Registrations: may not be empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the registrations with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestRegistrationsWithEmptyListSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Registrations = new List<Registration>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.Registrations.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert		
        }


        /// <summary>
        /// Tests the registrations with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestRegistrationsWithPopulatedListSaves()
        {
            #region Arrange
            var record = GetValid(9);
            
            MajorCodeRepository.DbContext.BeginTransaction();
            var majorCode = CreateValidEntities.MajorCode(1);
            majorCode.SetIdTo("1");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            var state = CreateValidEntities.State(1);
            state.SetIdTo("1");
            StateRepository.EnsurePersistent(state, true);
            MajorCodeRepository.DbContext.CommitTransaction();

            record.Registrations = new List<Registration>();
            record.Registrations.Add(CreateValidEntities.Registration(1));
            record.Registrations.Add(CreateValidEntities.Registration(2));
            foreach (var registration in record.Registrations)
            {
                registration.Major = MajorCodeRepository.GetById("1");
                registration.State = StateRepository.GetById("1");
                registration.Ceremony = record;
            }
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, record.Registrations.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert		
        }
        #endregion Valid Tests
        #endregion Registrations Tests

        #region Majors Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Majors with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMajorsWithNullValueDoesNotSave()
        {
            Ceremony ceremony = null;
            try
            {
                #region Arrange
                ceremony = GetValid(9);
                ceremony.Majors = null;
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
                Assert.IsNull(ceremony.Majors);
                var results = ceremony.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Majors: may not be empty");
                Assert.IsTrue(ceremony.IsTransient());
                Assert.IsFalse(ceremony.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Majors with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithEmptyListSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0, record.Majors.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }


        /// <summary>
        /// Tests the Majors with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithPopulatedListSaves()
        {
            #region Arrange
            var record = GetValid(9);

            MajorCodeRepository.DbContext.BeginTransaction();
            var majorCode = CreateValidEntities.MajorCode(1);
            majorCode.SetIdTo("1");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            majorCode = CreateValidEntities.MajorCode(1);

            majorCode.SetIdTo("2");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            MajorCodeRepository.DbContext.CommitTransaction();

            record.Majors = new List<MajorCode>();
            record.Majors.Add(MajorCodeRepository.GetById("1"));
            record.Majors.Add(MajorCodeRepository.GetById("2"));
      
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, record.Majors.Count());
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Majors Tests

        #region Name Tests (Getter only)

        /// <summary>
        /// Tests the name returns expected result for term code ending in 03.
        /// </summary>
        [TestMethod]
        public void TestNameReturnsExpectedResultForTermCodeEndingIn03()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(1);
            termCode.Name = "Spring Quarter 2010";
            termCode.IsActive = true;
            termCode.SetIdTo("201003");

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCode;
            #endregion Arrange

            #region Act
            var result = ceremony.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("CA&ES Spring Commencement 2010", result);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the name returns expected result for term code ending in 10.
        /// </summary>
        [TestMethod]
        public void TestNameReturnsExpectedResultForTermCodeEndingIn10()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(1);
            termCode.Name = "Spring Quarter 2010";
            termCode.IsActive = true;
            termCode.SetIdTo("201210");

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCode;
            #endregion Arrange

            #region Act
            var result = ceremony.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("CA&ES Fall Commencement 2012", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the name returns expected result for term code not ending in 10 or 03.
        /// </summary>
        [TestMethod]
        public void TestNameReturnsExpectedResultForTermCodeNotEndingIn10Or03()
        {
            #region Arrange
            var termCode = CreateValidEntities.TermCode(1);
            termCode.Name = "Spring Quarter 2010";
            termCode.IsActive = true;
            termCode.SetIdTo("201105");

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCode;
            #endregion Arrange

            #region Act
            var result = ceremony.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("CA&ES Commencement 2011", result);
            #endregion Assert
        }

        #endregion Name Tests (Getter only)

        #region AvailableTickets Tests

        /// <summary>
        /// Tests the available tickets returns expected value.
        /// </summary>
        [TestMethod]
        public void TestAvailableTicketsReturnsExpectedValue()
        {
            #region Arrange
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TotalTickets = 500;
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations.Add(new Registration());
            ceremony.Registrations[0].NumberTickets = 3;
            ceremony.Registrations[1].NumberTickets = 5;
            ceremony.Registrations[2].NumberTickets = 11;
            #endregion Arrange

            #region Act
            var result = ceremony.AvailableTickets;
            #endregion Act

            #region Assert
            Assert.AreEqual(481, result);
            #endregion Assert		
        }

        #endregion AvailableTickets Tests

        #region Cascade Update And Delete Tests

        /// <summary>
        /// Tests the cascade delete removed related registrations.
        /// </summary>
        [TestMethod]
        public void TestCascadeDeleteRemovedRelatedRegistrations()
        {
            #region Arrange
            var startCountregistrations = Repository.OfType<Registration>().GetAll().Count;
            var record = GetValid(9);

            MajorCodeRepository.DbContext.BeginTransaction();
            var majorCode = CreateValidEntities.MajorCode(1);
            majorCode.SetIdTo("1");
            MajorCodeRepository.EnsurePersistent(majorCode, true);
            var state = CreateValidEntities.State(1);
            state.SetIdTo("1");
            StateRepository.EnsurePersistent(state, true);
            MajorCodeRepository.DbContext.CommitTransaction();

            record.Registrations = new List<Registration>();
            record.Registrations.Add(CreateValidEntities.Registration(1));
            record.Registrations.Add(CreateValidEntities.Registration(2));
            foreach (var registration in record.Registrations)
            {
                registration.Major = MajorCodeRepository.GetById("1");
                registration.State = StateRepository.GetById("1");
                registration.Ceremony = record;
            }

            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(record);
            CeremonyRepository.DbContext.CommitTransaction();

            var registrationsAdded = Repository.OfType<Registration>().GetAll().Count - startCountregistrations;
            Assert.AreEqual(2, registrationsAdded);
            #endregion Arrange

            #region Act
            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.Remove(record);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(Repository.OfType<Registration>().GetAll().Count, startCountregistrations);
            #endregion Assert		
        }
       
        #endregion Cascade Update And Delete Tests

        #region Constructor Tests

        /// <summary>
        /// Tests the constructor with no parameters defaults expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithNoParametersDefaultsExpectedValues()
        {
            #region Arrange
            var ceremony = new Ceremony();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.Registrations);
            Assert.IsNotNull(ceremony.Majors);
            Assert.AreEqual(DateTime.Now.Date, ceremony.DateTime.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.RegistrationDeadline.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.ExtraTicketDeadline.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.PrintingDeadline.Date);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the constructor with parameters defaults expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithParametersDefaultsExpectedValues()
        {
            #region Arrange
            var ceremony = new Ceremony("Location", DateTime.Now.AddDays(10), 10, 100, DateTime.Now.AddDays(15), DateTime.Now.AddDays(20), CreateValidEntities.TermCode(4));
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("Location", ceremony.Location);
            Assert.IsNotNull(ceremony.Registrations);
            Assert.IsNotNull(ceremony.Majors);
            Assert.AreEqual(DateTime.Now.AddDays(10).Date, ceremony.DateTime.Date);
            Assert.AreEqual(10, ceremony.TicketsPerStudent);
            Assert.AreEqual(100, ceremony.TotalTickets);
            Assert.AreEqual(DateTime.Now.AddDays(20).Date, ceremony.RegistrationDeadline.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.ExtraTicketDeadline.Date);
            Assert.AreEqual(DateTime.Now.AddDays(15).Date, ceremony.PrintingDeadline.Date);
            Assert.AreEqual("Name4", ceremony.TermCode.Name);
            #endregion Assert
        }
        #endregion Constructo Tests


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
            expectedFields.Add(new NameAndType("AvailableTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("DateTime", "System.DateTime", new List<string>
            {
                "[NHibernate.Validator.Constraints.FutureAttribute()]", 
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ExtraTicketDeadline", "System.DateTime", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ExtraTicketPerStudent", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Location", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Majors", "System.Collections.Generic.IList`1[Commencement.Core.Domain.MajorCode]", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("PrintingDeadline", "System.DateTime", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("RegistrationDeadline", "System.DateTime", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));

            expectedFields.Add(new NameAndType("Registrations", "System.Collections.Generic.IList`1[Commencement.Core.Domain.Registration]", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TermCode", "Commencement.Core.Domain.TermCode", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TicketsPerStudent", "System.Int32", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)1)]",
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"                
            }));
            expectedFields.Add(new NameAndType("TotalTickets", "System.Int32", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)1)]",
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"     
            }));
            

            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Ceremony));

        }

        #endregion Reflection of Database.	
		
		
    }
}