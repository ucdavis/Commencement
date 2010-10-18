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
    /// Entity Name:		Student
    /// LookupFieldName:	FirstName
    /// </summary>
    [TestClass]
    public class StudentRepositoryTests : AbstractRepositoryTests<Student, Guid, StudentMap>
    {
        /// <summary>
        /// Gets or sets the Student repository.
        /// </summary>
        /// <value>The Student repository.</value>
        public IRepositoryWithTypedId<Student, Guid> StudentRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string > TermCodeRepository { get; set; }
		
        #region Init and Overrides

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepositoryTests"/> class.
        /// </summary>
        public StudentRepositoryTests()
        {
            //ForceSave = true;
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            StudentRepository = new RepositoryWithTypedId<Student, Guid>();
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected override Student GetValid(int? counter)
        {
            var rtvalue = CreateValidEntities.Student(counter);
            var localCounter = 99;
            if(counter!= null)
            {
                localCounter = (int) counter;
            }
            rtvalue.SetIdTo(SpecificGuid.GetGuid(localCounter));
            return rtvalue;

        }

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected override IQueryable<Student> GetQuery(int numberAtEnd)
        {
            return StudentRepository.Queryable.Where(a => a.FirstName.EndsWith(numberAtEnd.ToString()));
        }

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected override void FoundEntityComparison(Student entity, int counter)
        {
            Assert.AreEqual("FirstName" + counter, entity.FirstName);
        }

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected override void UpdateUtility(Student entity, ARTAction action)
        {
            const string updateValue = "Updated";
            switch (action)
            {
                case ARTAction.Compare:
                    Assert.AreEqual(updateValue, entity.FirstName);
                    break;
                case ARTAction.Restore:
                    entity.FirstName = RestoreValue;
                    break;
                case ARTAction.Update:
                    RestoreValue = entity.FirstName;
                    entity.FirstName = updateValue;
                    break;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        protected override void LoadData()
        {
            StudentRepository.DbContext.BeginTransaction();
            LoadRecords(5);
            StudentRepository.DbContext.CommitTransaction();
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.Pidm = null;
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.Pidm = string.Empty;
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.Pidm = " ";
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: may not be null or empty");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.Pidm = "x".RepeatTimes((8 + 1));
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                Assert.AreEqual(8 + 1, student.Pidm.Length);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Pidm: length must be between 0 and 8");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            var student = GetValid(9);
            student.Pidm = "x";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Pidm with long value saves.
        /// </summary>
        [TestMethod]
        public void TestPidmWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Pidm = "x".RepeatTimes(8);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(8, student.Pidm.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.StudentId = null;
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: may not be null or empty");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.StudentId = string.Empty;
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: may not be null or empty");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.StudentId = " ";
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: may not be null or empty");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.StudentId = "x".RepeatTimes((9 + 1));
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                Assert.AreEqual(9 + 1, student.StudentId.Length);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("StudentId: length must be between 0 and 9");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            var student = GetValid(9);
            student.StudentId = "x";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the StudentId with long value saves.
        /// </summary>
        [TestMethod]
        public void TestStudentIdWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.StudentId = "x".RepeatTimes(9);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(9, student.StudentId.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion StudentId Tests

        #region FirstName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the FirstName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestFirstNameWithTooLongValueDoesNotSave()
        {
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.FirstName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                Assert.AreEqual(50 + 1, student.FirstName.Length);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("FirstName: length must be between 0 and 50");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the FirstName with null value saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithNullValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.FirstName = null;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithEmptyStringSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.FirstName = string.Empty;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with one space saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithOneSpaceSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.FirstName = " ";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithOneCharacterSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.FirstName = "x";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the FirstName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestFirstNameWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.FirstName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, student.FirstName.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion FirstName Tests
        // ReSharper disable InconsistentNaming
        #region MI Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the MI with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestMIWithTooLongValueDoesNotSave()
        {
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.MI = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                Assert.AreEqual(50 + 1, student.MI.Length);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("MI: length must be between 0 and 50");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the MI with null value saves.
        /// </summary>
        [TestMethod]
        public void TestMIWithNullValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.MI = null;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the MI with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestMIWithEmptyStringSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.MI = string.Empty;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the MI with one space saves.
        /// </summary>
        [TestMethod]
        public void TestMIWithOneSpaceSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.MI = " ";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the MI with one character saves.
        /// </summary>
        [TestMethod]
        public void TestMIWithOneCharacterSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.MI = "x";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the MI with long value saves.
        /// </summary>
        [TestMethod]
        public void TestMIWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.MI = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, student.MI.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion MI Tests
        // ReSharper restore InconsistentNaming
        #region LastName Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the LastName with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLastNameWithTooLongValueDoesNotSave()
        {
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.LastName = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                Assert.AreEqual(50 + 1, student.LastName.Length);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("LastName: length must be between 0 and 50");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the LastName with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithNullValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.LastName = null;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithEmptyStringSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.LastName = string.Empty;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with one space saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithOneSpaceSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.LastName = " ";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithOneCharacterSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.LastName = "x";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the LastName with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLastNameWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.LastName = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, student.LastName.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion LastName Tests

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
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
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
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
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
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0m, record.Units);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion Units Tests

        #region Email Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Email with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestEmailWithTooLongValueDoesNotSave()
        {
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.Email = "x".RepeatTimes((100 + 1));
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                Assert.AreEqual(100 + 1, student.Email.Length);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Email: length must be between 0 and 100");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
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
            var student = GetValid(9);
            student.Email = null;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithEmptyStringSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Email = string.Empty;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with one space saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithOneSpaceSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Email = " ";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with one character saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithOneCharacterSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Email = "x";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Email with long value saves.
        /// </summary>
        [TestMethod]
        public void TestEmailWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Email = "x".RepeatTimes(100);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(100, student.Email.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Email Tests

        #region Login Tests
        #region Invalid Tests

        /// <summary>
        /// Tests the Login with too long value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestLoginWithTooLongValueDoesNotSave()
        {
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.Login = "x".RepeatTimes((50 + 1));
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(student);
                Assert.AreEqual(50 + 1, student.Login.Length);
                var results = student.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Login: length must be between 0 and 50");
                //Assert.IsTrue(student.IsTransient());
                Assert.IsFalse(student.IsValid());
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Login with null value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginWithNullValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Login = null;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Login with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestLoginWithEmptyStringSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Login = string.Empty;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Login with one space saves.
        /// </summary>
        [TestMethod]
        public void TestLoginWithOneSpaceSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Login = " ";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Login with one character saves.
        /// </summary>
        [TestMethod]
        public void TestLoginWithOneCharacterSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Login = "x";
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the Login with long value saves.
        /// </summary>
        [TestMethod]
        public void TestLoginWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Login = "x".RepeatTimes(50);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(50, student.Login.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Login Tests

        #region DateAdded Tests

        /// <summary>
        /// Tests the DateAdded with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateAddedWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Student record = GetValid(99);
            record.DateAdded = compareDate;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateAdded);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateAdded with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateAddedWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateAdded = compareDate;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateAdded);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateAdded with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateAddedWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateAdded = compareDate;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateAdded);
            #endregion Assert
        }
        #endregion DateAdded Tests

        #region DateUpdated Tests

        /// <summary>
        /// Tests the DateUpdated with past date will save.
        /// </summary>
        [TestMethod]
        public void TestDateUpdatedWithPastDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(-10);
            Student record = GetValid(99);
            record.DateUpdated = compareDate;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateUpdated);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the DateUpdated with current date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateUpdatedWithCurrentDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now;
            var record = GetValid(99);
            record.DateUpdated = compareDate;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateUpdated);
            #endregion Assert
        }

        /// <summary>
        /// Tests the DateUpdated with future date date will save.
        /// </summary>
        [TestMethod]
        public void TestDateUpdatedWithFutureDateDateWillSave()
        {
            #region Arrange
            var compareDate = DateTime.Now.AddDays(15);
            var record = GetValid(99);
            record.DateUpdated = compareDate;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.AreEqual(compareDate, record.DateUpdated);
            #endregion Assert
        }
        #endregion DateUpdated Tests

        #region TermCode Tests

        #region Invalid Tests
        ///// <summary>
        ///// Tests the TermCode with A value of null does not save.
        ///// </summary>
        //[TestMethod]
        //[ExpectedException(typeof(ApplicationException))]
        //public void TestTermCodeWithAValueOfNullNDoesNotSave()
        //{
        //    Student student = null;
        //    try
        //    {
        //        #region Arrange
        //        student = GetValid(9);
        //        student.TermCode = null;
        //        #endregion Arrange

        //        #region Act
        //        StudentRepository.DbContext.BeginTransaction();
        //        StudentRepository.EnsurePersistent(student);
        //        StudentRepository.DbContext.CommitTransaction();
        //        #endregion Act
        //    }
        //    catch (Exception)
        //    {
        //        Assert.IsNotNull(student);
        //        Assert.AreEqual(student.TermCode, null);
        //        var results = student.ValidationResults().AsMessageList();
        //        results.AssertErrorsAre("TermCode: may not be empty");
        //        Assert.IsTrue(student.IsTransient());
        //        Assert.IsFalse(student.IsValid());
        //        throw;
        //    }
        //}
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the update to use A different TermCode saves.
        /// </summary>
        [TestMethod]
        public void TestUpdateToUseADifferentTermCodeSaves()
        {
            #region Arrange
            var student = StudentRepository.GetById(SpecificGuid.GetGuid(1));
            Assert.AreNotSame(student.TermCode, TermCodeRepository.GetById("2"));
            student.TermCode = TermCodeRepository.GetById("2");
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreSame(student.TermCode, TermCodeRepository.GetById("2"));
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion TermCode Tests

        #region Ceremony Tests

        #region Invalid Tests
        /// <summary>
        /// Tests the Ceremony with A new value does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestCeremonyWithANewValueDoesNotSave()
        {
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.Ceremony = new Ceremony();
                #endregion Arrange

                #region Act
                StudentRepository.DbContext.BeginTransaction();
                StudentRepository.EnsurePersistent(student);
                StudentRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(student);
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }
        }
        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the ceremony with null value saves.
        /// </summary>
        [TestMethod]
        public void TestCeremonyWithNullValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.Ceremony = null;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.IsNull(record.Ceremony);
            #endregion Assert
        }
        /// <summary>
        /// Tests the ceremony with new value saves.
        /// </summary>
        [TestMethod]
        public void TestCeremonyWithExistingValueSaves()
        {
            #region Arrange
            LoadCeremony(1);
            var record = GetValid(9);
            record.Ceremony = Repository.OfType<Ceremony>().GetById(1);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitChanges();
            #endregion Act

            #region Assert
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            Assert.IsNotNull(record.Ceremony);
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion Ceremony Tests

        #region SjaBlock Tests

        /// <summary>
        /// Tests the SjaBlock is false saves.
        /// </summary>
        [TestMethod]
        public void TestSjaBlockIsFalseSaves()
        {
            #region Arrange

            Student student = GetValid(9);
            student.SjaBlock = false;

            #endregion Arrange

            #region Act

            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(student.SjaBlock);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the SjaBlock is true saves.
        /// </summary>
        [TestMethod]
        public void TestSjaBlockIsTrueSaves()
        {
            #region Arrange

            var student = GetValid(9);
            student.SjaBlock = true;

            #endregion Arrange

            #region Act

            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(student.SjaBlock);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());

            #endregion Assert
        }

        #endregion SjaBlock Tests

        #region Majors Tests

        /// <summary>
        /// Tests the majors with null value saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithNullValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Majors = null;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNull(student.Majors);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert	
        }

        /// <summary>
        /// Tests the majors with empty list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithEmptyListSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(student.Majors);
            Assert.AreEqual(0, student.Majors.Count);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the majors with populated list saves.
        /// </summary>
        [TestMethod]
        public void TestMajorsWithPopulatedListSaves()
        {
            #region Arrange
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            LoadMajorCode(3);
            var student = GetValid(9);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(majorCodeRepository.GetById("1"));
            student.Majors.Add(majorCodeRepository.GetById("3"));
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsNotNull(student.Majors);
            Assert.AreEqual(2, student.Majors.Count);
            Assert.AreSame(student.Majors[1], majorCodeRepository.GetById("3"));
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Majors Tests

        #region FullName Tests

        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult1()
        {
            #region Arrange
            var student = new Student();
            student.MI = null;
            #endregion Arrange

            #region Act
            student.FirstName = "Johan";
            student.LastName = "Fuller";
            #endregion Act

            #region Assert
            Assert.AreEqual("Johan Fuller", student.FullName);
            #endregion Assert		
        }
        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult2()
        {
            #region Arrange
            var student = new Student();
            student.MI = string.Empty;
            #endregion Arrange

            #region Act
            student.FirstName = "Johan";
            student.LastName = "Fuller";
            #endregion Act

            #region Assert
            Assert.AreEqual("Johan Fuller", student.FullName);
            #endregion Assert
        }
        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult3()
        {
            #region Arrange
            var student = new Student();
            student.MI = "xx";
            #endregion Arrange

            #region Act
            student.FirstName = "Johan";
            student.LastName = "Fuller";
            #endregion Act

            #region Assert
            Assert.AreEqual("Johan xx Fuller", student.FullName);
            #endregion Assert
        }

        /// <summary>
        /// Tests the full name returns expected result.
        /// </summary>
        [TestMethod]
        public void TestFullNameReturnsExpectedResult4()
        {
            #region Arrange
            var student = new Student();
            student.MI = "   ";
            #endregion Arrange

            #region Act
            student.FirstName = "Johan";
            student.LastName = "Fuller";
            #endregion Act

            #region Assert
            Assert.AreEqual("Johan Fuller", student.FullName);
            #endregion Assert
        }
        #endregion FullName Tests

        #region StrMajors Tests

        /// <summary>
        /// Tests the STR majors returns expected results.
        /// </summary>
        [TestMethod]
        public void TestStrMajorsReturnsExpectedResults1()
        {
            #region Arrange
            var student = new Student();
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            LoadMajorCode(3);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(majorCodeRepository.GetById("1"));
            student.Majors.Add(majorCodeRepository.GetById("3"));
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("Name1,Name3", student.StrMajors);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the STR majors returns expected results.
        /// </summary>
        [TestMethod]
        public void TestStrMajorsReturnsExpectedResults2()
        {
            #region Arrange
            var student = new Student();
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            LoadMajorCode(3);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(majorCodeRepository.GetById("2"));
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("Name2", student.StrMajors);
            #endregion Assert
        }

        /// <summary>
        /// Tests the STR majors returns expected results.
        /// </summary>
        [TestMethod]
        public void TestStrMajorsReturnsExpectedResults3()
        {
            #region Arrange
            var student = new Student();
            //var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            //LoadMajorCode(3);
            student.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual(string.Empty, student.StrMajors);
            #endregion Assert
        }

        #endregion StrMajors Tests

        #region StrMajorCodes Tests

        /// <summary>
        /// Tests the StrMajorCodes returns expected results.
        /// </summary>
        [TestMethod]
        public void TestStrMajorCodesReturnsExpectedResults1()
        {
            #region Arrange
            var student = new Student();
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            LoadMajorCode(3);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(majorCodeRepository.GetById("1"));
            student.Majors.Add(majorCodeRepository.GetById("3"));
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("1,3", student.StrMajorCodes);
            #endregion Assert
        }

        /// <summary>
        /// Tests the StrMajorCodes returns expected results.
        /// </summary>
        [TestMethod]
        public void TestStrMajorCodesReturnsExpectedResults2()
        {
            #region Arrange
            var student = new Student();
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            LoadMajorCode(3);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(majorCodeRepository.GetById("2"));
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("2", student.StrMajorCodes);
            #endregion Assert
        }

        /// <summary>
        /// Tests the StrMajorCodes returns expected results.
        /// </summary>
        [TestMethod]
        public void TestStrMajorCodesReturnsExpectedResults3()
        {
            #region Arrange
            var student = new Student();
            //var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            //LoadMajorCode(3);
            student.Majors = new List<MajorCode>();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual(string.Empty, student.StrMajorCodes);
            #endregion Assert
        }

        #endregion StrMajorCodes Tests

        #region Constructor Tests

        /// <summary>
        /// Tests the constructor with no parameters sets expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithNoParametersSetsExpectedValues()
        {
            #region Arrange
            var student = new Student();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsNotNull(student.Majors);
            Assert.AreEqual(0, student.Majors.Count);
            Assert.AreEqual(DateTime.Now.Date, student.DateAdded.Date);
            Assert.AreEqual(DateTime.Now.Date, student.DateUpdated.Date);
            Assert.AreEqual(Guid.Empty, student.Id);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the constructor with parameters sets expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithParametersSetsExpectedValues()
        {
            #region Arrange
            var termCode = new TermCode();
            termCode.Name = "Tname";
            var student = new Student("pidm", "studentId", "FName", "MI", "LName", 12.3m, "email", "login", termCode);
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsNotNull(student.Majors);
            Assert.AreEqual(0, student.Majors.Count);
            Assert.AreEqual(DateTime.Now.Date, student.DateAdded.Date);
            Assert.AreEqual(DateTime.Now.Date, student.DateUpdated.Date);
            Assert.AreEqual("pidm", student.Pidm);
            Assert.AreEqual("studentId", student.StudentId);
            Assert.AreEqual("FName", student.FirstName);
            Assert.AreEqual("MI", student.MI);
            Assert.AreEqual("LName", student.LastName);
            Assert.AreEqual(12.3m, student.Units);
            Assert.AreEqual("email", student.Email);
            Assert.AreEqual("login", student.Login);
            Assert.AreEqual("Tname", student.TermCode.Name);
            Assert.AreNotEqual(Guid.Empty, student.Id);
            #endregion Assert
        }
        #endregion Constructor Tests

        #region Cascade Tests

        /// <summary>
        /// Tests the delete student does not cascade to term code.
        /// </summary>
        [TestMethod]
        public void TestDeleteStudentDoesNotCascadeToTermCode()
        {
            #region Arrange
            LoadTermCode(3);
            var student = GetValid(9);
            student.TermCode = TermCodeRepository.GetById("2");
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            Assert.AreSame(student.TermCode, TermCodeRepository.GetById("2"));
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            var termCodeCount = TermCodeRepository.GetAll().Count;
            Assert.IsTrue(termCodeCount > 0);
            var studentCount = StudentRepository.GetAll().Count;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.Remove(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(studentCount - 1, StudentRepository.GetAll().Count);
            Assert.AreEqual(termCodeCount, TermCodeRepository.GetAll().Count);
            #endregion Assert		
        }


        /// <summary>
        /// Tests the new term code does not cascade save.
        /// </summary>
        [TestMethod]
        public void TestNewTermCodeDoesNotCascadeSave()
        {
            #region Arrange
            var student = StudentRepository.GetById(SpecificGuid.GetGuid(1));
            student.TermCode = new TermCode();
            student.TermCode.Name = "NewTerm";
            student.TermCode.SetIdTo("NT");
            var termCodeCount = TermCodeRepository.GetAll().Count;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            //Assert.AreSame(student.TermCode, TermCodeRepository.GetById("2"));
            Assert.AreEqual(termCodeCount, TermCodeRepository.GetAll().Count);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert		
        }

        /// <summary>
        /// Tests the delete student does not cascade to major codes.
        /// Can't use GetAll because of majorCodes "Where" clause.
        /// </summary>
        [TestMethod]
        public void TestDeleteStudentDoesNotCascadeToMajorCodes()
        {
            #region Arrange
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            LoadMajorCode(3);
            var student = GetValid(9);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(majorCodeRepository.GetById("1"));
            student.Majors.Add(majorCodeRepository.GetById("3"));
 
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.Remove(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual("Name1", majorCodeRepository.GetById("1").Name);
            Assert.AreEqual("Name2", majorCodeRepository.GetById("2").Name);
            Assert.AreEqual("Name3", majorCodeRepository.GetById("3").Name);
            #endregion Assert		
        }


        /// <summary>
        /// Tests the delete student does not cascade to ceremony.
        /// </summary>
        [TestMethod]
        public void TestDeleteStudentDoesNotCascadeToCeremony()
        {
            #region Arrange
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            LoadCeremony(1);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            var student = GetValid(9);
            var ceremony = Repository.OfType<Ceremony>().GetById(1);
            student.Ceremony = ceremony;
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            Assert.IsNotNull(student.Ceremony);
            var saveCeremonyId = student.Ceremony.Id;
            Console.WriteLine("Exiting Arrange...");
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.Remove(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Console.WriteLine("Evicting...");
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.Id == saveCeremonyId).Single();
            Assert.IsNotNull(ceremony);
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
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>()));
            expectedFields.Add(new NameAndType("DateAdded", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("DateUpdated", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
            }));
            expectedFields.Add(new NameAndType("FirstName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("FullName", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Guid", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("LastName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("Login", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("Majors", "System.Collections.Generic.IList`1[Commencement.Core.Domain.MajorCode]", new List<string>()));
            expectedFields.Add(new NameAndType("MI", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("Pidm", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)8)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("SjaBlock", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("StrMajorCodes", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("StrMajors", "System.String", new List<string>()));            
            expectedFields.Add(new NameAndType("StudentId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)9)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TermCode", "Commencement.Core.Domain.TermCode", new List<string>()));
            expectedFields.Add(new NameAndType("Units", "System.Decimal", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Student));

        }

        #endregion Reflection of Database.	
		
		
    }
}