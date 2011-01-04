using System;
using System.Collections.Generic;
using Commencement.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region IsEditorTests

        [TestMethod]
        public void TestIsEditorReturnsTrueIfLoginIdFound()
        {
            #region Arrange
            var ceremony = GetValid(9);
            LoadUsers(3);
            ceremony.Editors = new List<CeremonyEditor>();
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(3), false);

            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Arrange

            #region Act
            var isFound = ceremony.IsEditor("LoginId1");
            #endregion Act

            #region Assert
            Assert.IsTrue(isFound);
            #endregion Assert		
        }
        [TestMethod]
        public void TestIsEditorReturnsFalseIfLoginIdNotFound()
        {
            #region Arrange
            var ceremony = GetValid(9);
            LoadUsers(3);
            ceremony.Editors = new List<CeremonyEditor>();
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(3), false);

            CeremonyRepository.DbContext.BeginTransaction();
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitTransaction();
            #endregion Arrange

            #region Act
            var isFound = ceremony.IsEditor("LoginId2");
            #endregion Act

            #region Assert
            Assert.IsFalse(isFound);
            #endregion Assert
        }

        #endregion IsEditorTests

        #region CanRegister Tests
        [TestMethod]
        public void TestCanRegisterReturnsTrueIfCurrentDateInRange1()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.RegistrationBegin = DateTime.Now.Date;
            ceremony.RegistrationDeadline = DateTime.Now;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(ceremony.CanRegister());
            #endregion Assert		
        }

        [TestMethod]
        public void TestCanRegisterReturnsTrueIfCurrentDateInRange2()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.RegistrationBegin = DateTime.Now.Date.AddDays(-1);
            ceremony.RegistrationDeadline = DateTime.Now.AddDays(1);
            #endregion Arrange

            #region Assert
            Assert.IsTrue(ceremony.CanRegister());
            #endregion Assert
        }

        [TestMethod]
        public void TestCanRegisterReturnsFalseIfCurrentDateNotInRange1()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.RegistrationBegin = DateTime.Now.Date.AddDays(1);
            ceremony.RegistrationDeadline = ceremony.RegistrationBegin.AddDays(10);
            #endregion Arrange

            #region Assert
            Assert.IsFalse(ceremony.CanRegister());
            #endregion Assert
        }

        [TestMethod]
        public void TestCanRegisterReturnsFalseIfCurrentDateNotInRange2()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.RegistrationBegin = DateTime.Now.Date.AddDays(-10);
            ceremony.RegistrationDeadline = DateTime.Now.Date.AddDays(-1);
            #endregion Arrange

            #region Assert
            Assert.IsFalse(ceremony.CanRegister());
            #endregion Assert
        }
        #endregion CanRegister Tests

        #region ExtraTicketDeadline Tests
        [TestMethod]
        public void TestCanSubmitExtraTicketReturnsTrueIfCurrentDateInRange1()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.ExtraTicketBegin = DateTime.Now.Date;
            ceremony.ExtraTicketDeadline = DateTime.Now;
            #endregion Arrange

            #region Assert
            Assert.IsTrue(ceremony.CanRegister());
            #endregion Assert
        }

        [TestMethod]
        public void TestCanSubmitExtraTicketReturnsTrueIfCurrentDateInRange2()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.ExtraTicketBegin = DateTime.Now.Date.AddDays(-1);
            ceremony.ExtraTicketDeadline = DateTime.Now.AddDays(1);
            #endregion Arrange

            #region Assert
            Assert.IsTrue(ceremony.CanSubmitExtraTicket());
            #endregion Assert
        }

        [TestMethod]
        public void TestCanSubmitExtraTicketReturnsFalseIfCurrentDateNotInRange1()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.ExtraTicketBegin = DateTime.Now.Date.AddDays(1);
            ceremony.ExtraTicketDeadline = ceremony.RegistrationBegin.AddDays(10);
            #endregion Arrange

            #region Assert
            Assert.IsFalse(ceremony.CanSubmitExtraTicket());
            #endregion Assert
        }

        [TestMethod]
        public void TestCanSubmitExtraTicketReturnsFalseIfCurrentDateNotInRange2()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.ExtraTicketBegin = DateTime.Now.Date.AddDays(-10);
            ceremony.ExtraTicketDeadline = DateTime.Now.Date.AddDays(-1);
            #endregion Arrange

            #region Assert
            Assert.IsFalse(ceremony.CanSubmitExtraTicket());
            #endregion Assert
        }
        #endregion CanSubmitExtraTicket Tests

        #region IsPastPrintingDeadline Tests
        [TestMethod]
        public void TestIsPastPrintingDeadlineReturnsTrue1()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.PrintingDeadline = DateTime.Now.AddDays(-1);
            #endregion Arrange

            #region Assert
            Assert.IsTrue(ceremony.IsPastPrintingDeadline());
            #endregion Assert
        }
        [TestMethod]
        public void TestIsPastPrintingDeadlineReturnsTrue12()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.PrintingDeadline = DateTime.Now.Date.AddMinutes(-1);
            #endregion Arrange

            #region Assert
            Assert.IsTrue(ceremony.IsPastPrintingDeadline());
            #endregion Assert
        }
        /// <summary>
        /// If you run this test around midnight it will fail
        /// </summary>
        [TestMethod]
        public void TestIsPastPrintingDeadlineReturnsFalse1()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.PrintingDeadline = DateTime.Now.AddHours(-1);
            #endregion Arrange

            #region Assert
            Assert.IsFalse(ceremony.IsPastPrintingDeadline());
            #endregion Assert
        }
        [TestMethod]
        public void TestIsPastPrintingDeadlineReturnsFalse2()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.PrintingDeadline = DateTime.Now.AddHours(1);
            #endregion Arrange

            #region Assert
            Assert.IsFalse(ceremony.IsPastPrintingDeadline());
            #endregion Assert
        }

        [TestMethod]
        public void TestIsPastPrintingDeadlineReturnsFalse3()
        {
            #region Arrange
            var ceremony = GetValid(9);
            ceremony.PrintingDeadline = DateTime.Now.Date.AddDays(1);
            #endregion Arrange

            #region Assert
            Assert.IsFalse(ceremony.IsPastPrintingDeadline());
            #endregion Assert
        }
        #endregion IsPastPrintingDeadline Tests
    }
}
