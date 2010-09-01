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
    /// Entity Name:		Registration
    /// LookupFieldName:	Address1
    /// </summary>
    [TestClass]
    public class RegistrationRepositoryTests : AbstractRepositoryTests<Registration, int>
    {
        /// <summary>
        /// Gets or sets the Registration repository.
        /// </summary>
        /// <value>The Registration repository.</value>
        public IRepository<Registration> RegistrationRepository { get; set; }
        public IRepositoryWithTypedId<State, string> StateRepository { get; set; }
        public IRepositoryWithTypedId<Student, Guid> StudentRepository { get; set; }
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationRepositoryTests"/> class.
        /// </summary>
        public RegistrationRepositoryTests()
        {
            RegistrationRepository = new Repository<Registration>();
            StateRepository = new RepositoryWithTypedId<State, string>();
            StudentRepository = new RepositoryWithTypedId<Student, Guid>();
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Registration GetValid(int? counter)
        {
            var rtValue = CreateValidEntities.Registration(counter);
            rtValue.State = StateRepository.GetById("1");
            rtValue.Student = StudentRepository.Queryable.Where(a => a.Pidm == "Pidm1").Single();
            rtValue.Major = MajorCodeRepository.GetById("1");
            rtValue.Ceremony = Repository.OfType<Ceremony>().GetById(1);
            return rtValue;
        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Registration> GetQuery(int numberAtEnd)
        {
            return RegistrationRepository.Queryable.Where(a => a.Address1.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Registration entity, int counter)
        {
            Assert.AreEqual("Address1" + counter, entity.Address1);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Registration entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.Address1);
                    break;
                case ARTAction.Restore:
                    entity.Address1 = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.Address1;
                    entity.Address1 = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            RegistrationRepository.DbContext.BeginTransaction();
            LoadCeremony(3);
            LoadMajorCode(3);
            LoadState(3);
            LoadStudent(3);
            LoadRecords(5);
            RegistrationRepository.DbContext.CommitTransaction();
        }

        #endregion Init and Overrides	

        #region Student Tests
        #region Invalid Tests
        /// <summary>
        /// Tests the Student with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestStudentWithAValueOfNullDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Student = null;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.IsNull(registration.Student);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Student: may not be empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }	
        }

        #endregion Invalid Tests
        #region Valid Tests

        /// <summary>
        /// Tests the student with A new value saves.
        /// </summary>
        [TestMethod]
        public void TestStudentWithANewValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Student = new Student("pidm", "123456789", "First", "Midde", "last", 1.10m, "test@ucdavis.edu", "login", new TermCode());
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual("pidm", registration.Student.Pidm);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert		
        }


        /// <summary>
        /// Tests the update to use A different student saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentStudentSaves()
        {
            #region Arrange
            var registration = RegistrationRepository.GetById(1);
            Assert.AreNotSame(registration.Student, StudentRepository.Queryable.Where(a => a.Pidm == "Pidm2").Single());
            registration.Student = StudentRepository.Queryable.Where(a => a.Pidm == "Pidm2").Single();
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registration.Student, StudentRepository.Queryable.Where(a => a.Pidm == "Pidm2").Single());
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert		
        }
        #endregion Valid Tests
        #endregion Student Tests

        #region Major Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Major with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMajorWithAValueOfnullNDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Major = null;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(registration.Major, null);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Major: may not be empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
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
            var registration = RegistrationRepository.GetById(1);
            Assert.AreNotSame(registration.Major, MajorCodeRepository.GetById("2"));
            registration.Major = MajorCodeRepository.GetById("2");
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registration.Major, MajorCodeRepository.GetById("2"));
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert			
        }
        #endregion Valid Tests
        #endregion Major Tests

        #region Address1 Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Address1 with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddress1WithNullValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Address1 = null;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Address1: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Address1 with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddress1WithEmptyStringDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Address1 = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Address1: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Address1 with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddress1WithSpacesOnlyDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Address1 = " ";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Address1: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Address1 with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddress1WithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Address1 = "x".RepeatTimes((200 + 1));
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(200 + 1, registration.Address1.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Address1: length must be between 0 and 200");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Address1 with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAddress1WithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address1 = "x";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address1 with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAddress1WithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address1 = "x".RepeatTimes(200);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(200, registration.Address1.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Address1 Tests
        
        #region Address2 Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Address2 with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddress2WithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Address2 = "x".RepeatTimes((200 + 1));
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(200 + 1, registration.Address2.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Address2: length must be between 0 and 200");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Address2 with null value saves.
        /// </summary>
        [TestMethod]
        public void TestAddress2WithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address2 = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address2 with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestAddress2WithEmptyStringSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address2 = string.Empty;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address2 with one space saves.
        /// </summary>
        [TestMethod]
        public void TestAddress2WithOneSpaceSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address2 = " ";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address2 with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAddress2WithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address2 = "x";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address2 with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAddress2WithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address2 = "x".RepeatTimes(200);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(200, registration.Address2.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Address2 Tests

        #region Address3 Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Address3 with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestAddress3WithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Address3 = "x".RepeatTimes((200 + 1));
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(200 + 1, registration.Address3.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Address3: length must be between 0 and 200");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Address3 with null value saves.
        /// </summary>
        [TestMethod]
        public void TestAddress3WithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address3 = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address3 with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestAddress3WithEmptyStringSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address3 = string.Empty;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address3 with one space saves.
        /// </summary>
        [TestMethod]
        public void TestAddress3WithOneSpaceSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address3 = " ";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address3 with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAddress3WithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address3 = "x";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Address3 with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAddress3WithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Address3 = "x".RepeatTimes(200);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(200, registration.Address3.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Address3 Tests

        #region City Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the City with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCityWithNullValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.City = null;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("City: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the City with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCityWithEmptyStringDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.City = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("City: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the City with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCityWithSpacesOnlyDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.City = " ";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("City: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the City with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCityWithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.City = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(100 + 1, registration.City.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("City: length must be between 0 and 100");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the City with one character saves.
        /// </summary>
        [TestMethod]
        public void TestCityWithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.City = "x";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the City with long value saves.
        /// </summary>
        [TestMethod]
        public void TestCityWithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.City = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, registration.City.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion City Tests

        #region State Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the State with A value of null does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestStateWithAValueOfNullDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.State = null;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(registration.State, null);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("State: may not be empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }	
        }
        #endregion Invalid Tests

        #region Valid Tests
        /// <summary>
        /// Tests the update to use A different state saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentStateSaves()
        {
            #region Arrange
            var registration = RegistrationRepository.GetById(1);
            Assert.AreNotSame(registration.State, StateRepository.GetById("2"));
            registration.State = StateRepository.GetById("2");
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registration.State, StateRepository.GetById("2"));
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion State Tests

        #region Zip Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Zip with null value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithNullValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = null;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Zip with empty string does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithEmptyStringDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = string.Empty;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Zip with spaces only does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithSpacesOnlyDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = " ";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: may not be null or empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Zip with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestZipWithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Zip = "x".RepeatTimes((15 + 1));
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(15 + 1, registration.Zip.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Zip: length must be between 0 and 15");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Zip with one character saves.
        /// </summary>
        [TestMethod]
        public void TestZipWithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Zip = "x";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Zip with long value saves.
        /// </summary>
        [TestMethod]
        public void TestZipWithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Zip = "x".RepeatTimes(15);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(15, registration.Zip.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Zip Tests
 
        #region Email Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Email with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(100 + 1, registration.Email.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: length must be between 0 and 100", 
                                        "Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with one character does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithOneCharacterDoesNotSaveSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "x";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave1()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.edu;test@test.edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave2()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.edu,test@test.edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave3()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.edu test@test.edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave4()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@te@st.edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave5()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@te..edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave6()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@test.";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave7()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test@.edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave8()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = "test.edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        /// <summary>
        /// Tests the email with poorly formed values does not save save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithPoorlyFormedValuesDoesNotSaveSave9()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Email = " @test.edu";
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: not a well-formed email address");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Email with null value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithEmptyStringSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email = string.Empty;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with long value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email =  "x".RepeatTimes(91) + "@test.edu";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, registration.Email.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the email with well formed value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithWellFormedValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Email = "john@test.edu";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert		
        }

        #endregion Valid Tests
        #endregion Email Tests

        #region NumberTickets Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the NumberTickets with A value of 0 does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestNumberTicketsWithAValueOf0DoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.NumberTickets = 0;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(registration.NumberTickets, 0);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("NumberTickets: must be greater than or equal to 1");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }	
        }
        #endregion Invalid Tests

        #region Valid Tests
        /// <summary>
        /// Tests the NumberTickets with max int value saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithMaxIntValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = int.MaxValue;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(int.MaxValue, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the number tickets with value of one saves.
        /// </summary>
        [TestMethod]
        public void TestNumberTicketsWithValueOfOneSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.NumberTickets = 1;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, record.NumberTickets);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion NumberTickets Tests

        #region MailTickets Tests

        /// <summary>
        /// Tests the MailTickets is false saves.
        /// </summary>
        [TestMethod]
        public void TestMailTicketsIsFalseSaves()
        {
            #region Arrange

            Registration registration = GetValid(9);
            registration.MailTickets = false;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(registration.MailTickets);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the MailTickets is true saves.
        /// </summary>
        [TestMethod]
        public void TestMailTicketsIsTrueSaves()
        {
            #region Arrange

            var registration = GetValid(9);
            registration.MailTickets = true;

            #endregion Arrange

            #region Act

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(registration.MailTickets);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());

            #endregion Assert
        }

        #endregion MailTickets Tests

        #region Comments Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Comments with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestCommentsWithTooLongValueDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Comments = "x".RepeatTimes((1000 + 1));
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                Assert.AreEqual(1000 + 1, registration.Comments.Length);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Comments: Please enter less than 1,000 characters");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Comments with null value saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Comments with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithEmptyStringSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = string.Empty;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Comments with one space saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithOneSpaceSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = " ";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Comments with one character saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithOneCharacterSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = "x";
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Comments with long value saves.
        /// </summary>
        [TestMethod]
        public void TestCommentsWithLongValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.Comments = "x".RepeatTimes(1000);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1000, registration.Comments.Length);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Comments Tests

        #region Ceremony Tests

        #region Invalid Tests

        /// <summary>
        /// Tests the Registration with null ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestRegistrationWithNullCeremonyDoesNotSave()
        {
            Registration registration = null;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Ceremony = null;
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(registration);
                var results = registration.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Ceremony: may not be empty");
                Assert.IsTrue(registration.IsTransient());
                Assert.IsFalse(registration.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Registration with new ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestRegistrationWithNewCeremonyDoesNotSave()
        {
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            Registration registration;
            try
            {
                #region Arrange
                registration = GetValid(9);
                registration.Ceremony = CreateValidEntities.Ceremony(9);
                registration.Ceremony.TermCode = termCodeRepository.GetById("1");
                #endregion Arrange

                #region Act
                RegistrationRepository.DbContext.BeginTransaction();
                RegistrationRepository.EnsurePersistent(registration);
                RegistrationRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }
        }


        #endregion Invalid Tests

        #region Valid Tests
        /// <summary>
        /// Tests the update to use A different Ceremony saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentCeremonySaves()
        {
            #region Arrange
            var registration = RegistrationRepository.GetById(1);
            Assert.AreNotSame(registration.Ceremony, Repository.OfType<Ceremony>().GetById(2));
            registration.Ceremony = Repository.OfType<Ceremony>().GetById(2);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(registration.Ceremony,  Repository.OfType<Ceremony>().GetById(2));
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Ceremony Tests

        #region ExtraTicketPetition Tests

        #region Valid Tests

        /// <summary>
        /// Tests the extra ticket petition with null value saves.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionWithNullValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.ExtraTicketPetition = null;

            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(registration.ExtraTicketPetition);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert		
        }

        /// <summary>
        /// Tests the extra ticket petition with new value saves.
        /// </summary>
        [TestMethod]
        public void TestExtraTicketPetitionWithNewValueSaves()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);

            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(registration.ExtraTicketPetition);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            #endregion Assert
        }
        
        #endregion Valid Tests

        #endregion ExtraTicketPetition Tests

        #region DateRegistered Tests

        /// <summary>
        /// Tests the DateRegistered with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Registration record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateRegistered with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateRegistered with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateRegisteredWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateRegistered = compareDate;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(record);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateRegistered);
            #endregion Assert
        }
        #endregion DateRegistered Tests

        #region TicketDistributionMethod Tests

        /// <summary>
        /// Tests the ticket distribution method returns expected result.
        /// </summary>
        [TestMethod]
        public void TestTicketDistributionMethodReturnsExpectedResult1()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            #endregion Arrange

            #region Act
            registration.MailTickets = true;
            #endregion Act

            #region Assert
            Assert.AreEqual("Mail tickets to provided address", registration.TicketDistributionMethod);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the ticket distribution method returns expected result.
        /// </summary>
        [TestMethod]
        public void TestTicketDistributionMethodReturnsExpectedResult2()
        {
            #region Arrange
            var registration = CreateValidEntities.Registration(1);
            #endregion Arrange

            #region Act
            registration.MailTickets = false;
            #endregion Act

            #region Assert
            Assert.AreEqual("Pickup tickets at Arc Ticket Office", registration.TicketDistributionMethod);
            #endregion Assert
        }

        #endregion TicketDistributionMethod Tests

        #region TotalTickets Tests

        /// <summary>
        /// Tests the total tickets when no extra ticket petition and only one ticket requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenNoExtraTicketPetitionAndOnlyOneTicketRequested()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 1;
            registration.ExtraTicketPetition = null;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(1, registration.TotalTickets);
            #endregion Assert		
        }
        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionNotApprovedAndOnly2TicketsRequested1()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = false;
            registration.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, registration.TotalTickets);
            #endregion Assert
        }
        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionNotApprovedAndOnly2TicketsRequested2()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = false;
            registration.ExtraTicketPetition.IsPending = true;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, registration.TotalTickets);
            #endregion Assert
        }
        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionApprovedAndOnly2TicketsRequested3()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = true;
            registration.ExtraTicketPetition.IsPending = true;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(2, registration.TotalTickets);
            #endregion Assert
        }

        /// <summary>
        /// Tests the total tickets when extra ticket petition not approved and only 2 tickets requested.
        /// </summary>
        [TestMethod]
        public void TestTotalTicketsWhenExtraTicketPetitionApprovedAndOnly2TicketsRequested4()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.NumberTickets = 2;
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);
            registration.ExtraTicketPetition.NumberTickets = 9;
            registration.ExtraTicketPetition.IsApproved = true;
            registration.ExtraTicketPetition.IsPending = false;
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(11, registration.TotalTickets);
            #endregion Assert
        }
        #endregion TotalTickets Tests

        #region Cascade Tests

        /// <summary>
        /// Tests the delete registration does not delete student.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteStudent()
        {
            #region Arrange
            var studentCount = StudentRepository.GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.IsNotNull(registration.Student);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(studentCount, StudentRepository.GetAll().Count);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the delete registration does not delete major code.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteMajorCode()
        {
            #region Arrange
            var majorCode = MajorCodeRepository.GetById("1");
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.AreSame(registration.Major, majorCode);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            var majorCodeCompare = MajorCodeRepository.GetById("1");
            Assert.AreSame(majorCode, majorCodeCompare);
            #endregion Assert
        }

        /// <summary>
        /// Tests the state of the delete registration does not delete.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteState()
        {
            #region Arrange
            var stateCount = StateRepository.GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.IsNotNull(registration.State);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(stateCount, StateRepository.GetAll().Count);
            #endregion Assert
        }

        /// <summary>
        /// Tests the delete registration does not delete ceremony.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesNotDeleteCeremony()
        {
            #region Arrange
            var ceremonyCount = Repository.OfType<Ceremony>().GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            var registration = RegistrationRepository.GetById(1);
            Assert.IsNotNull(registration.Ceremony);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(ceremonyCount, Repository.OfType<Ceremony>().GetAll().Count);
            #endregion Assert
        }


        /// <summary>
        /// Tests the delete registration does cascade to extra ticket petition.
        /// </summary>
        [TestMethod]
        public void TestDeleteRegistrationDoesCascadeToExtraTicketPetition()
        {
            #region Arrange
            var registration = GetValid(9);
            registration.ExtraTicketPetition = CreateValidEntities.ExtraTicketPetition(1);

            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.EnsurePersistent(registration);
            RegistrationRepository.DbContext.CommitTransaction();
 
            Assert.IsNotNull(registration.ExtraTicketPetition);
            Assert.IsFalse(registration.IsTransient());
            Assert.IsTrue(registration.IsValid());
            var extraTicketPetitionCount = Repository.OfType<ExtraTicketPetition>().GetAll().Count;
            var registrationCount = RegistrationRepository.GetAll().Count;
            Assert.IsTrue(extraTicketPetitionCount > 0);
            #endregion Arrange

            #region Act
            RegistrationRepository.DbContext.BeginTransaction();
            RegistrationRepository.Remove(registration);
            RegistrationRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.AreEqual(registrationCount - 1, RegistrationRepository.GetAll().Count);
            Assert.AreEqual(extraTicketPetitionCount - 1, Repository.OfType<ExtraTicketPetition>().GetAll().Count);
            #endregion Assert		
        }

        #endregion Cascade Tests

        #region Constructor Tests

        /// <summary>
        /// Tests the constructor sets date registered to current date.
        /// </summary>
        [TestMethod]
        public void TestConstructorSetsDateRegisteredToCurrentDate()
        {
            #region Arrange
            var registration = new Registration();            
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual(DateTime.Now.Date, registration.DateRegistered.Date);
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
            expectedFields.Add(new NameAndType("Address1", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Address2", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]"
            }));
            expectedFields.Add(new NameAndType("Address3", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]"
            }));
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("City", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Comments", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)1000, Message = \"Please enter less than 1,000 characters\")]"
            }));
            expectedFields.Add(new NameAndType("DateRegistered", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
            {
                "[NHibernate.Validator.Constraints.EmailAttribute()]",
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
            }));
            expectedFields.Add(new NameAndType("ExtraTicketPetition", "Commencement.Core.Domain.ExtraTicketPetition", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("MailTickets", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Major", "Commencement.Core.Domain.MajorCode", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("NumberTickets", "System.Int32", new List<string>
            {
                "[NHibernate.Validator.Constraints.MinAttribute((Int64)1)]"
            }));
            expectedFields.Add(new NameAndType("State", "Commencement.Core.Domain.State", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("Student", "Commencement.Core.Domain.Student", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TicketDistributionMethod", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("TotalTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Zip", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)15)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Registration));

        }

        #endregion Reflection of Database.	
		
		
    }
}