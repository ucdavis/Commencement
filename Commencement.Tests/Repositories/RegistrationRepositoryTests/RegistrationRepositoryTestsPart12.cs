using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        
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

    

        #endregion Cascade Tests


    }
}
