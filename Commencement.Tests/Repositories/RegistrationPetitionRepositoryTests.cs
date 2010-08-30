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
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    [TestClass]
    public class RegistrationPetitionRepositoryTests : AbstractRepositoryTests<RegistrationPetition, int>
    {
        /// <summary>
        /// Gets or sets the RegistrationPetition repository.
        /// </summary>
        /// <value>The RegistrationPetition repository.</value>
        public IRepository<RegistrationPetition> RegistrationPetitionRepository { get; set; }
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string> TermCodeRepository { get; set; }

        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationPetitionRepositoryTests"/> class.
        /// </summary>
        public RegistrationPetitionRepositoryTests()
        {
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            RegistrationPetitionRepository = new Repository<RegistrationPetition>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override RegistrationPetition GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.RegistrationPetition(counter);
            rtValue.MajorCode = MajorCodeRepository.GetById("1");
            rtValue.TermCode = TermCodeRepository.GetById("1");
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<RegistrationPetition> GetQuery(int numberAtEnd)
        {
            return RegistrationPetitionRepository.Queryable.Where(a => a.LastName.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(RegistrationPetition entity, int counter)
        {
            Assert.AreEqual("LastName" + counter, entity.LastName);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(RegistrationPetition entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.LastName);
                    break;
                case ARTAction.Restore:
                    entity.LastName = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.LastName;
                    entity.LastName = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            LoadMajorCode(3);
            LoadTermCode(3);
            LoadRecords(5);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region Pidm Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Pidm with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Pidm with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Pidm with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Pidm with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestPidmWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Pidm = "x".RepeatTimes((8 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(8 + 1, registrationPetition.Pidm.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: length must be between 0 and 8");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Pidm with one character saves.
        /// </summary>
        [TestMethod]
        public void TestPidmWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Pidm = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Pidm with long value saves.
        /// </summary>
        [TestMethod]
        public void TestPidmWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Pidm = "x".RepeatTimes(8);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(8, registrationPetition.Pidm.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Pidm Tests

        #region StudentId Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the StudentId with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestStudentIdWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.StudentId = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the StudentId with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestStudentIdWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.StudentId = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the StudentId with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestStudentIdWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.StudentId = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the StudentId with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestStudentIdWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.StudentId = "x".RepeatTimes((9 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(9 + 1, registrationPetition.StudentId.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: length must be between 0 and 9");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the StudentId with one character saves.
        /// </summary>
        [TestMethod]
        public void TestStudentIdWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.StudentId = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the StudentId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestStudentIdWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.StudentId = "x".RepeatTimes(9);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(9, registrationPetition.StudentId.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion StudentId Tests

        #region FirstName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the FirstName with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFirstNameWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.FirstName = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FirstName: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FirstName with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFirstNameWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.FirstName = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FirstName: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FirstName with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFirstNameWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.FirstName = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FirstName: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the FirstName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFirstNameWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.FirstName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(50 + 1, registrationPetition.FirstName.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FirstName: length must be between 0 and 50");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the FirstName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.FirstName = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.FirstName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, registrationPetition.FirstName.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion FirstName Tests

        #region LastName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the LastName with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLastNameWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.LastName = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LastName: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LastName with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLastNameWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.LastName = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LastName: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LastName with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLastNameWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.LastName = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LastName: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the LastName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLastNameWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.LastName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(50 + 1, registrationPetition.LastName.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LastName: length must be between 0 and 50");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LastName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.LastName = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.LastName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, registrationPetition.LastName.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion LastName Tests

        #region Email Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Email with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Email = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Email with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Email = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Email with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Email = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Email with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Email = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(50 + 1, registrationPetition.Email.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: length must be between 0 and 50");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Email with one character saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Email = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with long value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Email = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, registrationPetition.Email.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Email Tests
 
        #region Login Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Login with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Login = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Login: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Login with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Login = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Login: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Login with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Login = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Login: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Login with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.Login = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(50 + 1, registrationPetition.Login.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Login: length must be between 0 and 50");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Login with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLoginWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Login = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Login with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.Login = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, registrationPetition.Login.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Login Tests

        #region MajorCode Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the MajorCode with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMajorCodeWithAValueOfNullNDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.MajorCode = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(registrationPetition.MajorCode, null);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("MajorCode: may not be empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the update to use A different major saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentMajorSaves()
        {
            #region Arrange
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.AreNotSame(registrationPetition.MajorCode, MajorCodeRepository.GetById("2"));
            registrationPetition.MajorCode = MajorCodeRepository.GetById("2");
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registrationPetition.MajorCode, MajorCodeRepository.GetById("2"));
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion MajorCode Tests
        
        #region Units Tests

        /// <summary>
        /// Tests the Units with max decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestUnitsWithMaxDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Units = decimal.MaxValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MaxValue, record.Units);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Units with min decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestUnitsWithMinDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Units = decimal.MinValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MinValue, record.Units);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the units with value of zero saves.
        /// </summary>
        [TestMethod]
        public void TestUnitsWithValueOfZeroSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Units = 0m;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0m, record.Units);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Units Tests

        #region ExceptionReason Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the ExceptionReason with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ExceptionReason with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ExceptionReason with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the ExceptionReason with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestExceptionReasonWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.ExceptionReason = "x".RepeatTimes((1000 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(1000 + 1, registrationPetition.ExceptionReason.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("ExceptionReason: length must be between 0 and 1000");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ExceptionReason with one character saves.
        /// </summary>
        [TestMethod]
        public void TestExceptionReasonWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.ExceptionReason = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the ExceptionReason with long value saves.
        /// </summary>
        [TestMethod]
        public void TestExceptionReasonWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.ExceptionReason = "x".RepeatTimes(1000);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1000, registrationPetition.ExceptionReason.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion ExceptionReason Tests

        #region CompletionTerm Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the CompletionTerm with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithNullValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.CompletionTerm = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: may not be null or empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the CompletionTerm with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithEmptyStringDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.CompletionTerm = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: may not be null or empty", "CompletionTerm: length must be between 6 and 6");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the CompletionTerm with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithSpacesOnlyDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.CompletionTerm = " ";
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: may not be null or empty", "CompletionTerm: length must be between 6 and 6");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the CompletionTerm with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.CompletionTerm = "x".RepeatTimes((6 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(6 + 1, registrationPetition.CompletionTerm.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: length must be between 6 and 6");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        /// <summary>
        /// Tests the completion term with too short value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCompletionTermWithTooShortValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.CompletionTerm = "x".RepeatTimes((6 - 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(6 - 1, registrationPetition.CompletionTerm.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("CompletionTerm: length must be between 6 and 6");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the CompletionTerm with long value saves.
        /// </summary>
        [TestMethod]
        public void TestCompletionTermWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.CompletionTerm = "x".RepeatTimes(6);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(6, registrationPetition.CompletionTerm.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion CompletionTerm Tests

        #region TransferUnitsFrom Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the TransferUnitsFrom with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTransferUnitsFromWithTooLongValueDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.TransferUnitsFrom = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(100 + 1, registrationPetition.TransferUnitsFrom.Length);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TransferUnitsFrom: length must be between 0 and 100");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the TransferUnitsFrom with null value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithNullValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithEmptyStringSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = string.Empty;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with one space saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithOneSpaceSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = " ";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with one character saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithOneCharacterSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = "x";
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnitsFrom with long value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsFromWithLongValueSaves()
        {
            #region Arrange
            var registrationPetition = GetValid(9);
            registrationPetition.TransferUnitsFrom = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, registrationPetition.TransferUnitsFrom.Length);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion TransferUnitsFrom Tests

        #region TransferUnits Tests

        /// <summary>
        /// Tests the TransferUnits with null value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithNullValueSaves()
        {
            #region Arrange
            RegistrationPetition record = GetValid(9);
            record.TransferUnits = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(record.TransferUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert		
        }

        /// <summary>
        /// Tests the TransferUnits with max double value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithMaxDoubleValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TransferUnits = double.MaxValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(double.MaxValue, record.TransferUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the TransferUnits with min double value saves.
        /// </summary>
        [TestMethod]
        public void TestTransferUnitsWithMinDoubleValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.TransferUnits = double.MinValue;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(double.MinValue, record.TransferUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion TransferUnits Tests

        #region IsPending Tests

        /// <summary>
        /// Tests the IsPending is false saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsFalseSaves()
        {
            #region Arrange

            RegistrationPetition registrationPetition = GetValid(9);
            registrationPetition.IsPending = false;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationPetition.IsPending);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsPending is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsPendingIsTrueSaves()
        {
            #region Arrange

            var registrationPetition = GetValid(9);
            registrationPetition.IsPending = true;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationPetition.IsPending);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

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

            RegistrationPetition registrationPetition = GetValid(9);
            registrationPetition.IsApproved = false;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registrationPetition.IsApproved);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the IsApproved is true saves.
        /// </summary>
        [TestMethod]
        public void TestIsApprovedIsTrueSaves()
        {
            #region Arrange

            var registrationPetition = GetValid(9);
            registrationPetition.IsApproved = true;

            #endregion Arrange

            #region Act

            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registrationPetition.IsApproved);
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());

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
            RegistrationPetition record = GetValid(99);
            record.DateSubmitted = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
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
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
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
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
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
        /// Tests the date decision with null value will save.
        /// </summary>
        [TestMethod]
        public void TestDateDecisionWithNullValueWillSave()
        {
            #region Arrange
            RegistrationPetition record = GetValid(99);
            record.DateDecision = null;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
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
            RegistrationPetition record = GetValid(99);
            record.DateDecision = compareDate;
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
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
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
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
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(record);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateDecision);
            #endregion Assert
        }
        #endregion DateDecision Tests

        #region TermCode Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the TermCode with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTermCodeWithAValueOfNullNDoesNotSave()
        {
            RegistrationPetition registrationPetition = null;
            try
            {
                #region Arrange
                registrationPetition = GetValid(9);
                registrationPetition.TermCode = null;
                #endregion Arrange

                #region Act
                RegistrationPetitionRepository.DbContext.BeginTransaction();
                RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
                RegistrationPetitionRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registrationPetition);
                Assert.AreEqual(registrationPetition.TermCode, null);
                var results = registrationPetition.ValidationResults().AsMessageList();
                results.AssertErrorsAre("TermCode: may not be empty");
                Assert.IsTrue(registrationPetition.IsTransient());
                Assert.IsFalse(registrationPetition.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the update to use A different TermCode saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentTermCodeSaves()
        {
            #region Arrange
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.AreNotSame(registrationPetition.TermCode, TermCodeRepository.GetById("2"));
            registrationPetition.TermCode = TermCodeRepository.GetById("2");
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.EnsurePersistent(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registrationPetition.TermCode, TermCodeRepository.GetById("2"));
            Assert.IsFalse(registrationPetition.IsTransient());
            Assert.IsTrue(registrationPetition.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion TermCode Tests

        #region FullName Tests

        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult()
        {
            #region Arrange
            var record = CreateValidEntities.RegistrationPetition(99);          
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("FirstName99 LastName99", record.FullName);
            #endregion Assert		
        }
        #endregion FullName Tests

        #region SetDecission Tests

        /// <summary>
        /// Tests the set decision sets expected values.
        /// </summary>
        [TestMethod]
        public void TestSetDecisionSetsExpectedValues1()
        {
            #region Arrange
            var record = new RegistrationPetition();
            record.DateDecision = null;
            record.IsApproved = false;
            record.IsPending = true;
            #endregion Arrange

            #region Act
            record.SetDecision(true);
            #endregion Act

            #region Assert
            Assert.IsNotNull(record.DateDecision);
            var compareDate = (DateTime)record.DateDecision;
            Assert.AreEqual(DateTime.Now.Date, compareDate.Date);
            Assert.IsTrue(record.IsApproved);
            Assert.IsFalse(record.IsPending);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the set decision sets expected values.
        /// </summary>
        [TestMethod]
        public void TestSetDecisionSetsExpectedValues2()
        {
            #region Arrange
            var record = new RegistrationPetition();
            record.DateDecision = null;
            record.IsApproved = false;
            record.IsPending = true;
            #endregion Arrange

            #region Act
            record.SetDecision(false);
            #endregion Act

            #region Assert
            Assert.IsNotNull(record.DateDecision);
            var compareDate = (DateTime)record.DateDecision;
            Assert.AreEqual(DateTime.Now.Date, compareDate.Date);
            Assert.IsFalse(record.IsApproved);
            Assert.IsFalse(record.IsPending);
            #endregion Assert
        }
        #endregion SetDecission Tests

        #region Constructor Tests

        /// <summary>
        /// Tests the constructor sets expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorSetsExpectedValues()
        {
            #region Arrange
            var record = new RegistrationPetition();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsTrue(record.IsPending);
            Assert.IsFalse(record.IsApproved);
            Assert.AreEqual(DateTime.Now.Date, record.DateSubmitted.Date);
            Assert.IsNull(record.DateDecision);
            #endregion Assert		
        }

        #endregion Constructor Tests

        #region Cascade Tests
        /// <summary>
        /// Tests the delete registration petition does not delete major code.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationPetitionDoesNotDeleteMajorCode()
        {
            #region Arrange
            var majorCode = MajorCodeRepository.GetById("1");
            var registrationPetitionCount = RegistrationPetitionRepository.GetAll().Count;
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.AreSame(registrationPetition.MajorCode, majorCode);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationPetitionCount - 1, RegistrationPetitionRepository.GetAll().Count);
            var majorCodeCompare = MajorCodeRepository.GetById("1");
            Assert.AreSame(majorCode, majorCodeCompare);
            #endregion Assert
        }

        /// <summary>
        /// Tests the delete registration petition does not delete term code.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationPetitionDoesNotDeleteTermCode()
        {
            #region Arrange
            var termCodeCount = TermCodeRepository.GetAll().Count;
            var registrationPetitionCount = RegistrationPetitionRepository.GetAll().Count;
            var registrationPetition = RegistrationPetitionRepository.GetById(1);
            Assert.IsNotNull(registrationPetition.TermCode);
            #endregion Arrange

            #region Act
            RegistrationPetitionRepository.DbContext.BeginTransaction();
            RegistrationPetitionRepository.Remove(registrationPetition);
            RegistrationPetitionRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationPetitionCount - 1, RegistrationPetitionRepository.GetAll().Count);
            Assert.AreEqual(termCodeCount, TermCodeRepository.GetAll().Count);
            #endregion Assert
        }
        #endregion Cascade Tests

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
            expectedFields.Add(new NameAndType("CompletionTerm", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)6, (Int32)6)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("DateDecision", "System.Nullable`1[System.DateTime]", new List<string>()));
            expectedFields.Add(new NameAndType("DateSubmitted", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ExceptionReason", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)1000)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("FirstName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("FullName", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("IsApproved", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("IsPending", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("LastName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Login", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("MajorCode", "Commencement.Core.Domain.MajorCode", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Pidm", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)8)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("StudentId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)9)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TermCode", "Commencement.Core.Domain.TermCode", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TransferUnits", "System.Nullable`1[System.Double]", new List<string>()));
            expectedFields.Add(new NameAndType("TransferUnitsFrom", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
            }));
            expectedFields.Add(new NameAndType("Units", "System.Decimal", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(RegistrationPetition));

        }

        #endregion Reflection of Database.	
		
		
    }
}