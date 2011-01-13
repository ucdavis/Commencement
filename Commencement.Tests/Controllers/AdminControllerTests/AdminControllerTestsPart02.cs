using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Commencement.Controllers.Filters;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Core.Resources;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    public partial class AdminControllerTests
    {

        #region Misc Tests        
        [TestMethod]
        public void TestIndexReturnsView()
        {
            #region Assert
            Controller.Index().AssertViewRendered();
            #endregion Assert		
        }

        [TestMethod]
        public void TestAdminLandingReturnsView()
        {
            #region Assert
            Controller.AdminLanding().AssertViewRendered();
            #endregion Assert
        }
        #endregion Misc Tests

        #region Students Tests (List)


        [TestMethod]
        public void TestStudentsReturnsViewModelWithExpectedData1()
        {
            #region Arrange
            LoadTermCodes("201003");
            ControllerRecordFakes.FakeStudent(3, StudentRepository, null, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(9);
            ceremony.TermCode = TermCodeRepository.Queryable.First();
            var registrationParticipations = new List<RegistrationParticipation>();
            for (int i = 0; i < 3; i++)
            {
                registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(i+1));
                registrationParticipations[i].Ceremony = ceremony;
            }

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository,registrationParticipations );
            #endregion Arrange

            #region Act
            var result = Controller.Students("123456789", null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123456789", result.studentidFilter);
            Assert.IsNull(result.lastNameFilter);
            Assert.IsNull(result.firstNameFilter);
            Assert.IsNull(result.majorCodeFilter);
            Assert.AreEqual(0, result.StudentRegistrationModels.Count);
            #endregion Assert		
        }


        [TestMethod]
        public void TestStudentsReturnsViewModelWithExpectedData2()
        {
            #region Arrange
            LoadTermCodes("201003");
            var students = new List<Student>();
            for (int i = 0; i < 5; i++)
            {
                students.Add(CreateValidEntities.Student(i+1));
                students[i].TermCode = TermCodeRepository.Queryable.First();
                students[i].StudentId = "123456789";
            }
            ControllerRecordFakes.FakeStudent(3, StudentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(9);
            ceremony.TermCode = TermCodeRepository.Queryable.First();

            var registrationParticipations = new List<RegistrationParticipation>();
            for (int i = 0; i < 3; i++)
            {
                registrationParticipations.Add(CreateValidEntities.RegistrationParticipation(i + 1));
                registrationParticipations[i].Ceremony = ceremony;
                registrationParticipations[i].Registration = CreateValidEntities.Registration(i + 1);
                registrationParticipations[i].Registration.Student = students[i];
            }

            ControllerRecordFakes.FakeRegistrationParticipation(0, RegistrationParticipationRepository, registrationParticipations);
            #endregion Arrange

            #region Act
            var result = Controller.Students("123456789", null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("123456789", result.studentidFilter);
            Assert.IsNull(result.lastNameFilter);
            Assert.IsNull(result.firstNameFilter);
            Assert.IsNull(result.majorCodeFilter);
            Assert.AreEqual(5, result.StudentRegistrationModels.Count);
            Assert.AreEqual(3, result.StudentRegistrationModels.Where(a => a.Registration).Count());
            Assert.AreEqual(2, result.StudentRegistrationModels.Where(a => !a.Registration).Count());
            #endregion Assert
        }
        #endregion Students Tests (List)

        #region Registrations Tests (List)

        [TestMethod]
        public void TestRegistrationsReturnsViewWithExpectedResult1()
        {
            #region Arrange
            LoadTermCodes("201003");
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i+1));
            }

            CeremonyService.Expect(a => a.GetCeremonies("UserName", TermCodeRepository.Queryable.First())).Return(
                ceremonies).Repeat.Any();
            

            #endregion Arrange

            #region Act
            var result = Controller.Registrations("123456789", null, null, null, null,null)
                .AssertViewRendered()
                .WithViewData<AdminRegistrationViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Ceremonies.Count());
            Assert.AreEqual("123456789", result.studentidFilter);
            #endregion Assert		
        }

        [TestMethod]
        public void TestRegistrationsReturnsViewWithExpectedResult2()
        {
            #region Arrange
            LoadTermCodes("201003");
            Controller.ControllerContext.HttpContext = new MockHttpContext(1, new[] { RoleNames.RoleUser });
            var colleges = new List<College>();
            colleges.Add(CreateValidEntities.College(1));
            colleges.Add(CreateValidEntities.College(2));
            colleges.Add(CreateValidEntities.College(3));
            colleges.Add(CreateValidEntities.College(4));

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 4; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
            }
            ceremonies[0].Colleges.Add(colleges[0]);
            ceremonies[0].Colleges.Add(colleges[2]);

            ceremonies[1].Colleges.Add(colleges[3]);
            ceremonies[1].Colleges.Add(colleges[2]);

            ceremonies[3].Colleges.Add(colleges[2]);

            CeremonyService.Expect(a => a.GetCeremonies("UserName", TermCodeRepository.Queryable.First())).Return(
                ceremonies).Repeat.Any();


            #endregion Arrange

            #region Act
            var result = Controller.Registrations("123456789", null, null, null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminRegistrationViewModel>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Ceremonies.Count());
            Assert.AreEqual(3, result.Colleges.Count());
            Assert.AreEqual("123456789", result.studentidFilter);
            #endregion Assert
        }


        #endregion Registrations Tests (List)



        


    }
}
