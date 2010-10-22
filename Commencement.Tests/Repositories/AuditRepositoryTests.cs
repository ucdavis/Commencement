using System;
using System.Collections;
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
    /// Entity Name:		Audit
    /// LookupFieldName:	ObjectName
    /// </summary>
    [TestClass]
    public class AuditRepositoryTests : AbstractRepositoryTests<Audit, Guid, AuditMap>
    {
        /// <summary>
        /// Gets or sets the Audit repository.
        /// </summary>
        /// <value>The Audit repository.</value>
        public IRepositoryWithTypedId<Audit, Guid> AuditRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRepositoryTests"/> class.
        /// </summary>
        public AuditRepositoryTests()
        {
            AuditRepository = new RepositoryWithTypedId<Audit, Guid>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Audit GetValid(int? counter)
        {
            var localCounter = 99;
            if(counter != null)
            {
                localCounter = (int) counter;
            }
            return CreateValidEntities.Audit(localCounter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Audit> GetQuery(int numberAtEnd)
        {
            return AuditRepository.Queryable.Where(a => a.ObjectName.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Audit entity, int counter)
        {
            Assert.AreEqual("ObjectName" + counter, entity.ObjectName);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Audit entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.ObjectName);
                    break;
                case ARTAction.Restore:
                    entity.ObjectName = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.ObjectName;
                    entity.ObjectName = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            AuditRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            AuditRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapAttachment()
        {
            #region Arrange
            var id = Guid.NewGuid();
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck = new DateTime(2010, 01, 01);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Audit>(session, new AuditEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.AuditDate, dateToCheck)
                .CheckProperty(c => c.AuditActionTypeId, "S")
                .CheckProperty(c => c.ObjectId, "ObjectId")
                .CheckProperty(c => c.ObjectName, "ObjectName")
                .CheckProperty(c => c.Username, "UserName")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class AuditEqualityComparer : IEqualityComparer
        {
            public bool Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                if (x is Guid && y is Guid)
                {
                    if (((Guid)x) != ((Guid)y))
                    {
                        return true;
                    }
                    return false; //They should never match
                }

                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }

        #endregion Fluent Mapping Tests

        #region ObjectName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ObjectName with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithNullValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = null;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ObjectName with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithEmptyStringDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = string.Empty;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ObjectName with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithSpacesOnlyDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = " ";
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ObjectName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectNameWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(50 + 1, audit.ObjectName.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectName: length must be between 0 and 50");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ObjectName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestObjectNameWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectName = "x";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestObjectNameWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, audit.ObjectName.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ObjectName Tests

        #region ObjectId Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ObjectId with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestObjectIdWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.ObjectId = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(50 + 1, audit.ObjectId.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ObjectId: length must be between 0 and 50");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ObjectId with null value saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithNullValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = null;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithEmptyStringSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = string.Empty;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with one space saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithOneSpaceSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = " ";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with one character saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = "x";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ObjectId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestObjectIdWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.ObjectId = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, audit.ObjectId.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ObjectId Tests

        #region AuditActionTypeId Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the AuditActionTypeId with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionTypeIdWithNullValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditActionTypeId = null;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditActionTypeId: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the AuditActionTypeId with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionTypeIdWithEmptyStringDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditActionTypeId = string.Empty;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditActionTypeId: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the AuditActionTypeId with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionTypeIdWithSpacesOnlyDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditActionTypeId = " ";
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditActionTypeId: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the AuditActionTypeId with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAuditActionTypeIdWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.AuditActionTypeId = "x".RepeatTimes((1 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(1 + 1, audit.AuditActionTypeId.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("AuditActionTypeId: length must be between 0 and 1");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the AuditActionTypeId with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAuditActionTypeIdWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.AuditActionTypeId = "x";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the AuditActionTypeId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAuditActionTypeIdWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.AuditActionTypeId = "x".RepeatTimes(1);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, audit.AuditActionTypeId.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion AuditActionTypeId Tests

        #region Username Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Username with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithNullValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = null;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Username with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithEmptyStringDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = string.Empty;
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Username with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithSpacesOnlyDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = " ";
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: may not be null or empty");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Username with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestUsernameWithTooLongValueDoesNotSave()
        {
            Audit audit = null;
            try
            {
                #region Arrange
                audit = GetValid(9);
                audit.Username = "x".RepeatTimes((256 + 1));
                #endregion Arrange

                #region Act
                AuditRepository.DbContext.BeginTransaction();
                AuditRepository.EnsurePersistent(audit);
                AuditRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(audit);
                Assert.AreEqual(256 + 1, audit.Username.Length);
                var results = audit.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Username: length must be between 0 and 256");
                Assert.IsTrue(audit.IsTransient());
                Assert.IsFalse(audit.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Username with one character saves.
        /// </summary>
        [TestMethod]
        public void TestUsernameWithOneCharacterSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.Username = "x";
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Username with long value saves.
        /// </summary>
        [TestMethod]
        public void TestUsernameWithLongValueSaves()
        {
            #region Arrange
            var audit = GetValid(9);
            audit.Username = "x".RepeatTimes(256);
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(audit);
            AuditRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(256, audit.Username.Length);
            Assert.IsFalse(audit.IsTransient());
            Assert.IsTrue(audit.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Username Tests

        #region AuditDate Tests

        /// <summary>
        /// Tests the AuditDate with past date will save.
        /// </summary>
        [TestMethod]
        public void TestAuditDateWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Audit record = GetValid(99);
            record.AuditDate = compareDate;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(record);
            AuditRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.AuditDate);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the AuditDate with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestAuditDateWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.AuditDate = compareDate;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(record);
            AuditRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.AuditDate);
            #endregion Assert
        }

        /// <summary>
        /// Tests the AuditDate with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestAuditDateWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.AuditDate = compareDate;
            #endregion Arrange

            #region Act
            AuditRepository.DbContext.BeginTransaction();
            AuditRepository.EnsurePersistent(record);
            AuditRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.AuditDate);
            #endregion Assert
        }
        #endregion AuditDate Tests

        #region SetActionCode Tests

        /// <summary>
        /// Tests the set action code of create writes C.
        /// </summary>
        [TestMethod]
        public void TestSetActionCodeOfCreateWritesC()
        {
            #region Arrange
            var audit = new Audit();            
            #endregion Arrange

            #region Act
            audit.SetActionCode(AuditActionType.Create);
            #endregion Act

            #region Assert
            Assert.AreEqual("C", audit.AuditActionTypeId);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the set action code of update writes U.
        /// </summary>
        [TestMethod]
        public void TestSetActionCodeOfUpdateWritesU()
        {
            #region Arrange
            var audit = new Audit();
            #endregion Arrange

            #region Act
            audit.SetActionCode(AuditActionType.Update);
            #endregion Act

            #region Assert
            Assert.AreEqual("U", audit.AuditActionTypeId);
            #endregion Assert
        }

        /// <summary>
        /// Tests the set action code of delete writes D.
        /// </summary>
        [TestMethod]
        public void TestSetActionCodeOfDeleteWritesD()
        {
            #region Arrange
            var audit = new Audit();
            #endregion Arrange

            #region Act
            audit.SetActionCode(AuditActionType.Delete);
            #endregion Act

            #region Assert
            Assert.AreEqual("D", audit.AuditActionTypeId);
            #endregion Assert
        }
        #endregion SetActionCode Tests

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
            expectedFields.Add(new NameAndType("AuditActionTypeId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)1)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("AuditDate", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Guid", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ObjectId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("ObjectName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Username", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)256)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Audit));

        }

        #endregion Reflection of Database.	
		
		
    }
}