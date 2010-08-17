using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commencement.Controllers;
//using Commencement.Controllers.Filter;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;
using Commencement.Tests.Core.Extensions;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTests : ControllerTestBase<StudentController>
    {
        private readonly Type _controllerClass = typeof(StudentController);
        private IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private IRepository<Ceremony> _ceremonyRepository;
        private IRepository<Registration> _registrationRepository;
        private IStudentService _studentService;
        private IEmailService _emailService;

        #region Init


        protected override void SetupController()
        {

            _studentService = MockRepository.GenerateStub<IStudentService>();
            _emailService = MockRepository.GenerateStub<IEmailService>();
            _ceremonyRepository = MockRepository.GenerateStub<IRepository<Ceremony>>();
            _registrationRepository = MockRepository.GenerateStub<IRepository<Registration>>();
            _studentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();

            Controller = new TestControllerBuilder().CreateController<StudentController>
                (_studentService,
                _emailService, 
                _studentRepository,
                _ceremonyRepository,
                _registrationRepository);
        }
        /// <summary>
        /// Registers the routes.
        /// </summary>
        protected override void RegisterRoutes()
        {
            new RouteConfigurator().RegisterRoutes();
        }
        #endregion Init

        #region Mapping

        /// <summary>
        /// Tests the index mapping.
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Student/Index".ShouldMapTo<StudentController>(a => a.Index());
        }

        /// <summary>
        /// Tests the choose ceremony mapping.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyMapping()
        {
            "~/Student/ChooseCeremony".ShouldMapTo<StudentController>(a => a.ChooseCeremony());
        }

        /// <summary>
        /// Tests the display registration mapping.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationMapping()
        {
            "~/Student/DisplayRegistration/5".ShouldMapTo<StudentController>(a => a.DisplayRegistration(5));
        }

        /// <summary>
        /// Tests the registration confirmation mapping.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationMapping()
        {
            "~/Student/RegistrationConfirmation/5".ShouldMapTo<StudentController>(a => a.RegistrationConfirmation(5));
        }

        [TestMethod]
        public void TestRegisterGetMapping()
        {
            //"~/Student/Register/?id=5&major=AABB1".ShouldMapTo<StudentController>(a => a.Register(5, "AABB1"));
            "~/Student/Register/5".ShouldMapTo<StudentController>(a => a.Register(5, "AABB1"),true);
        }

        [TestMethod]
        public void TestRegisterPutMapping()
        {
            //"~/Student/Register/?id=5&major=AABB1".ShouldMapTo<StudentController>(a => a.Register(5, "AABB1"));
            "~/Student/Register/5".ShouldMapTo<StudentController>(a => a.Register(5, new Registration(), true), true);
        }

        [TestMethod]
        public void TestEditRegistrationGetMapping()
        {
            "~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5));
        }
        [TestMethod]
        public void TestEditRegistrationPutMapping()
        {
            "~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5, new Registration(), true), true);
        }

        [TestMethod]
        public void TestNoCeremonyMapping()
        {
            "~/Student/NoCeremony".ShouldMapTo<StudentController>(a => a.NoCeremony());
        }

        
        #endregion Mapping

        #region Index Tests


        [TestMethod]
        public void TestIndexRedirectsToDisplayRegistrationWhenPriorRegistrationIsNull()
        {
            #region Arrange
            _studentService.Expect(a => a.GetPriorRegistration(Arg<Student>.Is.Anything)).Return(null).Repeat.Any();
            //_studentService.Expect(a => a.GetCurrentStudent(Arg.Is()))
            #endregion Arrange

            #region Act/Assert
            Controller.Index().AssertActionRedirect().ToAction<StudentController>(a => a.ChooseCeremony());
            #endregion Act/Assert

        }

        #endregion Index Tests


        #region Reflection

        

        #endregion Reflection
    }
}
