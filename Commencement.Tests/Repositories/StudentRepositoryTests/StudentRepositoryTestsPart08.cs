using System;
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
        #region TermCode Tests

        #region Invalid Tests
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestTermCodeWithANewValueDoesNotSave()
        {
            Student student = null;
            try
            {
                #region Arrange
                student = GetValid(9);
                student.TermCode = CreateValidEntities.TermCode(9);
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
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.TermCode, Entity: Commencement.Core.Domain.TermCode", ex.Message);
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
        #endregion Cascade Tests
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
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            LoadTermCode(1);
            LoadCeremony(1);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
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

        #region Cascade Tests
        /// <summary>
        /// Tests the delete student does not cascade to ceremony.
        /// </summary>
        [TestMethod]
        public void TestDeleteStudentDoesNotCascadeToCeremony()
        {
            #region Arrange
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            LoadTermCode(1);
            LoadCeremony(1);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            var student = GetValid(9);
            var ceremony = Repository.OfType<Ceremony>().GetById(1);
            student.Ceremony = ceremony;
            StudentRepository.DbContext.BeginTransaction();
            StudentRepository.EnsurePersistent(student);
            StudentRepository.DbContext.CommitTransaction();
            var saveStudentId = student.Id;
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
            Assert.IsNull(StudentRepository.GetNullableById(saveStudentId));
            #endregion Assert
        }
        
        #endregion Cascade Tests
        #endregion Ceremony Tests
    }
}
