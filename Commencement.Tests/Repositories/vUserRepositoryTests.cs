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
    /// Entity Name:		vUser
    /// LookupFieldName:	LoginId
    /// </summary>
    [TestClass]
// ReSharper disable InconsistentNaming
    public class vUserRepositoryTests : AbstractRepositoryTests<vUser, int, vUserMap>

    {
        /// <summary>
        /// Gets or sets the vUser repository.
        /// </summary>
        /// <value>The vUser repository.</value>
        public IRepository<vUser> vUserRepository { get; set; }
// ReSharper restore InconsistentNaming		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="vUserRepositoryTests"/> class.
        /// </summary>
        public vUserRepositoryTests()
        {
            vUserRepository = new Repository<vUser>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override vUser GetValid(int? counter)
        {
            return CreateValidEntities.vUser(counter);
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<vUser> GetQuery(int numberAtEnd)
        {
            return vUserRepository.Queryable.Where(a => a.LoginId.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(vUser entity, int counter)
        {
            Assert.AreEqual("LoginId" + counter, entity.LoginId);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(vUser entity, ARTAction action)
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
                Assert.AreEqual("Attempted to delete an object of immutable class: [Commencement.Core.Domain.vUser]", ex.Message);
                throw;
            }
        }

        public override void CanUpdateEntity()
        {
            CanUpdateEntity(false);
        }


        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            vUserRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            vUserRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	
        
        #region LoginId Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LoginId with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithNullValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LoginId = null;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithEmptyStringSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LoginId = string.Empty;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with one space saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithOneSpaceSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LoginId = " ";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithOneCharacterSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LoginId = "x";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LoginId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginIdWithLongValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LoginId = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, vUser.LoginId.Length);
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion LoginId Tests

        #region FirstName Tests

        #region Valid Tests

        /// <summary>
        /// Tests the FirstName with null value saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithNullValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.FirstName = null;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithEmptyStringSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.FirstName = string.Empty;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with one space saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithOneSpaceSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.FirstName = " ";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithOneCharacterSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.FirstName = "x";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithLongValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.FirstName = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, vUser.FirstName.Length);
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion FirstName Tests
 
        #region LastName Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LastName with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithNullValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LastName = null;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithEmptyStringSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LastName = string.Empty;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with one space saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithOneSpaceSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LastName = " ";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithOneCharacterSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LastName = "x";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithLongValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.LastName = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, vUser.LastName.Length);
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion LastName Tests

        #region Email Tests
        #region Valid Tests

        /// <summary>
        /// Tests the Email with null value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithNullValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.Email = null;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithEmptyStringSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.Email = string.Empty;
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with one space saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithOneSpaceSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.Email = " ";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with one character saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithOneCharacterSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.Email = "x";
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with long value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithLongValueSaves()
        {
            #region Arrange
            var vUser = GetValid(9);
            vUser.Email = "x".RepeatTimes(999);
            #endregion Arrange

            #region Act
            vUserRepository.DbContext.BeginTransaction();
            vUserRepository.EnsurePersistent(vUser);
            vUserRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(999, vUser.Email.Length);
            Assert.IsFalse(vUser.IsTransient());
            Assert.IsTrue(vUser.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Email Tests

        #region FullName Tests

        [TestMethod]
        public void TestFullNameReturnsExpectedResult()
        {
            #region Arrange
            var record = CreateValidEntities.vUser(1);
            #endregion Arrange

            #region Assert
            Assert.AreEqual("FirstName1 LastName1", record.FullName);
            #endregion Assert		
        }
        #endregion FullName Tests

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapvUser()
        {
            #region Arrange
            var id = vUserRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<vUser>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.LoginId, "Login")
                .CheckProperty(c => c.FirstName, "FirstName")
                .CheckProperty(c => c.LastName, "LastName")
                .CheckProperty(c => c.Email, "Email")
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
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("FirstName", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("FullName", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("LastName", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("LoginId", "System.String", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(vUser));

        }

        #endregion Reflection of Database.	
		
		
    }
}