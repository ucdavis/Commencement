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
    /// Entity Name:		vTermCode
    /// LookupFieldName:	Description
    /// </summary>
    [TestClass]
    public class vTermCodeRepositoryTests : AbstractRepositoryTests<vTermCode, string , vTermCodeMap>
    {
        /// <summary>
        /// Gets or sets the vTermCode repository.
        /// </summary>
        /// <value>The vTermCode repository.</value>
        public IRepositoryWithTypedId<vTermCode, string > vTermCodeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="vTermCodeRepositoryTests"/> class.
        /// </summary>
        public vTermCodeRepositoryTests()
        {
            vTermCodeRepository = new RepositoryWithTypedId<vTermCode, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override vTermCode GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.vTermCode(counter);
            var localCounter = "";
            if(counter!=null)
            {
                localCounter = counter.ToString();
            }
            rtValue.SetIdTo(localCounter);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<vTermCode> GetQuery(int numberAtEnd)
        {
            return vTermCodeRepository.Queryable.Where(a => a.Description.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(vTermCode entity, int counter)
        {
            Assert.AreEqual("Description" + counter, entity.Description);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(vTermCode entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Description);
                    break;
                case ARTAction.Restore:
                    entity.Description = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Description;
                    entity.Description = updateValue;
                    break;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NHibernate.ADOException))]
        public override void CanDeleteEntity()
        {
            try
            {
                base.CanDeleteEntity();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                //Assert.AreEqual("could not execute query\r\n[ SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03')) ]\r\n[SQL: SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03'))]", ex.Message);
                throw;
            }
            
        }

        /// <summary>
        /// Determines whether this instance [can get all entities].
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.ADOException))]
        public override void CanGetAllEntities()
        {
            try
            {
                base.CanGetAllEntities();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                //Assert.AreEqual("could not execute query\r\n[ SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03')) ]\r\n[SQL: SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03'))]", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Determines whether this instance [can query entities].
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.ADOException))]
        public override void CanQueryEntities()
        {
            try
            {
                base.CanQueryEntities();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                //Assert.AreEqual("could not execute query\r\n[ SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03')) AND this_.Description like @p0 ]\r\nPositional parameters:  #0>%3\r\n[SQL: SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03')) AND this_.Description like @p0]", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Determines whether this instance [can update entity].
        /// Defaults to true unless overridden
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.ADOException))]
        public override void CanUpdateEntity()
        {
            try
            {
                base.CanUpdateEntity();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                //Assert.AreEqual("could not execute query\r\n[ SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03')) ]\r\n[SQL: SELECT this_.id as id31_0_, this_.Description as Descript2_31_0_, this_.StartDate as StartDate31_0_, this_.EndDate as EndDate31_0_ FROM vTermCodes this_ WHERE ( this_.TypeCode='Q' and (this_.id like '%10' or this_.id like '%03'))]", ex.Message);
                throw;
            }
            
        }

        /// <summary>
        /// Determines whether this instance [can get null value using get by nullable with invalid id where id is int].
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.Exceptions.GenericADOException))]
        public override void CanGetNullValueUsingGetByNullableWithInvalidId()
        {
            try
            {
                base.CanGetNullValueUsingGetByNullableWithInvalidId();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                //Assert.AreEqual("could not load an entity: [Commencement.Core.Domain.vTermCode#6][SQL: SELECT vtermcode0_.id as id31_0_, vtermcode0_.Description as Descript2_31_0_, vtermcode0_.StartDate as StartDate31_0_, vtermcode0_.EndDate as EndDate31_0_ FROM vTermCodes vtermcode0_ WHERE vtermcode0_.id=? and ( vtermcode0_.TypeCode='Q' and (vtermcode0_.id like '%10' or vtermcode0_.id like '%03')) ]", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            vTermCodeRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            vTermCodeRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region Description Tests
      

        #region Valid Tests

        /// <summary>
        /// Tests the Description with null value saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithNullValueSaves()
        {
            #region Arrange
            var vTermCode = GetValid(9);
            vTermCode.Description = null;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(vTermCode);
            vTermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vTermCode.IsTransient());
            Assert.IsTrue(vTermCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithEmptyStringSaves()
        {
            #region Arrange
            var vTermCode = GetValid(9);
            vTermCode.Description = string.Empty;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(vTermCode);
            vTermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vTermCode.IsTransient());
            Assert.IsTrue(vTermCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with one space saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithOneSpaceSaves()
        {
            #region Arrange
            var vTermCode = GetValid(9);
            vTermCode.Description = " ";
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(vTermCode);
            vTermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vTermCode.IsTransient());
            Assert.IsTrue(vTermCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with one character saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithOneCharacterSaves()
        {
            #region Arrange
            var vTermCode = GetValid(9);
            vTermCode.Description = "x";
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(vTermCode);
            vTermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vTermCode.IsTransient());
            Assert.IsTrue(vTermCode.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Description with long value saves.
        /// </summary>
        [TestMethod]
        public void TestDescriptionWithLongValueSaves()
        {
            #region Arrange
            var vTermCode = GetValid(9);
            vTermCode.Description = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(vTermCode);
            vTermCodeRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, vTermCode.Description.Length);
            Assert.IsFalse(vTermCode.IsTransient());
            Assert.IsTrue(vTermCode.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Description Tests
  
        #region StartDate Tests

        /// <summary>
        /// Tests the StartDate with past date will save.
        /// </summary>
        [TestMethod]
        public void TestStartDateWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            vTermCode record = GetValid(99);
            record.StartDate = compareDate;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(record);
            vTermCodeRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.StartDate);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the StartDate with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestStartDateWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.StartDate = compareDate;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(record);
            vTermCodeRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.StartDate);
            #endregion Assert
        }

        /// <summary>
        /// Tests the StartDate with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestStartDateWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.StartDate = compareDate;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(record);
            vTermCodeRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.StartDate);
            #endregion Assert
        }
        #endregion StartDate Tests
             
        
        #region EndDate Tests

        /// <summary>
        /// Tests the EndDate with past date will save.
        /// </summary>
        [TestMethod]
        public void TestEndDateWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            vTermCode record = GetValid(99);
            record.EndDate = compareDate;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(record);
            vTermCodeRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.EndDate);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the EndDate with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestEndDateWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.EndDate = compareDate;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(record);
            vTermCodeRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.EndDate);
            #endregion Assert
        }

        /// <summary>
        /// Tests the EndDate with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestEndDateWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.EndDate = compareDate;
            #endregion Arrange

            #region Act
            vTermCodeRepository.DbContext.BeginTransaction();
            vTermCodeRepository.EnsurePersistent(record);
            vTermCodeRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.EndDate);
            #endregion Assert
        }
        #endregion EndDate Tests
       
        
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
            expectedFields.Add(new NameAndType("Description", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("EndDate", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.String", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("StartDate", "System.DateTime", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(vTermCode));

        }

        #endregion Reflection of Database.	
		
		
    }
}