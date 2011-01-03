using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    public partial class AdminControllerTests
    {
        #region AddStudentConfirm Tests

        //#region Get Tests

        ///// <summary>
        ///// Tests the add student confirm get redirects when student id is null.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmGetRedirectsWhenStudentIdIsNull()
        //{
        //    #region Arrange
        //    string studentId = null;
        //    const string majorId = "Test";
        //    #endregion Arrange

        //    #region Act
        //    Controller.AddStudentConfirm(studentId, majorId)
        //        .AssertActionRedirect()
        //        .ToAction<AdminController>(a => a.AddStudent(studentId));
        //    #endregion Act

        //    #region Assert

        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm get redirects when student id is empty.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmGetRedirectsWhenStudentIdIsEmpty()
        //{
        //    #region Arrange
        //    string studentId = string.Empty;
        //    const string majorId = "Test";
        //    #endregion Arrange

        //    #region Act
        //    Controller.AddStudentConfirm(studentId, majorId)
        //        .AssertActionRedirect()
        //        .ToAction<AdminController>(a => a.AddStudent(studentId));
        //    #endregion Act

        //    #region Assert

        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm get redirects when major id is null.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmGetRedirectsWhenMajorIdIsNull()
        //{
        //    #region Arrange
        //    const string studentId = "Test";
        //    string majorId = null;
        //    #endregion Arrange

        //    #region Act
        //    Controller.AddStudentConfirm(studentId, majorId)
        //        .AssertActionRedirect()
        //        .ToAction<AdminController>(a => a.AddStudent(studentId));
        //    #endregion Act

        //    #region Assert

        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm get redirects when major id is empty.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmGetRedirectsWhenMajorIdIsEmpty()
        //{
        //    #region Arrange
        //    const string studentId = "Test";
        //    string majorId = string.Empty;
        //    #endregion Arrange

        //    #region Act
        //    Controller.AddStudentConfirm(studentId, majorId)
        //        .AssertActionRedirect()
        //        .ToAction<AdminController>(a => a.AddStudent(studentId));
        //    #endregion Act

        //    #region Assert

        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm throws exception if no students found.
        ///// </summary>
        //[TestMethod, Ignore]
        //[ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        //public void TestAddStudentConfirmGetThrowsExceptionIfNoStudentsFound1()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    StudentService.Expect(a => a.SearchStudent(studentId, termCode)).Return(new List<SearchStudent>()).Repeat.Any();

        //    #endregion Arrange
        //    try
        //    {
        //        #region Act
        //        Controller.AddStudentConfirm(studentId, majorId);
        //        #endregion Act
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsNotNull(ex);
        //        Assert.AreEqual("Unable to find requested record.", ex.Message);
        //        StudentService.AssertWasCalled(a => a.SearchStudent(studentId, termCode));
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Tests the add student confirm throws exception if no students found.
        ///// </summary>
        //[TestMethod, Ignore]
        //[ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        //public void TestAddStudentConfirmGetThrowsExceptionIfNoStudentsFound2()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var searchStudents = new List<SearchStudent>();
        //    searchStudents.Add(CreateValidEntities.SearchStudent(1));
        //    searchStudents.Add(CreateValidEntities.SearchStudent(2));
        //    StudentService.Expect(a => a.SearchStudent(studentId, termCode)).Return(searchStudents).Repeat.Any();

        //    #endregion Arrange
        //    try
        //    {
        //        #region Act
        //        Controller.AddStudentConfirm(studentId, majorId);
        //        #endregion Act
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsNotNull(ex);
        //        Assert.AreEqual("Unable to find requested record.", ex.Message);
        //        StudentService.AssertWasCalled(a => a.SearchStudent(studentId, termCode));
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// Tests the add student confirm get returns view if A student is found.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmGetReturnsViewIfAStudentIsFound1()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var searchStudents = new List<SearchStudent>();
        //    searchStudents.Add(CreateValidEntities.SearchStudent(1));
        //    searchStudents.Add(CreateValidEntities.SearchStudent(2));
        //    searchStudents[0].MajorCode = majorId;
        //    searchStudents[1].MajorCode = majorId;
        //    StudentService.Expect(a => a.SearchStudent(studentId, termCode)).Return(searchStudents).Repeat.Any();
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudentConfirm(studentId, majorId)
        //        .AssertViewRendered()
        //        .WithViewData<Student>();

        //    #endregion Act

        //    #region Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Pidm1", result.Pidm);
        //    Assert.AreEqual("Id1", result.StudentId);
        //    Assert.AreEqual("FirstName1", result.FirstName);
        //    Assert.AreEqual("MI1", result.MI);
        //    Assert.AreEqual("LastName1", result.LastName);
        //    Assert.AreEqual(100m, result.CurrentUnits);
        //    Assert.AreEqual("Email1", result.Email);
        //    Assert.AreEqual("LoginId1", result.Login);
        //    Assert.AreEqual("201003", result.TermCode.Id);
        //    Assert.AreSame(majors[0], result.Majors[0]);
        //    #endregion Assert
        //}

        //#endregion Get Tests

        //#region Post Tests
        ///// <summary>
        ///// Tests the add student confirm post throws exception if student is null.
        ///// </summary>
        //[TestMethod, Ignore]
        //[ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        //public void TestAddStudentConfirmPostThrowsExceptionIfStudentIsNull()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var ceremonies = new List<Ceremony>();
        //    ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ceremonies[0].Majors.Add(majors[0]);
        //    ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    var students = new List<Student>();
        //    students.Add(CreateValidEntities.Student(1));
        //    students[0].StudentId = studentId;
        //    students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    #endregion Arrange
        //    try
        //    {
        //        #region Act
        //        Controller.AddStudentConfirm(studentId, majorId, null);
        //        #endregion Act
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsNotNull(ex);
        //        Assert.AreEqual("Student cannot be null.", ex.Message);
        //        throw;
        //    }
        //}

        //[TestMethod, Ignore]
        //[ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        //public void TestAddStudentConfirmPostThrowsExceptionIfMajorCodeNull()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    string majorId = null;
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var ceremonies = new List<Ceremony>();
        //    ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ceremonies[0].Majors.Add(majors[0]);
        //    ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    var students = new List<Student>();
        //    students.Add(CreateValidEntities.Student(1));
        //    students[0].StudentId = studentId;
        //    students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    #endregion Arrange
        //    try
        //    {
        //        #region Act
        //        Controller.AddStudentConfirm(studentId, majorId, new Student());
        //        #endregion Act
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsNotNull(ex);
        //        Assert.AreEqual("Major code is required.", ex.Message);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Tests the add student confirm post throws exception if major code not found.
        ///// </summary>
        //[TestMethod, Ignore]
        //[ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        //public void TestAddStudentConfirmPostThrowsExceptionIfMajorCodeNotFound()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "2";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var ceremonies = new List<Ceremony>();
        //    ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ceremonies[0].Majors.Add(majors[0]);
        //    ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    var students = new List<Student>();
        //    students.Add(CreateValidEntities.Student(1));
        //    students[0].StudentId = studentId;
        //    students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    #endregion Arrange
        //    try
        //    {
        //        #region Act
        //        Controller.AddStudentConfirm(studentId, majorId, new Student());
        //        #endregion Act
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.IsNotNull(ex);
        //        Assert.AreEqual("Unable to find major.", ex.Message);
        //        throw;
        //    }
        //}


        ///// <summary>
        ///// Tests the add student confirm post does not save if ceremony not found.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesNotSaveIfCeremonyNotFound1()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var ceremonies = new List<Ceremony>();
        //    ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    ceremonies[0].TermCode = CreateValidEntities.TermCode(9); //TermCodeRepository.Queryable.FirstOrDefault();
        //    ceremonies[0].Majors.Add(majors[0]);
        //    ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    var students = new List<Student>();
        //    students.Add(CreateValidEntities.Student(1));
        //    students[0].StudentId = studentId;
        //    students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudentConfirm(studentId, majorId, new Student())
        //        .AssertViewRendered()
        //        .WithViewData<Student>();
        //    #endregion Act

        //    #region Assert
        //    Controller.ModelState.AssertErrorsAre("No ceremony exists for this major for the current term.");
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(termCode, result.TermCode.Id);
        //    StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm post does not save if ceremony not found.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesNotSaveIfCeremonyNotFound2()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var ceremonies = new List<Ceremony>();
        //    ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
        //    ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
        //    ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    var students = new List<Student>();
        //    students.Add(CreateValidEntities.Student(1));
        //    students[0].StudentId = studentId;
        //    students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudentConfirm(studentId, majorId, new Student())
        //        .AssertViewRendered()
        //        .WithViewData<Student>();
        //    #endregion Act

        //    #region Assert
        //    Controller.ModelState.AssertErrorsAre("No ceremony exists for this major for the current term.");
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(termCode, result.TermCode.Id);
        //    StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm post does not save if student already exists and has that major.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesNotSaveIfStudentAlreadyExistsAndHasThatMajor()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var ceremonies = new List<Ceremony>();
        //    ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
        //    ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
        //    ceremonies[0].Majors.Add(majors[0]);
        //    ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    var students = new List<Student>();
        //    students.Add(CreateValidEntities.Student(1));
        //    students[0].StudentId = studentId;
        //    students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    students[0].Majors.Add(majors[0]);
        //    ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    #endregion Arrange

        //    #region Act
        //    var result = Controller.AddStudentConfirm(studentId, majorId, new Student())
        //        .AssertViewRendered()
        //        .WithViewData<Student>();
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual("Student already exists.", Controller.Message);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(termCode, result.TermCode.Id);
        //    StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm post does save if student already exists and does not have that major.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesSaveIfStudentAlreadyExistsAndDoesNotHaveThatMajor()
        //{
        //    Assert.Inconclusive("Review");
        //    //#region Arrange
        //    //const string studentId = "1";
        //    //const string majorId = "1";
        //    //const string termCode = "201003";
        //    //LoadTermCodes(termCode);
        //    //var majors = new List<MajorCode>();
        //    //majors.Add(CreateValidEntities.MajorCode(1));
        //    //ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    //var ceremonies = new List<Ceremony>();
        //    //ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    //ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
        //    //ceremonies[0].Majors.Add(majors[0]);
        //    //ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    //var students = new List<Student>();
        //    //students.Add(CreateValidEntities.Student(1));
        //    //students[0].StudentId = studentId;
        //    //students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    //students[0].Majors.Add(ceremonies[0].Majors[0]);
        //    //ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    //#endregion Arrange

        //    //#region Act
        //    //Controller.AddStudentConfirm(studentId, majorId, new Student())
        //    //    .AssertActionRedirect()
        //    //    .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
        //    //#endregion Act

        //    //#region Assert
        //    //Assert.IsNull(Controller.Message);
        //    //StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    //EmailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
        //    //var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
        //    //Assert.IsNotNull(args);
        //    //Assert.AreEqual(2, args.Majors.Count);
        //    //Assert.AreSame(ceremonies[0].Majors[0], args.Majors[0]);
        //    //Assert.AreSame(majors[0], args.Majors[1]);
        //    //#endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm post does save if student already exists and does not have that major notifies users if email did not work.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesSaveIfStudentAlreadyExistsAndDoesNotHaveThatMajorNotifiesUsersIfEmailDidNotWork()
        //{
        //    Assert.Inconclusive("Review");
        //    //#region Arrange
        //    //const string studentId = "1";
        //    //const string majorId = "1";
        //    //const string termCode = "201003";
        //    //LoadTermCodes(termCode);
        //    //var majors = new List<MajorCode>();
        //    //majors.Add(CreateValidEntities.MajorCode(1));
        //    //ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    //var ceremonies = new List<Ceremony>();
        //    //ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    //ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
        //    //ceremonies[0].Majors.Add(majors[0]);
        //    //ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    //var students = new List<Student>();
        //    //students.Add(CreateValidEntities.Student(1));
        //    //students[0].StudentId = studentId;
        //    //students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    //students[0].Majors.Add(ceremonies[0].Majors[0]);
        //    //ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    //EmailService.Expect(a => a.SendAddPermission(Arg<IRepository>.Is.Anything,
        //    //    Arg<Student>.Is.Anything,
        //    //    Arg<Ceremony>.Is.Anything)).Throw(new Exception("An Exception."));
        //    //#endregion Arrange

        //    //#region Act
        //    //Controller.AddStudentConfirm(studentId, majorId, new Student())
        //    //    .AssertActionRedirect()
        //    //    .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
        //    //#endregion Act

        //    //#region Assert
        //    //Assert.AreEqual("There was a problem sending FirstName1 LastName1 an email.", Controller.Message);
        //    //StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    //EmailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
        //    //var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
        //    //Assert.IsNotNull(args);
        //    //Assert.AreEqual(2, args.Majors.Count);
        //    //Assert.AreSame(ceremonies[0].Majors[0], args.Majors[0]);
        //    //Assert.AreSame(majors[0], args.Majors[1]);
        //    //#endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm post does not save if student has validation errors.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesNotSaveIfStudentHasValidationErrors()
        //{
        //    #region Arrange
        //    const string studentId = "1";
        //    const string majorId = "1";
        //    const string termCode = "201003";
        //    LoadTermCodes(termCode);
        //    var majors = new List<MajorCode>();
        //    majors.Add(CreateValidEntities.MajorCode(1));
        //    ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    var ceremonies = new List<Ceremony>();
        //    ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
        //    ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
        //    ceremonies[0].Majors.Add(majors[0]);
        //    ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    var students = new List<Student>();
        //    students.Add(CreateValidEntities.Student(1));
        //    students[0].StudentId = studentId;
        //    students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    students[0].Majors.Add(ceremonies[0].Majors[0]);
        //    students[0].Pidm = null; //Invalid
        //    ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    #endregion Arrange

        //    #region Act
        //    Controller.AddStudentConfirm(studentId, majorId, new Student())
        //        .AssertViewRendered()
        //        .WithViewData<Student>();
        //    #endregion Act

        //    #region Assert
        //    Assert.IsNull(Controller.Message);
        //    StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    Controller.ModelState.AssertErrorsAre("Pidm: may not be null or empty");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm post does save if student does not exist.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesSaveIfStudentDoesNotExist()
        //{
        //    Assert.Inconclusive("Review");
        //    //#region Arrange
        //    //const string studentId = "1";
        //    //const string majorId = "1";
        //    //const string termCode = "201003";
        //    //LoadTermCodes(termCode);
        //    //var majors = new List<MajorCode>();
        //    //majors.Add(CreateValidEntities.MajorCode(1));
        //    //ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    //var ceremonies = new List<Ceremony>();
        //    //ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    //ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
        //    //ceremonies[0].Majors.Add(majors[0]);
        //    //ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    //var students = new List<Student>();
        //    //students.Add(CreateValidEntities.Student(99));
        //    //ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    //#endregion Arrange

        //    //#region Act
        //    //Controller.AddStudentConfirm(studentId, majorId, CreateValidEntities.Student(3))
        //    //    .AssertActionRedirect()
        //    //    .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
        //    //#endregion Act

        //    //#region Assert
        //    //Assert.IsNull(Controller.Message);
        //    //StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    //EmailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
        //    //var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
        //    //Assert.IsNotNull(args);
        //    //Assert.AreEqual(1, args.Majors.Count);
        //    //Assert.AreSame(majors[0], args.Majors[0]);
        //    //Assert.AreEqual("FirstName3", args.FirstName);
        //    //Assert.AreNotEqual(Guid.Empty, args.Id);
        //    //Console.WriteLine(args.Id);
        //    //#endregion Assert
        //}

        ///// <summary>
        ///// Tests the add student confirm post does save if student does not exist for that term.
        ///// </summary>
        //[TestMethod, Ignore]
        //public void TestAddStudentConfirmPostDoesSaveIfStudentDoesNotExistForThatTerm()
        //{
        //    Assert.Inconclusive("Review");
        //    //#region Arrange
        //    //const string studentId = "1";
        //    //const string majorId = "1";
        //    //const string termCode = "201003";
        //    //LoadTermCodes(termCode);
        //    //var majors = new List<MajorCode>();
        //    //majors.Add(CreateValidEntities.MajorCode(1));
        //    //ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
        //    //var ceremonies = new List<Ceremony>();
        //    //ceremonies.Add(CreateValidEntities.Ceremony(1));
        //    //ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
        //    //ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
        //    //ceremonies[0].Majors.Add(majors[0]);
        //    //ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
        //    //var students = new List<Student>();
        //    //students.Add(CreateValidEntities.Student(3));
        //    //students[0].TermCode = CreateValidEntities.TermCode(9);
        //    //ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
        //    //#endregion Arrange

        //    //#region Act
        //    //Controller.AddStudentConfirm(students[0].StudentId, majorId, CreateValidEntities.Student(3))
        //    //    .AssertActionRedirect()
        //    //    .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
        //    //#endregion Act

        //    //#region Assert
        //    //Assert.IsNull(Controller.Message);
        //    //StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
        //    //EmailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
        //    //var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
        //    //Assert.IsNotNull(args);
        //    //Assert.AreEqual(1, args.Majors.Count);
        //    //Assert.AreSame(majors[0], args.Majors[0]);
        //    //Assert.AreEqual("FirstName3", args.FirstName);
        //    //Assert.AreNotEqual(Guid.Empty, args.Id);
        //    //Assert.AreEqual(students[0].StudentId, args.StudentId);
        //    //Assert.AreNotSame(students[0].TermCode, args.TermCode);
        //    //#endregion Assert
        //}
        //#endregion Post Tests

        #endregion AddStudentConfirm Tests
    }
}
