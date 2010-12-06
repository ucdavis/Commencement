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
using UCDArch.Testing;

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		MajorCode
    /// LookupFieldName:	Name
    /// </summary>
    [TestClass]
    public class MajorCodeRepositoryTests : AbstractRepositoryTests<MajorCode, string, MajorCodeMap>
    {
        /// <summary>
        /// Gets or sets the MajorCode repository.
        /// </summary>
        /// <value>The MajorCode repository.</value>
        public IRepositoryWithTypedId<MajorCode, string > MajorCodeRepository { get; set; }
        public IRepositoryWithTypedId<College, string> CollegeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="MajorCodeRepositoryTests"/> class.
        /// </summary>
        public MajorCodeRepositoryTests()
        {
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            CollegeRepository = new RepositoryWithTypedId<College, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override MajorCode GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.MajorCode(counter);
            var notNullCounter = "99";
            if (counter != null)
            {
                notNullCounter = ((int) counter).ToString();
            }
            rtValue.SetIdTo(notNullCounter); 
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<MajorCode> GetQuery(int numberAtEnd)
        {
            return MajorCodeRepository.Queryable.Where(a => a.Name.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(MajorCode entity, int counter)
        {
            Assert.AreEqual("Name" + counter, entity.Name);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(MajorCode entity, ARTAction action)
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
            MajorCodeRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            MajorCodeRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapAttachment()
        {
            #region Arrange
            var session = NHibernateSessionManager.Instance.GetSession();
            LoadColleges(3);
            var college = CollegeRepository.GetById("2");
            Assert.IsNotNull(college);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<MajorCode>(session, new MajorCodeEqualityComparer())
                .CheckProperty(c => c.Id, "APRF")
                .CheckProperty(c => c.College, college)
                .CheckProperty(c => c.DisciplineCode, "ENVSC")
                .CheckProperty(c => c.Name, "Pre Forestry (C.W.0.) Program")                
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class MajorCodeEqualityComparer : IEqualityComparer
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param><exception cref="T:System.ArgumentException"><paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other.</exception>
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x is College && y is College)
                {
                    if (((College)x).Name == ((College)y).Name)
                    {
                        return true;
                    }
                    return false;
                }

                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }

        #endregion Fluent Mapping Tests

        #region Name Tests


        #region Valid Tests

        /// <summary>
        /// Tests the Name with null value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithNullValueSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.Name = null;
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithEmptyStringSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.Name = string.Empty;
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one space saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneSpaceSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.Name = " ";
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with one character saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithOneCharacterSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.Name = "x";
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Name with long value saves.
        /// </summary>
        [TestMethod]
        public void TestNameWithLongValueSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.Name = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, majorCode.Name.Length);
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Name Tests
     
        #region DisciplineCode Tests

        #region Valid Tests

        /// <summary>
        /// Tests the DisciplineCode with null value saves.
        /// </summary>
        [TestMethod]
        public void TestDisciplineCodeWithNullValueSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.DisciplineCode = null;
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the DisciplineCode with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestDisciplineCodeWithEmptyStringSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.DisciplineCode = string.Empty;
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the DisciplineCode with one space saves.
        /// </summary>
        [TestMethod]
        public void TestDisciplineCodeWithOneSpaceSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.DisciplineCode = " ";
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the DisciplineCode with one character saves.
        /// </summary>
        [TestMethod]
        public void TestDisciplineCodeWithOneCharacterSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.DisciplineCode = "x";
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the DisciplineCode with long value saves.
        /// </summary>
        [TestMethod]
        public void TestDisciplineCodeWithLongValueSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.DisciplineCode = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, majorCode.DisciplineCode.Length);
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion DisciplineCode Tests

        #region College Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCollegeWithNewValueDoesNotSave()
        {
            MajorCode majorCode;
            try
            {
                #region Arrange
                LoadColleges(3);
                majorCode = GetValid(9);
                majorCode.College = CreateValidEntities.College(99);
                #endregion Arrange

                #region Act
                MajorCodeRepository.DbContext.BeginTransaction();
                MajorCodeRepository.EnsurePersistent(majorCode);
                MajorCodeRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            { 
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.College, Entity: Commencement.Core.Domain.College", ex.Message);
                throw;
                #endregion Assert
            }
        }
        #endregion Invalid Tests

        #region Valid Tests
                
        [TestMethod]
        public void TestCollegeWithNullValueSaves()
        {
            #region Arrange
            var majorCode = GetValid(9);
            majorCode.College = null;
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(majorCode.College);
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert		
        }

        [TestMethod]
        public void TestCollegeWithExistingValueSaves()
        {
            #region Arrange
            LoadColleges(3);
            var majorCode = GetValid(9);
            majorCode.College = CollegeRepository.GetById("2");
            #endregion Arrange

            #region Act
            MajorCodeRepository.DbContext.BeginTransaction();
            MajorCodeRepository.EnsurePersistent(majorCode);
            MajorCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(majorCode.College);
            Assert.AreEqual("Name2", majorCode.College.Name);
            Assert.IsFalse(majorCode.IsTransient());
            Assert.IsTrue(majorCode.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion College Tests

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
            expectedFields.Add(new NameAndType("College", "Commencement.Core.Domain.College", new List<string>()));
            expectedFields.Add(new NameAndType("DisciplineCode", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.String", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(MajorCode));

        }

        #endregion Reflection of Database.	
		
		
    }
}