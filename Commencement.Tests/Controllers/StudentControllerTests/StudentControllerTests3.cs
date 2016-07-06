using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers;
using Commencement.Mvc.Controllers.Helpers;
using Commencement.Mvc.Controllers.ViewModels;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.StudentControllerTests
{
    public partial class StudentControllerTests
    {
        #region Register Post Tests

        [TestMethod]
        public void TestRegisterPostRedirectsToStudentsIfStudentIsNotFound()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(null, null, null, null));
            #endregion Act

            #region Assert

            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected1()
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
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected3()
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
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository, true, 0);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected5()
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
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.SJA());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected6()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "UserName";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.First();
            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected7()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            var colleges = new List<College>();
            colleges.Add(CreateValidEntities.College(1));
            colleges.Add(CreateValidEntities.College(2));
            colleges.Add(CreateValidEntities.College(3));
            colleges.Add(CreateValidEntities.College(4));
            for (int i = 0; i < 4; i++)
            {
                colleges[i].SetIdTo((i + 1).ToString());
            }

            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(1));
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.Majors[0].College = colleges[3];
            student.Majors[1].College = colleges[3];
            student.Majors[2].College = colleges[3];
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = CreateValidEntities.TermCode(99);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "UserName";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = colleges[3];

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.PreviouslyWalked());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected8()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(null).Repeat.Any();

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "UserName";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = CeremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything))[0];
            Assert.AreEqual(4, args.Count());
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
        public void TestRegisterPostRedirectsAsExpected9()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(new List<Ceremony>()).Repeat.Any();

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "UserName";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = CeremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything))[0];
            Assert.AreEqual(4, args.Count());
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
        public void TestRegisterPostRedirectsAsExpected10()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "UserName";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = CeremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything))[0];
            Assert.AreEqual(4, args.Count());
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
        public void TestRegisterPostWithoutAgreementDoesNotSave()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = false};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "Blah";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act

            var result = Controller.Register(registrationPostModel)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(2));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You must agree to the disclaimer");

            Assert.IsNotNull(result);
            Assert.AreEqual("Pidm1", result.Student.Pidm);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithInvalidRegistrationDoesNotSave()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
            registration.Address1 = string.Empty; //Invalid
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = true};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "UserName";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(2));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("Address1: may not be null or empty");

            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithRegistrationPetitionWithMissingReasonDoesNotSave()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
            var registrationPetition = CreateValidEntities.RegistrationPetition(1);
            registrationPetition.ExceptionReason = " ";
            registration.AddPetition(registrationPetition); //Invalid
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = true};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();


            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "Blah";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(2));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("Exception reason is required.");

            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithNoParticipationsOrPetitionsInRegistrationRedirects()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
            //var registrationPetition = CreateValidEntities.RegistrationPetition(1);
            //registration.AddPetition(registrationPetition);
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = true};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "Blah";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.First();

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);

            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(1));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsTrue(Controller.ModelState.IsValid);


            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithValidRegistrationSaves()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
            var registrationPetition = CreateValidEntities.RegistrationPetition(1);
            registration.AddPetition(registrationPetition);
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = true};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();


            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "Blah";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);

            //EmailService.Expect(a => a.QueueRegistartionPetitionDecision(Arg<Registration>.Is.Anything));
            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(1));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual("You have successfully submitted your registration petition.", Controller.Message);
            EmailService.AssertWasCalled(a => a.QueueRegistrationPetition(Arg<Registration>.Is.Anything));
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithValidRegistrationSavesAndErrorIsRecordedIfEmailProblem()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
            var registrationPetition = CreateValidEntities.RegistrationPetition(1);
            registration.AddPetition(registrationPetition);
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = true};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();


            EmailService.Expect(a => a.QueueRegistrationPetition(Arg<Registration>.Is.Anything)).Throw(new Exception("Test Exception"));

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "Blah";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);

            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(1));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(" There was a problem sending you an email.  Please print this page for your records.You have successfully submitted your registration petition.", Controller.Message);
            EmailService.AssertWasCalled(a => a.QueueRegistrationPetition(Arg<Registration>.Is.Anything));
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything));
            ErrorService.AssertWasCalled(a => a.ReportError(Arg<Exception>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithValidRegistrationSaves2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
            var registrationParticipation = CreateValidEntities.RegistrationParticipation(1);
            registration.RegistrationParticipations.Add(registrationParticipation);
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = true};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();

            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "Blah";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);

            //EmailService.Expect(a => a.QueueRegistartionPetitionDecision(Arg<Registration>.Is.Anything));
            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(1));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationPetition(Arg<Registration>.Is.Anything));
            EmailService.AssertWasCalled(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithValidRegistrationSavesAndErrorIsRecordedIfEmailProblem2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
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
            var registrationParticipation = CreateValidEntities.RegistrationParticipation(1);
            registration.RegistrationParticipations.Add(registrationParticipation);
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
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel {AgreeToDisclaimer = true};
            RegistrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();


            EmailService.Expect(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything)).Throw(new Exception("Test Exception"));


            var participations = new List<RegistrationParticipation>();
            participations.Add(CreateValidEntities.RegistrationParticipation(1));
            participations[0].Registration = CreateValidEntities.Registration(55);
            participations[0].Registration.Student = CreateValidEntities.Student(11);
            participations[0].Registration.Student.Login = "Blah";
            participations[0].Registration.TermCode = TermCodeRepository.Queryable.Last();
            participations[0].Major = CreateValidEntities.MajorCode(1);
            participations[0].Major.College = CreateValidEntities.College(9);

            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);
            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            StudentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            CeremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything, Arg<int?>.Is.Anything), x => x.Repeat.Times(1));
            RegistrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsTrue(Controller.ModelState.IsValid);
            Assert.AreEqual(" There was a problem sending you an email.  Please print this page for your records.You have successfully registered for commencement.", Controller.Message);
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationPetition(Arg<Registration>.Is.Anything));
            EmailService.AssertWasCalled(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything));
            ErrorService.AssertWasCalled(a => a.ReportError(Arg<Exception>.Is.Anything));
            #endregion Assert
        }
        #endregion Register Post Tests
    }
}
