using System;
using Commencement.Controllers;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.Services;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.PetitionControllerTests
{
    public partial class PetitionControllerTests
    {
        #region Index Tests


        [TestMethod]
        public void TestIndexReturnsView()
        {
            Controller.Index()
                .AssertViewRendered();
        }

        #endregion Index Tests

        #region ExtraTicketPetitions Tests


        [TestMethod]
        public void TestDescription()
        {
            #region Arrange

            Assert.Inconclusive("Write these tests");

            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert

            #endregion Assert		
        }

        #endregion ExtraTicketPetitions Tests
    }
}
