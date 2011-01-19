using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;

namespace Commencement.Tests.Controllers.StudentControllerTests
{
    public partial class StudentControllerTests
    {
        #region DisplayRegistration Tests


        [TestMethod]
        public void TestDisplayRegistrationRedirectsToIndexIfCurrentStudentNotFound()
        {
            #region Arrange
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.DisplayRegistration()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Student record could not be found.", Controller.Message);
            #endregion Assert		
        }

        [TestMethod]
        public void TestDisplayRegistrationRedirectsToIndexIfCurrentStudentNotFoundInRegistration()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            var petition = CreateValidEntities.RegistrationPetition(1);
            registration.AddPetition(petition);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(2));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.DisplayRegistration()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            #endregion Assert
        }

        [TestMethod]
        public void TestDisplayRegistrationReturnsViewIfCurrentStudentFoundInRegistration()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            var petition = CreateValidEntities.RegistrationPetition(1);
            registration.AddPetition(petition);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            var participation = CreateValidEntities.RegistrationParticipation(3);
            participation.Registration = registration;
            registration.RegistrationParticipations.Add(participation);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            var participations = new List<RegistrationParticipation>();
            participations.Add(participation);
            ControllerRecordFakes.FakeRegistrationParticipation(0, ParticipationRepository, participations);

            #endregion Arrange

            #region Act
            var result = Controller.DisplayRegistration()
                .AssertViewRendered()
                .WithViewData<StudentDisplayRegistrationViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsNull(Controller.Message);
            Assert.IsFalse(result.CanEditRegistration);
            Assert.IsFalse(result.CanPetitionForExtraTickets);
            Assert.AreEqual("Address12", result.Registration.Address1);
            #endregion Assert
        }

        #endregion DisplayRegistration Tests

        #region EditRegistration Get Tests

        [TestMethod]
        public void TestEditRegistrationGetRedirectsToIndexIfRegistrationIsNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(3)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            #endregion Assert		
        }

        [TestMethod]
        public void TestEditRegistrationGetRedirectsToIndexIfRegistrationIsFoundButDoesNotHaveCurrentStudent()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(2)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationGetRedirectsToErrorNotOpenIfNoOpenCeremonies1()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(1)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert

            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationGetRedirectsToErrorNotOpenIfNoOpenCeremonies2()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.RegistrationBegin = DateTime.Now.AddDays(10);
            var participation = CreateValidEntities.RegistrationParticipation(1);
            participation.Ceremony = ceremony;
            registration.RegistrationParticipations.Add(participation);
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(1)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert

            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationGetReturnsView()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
            ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
            var participation = CreateValidEntities.RegistrationParticipation(1);
            participation.Ceremony = ceremony;
            registration.RegistrationParticipations.Add(participation);
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(ceremony);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(ceremonies).Repeat.Any();
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            ControllerRecordFakes.FakevTermCode(3, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(3, SpecialNeedRepository);
            #endregion Arrange

            #region Act
            var result = Controller.EditRegistration(1)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Address12", result.Registration.Address1);
            #endregion Assert
        }

        #endregion EditRegistration Get Tests

        #region EditRegistration Post Tests
        [TestMethod]
        public void TestEditRegistrationPostRedirectsToIndexIfRegistrationIsNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(3, new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationPostRedirectsToIndexIfRegistrationIsFoundButDoesNotHaveCurrentStudent()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(2, new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationPostRedirectsToErrorNotOpenIfNoOpenCeremonies1()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(1, new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert

            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationPostRedirectsToErrorNotOpenIfNoOpenCeremonies2()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.RegistrationBegin = DateTime.Now.AddDays(10);
            var participation = CreateValidEntities.RegistrationParticipation(1);
            participation.Ceremony = ceremony;
            registration.RegistrationParticipations.Add(participation);
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.EditRegistration(1, new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert

            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationPostReturnsViewIfNotValid()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Address1 = string.Empty; //Invalid
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            registration.TermCode = CreateValidEntities.TermCode(2);
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
            ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
            var participation = CreateValidEntities.RegistrationParticipation(1);
            participation.Ceremony = ceremony;
            registration.RegistrationParticipations.Add(participation);
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(ceremony);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(ceremonies).Repeat.Any();
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            ControllerRecordFakes.FakevTermCode(3, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(3, SpecialNeedRepository);
            RegistrationPopulator.Expect(a =>a.UpdateRegistration(
                Arg<Registration>.Is.Anything, 
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything, 
                Arg<ModelStateDictionary>.Is.Anything, 
                Arg<bool>.Is.Anything)).Repeat.Any();

            #endregion Arrange

            #region Act            
            var result = Controller.EditRegistration(1, new RegistrationPostModel{SpecialNeeds = new List<string>{"test1", "test2"}})
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Controller.ModelState.AssertErrorsAre("Address1: may not be null or empty");
            Assert.IsFalse(Controller.ModelState.IsValid);
            RegistrationPopulator.AssertWasCalled(a => a.UpdateRegistration(
                Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything));
            var args = RegistrationPopulator.GetArgumentsForCallsMadeOn(a => a.UpdateRegistration(Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything))[0];
            Assert.IsNotNull(args);
            Assert.AreEqual("City2", ((Registration)args[0]).City);
            Assert.AreEqual(5, args.Count());
            Assert.AreEqual("test2", ((RegistrationPostModel)args[1]).SpecialNeeds[1]);
            Assert.AreEqual(SpecificGuid.GetGuid(1), ((Student)args[2]).Id);
            ((ModelStateDictionary)args[3]).AssertErrorsAre("Address1: may not be null or empty");
            Assert.IsFalse((bool)args[4]);

            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            EmailService.AssertWasNotCalled(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything));
            ErrorService.AssertWasNotCalled(a => a.ReportError(Arg<Exception>.Is.Anything));

            Assert.IsNull(Controller.Message);

            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationPostRedirectsToDisplayRegistration1()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            registration.TermCode = CreateValidEntities.TermCode(2);
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
            ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
            var participation = CreateValidEntities.RegistrationParticipation(1);
            participation.Ceremony = ceremony;
            registration.RegistrationParticipations.Add(participation);
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(ceremony);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(ceremonies).Repeat.Any();
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            ControllerRecordFakes.FakevTermCode(3, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(3, SpecialNeedRepository);
            RegistrationPopulator.Expect(a => a.UpdateRegistration(
                Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything)).Repeat.Any();
            RegistrationRepository.Expect(a => a.EnsurePersistent(RegistrationRepository.GetNullableById(1)));
            EmailService.Expect(a => a.QueueRegistrationConfirmation(RegistrationRepository.GetNullableById(1)));
            #endregion Arrange

            #region Act
            var result = Controller.EditRegistration(1,
                new RegistrationPostModel
                    {SpecialNeeds = new List<string> {"test1", "test2"}})
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            RegistrationPopulator.AssertWasCalled(a => a.UpdateRegistration(
                Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything));
            var args = RegistrationPopulator.GetArgumentsForCallsMadeOn(a => a.UpdateRegistration(Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything))[0];
            Assert.IsNotNull(args);
            Assert.AreEqual("City2", ((Registration)args[0]).City);
            Assert.AreEqual(5, args.Count());
            Assert.AreEqual("test2", ((RegistrationPostModel)args[1]).SpecialNeeds[1]);
            Assert.AreEqual(SpecificGuid.GetGuid(1), ((Student)args[2]).Id);
            Assert.IsTrue(((ModelStateDictionary)args[3]).IsValid);
            Assert.IsFalse((bool)args[4]);

            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(RegistrationRepository.GetNullableById(1)));
            EmailService.AssertWasCalled(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything));
            ErrorService.AssertWasNotCalled(a => a.ReportError(Arg<Exception>.Is.Anything));

            Assert.AreEqual("You have successfully edited your commencement registration. ", Controller.Message);
            #endregion Assert
        }

        [TestMethod]
        public void TestEditRegistrationPostRedirectsToDisplayRegistration2()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            StudentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(StudentRepository.Queryable.First()).Repeat.Any();
            var registration = CreateValidEntities.Registration(2);
            registration.Student = StudentRepository.GetNullableById(SpecificGuid.GetGuid(1));
            registration.TermCode = CreateValidEntities.TermCode(2);
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.RegistrationBegin = DateTime.Now.AddDays(-10);
            ceremony.RegistrationDeadline = DateTime.Now.AddDays(10);
            var participation = CreateValidEntities.RegistrationParticipation(1);
            participation.Ceremony = ceremony;
            registration.RegistrationParticipations.Add(participation);
            var registrations = new List<Registration>();
            registrations.Add(registration);
            registration = CreateValidEntities.Registration(3);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(ceremony);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(ceremonies).Repeat.Any();
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            ControllerRecordFakes.FakevTermCode(3, VTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(3, SpecialNeedRepository);
            RegistrationPopulator.Expect(a => a.UpdateRegistration(
                Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything)).Repeat.Any();
            RegistrationRepository.Expect(a => a.EnsurePersistent(RegistrationRepository.GetNullableById(1)));
            EmailService.Expect(a => a.QueueRegistrationConfirmation(RegistrationRepository.GetNullableById(1))).Throw(new Exception("Test Exception"));
            #endregion Arrange

            #region Act
            var result = Controller.EditRegistration(1,
                new RegistrationPostModel { SpecialNeeds = new List<string> { "test1", "test2" } })
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Controller.ModelState.IsValid);
            RegistrationPopulator.AssertWasCalled(a => a.UpdateRegistration(
                Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything));
            var args = RegistrationPopulator.GetArgumentsForCallsMadeOn(a => a.UpdateRegistration(Arg<Registration>.Is.Anything,
                Arg<RegistrationPostModel>.Is.Anything,
                Arg<Student>.Is.Anything,
                Arg<ModelStateDictionary>.Is.Anything,
                Arg<bool>.Is.Anything))[0];
            Assert.IsNotNull(args);
            Assert.AreEqual("City2", ((Registration)args[0]).City);
            Assert.AreEqual(5, args.Count());
            Assert.AreEqual("test2", ((RegistrationPostModel)args[1]).SpecialNeeds[1]);
            Assert.AreEqual(SpecificGuid.GetGuid(1), ((Student)args[2]).Id);
            Assert.IsTrue(((ModelStateDictionary)args[3]).IsValid);
            Assert.IsFalse((bool)args[4]);

            RegistrationRepository.AssertWasCalled(a => a.EnsurePersistent(RegistrationRepository.GetNullableById(1)));
            EmailService.AssertWasCalled(a => a.QueueRegistrationConfirmation(Arg<Registration>.Is.Anything));
            ErrorService.AssertWasCalled(a => a.ReportError(Arg<Exception>.Is.Anything));

            Assert.AreEqual("You have successfully edited your commencement registration.  There was a problem sending you an email.  Please print this page for your records.", Controller.Message);
            #endregion Assert
        }

        #endregion EditRegistration Post Tests        
    }
}