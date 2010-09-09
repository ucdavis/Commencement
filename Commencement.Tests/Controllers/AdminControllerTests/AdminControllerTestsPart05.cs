using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.Attributes;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    public partial class AdminControllerTests
    {
        #region ChangeMajor Tests
        #region Get Tests

        /// <summary>
        /// Tests the change major redirects when registration is not found.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorRedirectsWhenRegistrationIsNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeRegistration(2, RegistrationRepository);
            #endregion Arrange

            #region Act
            Controller.ChangeMajor(3)
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(null, null, null, null));
            #endregion Act

            #region Assert

            #endregion Assert
        }


        /// <summary>
        /// Tests the change major returns view when registration found.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorReturnsViewWhenRegistrationFound()
        {
            #region Arrange
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = CreateValidEntities.Student(3);
            ControllerRecordFakes.FakeRegistration(1, RegistrationRepository, registrations);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            majors.Add(CreateValidEntities.MajorCode(2));
            MajorService.Expect(a => a.GetAESMajors()).Return(majors.AsEnumerable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ChangeMajor(1)
                .AssertViewRendered()
                .WithViewData<ChangeMajorViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.MajorCodes.Count());
            Assert.AreEqual("FirstName3", result.Student.FirstName);
            Assert.AreSame(registrations[0], result.Registration);
            MajorService.AssertWasCalled(a => a.GetAESMajors());
            #endregion Assert		
        }
        #endregion Get Tests
        #region Post Tests

        /// <summary>
        /// Tests the change major post redirects when registration not found.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorPostRedirectsWhenRegistrationNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeRegistration(2, RegistrationRepository);
            ControllerRecordFakes.FakeMajors(2, MajorRepository);
            #endregion Arrange

            #region Act
            Controller.ChangeMajor(3, "1")
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(null, null, null, null));
            #endregion Act
            
            #region Assert
            Assert.AreEqual("Registration or major information was missing.", Controller.Message);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the change major post redirects when major not found.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorPostRedirectsWhenMajorNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeRegistration(2, RegistrationRepository);
            ControllerRecordFakes.FakeMajors(2, MajorRepository);
            #endregion Arrange

            #region Act
            Controller.ChangeMajor(1, "3")
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(null, null, null, null));
            #endregion Act

            #region Assert
            Assert.AreEqual("Registration or major information was missing.", Controller.Message);
            #endregion Assert
        }


        [TestMethod]
        public void TestRemainingTests()
        {
            #region Arrange
            Assert.Inconclusive("Test that the major is changed.");
            Assert.Inconclusive("Test that the ceremony is changed.");
            Assert.Inconclusive("Test that exceptions are thrown for private methods.");
            Assert.Inconclusive("Test that an invalid registration does not save.");
            Assert.Inconclusive("Test that a valid registration with major change is saved.");
            Assert.Inconclusive("Test that a valid registration with major change and ceremony is saved.");
            

            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert

            #endregion Assert		
        }
        //Test that major is assigned to registration
        //Test exceptions are thrown

        #endregion Post Tests
        #endregion ChangeMajor Tests
    }
}
