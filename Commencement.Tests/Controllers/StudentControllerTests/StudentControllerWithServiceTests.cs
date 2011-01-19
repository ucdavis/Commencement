using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
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
        #region StudentEligibility Tests (This is an interface)


        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors1, 201);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors2, 201);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult3()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors1, 300);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentEligibilityReturnsExpectedResult4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var majors1 = new List<MajorCode>();
            var majors2 = new List<MajorCode>();

            for (int i = 0; i < 5; i++)
            {
                majors1.Add(CreateValidEntities.MajorCode(i + 1));
            }

            majors2.Add(CreateValidEntities.MajorCode(9));


            CeremonyService = new CeremonyService(Controller.Repository);
            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 7; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].PetitionThreshold = 200;
                ceremonies[i].TermCode = TermCodeRepository.Queryable.First();
                Assert.IsNotNull(ceremonies[i].TermCode);
                ceremonies[i].Majors = majors1;
            }

            ceremonies[0].TermCode = CreateValidEntities.TermCode(9);
            Assert.IsNotNull(ceremonies[0].TermCode);
            ceremonies[1].Majors = majors2;
            ceremonies[2].PetitionThreshold = 300;
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            #endregion Arrange

            #region Act
            var result = CeremonyService.StudentEligibility(majors1, 201, ceremonies[0].TermCode);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Location1", result[0].Location);
            #endregion Assert
        }


        #endregion StudentEligibility Tests
    }
}
