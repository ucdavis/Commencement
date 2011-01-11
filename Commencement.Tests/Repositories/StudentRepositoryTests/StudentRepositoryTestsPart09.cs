using System.Collections.Generic;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.StudentRepositoryTests
{
    /// <summary>
    /// Entity Name:		Student
    /// LookupFieldName:	FirstName
    /// </summary>
    public partial class StudentRepositoryTests
    {
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

        #region AddedBy Tests


        #region Valid Tests

        /// <summary>
        /// Tests the AddedBy with null value saves.
        /// </summary>
        [TestMethod]
        public void TestAddedByWithNullValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.AddedBy = null;
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
        /// Tests the AddedBy with empty string saves.
        /// </summary>
        [TestMethod]
        public void TestAddedByWithEmptyStringSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.AddedBy = string.Empty;
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
        /// Tests the AddedBy with one space saves.
        /// </summary>
        [TestMethod]
        public void TestAddedByWithOneSpaceSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.AddedBy = " ";
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
        /// Tests the AddedBy with one character saves.
        /// </summary>
        [TestMethod]
        public void TestAddedByWithOneCharacterSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.AddedBy = "x";
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
        /// Tests the AddedBy with long value saves.
        /// </summary>
        [TestMethod]
        public void TestAddedByWithLongValueSaves()
        {
            #region Arrange
            var student = GetValid(9);
            student.AddedBy = "x".RepeatTimes(1000);
            #endregion Arrange

            #region Act
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.AreEqual(1000, student.AddedBy.Length);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());
            #endregion Assert
        }

        #endregion Valid Tests
        #endregion AddedBy Tests


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

        #region Cascade Tests


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
        #endregion Cascade Tests

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
    }
}
