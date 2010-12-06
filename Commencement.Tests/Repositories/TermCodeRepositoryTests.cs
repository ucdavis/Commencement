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

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		TermCode
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class TermCodeRepositoryTests : AbstractRepositoryTests<TermCode, string, TermCodeMap >
    {
        /// <summary>
        /// Gets or sets the TermCode repository.
        /// </summary>
        /// <value>The TermCode repository.</value>
        public IRepositoryWithTypedId<TermCode, string > TermCodeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="TermCodeRepositoryTests"/> class.
        /// </summary>
        public TermCodeRepositoryTests()
        {
            ForceSave = true;
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override TermCode GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.TermCode(counter);
            var localCount = "";
            if(counter != null)
            {
                localCount = counter.ToString();
            }
            rtValue.SetIdTo(localCount);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<TermCode> GetQuery(int numberAtEnd)
        {
            return TermCodeRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(TermCode entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(TermCode entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Name);
                    break;
                case ARTAction.Restore:
                    entity.Name = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Name;
                    entity.Name = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            TermCodeRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            TermCodeRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Name Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Name with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithNullValueDoesNotSave()
        {
            TermCode termCode = null;
            try
            {
                #region Arrange
                termCode = GetValid(9);
                termCode.Name = null;
                #endregion Arrange

                #region Act
                TermCodeRepository.DbContext.BeginTransaction();
                TermCodeRepository.EnsurePersistent(termCode, true);
                TermCodeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(termCode);
                var results = termCode.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                //Assert.IsTrue(termCode.IsTransient());
                Assert.IsFalse(termCode.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithEmptyStringDoesNotSave()
        {
            TermCode termCode = null;
            try
            {
                #region Arrange
                termCode = GetValid(9);
                termCode.Name = string.Empty;
                #endregion Arrange

                #region Act
                TermCodeRepository.DbContext.BeginTransaction();
                TermCodeRepository.EnsurePersistent(termCode, true);
                TermCodeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(termCode);
                var results = termCode.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                //Assert.IsTrue(termCode.IsTransient());
                Assert.IsFalse(termCode.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithSpacesOnlyDoesNotSave()
        {
            TermCode termCode = null;
            try
            {
                #region Arrange
                termCode = GetValid(9);
                termCode.Name = " ";
                #endregion Arrange

                #region Act
                TermCodeRepository.DbContext.BeginTransaction();
                TermCodeRepository.EnsurePersistent(termCode, true);
                TermCodeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(termCode);
                var results = termCode.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: may not be null or empty");
                //Assert.IsTrue(termCode.IsTransient());
                Assert.IsFalse(termCode.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Name with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNameWithTooLongValueDoesNotSave()
        {
            TermCode termCode = null;
            try
            {
                #region Arrange
                termCode = GetValid(9);
                termCode.Name = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                TermCodeRepository.DbContext.BeginTransaction();
                TermCodeRepository.EnsurePersistent(termCode, true);
                TermCodeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(termCode);
                Assert.AreEqual(50 + 1, termCode.Name.Length);
                var results = termCode.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Name: length must be between 0 and 50");
                //Assert.IsTrue(termCode.IsTransient());
                Assert.IsFalse(termCode.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var termCode = GetValid(9);
            termCode.Name = "x";
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var termCode = GetValid(9);
            termCode.Name = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, termCode.Name.Length);
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        #region IsActive Tests

        /// <summary>
        /// Tests the IsActive is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsActiveIsFalseSaves()
        {
            #region Arrange
            TermCode termCode = GetValid(9);
            termCode.IsActive = false;
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert

            Assert.IsFalse(termCode.IsActive);
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsActive is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsActiveIsTrueSaves()
        {
            #region Arrange
            var termCode = GetValid(9);
            termCode.IsActive = true;
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsTrue(termCode.IsActive);
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());
            #endregion Assert
        }

        #endregion IsActive Tests

        #region Ceremonies Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Ceremonies with A value of new does not save.
        /// </summary>
        [TestMethod, Ignore]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCeremoniesWithAValueOfNewDoesNotSave()
        {
            TermCode termCode = null;
            try
            {
                #region Arrange
                termCode = GetValid(9);
                termCode.Ceremonies.Add(CreateValidEntities.Ceremony(9));
                #endregion Arrange

                #region Act
                TermCodeRepository.DbContext.BeginTransaction();
                TermCodeRepository.EnsurePersistent(termCode, true);
                TermCodeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(termCode);
                Assert.AreEqual(1, termCode.Ceremonies.Count);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }	
        }
        #endregion Invalid Tests
        #region Valid Tests        
        /// <summary>
        /// Tests the ceremonies with A null value saves.
        /// </summary>
        [TestMethod]
        public void TestCeremoniesWithANullValueSaves()
        {
            #region Arrange
            var termCode = GetValid(9);
            termCode.Ceremonies = null;            
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(termCode.Ceremonies);
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());
            #endregion Assert		
        }

        /// <summary>
        /// Tests the ceremonies with an empty list saves.
        /// </summary>
        [TestMethod]
        public void TestCeremoniesWithAnEmptyListSaves()
        {
            #region Arrange
            var termCode = GetValid(9);
            termCode.Ceremonies = new List<Ceremony>();
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(termCode.Ceremonies);
            Assert.AreEqual(0, termCode.Ceremonies.Count);
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ceremonies with populated values saves.
        /// </summary>
        [TestMethod, Ignore]
        public void TestCeremoniesWithPopulatedValuesSaves()
        {
            #region Arrange
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            LoadCeremony(5);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            var termCode = GetValid(9);
            termCode.Ceremonies = new List<Ceremony>();
            termCode.Ceremonies.Add(Repository.OfType<Ceremony>().GetById(2));
            termCode.Ceremonies.Add(Repository.OfType<Ceremony>().GetById(4));
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(termCode.Ceremonies);
            Assert.AreEqual(2, termCode.Ceremonies.Count);
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestCeremoniesWithPopulatedUnSavedValuesSaves()
        {
            #region Arrange
            var termCode = GetValid(9);
            termCode.Ceremonies = new List<Ceremony>();
            termCode.Ceremonies.Add(CreateValidEntities.Ceremony(24));
            termCode.Ceremonies.Add(CreateValidEntities.Ceremony(25));
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(termCode.Ceremonies);
            Assert.AreEqual(2, termCode.Ceremonies.Count);
            Assert.IsFalse(termCode.IsTransient());
            Assert.IsTrue(termCode.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Ceremonies Tests

        #region Constructor Tests

        /// <summary>
        /// Tests the constructor with no parameters populates expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithNoParametersPopulatesExpectedValues()
        {
            #region Arrange
            var record = new TermCode();            
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsActive);
            Assert.IsNotNull(record.Ceremonies);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the constructor with parameters populates expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithParametersPopulatesExpectedValues()
        {
            #region Arrange
            var vTermCode = CreateValidEntities.vTermCode(9);
            vTermCode.SetIdTo("9");
            var record = new TermCode(vTermCode);
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsActive);
            Assert.IsNotNull(record.Ceremonies);
            Assert.AreEqual("Description9", record.Name);
            Assert.AreEqual("9", record.Id);
            #endregion Assert
        }
        #endregion Constructor Tests

        #region Cascade Tests
        /// <summary>
        /// Tests the ceremonies is not cascaded when record is deleted.
        /// </summary>
        [TestMethod, Ignore]
        public void TestCeremoniesIsNotCascadedWhenRecordIsDeleted()
        {
            #region Arrange
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            LoadCeremony(5);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            var termCode = GetValid(9);
            termCode.Ceremonies = new List<Ceremony>();
            termCode.Ceremonies.Add(Repository.OfType<Ceremony>().GetById(2));
            termCode.Ceremonies.Add(Repository.OfType<Ceremony>().GetById(4));
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            var ceremonyCount = Repository.OfType<Ceremony>().GetAll().Count;
            var termCodeCount = TermCodeRepository.GetAll().Count;
            #endregion Arrange

            #region Act
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.Remove(termCode);
            TermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(ceremonyCount, Repository.OfType<Ceremony>().GetAll().Count);
            Assert.AreEqual(termCodeCount - 1, TermCodeRepository.GetAll().Count);
            #endregion Assert
        }

        /// <summary>
        /// Tests the ceremonies is populated on read with existing related ceremony records.
        /// </summary>
        [TestMethod, Ignore]
        public void TestCeremoniesIsPopulatedOnReadWithExistingRelatedCeremonyRecords()
        {
            #region Arrange
            var termCode = GetValid(9);
            TermCodeRepository.DbContext.BeginTransaction();
            TermCodeRepository.EnsurePersistent(termCode, true);
            TermCodeRepository.DbContext.CommitTransaction();
            var termCodeId = termCode.Id;

            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            LoadCeremony(5);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            var ceremony = Repository.OfType<Ceremony>().GetById(2);
            ceremony.TermCode = termCode;
            Repository.OfType<Ceremony>().EnsurePersistent(ceremony);
            ceremony = Repository.OfType<Ceremony>().GetById(3);
            ceremony.TermCode = termCode;
            Repository.OfType<Ceremony>().EnsurePersistent(ceremony);
            ceremony = Repository.OfType<Ceremony>().GetById(5);
            ceremony.TermCode = termCode;
            Repository.OfType<Ceremony>().EnsurePersistent(ceremony);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            Console.WriteLine("Exiting Arrange...");
            #endregion Arrange
            
            #region Act
            NHibernateSessionManager.Instance.GetSession().Evict(termCode);
            var record = TermCodeRepository.GetById(termCodeId);
            #endregion Act

            #region Assert
            Console.WriteLine("TermCodeId = " + record.Id);
            var foundCer = Repository.OfType<Ceremony>().Queryable.Where(a => a.TermCode == record).ToList();
            Console.WriteLine(foundCer.Count);
            Assert.AreEqual(3, record.Ceremonies.Count);
            #endregion Assert		
        }

        #endregion Cascade Tests

        #region Reflection of Database.

        /// <summary>
        /// Tests all fields in the database have been tested.
        /// If this fails and no other tests, it means that a field has been added which has not been tested above.
        /// </summary>
        [TestMethod, Ignore]
        public void TestAllFieldsInTheDatabaseHaveBeenTested()
        {
            #region Arrange
            var expectedFields = new List<NameAndType>();
            expectedFields.Add(new NameAndType("Ceremonies", "System.Collections.Generic.IList`1[Commencement.Core.Domain.Ceremony]", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.String", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IsActive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(TermCode));

        }

        #endregion Reflection of Database.	
		
		
    }
}