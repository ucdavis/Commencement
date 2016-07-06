using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.ActionResults;

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests
    {
        #region ExtraTicketPetition Get Tests

        [TestMethod]
        public void TestExtraTicketPetitionRedirectsToStudentIndexIfRegistrationNotFound()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[]{""});
            var student = CreateValidEntities.Student(1);
            student.Login = "UserName";
            var registrations = new List<Registration>();
            for (int i = 0; i < 3; i++)
            {
                registrations.Add(CreateValidEntities.Registration(i+1));
                registrations[i].Student = student;
            }
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.ExtraTicketPetition(4)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert

            #endregion Assert		
        }


        [TestMethod]
        public void TestExtraTicketPetitionRedirectsToStudentIndexIfStudentDifferent()
        {
            #region Arrange
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { "" });
            var student = CreateValidEntities.Student(1);
            student.Login = "UserName";
            var registrations = new List<Registration>();
            for (int i = 0; i < 3; i++)
            {
                registrations.Add(CreateValidEntities.Registration(i + 1));
                registrations[i].Student = student;
            }
            registrations[1].Student = CreateValidEntities.Student(9);
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.ExtraTicketPetition(2)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert

            #endregion Assert
        }


        [TestMethod]
        public void TestDescription()
        {
            #region Arrange

            Assert.Inconclusive("Continue tests");

            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert

            #endregion Assert		
        }

        #endregion ExtraTicketPetition Get Tests
    }
}
