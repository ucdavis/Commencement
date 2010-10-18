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
    /// Entity Name:		College
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class CollegeRepositoryTests : AbstractRepositoryTests<College, string, CollegeMap>
    {
        /// <summary>
        /// Gets or sets the College repository.
        /// </summary>
        /// <value>The College repository.</value>
        public IRepository<College> CollegeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="CollegeRepositoryTests"/> class.
        /// </summary>
        public CollegeRepositoryTests()
        {
            CollegeRepository = new Repository<College>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override College GetValid(int? counter)
        {
            var rtResult = CreateValidEntities.College(counter);
            var count = "";
            if(counter != null)
            {
                count = ((int) counter).ToString();
            }
            rtResult.SetIdTo(count);
            return rtResult;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<College> GetQuery(int numberAtEnd)
        {
            return CollegeRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(College entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(College entity, ARTAction action)
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

        [TestMethod]
        [ExpectedException(typeof(NHibernate.HibernateException))]
        public override void CanDeleteEntity()
        {
            try
            {
                base.CanDeleteEntity();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("Attempted to delete an object of immutable class: [Commencement.Core.Domain.College]", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public override void CanUpdateEntity()
        {
            CanUpdateEntity(false);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            CollegeRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            CollegeRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Name Tests


        #region Valid Tests

        /// <summary>
        /// Tests the Name with null value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithNullValueSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = null;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithEmptyStringSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = string.Empty;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one space saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneSpaceSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = " ";
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = "x";
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Name = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, college.Name.Length);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests

        #region Majors Tests

        [TestMethod]
        public void TesMajorsWithNullValueSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Majors = null;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(college.Majors);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestMajorsWithEmptyListSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(college.Majors);
            Assert.AreEqual(0, college.Majors.Count);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }

        [TestMethod]
        public void TestEditorsWithPopulatedListSaves()
        {
            #region Arrange
            var college = GetValid(9);
            college.Majors = new List<MajorCode>();
            college.Majors.Add(CreateValidEntities.MajorCode(1));
            //college.Editors[0].Ceremony = ceremony;
            college.Majors.Add(CreateValidEntities.MajorCode(2));
            //college.Editors[1].Ceremony = ceremony;
            #endregion Arrange

            #region Act
            CollegeRepository.DbContext.BeginTransaction();
            CollegeRepository.EnsurePersistent(college);
            CollegeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(college.Majors);
            Assert.AreEqual(2, college.Majors.Count);
            Assert.IsFalse(college.IsTransient());
            Assert.IsTrue(college.IsValid());
            #endregion Assert
        }


        #endregion Majors Tests
        
        
        
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

            expectedFields.Add(new NameAndType("Id", "System.String", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Majors", "System.Collections.Generic.IList`1[Commencement.Core.Domain.MajorCode]", new List<string>()));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(College));

        }

        #endregion Reflection of Database.	
		
		
    }
}