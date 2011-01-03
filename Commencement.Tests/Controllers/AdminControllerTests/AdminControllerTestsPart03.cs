using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    public partial class AdminControllerTests
    {
        #region StudentDetails Tests

        /// <summary>
        /// Tests the student details redirects to index if the student is not found.
        /// </summary>
        [TestMethod, Ignore]
        public void TestStudentDetailsRedirectsToIndexIfTheStudentIsNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null, null);
            #endregion Arrange

            #region Act
            Controller.StudentDetails(SpecificGuid.GetGuid(4), true)
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Index());
            #endregion Act

            #region Assert

            #endregion Assert
        }

        /// <summary>
        /// Tests the student details view when student is found.
        /// </summary>
        [TestMethod, Ignore]
        public void TestStudentDetailsViewWhenStudentIsFound()
        {
            #region Arrange
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            ControllerRecordFakes.FakeStudent(3, StudentRepository, students, null);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = students[0];
            ControllerRecordFakes.FakeRegistration(3, RegistrationRepository, registrations);
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.StudentDetails(SpecificGuid.GetGuid(1), true)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreSame(result.Student, students[0]);
            Assert.AreEqual(3, result.States.Count());
            #endregion Assert
        }

        #endregion StudentDetails Tests

        #region AddStudent Tests

        ///// <summary>
        ///// Tests the add student with null parameter returns view.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentWithNullParameterReturnsView()
        //{
        //    #region Arrange
        //    string studentId = null;
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudent(studentId)
        //        .AssertViewRendered()
        //        .WithViewData<SearchStudentViewModel>();
        //    #endregion Act

        //    #region Assert
        //    Assert.IsNotNull(result);
        //    StudentService.AssertWasNotCalled(a => a.SearchStudent(Arg<string>.Is.Anything, Arg<string>.Is.Anything));
        //    #endregion Assert
        //}
        ///// <summary>
        ///// Tests the add student with empty string parameter returns view.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentWithEmptyStringParameterReturnsView()
        //{
        //    #region Arrange
        //    string studentId = string.Empty;
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudent(studentId)
        //        .AssertViewRendered()
        //        .WithViewData<SearchStudentViewModel>();
        //    #endregion Act

        //    #region Assert
        //    Assert.IsNotNull(result);
        //    StudentService.AssertWasNotCalled(a => a.SearchStudent(Arg<string>.Is.Anything, Arg<string>.Is.Anything));
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student with student id parameter returns view.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentWithStudentIdParameterReturnsView1()
        //{
        //    #region Arrange
        //    const string studentId = "123456789";
        //    var termCodes = new List<TermCode>();
        //    termCodes.Add(CreateValidEntities.TermCode(1));
        //    termCodes[0].IsActive = true;
        //    termCodes[0].SetIdTo("201003");
        //    ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);
        //    var searchStudents = new List<SearchStudent>();
        //    searchStudents.Add(CreateValidEntities.SearchStudent(1));
        //    searchStudents.Add(CreateValidEntities.SearchStudent(2));
        //    searchStudents.Add(CreateValidEntities.SearchStudent(3));
        //    StudentService.Expect(a => a.SearchStudent(studentId, termCodes[0].Id)).Return(searchStudents).Repeat.Any();
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudent(studentId)
        //        .AssertViewRendered()
        //        .WithViewData<SearchStudentViewModel>();
        //    #endregion Act

        //    #region Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(3, result.SearchStudents.Count);
        //    Assert.IsNull(Controller.Message);
        //    Assert.AreEqual(studentId, result.StudentId);
        //    StudentService.AssertWasCalled(a => a.SearchStudent(studentId, termCodes[0].Id));
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student with student id parameter returns view.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentWithStudentIdParameterReturnsView2()
        //{
        //    #region Arrange
        //    const string studentId = "123456789";
        //    var termCodes = new List<TermCode>();
        //    termCodes.Add(CreateValidEntities.TermCode(1));
        //    termCodes[0].IsActive = true;
        //    termCodes[0].SetIdTo("201003");
        //    ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);
        //    var searchStudents = new List<SearchStudent>();
        //    StudentService.Expect(a => a.SearchStudent(studentId, termCodes[0].Id)).Return(searchStudents).Repeat.Any();
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudent(studentId)
        //        .AssertViewRendered()
        //        .WithViewData<SearchStudentViewModel>();
        //    #endregion Act

        //    #region Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(0, result.SearchStudents.Count);
        //    Assert.AreEqual("No results for the student were found.", Controller.Message);
        //    Assert.AreEqual(studentId, result.StudentId);
        //    StudentService.AssertWasCalled(a => a.SearchStudent(studentId, termCodes[0].Id));
        //    #endregion Assert
        //}

        #endregion AddStudent Tests
    }
}
