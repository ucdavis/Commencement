using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
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
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="CeremonyRepositoryTests"/> class.
        /// </summary>
        public CeremonyRepositoryTests()
        {
            CeremonyRepository = new Repository<Ceremony>();
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
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

        protected override void LoadRecords(int entriesToAdd)
        {
            EntriesAdded += entriesToAdd;
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = GetValid(i + 1);
                //validEntity.SetIdTo(0);
                CeremonyRepository.EnsurePersistent(validEntity);
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            TermCodeRepository.DbContext.BeginTransaction();
            LoadTermCode(5);
            TermCodeRepository.DbContext.CommitTransaction();            
            CeremonyRepository.DbContext.BeginTransaction();            
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
            expectedFields.Add(new NameAndType("DateTime", "System.DateTime", new List<string>
            {
                "", 
                ""
            }));
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

            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Ceremony));

        }

        #endregion Reflection of Database.	
		
		
    }
}