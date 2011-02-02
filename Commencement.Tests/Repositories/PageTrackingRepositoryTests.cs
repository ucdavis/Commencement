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

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		PageTracking
    /// LookupFieldName:	LoginId
    /// </summary>
    [TestClass]
    public class PageTrackingRepositoryTests : AbstractRepositoryTests<PageTracking, int, PageTrackingMap>
    {
        /// <summary>
        /// Gets or sets the PageTracking repository.
        /// </summary>
        /// <value>The PageTracking repository.</value>
        public IRepository<PageTracking> PageTrackingRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="PageTrackingRepositoryTests"/> class.
        /// </summary>
        public PageTrackingRepositoryTests()
        {
            PageTrackingRepository = new Repository<PageTracking>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override PageTracking GetValid(int? counter)
        {
            return CreateValidEntities.PageTracking(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<PageTracking> GetQuery(int numberAtEnd)
        {
            return PageTrackingRepository.Queryable.Where(a => a.LoginId.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(PageTracking entity, int counter)
        {
            Assert.AreEqual("LoginId" + counter, entity.LoginId);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(PageTracking entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.LoginId);
                    break;
                case ARTAction.Restore:
                    entity.LoginId = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.LoginId;
                    entity.LoginId = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            PageTrackingRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            PageTrackingRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapPageTracking()
        {
            #region Arrange
            var session = NHibernateSessionManager.Instance.GetSession();
            var id = PageTrackingRepository.Queryable.Max(x => x.Id) + 1;
            var dateToCompare = new DateTime(2010,01,01);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<PageTracking>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.DateTime, dateToCompare)
                .CheckProperty(c => c.IPAddress, "IPAddress")
                .CheckProperty(c => c.Location, "Location")
                .CheckProperty(c => c.LoginId, "LoginId")               
                .VerifyTheMappings();
            #endregion Act/Assert
        }



        #endregion Fluent Mapping Tests

        #region LoginId Tests

        #region Invalid Tests

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LoginId with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithNullValueSaves()
        {
            #region Arrange

            PageTracking pageTracking = GetValid(9);
            pageTracking.LoginId = null;

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithEmptyStringSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.LoginId = string.Empty;

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with spaces only saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithSpacesOnlySaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.LoginId = " ";

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithOneCharacterSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.LoginId = "x";

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithLongValueSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.LoginId = "x".RepeatTimes(999);

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.AreEqual(999, pageTracking.LoginId.Length);
            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        #endregion Valid Tests

        #endregion LoginId Tests

        #region Location Tests

        #region Invalid Tests

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Location with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithNullValueSaves()
        {
            #region Arrange

            PageTracking pageTracking = GetValid(9);
            pageTracking.Location = null;

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithEmptyStringSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.Location = string.Empty;

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with spaces only saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithSpacesOnlySaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.Location = " ";

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithOneCharacterSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.Location = "x";

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Location with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLocationWithLongValueSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.Location = "x".RepeatTimes(999);

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.AreEqual(999, pageTracking.Location.Length);
            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        #endregion Valid Tests

        #endregion Location Tests

        #region IPAddress Tests

        #region Invalid Tests

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the IPAddress with null value saves.
        /// </summary>
        [TestMethod]
        public void TestIPAddressWithNullValueSaves()
        {
            #region Arrange

            PageTracking pageTracking = GetValid(9);
            pageTracking.IPAddress = null;

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IPAddress with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestIPAddressWithEmptyStringSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.IPAddress = string.Empty;

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IPAddress with spaces only saves.
        /// </summary>
        [TestMethod]
        public void TestIPAddressWithSpacesOnlySaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.IPAddress = " ";

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IPAddress with one character saves.
        /// </summary>
        [TestMethod]
        public void TestIPAddressWithOneCharacterSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.IPAddress = "x";

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IPAddress with long value saves.
        /// </summary>
        [TestMethod]
        public void TestIPAddressWithLongValueSaves()
        {
            #region Arrange

            var pageTracking = GetValid(9);
            pageTracking.IPAddress = "x".RepeatTimes(999);

            #endregion Arrange

            #region Act

            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(pageTracking);
            PageTrackingRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.AreEqual(999, pageTracking.IPAddress.Length);
            Assert.IsFalse(pageTracking.IsTransient());
            Assert.IsTrue(pageTracking.IsValid());

            #endregion Assert
        }

        #endregion Valid Tests

        #endregion IPAddress Tests

        #region DateTime Tests

        /// <summary>
        /// Tests the DateTime with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateTimeWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            PageTracking record = GetValid(99);
            record.DateTime = compareDate;
            #endregion Arrange

            #region Act
            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(record);
            PageTrackingRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateTime);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateTime with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateTimeWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateTime = compareDate;
            #endregion Arrange

            #region Act
            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(record);
            PageTrackingRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateTime);
            #endregion Assert
        }

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
            PageTrackingRepository.DbContext.BeginTransaction();
            PageTrackingRepository.EnsurePersistent(record);
            PageTrackingRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateTime);
            #endregion Assert
        }
        #endregion DateTime Tests

        #region Constructor Tests

        /// <summary>
        /// Tests the constructor with no parameters defaults expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithNoParametersDefaultsExpectedValues()
        {
            #region Arrange
            var record = new PageTracking();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual(DateTime.Now.Date, record.DateTime.Date);
            #endregion Assert		
        }
        /// <summary>
        /// Tests the constructor with parameters defaults expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithParametersDefaultsExpectedValues()
        {
            #region Arrange
            var record = new PageTracking("loginId", "location", "ipAddress", true);
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual(DateTime.Now.Date, record.DateTime.Date);
            Assert.AreEqual("loginId", record.LoginId);
            Assert.AreEqual("location", record.Location);
            Assert.AreEqual("ipAddress", record.IPAddress);
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
            expectedFields.Add(new NameAndType("DateTime", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IPAddress", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Location", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("LoginId", "System.String", new List<string>()));

            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(PageTracking));

        }

        #endregion Reflection of Database.	
		
		
    }
}