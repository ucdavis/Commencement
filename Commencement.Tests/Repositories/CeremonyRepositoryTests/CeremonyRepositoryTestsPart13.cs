using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region Constructor Tests

        /// <summary>
        /// Tests the constructor with no parameters defaults expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithNoParametersDefaultsExpectedValues()
        {
            #region Arrange
            var ceremony = new Ceremony();
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.IsNotNull(ceremony.RegistrationParticipations);
            Assert.IsNotNull(ceremony.Majors);
            Assert.IsNotNull(ceremony.RegistrationPetitions);
            Assert.IsNotNull(ceremony.Editors);
            Assert.IsNotNull(ceremony.Colleges);
            Assert.IsNotNull(ceremony.Templates);
            Assert.AreEqual(DateTime.Now.Date, ceremony.DateTime.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.RegistrationBegin.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.RegistrationDeadline.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.ExtraTicketBegin.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.ExtraTicketDeadline.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.PrintingDeadline.Date);
            #endregion Assert
        }

        /// <summary>
        /// Tests the constructor with parameters defaults expected values.
        /// </summary>
        [TestMethod]
        public void TestConstructorWithParametersDefaultsExpectedValues()
        {
            #region Arrange
            var ceremony = new Ceremony("Location", DateTime.Now.AddDays(10), 10, 100, DateTime.Now.AddDays(15), DateTime.Now.AddDays(20), CreateValidEntities.TermCode(4));
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("Location", ceremony.Location);
            Assert.IsNotNull(ceremony.RegistrationParticipations);
            Assert.IsNotNull(ceremony.Majors);
            Assert.IsNotNull(ceremony.RegistrationPetitions);
            Assert.IsNotNull(ceremony.Editors);
            Assert.IsNotNull(ceremony.Colleges);
            Assert.IsNotNull(ceremony.Templates);
            Assert.AreEqual(DateTime.Now.AddDays(10).Date, ceremony.DateTime.Date);
            Assert.AreEqual(10, ceremony.TicketsPerStudent);
            Assert.AreEqual(100, ceremony.TotalTickets);
            Assert.AreEqual(DateTime.Now.Date, ceremony.RegistrationBegin.Date);
            Assert.AreEqual(DateTime.Now.AddDays(20).Date, ceremony.RegistrationDeadline.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.ExtraTicketBegin.Date);
            Assert.AreEqual(DateTime.Now.Date, ceremony.ExtraTicketDeadline.Date);
            Assert.AreEqual(DateTime.Now.AddDays(15).Date, ceremony.PrintingDeadline.Date);
            Assert.AreEqual("Name4", ceremony.TermCode.Name);
            #endregion Assert
        }
        #endregion Constructor Tests

        #region Default Value Tests

        [TestMethod]
        public void TestAddedEditorDefaultValue()
        {
            #region Arrange
            var ceremony = new Ceremony();
            #endregion Arrange

            #region Act
            ceremony.AddEditor(CreateValidEntities.vUser(1), true);
            ceremony.AddEditor(CreateValidEntities.vUser(2), false);
            ceremony.AddEditor(CreateValidEntities.vUser(3)); //Defaulted
            ceremony.AddEditor(CreateValidEntities.vUser(4), true);
            #endregion Act

            #region Assert
            Assert.AreEqual(4, ceremony.Editors.Count);
            Assert.IsTrue(ceremony.Editors[0].Owner);
            Assert.IsFalse(ceremony.Editors[1].Owner);
            Assert.IsFalse(ceremony.Editors[2].Owner);
            Assert.IsTrue(ceremony.Editors[3].Owner);
            #endregion Assert
        }
        #endregion Default Value Tests
    }
}
