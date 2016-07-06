using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers;
using Commencement.Mvc.Controllers.Filters;
using Commencement.Mvc.Controllers.ViewModels;
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
        [TestMethod]
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
        [TestMethod]
        public void TestStudentDetailsViewWhenStudentIsFound()
        {
            #region Arrange

            //LoadTermCodes("201003");
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
            }

            CeremonyService.Expect(a => a.GetCeremonies("UserName")).Return(
                ceremonies).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            ControllerRecordFakes.FakeStudent(3, StudentRepository, students, null);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = students[0];
            ControllerRecordFakes.FakeRegistration(3, RegistrationRepository, registrations);
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            ControllerRecordFakes.FakevTermCode(4, vTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(3, specialNeedRepository);

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

        #region Block Tests

        #region Get Tests

        [TestMethod]
        public void TestBlockGetRedirectsToIndexIfStudentNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            #endregion Arrange

            #region Act
            Controller.Block(SpecificGuid.GetGuid(4))
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Student record could not be found.", Controller.Message);
            #endregion Assert		
        }

        [TestMethod]
        public void TestBlockGetReturnsViewIfStudentFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            #endregion Arrange

            #region Act
            var result = Controller.Block(SpecificGuid.GetGuid(3))
                .AssertViewRendered()
                .WithViewData<Student>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pidm3", result.Pidm);
            Assert.IsNull(Controller.Message);
            #endregion Assert
        }
        


        #endregion Get Tests

        #region Post Tests
        [TestMethod]
        public void TestBlockPostRedirectsToIndexIfStudentNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            #endregion Arrange

            #region Act
            Controller.Block(SpecificGuid.GetGuid(4), true, "Cause")
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Student record could not be found.", Controller.Message);
            #endregion Assert
        }


        [TestMethod]
        public void TestBlockPostRedirectsToStudentDetailsIfStudentFound1()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            #endregion Arrange

            #region Act
            var result = Controller.Block(SpecificGuid.GetGuid(3), true, "Cause")
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.StudentDetails(SpecificGuid.GetGuid(3), false));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SpecificGuid.GetGuid(3), result.RouteValues["id"]);
            Assert.AreEqual(false, result.RouteValues["Registration"]);
            StudentRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            var args = (Student)StudentRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual("Pidm3", args.Pidm);
            Assert.AreEqual(true, args.Blocked);
            Assert.AreEqual(false, args.SjaBlock);
            Assert.AreEqual("Student has been blocked from the registration system.", Controller.Message);
            #endregion Assert		
        }

        [TestMethod]
        public void TestBlockPostRedirectsToStudentDetailsIfStudentFound2()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            #endregion Arrange

            #region Act
            var result = Controller.Block(SpecificGuid.GetGuid(3), true, "SJA")
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.StudentDetails(SpecificGuid.GetGuid(3), false));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SpecificGuid.GetGuid(3), result.RouteValues["id"]);
            Assert.AreEqual(false, result.RouteValues["Registration"]);
            StudentRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            var args = (Student)StudentRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual("Pidm3", args.Pidm);
            Assert.AreEqual(true, args.Blocked);
            Assert.AreEqual(false, args.SjaBlock);
            Assert.AreEqual("Student has been blocked from the registration system.", Controller.Message);
            #endregion Assert
        }

        [TestMethod]
        public void TestBlockPostRedirectsToStudentDetailsIfStudentFound3()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null);
            #endregion Arrange

            #region Act
            var result = Controller.Block(SpecificGuid.GetGuid(3), true, "sja")
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.StudentDetails(SpecificGuid.GetGuid(3), false));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SpecificGuid.GetGuid(3), result.RouteValues["id"]);
            Assert.AreEqual(false, result.RouteValues["Registration"]);
            StudentRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            var args = (Student)StudentRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual("Pidm3", args.Pidm);
            Assert.AreEqual(false, args.Blocked);
            Assert.AreEqual(true, args.SjaBlock);
            Assert.AreEqual("Student has been blocked from the registration system.", Controller.Message);
            #endregion Assert
        }

        [TestMethod]
        public void TestBlockPostRedirectsToStudentDetailsIfStudentFound4()
        {
            #region Arrange
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].SjaBlock = true;
            students[0].Blocked = true;
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, null);
            #endregion Arrange

            #region Act
            var result = Controller.Block(SpecificGuid.GetGuid(1), false, "sja")
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.StudentDetails(SpecificGuid.GetGuid(7), false));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SpecificGuid.GetGuid(1), result.RouteValues["id"]);
            Assert.AreEqual(false, result.RouteValues["Registration"]);
            StudentRepository.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            var args = (Student)StudentRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual("Pidm1", args.Pidm);
            Assert.AreEqual(false, args.Blocked);
            Assert.AreEqual(false, args.SjaBlock);
            Assert.AreEqual("Student has been unblocked and is allowed into the system.", Controller.Message);
            #endregion Assert
        }

        #endregion Post Tests


        #endregion Block Tests

    }
}
