using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [TestMethod, Ignore]
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
    }
}
