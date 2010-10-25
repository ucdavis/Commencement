using System;
using System.Linq;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
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
                results.AssertErrorsAre("Student: may not be null");
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
            registration.Student = new Student("pidm", "123456789", "First", "Middle", "last", 1.10m, "test@ucdavis.edu", "login", new TermCode());
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
        public void TestMajorWithAValueOfNullNDoesNotSave()
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
                results.AssertErrorsAre("Major: may not be null");
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
    }
}
