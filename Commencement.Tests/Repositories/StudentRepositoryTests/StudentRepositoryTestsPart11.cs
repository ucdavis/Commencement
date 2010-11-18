using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;

namespace Commencement.Tests.Repositories.StudentRepositoryTests
{
    /// <summary>
    /// Entity Name:		Student
    /// LookupFieldName:	FirstName
    /// </summary>
    public partial class StudentRepositoryTests
    {
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
            Assert.AreEqual(12.3m, student.CurrentUnits);
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
    }
}
