using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.StudentRepositoryTests
{
    /// <summary>
    /// Entity Name:		Student
    /// LookupFieldName:	FirstName
    /// </summary>
    public partial class StudentRepositoryTests
    {
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

        #region EarnedUnits Tests

        /// <summary>
        /// Tests the EarnedUnits with max decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestEarnedUnitsWithMaxDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.EarnedUnits = decimal.MaxValue;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MaxValue, record.EarnedUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the EarnedUnits with min decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestEarnedUnitsWithMinDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.EarnedUnits = decimal.MinValue;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MinValue, record.EarnedUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the EarnedUnits with value of zero saves.
        /// </summary>
        [TestMethod]
        public void TestEarnedUnitsWithValueOfZeroSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.EarnedUnits = 0m;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0m, record.EarnedUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion EarnedUnits Tests

        #region CurrentUnits Tests

        /// <summary>
        /// Tests the CurrentUnits with max decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestCurrentUnitsWithMaxDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.CurrentUnits = decimal.MaxValue;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MaxValue, record.CurrentUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the CurrentUnits with min decimal value saves.
        /// </summary>
        [TestMethod]
        public void TestCurrentUnitsWithMinDecimalValueSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.CurrentUnits = decimal.MinValue;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(decimal.MinValue, record.CurrentUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        /// <summary>
        /// Tests the CurrentUnits with value of zero saves.
        /// </summary>
        [TestMethod]
        public void TestCurrentUnitsWithValueOfZeroSaves()
        {
            #region Arrange
            var record = GetValid(9);
            record.CurrentUnits = 0m;
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(record);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(0m, record.CurrentUnits);
            Assert.IsFalse(record.IsTransient());
            Assert.IsTrue(record.IsValid());
            #endregion Assert
        }

        #endregion CurrentUnits Tests
    }
}
