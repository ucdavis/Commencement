using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Commencement.Controllers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Commencement.Tests.Controllers.StudentControllerTests
{
    public partial class StudentControllerTests
    {

        #region Register Get Tests
        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            //var students = new List<Student>();
            //var student = CreateValidEntities.Student(1);
            //student.Blocked = true;
            //students.Add(student);
            //ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected3()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository, true);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected5()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.SjaBlock = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.SJA());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected6()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected7()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = CreateValidEntities.TermCode(99);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.PreviouslyWalked());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected8()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);

            CeremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(null).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = CeremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected9()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);

            CeremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(new List<Ceremony>()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = CeremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected10()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }

            CeremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = CeremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetReturnsView()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }
            ceremonies[1].RegistrationBegin = DateTime.Now.AddDays(-5);

            CeremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);
            #endregion Arrange

            #region Act

            var result = Controller.Register()
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = CeremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SpecialNeeds.Count());
            Assert.AreEqual(0, result.Registration.Id);
            Assert.AreEqual("Pidm1", result.Student.Pidm);
            #endregion Assert
        }
        #endregion Register Get Tests


    }
}
