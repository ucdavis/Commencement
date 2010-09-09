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


        /// <summary>
        /// Tests the change major throws exception if no matching ceremony found for major and term.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestChangeMajorThrowsExceptionIfNoMatchingCeremonyFoundForMajorAndTerm1()
        {
            #region Arrange
            LoadTermCodes("201003");

            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            majors.Add(CreateValidEntities.MajorCode(2));

            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].Majors.Add(majors[0]);
            ceremonies[0].TermCode = CreateValidEntities.TermCode(99); //Does Not Match current term
            
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremonies[0];
            registrations[0].Major = majors[1];
            
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);  
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            try
            {
                #region Act
                Controller.ChangeMajor(1, "1");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Ceremony is required.", ex.Message);
                #endregion Assert

                throw;
            }		
        }

        /// <summary>
        /// Tests the change major throws exception if no matching ceremony found for major and term.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestChangeMajorThrowsExceptionIfNoMatchingCeremonyFoundForMajorAndTerm2()
        {
            #region Arrange
            LoadTermCodes("201003");

            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            majors.Add(CreateValidEntities.MajorCode(2));

            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].Majors.Add(majors[0]);
            ceremonies[0].TermCode = TermService.GetCurrent();

            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremonies[0];
            registrations[0].Major = majors[1];

            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            try
            {
                #region Act
                Controller.ChangeMajor(1, "2");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Ceremony is required.", ex.Message);
                #endregion Assert

                throw;
            }
        }


        /// <summary>
        /// Tests the change major does not save when different ceremony has no available tickets.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorDoesNotSaveWhenDifferentCeremonyHasAvailableTickets()
        {
            #region Arrange
            LoadTermCodes("201003");

            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            majors.Add(CreateValidEntities.MajorCode(2));

            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].Majors.Add(majors[0]);
            ceremonies[0].TermCode = TermService.GetCurrent();
            ceremonies.Add(CreateValidEntities.Ceremony(2));
            ceremonies[1].Majors.Add(majors[1]);
            ceremonies[1].TermCode = TermService.GetCurrent();

            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremonies[0];
            registrations[0].Major = majors[0];

            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            ControllerRecordFakes.FakeMajors(0, MajorRepository, majors);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            MajorService.Expect(a => a.GetAESMajors()).Return(majors.AsEnumerable()).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ChangeMajor(1, "2")
                .AssertViewRendered()
                .WithViewData<ChangeMajorViewModel>();
            #endregion Act

            #region Assert
            Controller.ModelState.AssertErrorsAre("There are enough tickets to move this students major.Student will be moved into a different ceremony if you proceed.");
            Assert.IsFalse(Controller.ModelState.IsValid);
            RegistrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            MajorService.AssertWasCalled(a => a.GetAESMajors());
            Assert.IsNotNull(result);
            Assert.AreSame(majors[1], result.Registration.Major);
            Assert.AreSame(ceremonies[1], result.Registration.Ceremony, "Depending on how this should work, changing the major should also be able to change the ceremony.");
            #endregion Assert		
        }

        [TestMethod]
        public void TestRemainingTests()
        {
            #region Arrange
            Assert.Inconclusive("Test that the major is changed.");
            Assert.Inconclusive("Test that the ceremony is changed."); //Review
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
