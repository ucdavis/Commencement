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
            Assert.AreNotEqual(Guid.Empty, student.Id);
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
            var student = new Student("pidm", "studentId", "FName", "MI", "LName", 12.3m, 100m, "email", "login", termCode);
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
            Assert.AreEqual(100m, student.EarnedUnits);
            Assert.AreEqual("email", student.Email);
            Assert.AreEqual("login", student.Login);
            Assert.AreEqual("Tname", student.TermCode.Name);
            Assert.AreNotEqual(Guid.Empty, student.Id);
            #endregion Assert
        }
        #endregion Constructor Tests

        #region TotalUnits Tests

        [TestMethod]
        public void TestTotalUnitsReturnsExpectedvalue()
        {
            #region Arrange
            var record = new Student();
            record.EarnedUnits = 200m;
            record.CurrentUnits = 150m;
            #endregion Arrange

            #region Assert
            Assert.AreEqual(350m, record.TotalUnits);
            #endregion Assert		
        }


        #endregion TotalUnits Tests
    }
}
