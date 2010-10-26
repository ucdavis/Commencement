using System.Collections.Generic;
using Commencement.Core.Domain;
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

        #region Blocked Tests

        /// <summary>
        /// Tests the Blocked is false saves.
        /// </summary>
        [TestMethod]
        public void TestBlockedIsFalseSaves()
        {
            #region Arrange

            Student student = GetValid(9);
            student.Blocked = false;

            #endregion Arrange

            #region Act

            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsFalse(student.Blocked);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());

            #endregion Assert
        }

        /// <summary>
        /// Tests the Blocked is true saves.
        /// </summary>
        [TestMethod]
        public void TestBlockedIsTrueSaves()
        {
            #region Arrange

            var student = GetValid(9);
            student.Blocked = true;

            #endregion Arrange

            #region Act

            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();

            #endregion Act

            #region Assert

            Assert.IsTrue(student.Blocked);
            Assert.IsFalse(student.IsTransient());
            Assert.IsTrue(student.IsValid());

            #endregion Assert
        }

        #endregion Blocked Tests
    }
}
