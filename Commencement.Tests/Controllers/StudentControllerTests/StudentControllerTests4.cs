using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Commencement.Controllers;
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
            Assert.Inconclusive("Test the result");
            #endregion Assert
        }

        #endregion DisplayRegistration Tests
    }
}