﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Castle.Windsor;
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
using UCDArch.Core.PersistanceSupport;
using UCDArch.Testing;
using UCDArch.Web.Attributes;

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
        private readonly IRepository<State> _stateRepository;
        private IErrorService _errorService;
        private ICeremonyService _ceremonyService;
        private IRepository<RegistrationPetition> _registrationPetitionRepository;
        private IRepository<RegistrationParticipation> _participationRepository;
        private IRegistrationPopulator _registrationPopulator;

        public IRepository<TermCode> TermCodeRepository;
        public IRepository<vTermCode> vTermCodeRepository;
        public IRepository<SpecialNeed> SpecialNeedRepository;

        #region Init

        public StudentControllerTests()
        {
            _stateRepository = FakeRepository<State>();
            Controller.Repository.Expect(a => a.OfType<State>()).Return(_stateRepository).Repeat.Any();

            vTermCodeRepository = FakeRepository<vTermCode>();
            Controller.Repository.Expect(a => a.OfType<vTermCode>()).Return(vTermCodeRepository).Repeat.Any();

            SpecialNeedRepository = FakeRepository<SpecialNeed>();
            Controller.Repository.Expect(a => a.OfType<SpecialNeed>()).Return(SpecialNeedRepository).Repeat.Any();
        }

        protected override void SetupController()
        {

            /*
        public StudentController(IStudentService studentService, 
            IEmailService emailService,
            IRepositoryWithTypedId<Student, Guid> studentRepository, 
            IRepository<Ceremony> ceremonyRepository, 
            IRepository<Registration> registrationRepository,
            IErrorService errorService,
            ICeremonyService ceremonyService, 
             IRepository<RegistrationPetition> registrationPetitionRepository,
            IRepository<RegistrationParticipation> participationRepository, 
             IRegistrationPopulator registrationPopulator)
        {
            _studentRepository = studentRepository;
            _ceremonyRepository = ceremonyRepository;
            _registrationRepository = registrationRepository;
            _errorService = errorService;
            _ceremonyService = ceremonyService;
            _registrationPetitionRepository = registrationPetitionRepository;
            _participationRepository = participationRepository;
            _registrationPopulator = registrationPopulator;
            _studentService = studentService;
            _emailService = emailService;
        }
             */
            _studentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();
            _ceremonyRepository = MockRepository.GenerateStub<IRepository<Ceremony>>();
            _registrationRepository = MockRepository.GenerateStub<IRepository<Registration>>();
            _errorService = MockRepository.GenerateStub<IErrorService>();
            _ceremonyService = MockRepository.GenerateStub<ICeremonyService>();
            _registrationPetitionRepository = FakeRepository<RegistrationPetition>();
            _participationRepository = FakeRepository<RegistrationParticipation>();
            _registrationPopulator = MockRepository.GenerateStub<IRegistrationPopulator>();
            _studentService = MockRepository.GenerateStub<IStudentService>();
            _emailService = MockRepository.GenerateStub<IEmailService>();
            

            Controller = new TestControllerBuilder().CreateController<StudentController>
                (_studentService,
                _emailService,
                _studentRepository,
                _ceremonyRepository,
                _registrationRepository,
                _errorService,
                _ceremonyService,
                _registrationPetitionRepository,
                _participationRepository,
                _registrationPopulator
                );
        }


        /// <summary>
        /// Registers the routes.
        /// </summary>
        protected override void RegisterRoutes()
        {
            new RouteConfigurator().RegisterRoutes();
        }

        /// <summary>
        /// Need to do this because the call to the static class TermService.
        /// </summary>
        /// <param name="container"></param>
        protected override void RegisterAdditionalServices(IWindsorContainer container)
        {
            TermCodeRepository = MockRepository.GenerateStub<IRepository<TermCode>>();
            container.Kernel.AddComponentInstance<IRepository<TermCode>>(TermCodeRepository);
            base.RegisterAdditionalServices(container);
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

        [TestMethod]
        public void TestRegistrationRoutingMapping()
        {
            "~/Student/RegistrationRouting".ShouldMapTo<StudentController>(a => a.RegistrationRouting());
        }

        [TestMethod]
        public void TestRegisterGetMapping()
        {
            "~/Student/Register".ShouldMapTo<StudentController>(a => a.Register());
        }

        //[TestMethod]
        //public void TestEditRegistrationGetMapping()
        //{
        //    "~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5));
        //}
        //[TestMethod]
        //public void TestEditRegistrationPutMapping()
        //{
        //    Assert.Inconclusive("Review");
        //    //"~/Student/EditRegistration/5".ShouldMapTo<StudentController>(a => a.EditRegistration(5, new Registration(), true), true);
        //}

        //[TestMethod]
        //public void TestNoCeremonyMapping()
        //{
        //    "~/Student/NoCeremony".ShouldMapTo<StudentController>(a => a.NoCeremony());
        //}

        
        #endregion Mapping 

        #region Index Tests

        [TestMethod]
        public void TestIndexRedirectsToErrorIfCurrentStudentNotFound()
        {
            #region Arrange
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.Index()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotFound());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
            
        }

        [TestMethod]
        public void TestIndexRedirectsToRoutingIfCurrentStudentFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, _studentRepository, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.Index()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.RegistrationRouting());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        #endregion Index Tests

        #region RegistrationRouting Tests

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert		
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            //var students = new List<Student>();
            //var student = CreateValidEntities.Student(1);
            //student.Blocked = true;
            //students.Add(student);
            //ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected3()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository, true);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected5()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.SjaBlock = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.SJA());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected6()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected7()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = CreateValidEntities.TermCode(99);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.PreviouslyWalked());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected8()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything,Arg<TermCode>.Is.Anything)).Return(null).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything,Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected9()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(new List<Ceremony>()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected10()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i+1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegistrationRoutingRedirectsAsExpected11()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }
            ceremonies[1].RegistrationBegin = DateTime.Now.AddDays(-5);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.RegistrationRouting()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Register());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }
        #endregion RegistrationRouting Tests

        #region Register Tests

        #region Get Tests
        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected2()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            //var students = new List<Student>();
            //var student = CreateValidEntities.Student(1);
            //student.Blocked = true;
            //students.Add(student);
            //ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected3()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository, true);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected5()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.SjaBlock = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.SJA());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected6()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected7()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = CreateValidEntities.TermCode(99);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.PreviouslyWalked());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected8()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(null).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected9()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(new List<Ceremony>()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetRedirectsAsExpected10()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterGetReturnsView()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }
            ceremonies[1].RegistrationBegin = DateTime.Now.AddDays(-5);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, vTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);
            #endregion Arrange

            #region Act

            var result = Controller.Register()
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SpecialNeeds.Count());
            Assert.AreEqual(0, result.Registration.Id);
            Assert.AreEqual("Pidm1", result.Student.Pidm);
            #endregion Assert
        }
        #endregion Get Tests

        #region Post Tests

        [TestMethod]
        public void TestRegisterPostRedirectsToStudentsIfStudentIsNotFound()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(null, null, null, null));
            #endregion Act

            #region Assert

            #endregion Assert		
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected1()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }
 

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected3()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Blocked = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected4()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository, true);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected5()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.SjaBlock = true;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.SJA());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected6()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.DisplayRegistration());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected7()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = student;
            registration.TermCode = CreateValidEntities.TermCode(99);
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.PreviouslyWalked());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected8()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(null).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected9()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(new List<Ceremony>()).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotEligible());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostRedirectsAsExpected10()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            #endregion Arrange

            #region Act
            Controller.Register(new RegistrationPostModel())
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.NotOpen());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            var args = _ceremonyService.GetArgumentsForCallsMadeOn(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything))[0];
            Assert.AreEqual(3, args.Count());
            var majorCodes = args[0] as List<MajorCode>;
            Assert.IsNotNull(majorCodes);
            Assert.AreEqual(2, majorCodes.Count());
            Assert.AreEqual("Name2", majorCodes[0].Name);
            Assert.AreEqual("Name3", majorCodes[1].Name);
            Assert.AreEqual(23m, args[1]);
            Assert.IsNull(args[2]);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithoutAgreementDoesNotSave()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }
            ceremonies[1].RegistrationBegin = DateTime.Now.AddDays(-5);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, vTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.AgreeToDisclaimer = false;
            _registrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();

            #endregion Arrange

            #region Act

            var result = Controller.Register(registrationPostModel)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            _ceremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything), x => x.Repeat.Times(2));
            _registrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("You must agree to the disclaimer");

            Assert.IsNotNull(result);
            Assert.AreEqual("Pidm1", result.Student.Pidm);
            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithInvalidRegistrationDoesNotSave()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            registration.Address1 = string.Empty; //Invalid
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }
            ceremonies[1].RegistrationBegin = DateTime.Now.AddDays(-5);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, vTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.AgreeToDisclaimer = true;
            _registrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();

            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            _ceremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything), x => x.Repeat.Times(2));
            _registrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("Address1: may not be null or empty");

            #endregion Assert
        }

        [TestMethod]
        public void TestRegisterPostWithRegistrationPetitionWithMissingReasonDoesNotSave()
        {
            #region Arrange
            FakeTermCodeService.LoadTermCodes("201003", TermCodeRepository);
            var students = new List<Student>();
            var student = CreateValidEntities.Student(1);
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.EarnedUnits = 11m;
            student.CurrentUnits = 12m;
            students.Add(student);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, null);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(_studentRepository.Queryable.First()).Repeat.Any();

            var registrations = new List<Registration>();
            var registration = CreateValidEntities.Registration(1);
            registration.Student = CreateValidEntities.Student(99);
            registration.TermCode = TermCodeRepository.Queryable.First();
            var registrationPetition = CreateValidEntities.RegistrationPetition(1);
            registrationPetition.ExceptionReason = " ";
            registration.AddPetition(registrationPetition); //Invalid
            registrations.Add(registration);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);

            var ceremonies = new List<Ceremony>();
            for (int i = 0; i < 3; i++)
            {
                ceremonies.Add(CreateValidEntities.Ceremony(i + 1));
                ceremonies[i].RegistrationBegin = DateTime.Now.AddDays(7);
            }
            ceremonies[1].RegistrationBegin = DateTime.Now.AddDays(-5);

            _ceremonyService.Expect(
                a =>
                a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything)).Return(ceremonies).Repeat.Any();

            ControllerRecordFakes.FakeState(2, Controller.Repository, null);
            ControllerRecordFakes.FakevTermCode(1, vTermCodeRepository);
            ControllerRecordFakes.FakeSpecialNeeds(1, SpecialNeedRepository);

            var registrationPostModel = new RegistrationPostModel();
            registrationPostModel.AgreeToDisclaimer = true;
            _registrationPopulator.Expect(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything))
                .Return(registration)
                .Repeat.Any();

            #endregion Arrange

            #region Act

            Controller.Register(registrationPostModel)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            _ceremonyService.AssertWasCalled(a => a.StudentEligibility(Arg<List<MajorCode>>.Is.Anything, Arg<decimal>.Is.Anything, Arg<TermCode>.Is.Anything), x => x.Repeat.Times(2));
            _registrationPopulator.AssertWasCalled(a => a.PopulateRegistration(Arg<RegistrationPostModel>.Is.Anything, Arg<Student>.Is.Anything, Arg<ModelStateDictionary>.Is.Anything, Arg<bool>.Is.Anything));
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsFalse(Controller.ModelState.IsValid);
            Controller.ModelState.AssertErrorsAre("Exception reason is required.");

            #endregion Assert
        }
        #endregion Post Tests

        #endregion Register Tests

        /*
        #region GetCurrentStudent Tests

        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestGetStudentShouldThrowExceptionWhenStudentNotFound()
        {
            try
            {
                #region Arrange
                _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(null).Repeat.Any();
                #endregion Arrange

                #region Act
                Controller.Index();
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
                Assert.IsNotNull(ex);
                Assert.AreEqual("Current user is not a student or student not found.", ex.Message);
                throw;
                #endregion Assert
            }
        }

        #endregion GetCurrentStudent Tests

        #region ChooseCeremony Tests

        /// <summary>
        /// Tests the choose ceremony with no ceremony with major redirects to error controller.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyWithNoCeremonyWithMajorRedirectsToErrorController()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonyWithMajors = new List<CeremonyWithMajor>();
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _studentService.Expect(a => a.GetMajorsAndCeremoniesForStudent(student)).Return(ceremonyWithMajors).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.ChooseCeremony()
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.NoCeremony));
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorController.ErrorType.NoCeremony, result.RouteValues["errorType"]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the choose ceremony with only one ceremony with major redirects to register.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyWithOnlyOneCeremonyWithMajorRedirectsToRegister()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonyWithMajors = new List<CeremonyWithMajor>();
            var ceremonyWithMajor = CreateValidEntities.CeremonyWithMajor(1);
            ceremonyWithMajors.Add(ceremonyWithMajor);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _studentService.Expect(a => a.GetMajorsAndCeremoniesForStudent(student)).Return(ceremonyWithMajors).Repeat.Any();            
            #endregion Arrange

            #region Act
            Controller.ChooseCeremony()
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Register(1, string.Empty));
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            #endregion Assert
        }


        /// <summary>
        /// Tests the choose ceremony with more than one ceremony with major returns view.
        /// </summary>
        [TestMethod]
        public void TestChooseCeremonyWithMoreThanOneCeremonyWithMajorReturnsView()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonyWithMajors = new List<CeremonyWithMajor>();
            ceremonyWithMajors.Add(CreateValidEntities.CeremonyWithMajor(1));
            ceremonyWithMajors.Add(CreateValidEntities.CeremonyWithMajor(2));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            _studentService.Expect(a => a.GetMajorsAndCeremoniesForStudent(student)).Return(ceremonyWithMajors).Repeat.Any();  
            #endregion Arrange

            #region Act
            var result = Controller.ChooseCeremony()
                .AssertViewRendered()
                .WithViewData<List<CeremonyWithMajor>>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            #endregion Assert		
        }

        #endregion ChooseCeremony Tests

        #region DisplayRegistration Tests

        /// <summary>
        /// Tests the display registration redirects to index when registration is null.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationRedirectsToIndexWhenRegistrationIsNull()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNull(_registrationRepository.GetNullableById(4));
            #endregion Arrange

            #region Act/Assert
            Controller.DisplayRegistration(4)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert
        }


        /// <summary>
        /// Tests the display registration redirects to index if the registration student does not match the current student.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationRedirectsToIndexIfTheRegistrationStudentDoesNotMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student2).Repeat.Any();
            #endregion Arrange

            #region Act/Assert
            Controller.DisplayRegistration(1)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert		
        }

        /// <summary>
        /// Tests the display registration returns view if the registration student does match the current student.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationReturnsViewIfTheRegistrationStudentDoesMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.DisplayRegistration(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            #endregion Assert
        }

        /// <summary>
        /// Tests the display registration returns view with edit setting.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationReturnsViewWithEditSetting()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.DisplayRegistration(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            Assert.IsTrue((bool)Controller.ViewData["CanEditRegistration"]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the display registration returns view without edit setting.
        /// </summary>
        [TestMethod]
        public void TestDisplayRegistrationReturnsViewWithoutEditSetting()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.DisplayRegistration(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            Assert.IsFalse((bool)Controller.ViewData["CanEditRegistration"]);
            #endregion Assert
        }
        #endregion DisplayRegistration Tests

        #region RegistrationConfirmation Tests

        /// <summary>
        /// Tests the registration confirmation redirects to index when registration is null.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationRedirectsToIndexWhenRegistrationIsNull()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNull(_registrationRepository.GetNullableById(4));
            #endregion Arrange

            #region Act/Assert
            Controller.RegistrationConfirmation(4)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert
        }

        /// <summary>
        /// Tests the registration confirmation redirects to index if the registration student does not match the current student.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationRedirectsToIndexIfTheRegistrationStudentDoesNotMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student2).Repeat.Any();
            #endregion Arrange

            #region Act/Assert
            Controller.RegistrationConfirmation(1)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act/Assert
        }

        /// <summary>
        /// Tests the registration confirmation returns view if the registration student does match the current student.
        /// </summary>
        [TestMethod]
        public void TestRegistrationConfirmationReturnsViewIfTheRegistrationStudentDoesMatchTheCurrentStudent()
        {
            #region Arrange
            var student1 = CreateValidEntities.Student(1);
            //var student2 = CreateValidEntities.Student(2);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student1;
            ControllerRecordFakes.FakeRegistration(2, _registrationRepository, registrations);
            Assert.IsNotNull(_registrationRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student1).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.RegistrationConfirmation(1)
                .AssertViewRendered()
                .WithViewData<Registration>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual("Address11", result.Address1);
            #endregion Assert
        }       

        #endregion RegistrationConfirmation Tests

        #region Register Tests
        #region Register Get Tests

        /// <summary>
        /// Tests the register get redirects to index if ceremony not found.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetRedirectsToIndexIfCeremonyNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeCeremony(1, _ceremonyRepository);
            Assert.IsNull(_ceremonyRepository.GetNullableById(2));
            #endregion Arrange

            #region Act
            Controller.Register(2, string.Empty)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());                
            #endregion Act

            #region Assert
            Assert.AreEqual("No matching ceremony found.  Please try your registration again.", Controller.Message);
            #endregion Assert		
        }


       

        [TestMethod]
        public void TestRegisterGetRedirectsToErrorIfCeremonyDeadlinePassed()
        {
            #region Arrange
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, string.Empty)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            #endregion Act

            #region Assert
            //Assert.AreEqual("The deadline to register for the ceremony has passed.", Controller.Message);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorController.ErrorType.RegistrationClosed, result.RouteValues["errorType"]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get redirects to index if student major is empty and major not passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetRedirectsToIndexIfStudentMajorIsEmptyAndMajorNotPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            Controller.Register(1, string.Empty)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Student has multiple majors but did not supply a major code.", Controller.Message);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the register get redirects to index if student major has multiple values and major not passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetRedirectsToIndexIfStudentMajorHasMultipleValuesAndMajorNotPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(CreateValidEntities.MajorCode(1));
            student.Majors.Add(CreateValidEntities.MajorCode(2));
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            Controller.Register(1, string.Empty)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            Assert.AreEqual("Student has multiple majors but did not supply a major code.", Controller.Message);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get returns view if student has only one major and major not passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetReturnsViewIfStudentHasOnlyOneMajorAndMajorNotPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();            
            student.Majors.Add(CreateValidEntities.MajorCode(9));
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, string.Empty)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNull(Controller.Message);
            Assert.IsNotNull(result);
            Assert.AreEqual("Name9", result.Registration.Major.Name);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreSame(ceremonies[0], result.Ceremony);
            Assert.AreSame(student, result.Student);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get returns view if student has multiple majors and major is passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterGetReturnsViewIfStudentHasMultipleMajorsAndMajorIsPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            student.Majors = new List<MajorCode>();
            student.Majors.Add(CreateValidEntities.MajorCode(1));
            student.Majors.Add(CreateValidEntities.MajorCode(3));
            student.Majors.Add(CreateValidEntities.MajorCode(9));
            student.Majors[0].SetIdTo("1");
            student.Majors[1].SetIdTo("3");
            student.Majors[2].SetIdTo("9");

            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, "3")
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNull(Controller.Message);
            Assert.IsNotNull(result);
            Assert.AreEqual("Name3", result.Registration.Major.Name);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreSame(ceremonies[0], result.Ceremony);
            Assert.AreSame(student, result.Student);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register get throws exception if student has multiple majors and wrong major is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegisterGetThrowsExceptionIfStudentHasMultipleMajorsAndWrongMajorIsPassed()
        {
            try
            {
                #region Arrange
                var student = CreateValidEntities.Student(1);
                student.Majors = new List<MajorCode>();
                student.Majors.Add(CreateValidEntities.MajorCode(1));
                student.Majors.Add(CreateValidEntities.MajorCode(3));
                student.Majors.Add(CreateValidEntities.MajorCode(9));
                var ceremonies = new List<Ceremony>();
                ceremonies.Add(CreateValidEntities.Ceremony(1));
                ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
                ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
                Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
                _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
                ControllerRecordFakes.FakeState(3, Controller.Repository);
                #endregion Arrange

                #region Act
                Controller.Register(1, "2");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains no elements", ex.Message);
                throw;
                #endregion Assert
            }
        }

        /// <summary>
        /// Tests the register get throws exception if student has multiple majors with duplicates and major is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegisterGetThrowsExceptionIfStudentHasMultipleMajorsWithDuplicatesAndMajorIsPassed()
        {
            try
            {
                #region Arrange
                var student = CreateValidEntities.Student(1);
                student.Majors = new List<MajorCode>();
                student.Majors.Add(CreateValidEntities.MajorCode(1));
                student.Majors.Add(CreateValidEntities.MajorCode(3));
                student.Majors.Add(CreateValidEntities.MajorCode(3));
                student.Majors.Add(CreateValidEntities.MajorCode(9));
                student.Majors[0].SetIdTo("1");
                student.Majors[1].SetIdTo("3");
                student.Majors[2].SetIdTo("3");
                student.Majors[3].SetIdTo("9");
                var ceremonies = new List<Ceremony>();
                ceremonies.Add(CreateValidEntities.Ceremony(1));
                ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
                ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
                Assert.IsNotNull(_ceremonyRepository.GetNullableById(1));
                _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
                ControllerRecordFakes.FakeState(3, Controller.Repository);
                #endregion Arrange

                #region Act
                Controller.Register(1, "3");
                #endregion Act
            }
            catch (Exception ex)
            {
                #region Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual("Sequence contains more than one element", ex.Message);
                throw;
                #endregion Assert
            }
        }
        #endregion Register Get Tests
        #region Register Post Tests


        /// <summary>
        /// Tests the register post sets comments to null if it is an empty string.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostSetsCommentsToNullIfItIsAnEmptyString()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var ceremonies = new List<Ceremony>();
            //ceremonies.Add(CreateValidEntities.Ceremony(1));
            //ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            //ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            //var registration = CreateValidEntities.Registration(1);
            //registration.Student = null;
            //registration.Ceremony = null;
            //registration.Comments = string.Empty;

            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //_emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.Register(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            //var args = (Registration)_registrationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Registration>.Is.Anything))[0][0];
            //Assert.IsNull(args.Comments);
            //#endregion Assert	
        }

        /// <summary>
        /// Tests the register post sets comments to null if it is spaces only string.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostSetsCommentsToNullIfItIsSpacesOnlyString()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var ceremonies = new List<Ceremony>();
            //ceremonies.Add(CreateValidEntities.Ceremony(1));
            //ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            //ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            //var registration = CreateValidEntities.Registration(1);
            //registration.Student = null;
            //registration.Ceremony = null;
            //registration.Comments = "  ";

            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //_emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.Register(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            //var args = (Registration)_registrationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Registration>.Is.Anything))[0][0];
            //Assert.IsNull(args.Comments);
            //#endregion Assert
        }

        /// <summary>
        /// Tests the register post sets address2 to null if it is spaces only string.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostSetsAddress2ToNullIfItIsSpacesOnlyString()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var ceremonies = new List<Ceremony>();
            //ceremonies.Add(CreateValidEntities.Ceremony(1));
            //ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            //ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            //var registration = CreateValidEntities.Registration(1);
            //registration.Student = null;
            //registration.Ceremony = null;
            //registration.Address2 = "  ";

            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //_emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.Register(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            //var args = (Registration)_registrationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Registration>.Is.Anything))[0][0];
            //Assert.IsNull(args.Address2);
            //#endregion Assert
        }

        /// <summary>
        /// Tests the register post sets address3 to null if it is spaces only string.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostSetsAddress3ToNullIfItIsSpacesOnlyString()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var ceremonies = new List<Ceremony>();
            //ceremonies.Add(CreateValidEntities.Ceremony(1));
            //ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            //ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            //var registration = CreateValidEntities.Registration(1);
            //registration.Student = null;
            //registration.Ceremony = null;
            //registration.Address3 = "  ";

            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //_emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.Register(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            //var args = (Registration)_registrationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Registration>.Is.Anything))[0][0];
            //Assert.IsNull(args.Address3);
            //#endregion Assert
        }

        /// <summary>
        /// Tests the register post sets email to null if it is spaces only string.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostSetsEmailToNullIfItIsSpacesOnlyString()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var ceremonies = new List<Ceremony>();
            //ceremonies.Add(CreateValidEntities.Ceremony(1));
            //ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            //ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            //var registration = CreateValidEntities.Registration(1);
            //registration.Student = null;
            //registration.Ceremony = null;
            //registration.Email = "  ";

            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //_emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.Register(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            //var args = (Registration)_registrationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Registration>.Is.Anything))[0][0];
            //Assert.IsNull(args.Email);
            //#endregion Assert
        }

        /// <summary>
        /// Tests the register post returns view if agree to disclaimer is false.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostReturnsViewIfAgreeToDisclaimerIsFalse()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);            
            var registration = CreateValidEntities.Registration(1);
            registration.Student = null;
            registration.Ceremony = null;

            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, registration, false)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreSame(ceremonies[0], result.Registration.Ceremony);
            Assert.AreSame(student, result.Registration.Student);
            Controller.ModelState.AssertErrorsAre("You must agree to the disclaimer");
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            #endregion Assert		
        }


        /// <summary>
        /// Tests the register post redirects to error if ceremony deadline passed.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostRedirectsToErrorIfCeremonyDeadlinePassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            var registration = CreateValidEntities.Registration(1);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.Register(1, registration, true)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            #endregion Act

            #region Assert
            //Assert.AreEqual("The deadline to register for the ceremony has passed.", Controller.Message);
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorController.ErrorType.RegistrationClosed, result.RouteValues["errorType"]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the register post returns view if ceremony not found.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostReturnsViewIfCeremonyNotFound()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            var registration = CreateValidEntities.Registration(1);
            registration.Student = null;
            registration.Ceremony = null;

            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(3, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.Register(2, registration, true)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.States.Count());
            Assert.AreNotSame(ceremonies[0], result.Registration.Ceremony);
            Assert.AreSame(student, result.Registration.Student);
            Controller.ModelState.AssertErrorsAre("Ceremony: may not be null");
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            #endregion Assert
        }


        /// <summary>
        /// Tests the register post with valid data saves.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostWithValidDataSaves()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var ceremonies = new List<Ceremony>();
            //ceremonies.Add(CreateValidEntities.Ceremony(1));
            //ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            //ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            //var registration = CreateValidEntities.Registration(1);
            //registration.Student = null;
            //registration.Ceremony = null;

            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //_emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.Register(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully registered for commencement.", Controller.Message);
            //#endregion Assert		
        }

        /// <summary>
        /// Tests the register post with valid data saves but email fails.
        /// </summary>
        [TestMethod]
        public void TestRegisterPostWithValidDataSavesButEmailFails()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var ceremonies = new List<Ceremony>();
            //ceremonies.Add(CreateValidEntities.Ceremony(1));
            //ceremonies[0].RegistrationDeadline = DateTime.Now.AddDays(1);
            //ControllerRecordFakes.FakeCeremony(0, _ceremonyRepository, ceremonies);
            //var registration = CreateValidEntities.Registration(1);
            //registration.Student = null;
            //registration.Ceremony = null;

            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //_emailService.Expect(a => a.SendRegistrationConfirmation(Controller.Repository, registration)).Repeat.Any().Throw(new Exception("Faked Exception"));
            //#endregion Arrange

            //#region Act
            //Controller.Register(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully registered for commencement. There was a problem sending you an email.  Please print this page for your records.", Controller.Message);
            //#endregion Assert
        }

        #endregion Register Post Tests
        #endregion Register Tests

        #region EditRegistration Tests
        #region EditRegistration Get Tests

        /// <summary>
        /// Tests the edit registration get redirects to index if registration is null.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationGetRedirectsToIndexIfRegistrationIsNull()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            ControllerRecordFakes.FakeRegistration(1, _registrationRepository);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.EditRegistration(2)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the edit registration get redirects to index if registration student is not current student.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationGetRedirectsToIndexIfRegistrationStudentIsNotCurrentStudent()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>(1);
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = CreateValidEntities.Student(2);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act
            Controller.EditRegistration(1)
                .AssertActionRedirect()
                .ToAction<StudentController>(a => a.Index());
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            #endregion Assert
        }

        /// <summary>
        /// Tests the edit registration get redirects to error if registration ceremony deadline has passed.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationGetRedirectsToErrorIfRegistrationCeremonyDeadlineHasPassed()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>(1);
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student;
            registrations[0].Ceremony = CreateValidEntities.Ceremony(1);
            registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(-1);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.EditRegistration(1)
                .AssertActionRedirect()
                .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNull(Controller.Message);
            Assert.IsNotNull(result);
            Assert.AreEqual(ErrorController.ErrorType.RegistrationClosed, result.RouteValues["errorType"]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the edit registration returns view with valid data.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationReturnsViewWithValidData()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>(1);
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student;
            registrations[0].Ceremony = CreateValidEntities.Ceremony(1);
            registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(1);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            ControllerRecordFakes.FakeState(1, Controller.Repository);
            #endregion Arrange

            #region Act
            var result = Controller.EditRegistration(1)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            Assert.IsNull(Controller.Message);
            Assert.AreSame(registrations[0], result.Registration);
            Assert.AreSame(registrations[0].Ceremony, result.Registration.Ceremony);
            Assert.AreSame(registrations[0].Student, student);
            #endregion Assert
        }
        #endregion EditRegistration Get Tests
        #region EditRegistration Post Tests               
        /// <summary>
        /// Tests the edit registration post redirects to index if registration is null.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationPostRedirectsToIndexIfRegistrationIsNull()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var registrations = new List<Registration>(1);
            //registrations.Add(CreateValidEntities.Registration(1));
            //registrations[0].Student = student;
            //registrations[0].Ceremony = CreateValidEntities.Ceremony(1);
            //registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(1);

            //ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.EditRegistration(2, registrations[0], true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.Index());
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            //#endregion Assert
        }

        /// <summary>
        /// Tests the edit registration post redirects to index if registration student is not current student.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationPostRedirectsToIndexIfRegistrationStudentIsNotCurrentStudent()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var registrations = new List<Registration>(1);
            //registrations.Add(CreateValidEntities.Registration(1));
            //registrations[0].Student = CreateValidEntities.Student(2);
            //ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //Controller.EditRegistration(1, registrations[0], true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.Index());
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //Assert.AreEqual("No matching registration found.  Please try your registration again.", Controller.Message);
            //#endregion Assert
        }

        /// <summary>
        /// Tests the edit registration post redirects to error if registration ceremony deadline has passed.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationPostRedirectsToErrorIfRegistrationCeremonyDeadlineHasPassed()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var registrations = new List<Registration>(1);
            //registrations.Add(CreateValidEntities.Registration(1));
            //registrations[0].Student = student;
            //registrations[0].Ceremony = CreateValidEntities.Ceremony(1);
            //registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(-1);
            //ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //var result = Controller.EditRegistration(1, registrations[0], true)
            //    .AssertActionRedirect()
            //    .ToAction<ErrorController>(a => a.Index(ErrorController.ErrorType.RegistrationClosed));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //Assert.IsNull(Controller.Message);
            //Assert.IsNotNull(result);
            //Assert.AreEqual(ErrorController.ErrorType.RegistrationClosed, result.RouteValues["errorType"]);
            //#endregion Assert
        }


        /// <summary>
        /// Tests the edit registration post only updates expected values.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationPostOnlyUpdatesExpectedValues()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var majorCode1 = CreateValidEntities.MajorCode(1);
            //var majorCode2 = CreateValidEntities.MajorCode(2);
            //var state1 = CreateValidEntities.State(1);
            //var state2 = CreateValidEntities.State(2);
            //var ceremony1 = CreateValidEntities.Ceremony(1);
            //ceremony1.RegistrationDeadline = DateTime.Now.AddDays(5);
            //var ceremony2 = CreateValidEntities.Ceremony(2);
            //ceremony2.RegistrationDeadline = DateTime.Now.AddDays(10);
            //var extraTicketPetition1 = CreateValidEntities.ExtraTicketPetition(1);
            //var extraTicketPetition2 = CreateValidEntities.ExtraTicketPetition(2);

            //var student = CreateValidEntities.Student(1);
            //var registrations = new List<Registration>(1);
            //registrations.Add(CreateValidEntities.Registration(1));
            //registrations[0].Student = student;
            //registrations[0].Major = majorCode1;
            //registrations[0].Address1 = "Address1Old";
            //registrations[0].Address2 = "Address2Old";
            //registrations[0].Address3 = "Address3Old";
            //registrations[0].City = "CityOld";
            //registrations[0].State = state1;
            //registrations[0].Zip = "11111";
            //registrations[0].Email = "jasonold@ucdavis.edu";
            //registrations[0].NumberTickets = 1;
            //registrations[0].MailTickets = false;
            //registrations[0].Comments = "CommentsOld";
            //registrations[0].Ceremony = ceremony1;
            //registrations[0].ExtraTicketPetition = extraTicketPetition1;
            //registrations[0].DateRegistered = new DateTime(2010,01,01);            

            //ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();

            //var registration = CreateValidEntities.Registration(2);
            //registration.Student = student;
            //registration.Major = majorCode2;
            //registration.Address1 = "Address1New";
            //registration.Address2 = "Address2New";
            //registration.Address3 = "Address3New";
            //registration.City = "CityNew";
            //registration.State = state2;
            //registration.Zip = "22222";
            //registration.Email = "jasonnew@ucdavis.edu";
            //registration.NumberTickets = 2;
            //registration.MailTickets = true;
            //registration.Comments = "CommentsNew";
            //registration.Ceremony = ceremony2;
            //registration.ExtraTicketPetition = extraTicketPetition2;
            //registration.DateRegistered = new DateTime(2010, 05, 05);
            //registration.SetIdTo(1);
            //#endregion Arrange

            //#region Act
            //Controller.EditRegistration(1, registration, true)
            //    .AssertActionRedirect()
            //    .ToAction<StudentController>(a => a.RegistrationConfirmation(1));
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasCalled(a => a.EnsurePersistent(registration));
            //_emailService.AssertWasCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registration));
            //Assert.AreEqual("You have successfully edited your commencement registration. ", Controller.Message);

            //#region Assert only expected values were updated.
            //var args = (Registration)_registrationRepository.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Registration>.Is.Anything))[0][0];           
            //Assert.IsNotNull(args);
            //Assert.AreEqual(student.FirstName, args.Student.FirstName);
            //Assert.AreEqual(majorCode2.Name, args.Major.Name);
            //Assert.AreEqual("Address1New", args.Address1);
            //Assert.AreEqual("Address2New", args.Address2);
            //Assert.AreEqual("Address3Old", args.Address3);
            //Assert.AreEqual("CityNew", args.City);
            //Assert.AreEqual(state2.Name, args.State.Name);
            //Assert.AreEqual("22222", args.Zip);
            //Assert.AreEqual("jasonnew@ucdavis.edu", args.Email);
            //Assert.AreEqual(2, args.NumberTickets);
            //Assert.IsTrue(args.MailTickets);
            //Assert.AreEqual("CommentsNew", args.Comments);
            //Assert.AreEqual(ceremony2.Location, args.Ceremony.Location);
            //Assert.AreEqual(1, args.ExtraTicketPetition.NumberTickets);
            //Assert.AreEqual(new DateTime(2010, 01, 01), args.DateRegistered);                       
            //#endregion Assert only expected values were updated.
            //#endregion Assert		
        }


        /// <summary>
        /// Tests the edit registration post does not save if disclaimer is false.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationPostDoesNotSaveIfDisclaimerIsFalse()
        {
            Assert.Inconclusive("Review");
            //#region Arrange
            //var student = CreateValidEntities.Student(1);
            //var registrations = new List<Registration>(1);
            //registrations.Add(CreateValidEntities.Registration(1));
            //registrations[0].Student = student;
            //registrations[0].Ceremony = CreateValidEntities.Ceremony(1);
            //registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(5);
            //ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            //_studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();
            //#endregion Arrange

            //#region Act
            //var result = Controller.EditRegistration(1, registrations[0], false)
            //    .AssertViewRendered()
            //    .WithViewData<RegistrationModel>();
            //#endregion Act

            //#region Assert
            //_studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            //_registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            //_emailService.AssertWasNotCalled(a => a.SendRegistrationConfirmation(Controller.Repository, registrations[0]));
            //Assert.IsNull(Controller.Message);
            //Controller.ModelState.AssertErrorsAre("You must agree to the disclaimer");
            //Assert.IsNotNull(result);
            //Assert.AreSame(registrations[0], result.Registration);
            //#endregion Assert		
        }


        /// <summary>
        /// Tests the edit registration does not save if new value is invalid.
        /// </summary>
        [TestMethod]
        public void TestEditRegistrationDoesNotSaveIfNewValueIsInvalid()
        {
            #region Arrange
            var student = CreateValidEntities.Student(1);
            var registrations = new List<Registration>(1);
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = student;
            registrations[0].Ceremony = CreateValidEntities.Ceremony(1);
            registrations[0].Ceremony.RegistrationDeadline = DateTime.Now.AddDays(5);
            ControllerRecordFakes.FakeRegistration(0, _registrationRepository, registrations);
            _studentService.Expect(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything)).Return(student).Repeat.Any();

            var registration = registrations[0];
            registration.Address1 = string.Empty;
            #endregion Arrange

            #region Act
            var result = Controller.EditRegistration(1, registration)
                .AssertViewRendered()
                .WithViewData<RegistrationModel>();
            #endregion Act

            #region Assert
            _studentService.AssertWasCalled(a => a.GetCurrentStudent(Arg<IPrincipal>.Is.Anything));
            _registrationRepository.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Registration>.Is.Anything));
            _emailService.AssertWasNotCalled(a => a.SendRegistrationConfirmation(registrations[0]));
            Assert.IsNull(Controller.Message);
            Controller.ModelState.AssertErrorsAre("Address1: may not be null or empty");
            Assert.IsNotNull(result);
            Assert.AreSame(registrations[0], result.Registration);
            #endregion Assert			
        }

        #endregion EditRegistration Post Tests
        #endregion EditRegistration Tests

        #region NoCeremony Tests

        /// <summary>
        /// Tests the no ceremony returns view.
        /// </summary>
        [TestMethod]
        public void TestNoCeremonyReturnsView()
        {
            #region Arrange                                  

            #endregion Arrange

            #region Act
            Controller.NoCeremony()
                .AssertViewRendered();
            #endregion Act

            #region Assert

            #endregion Assert		
        }
        #endregion NoCeremony Tests


 
        */

        #region Reflection
        #region Controller Class Tests
        /// <summary>
        /// Tests the controller inherits from super controller.
        /// </summary>
        [TestMethod]
        public void TestControllerInheritsFromApplicationControllerThenSuperController()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            Assert.IsNotNull(controllerClass);
            Assert.IsNotNull(controllerClass.BaseType);
            Assert.IsNotNull(controllerClass.BaseType.BaseType);
            var result = controllerClass.BaseType.BaseType.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("SuperController", result);
            #endregion Assert
        }
        /// <summary>
        /// Tests the controller inherits from super controller.
        /// </summary>
        [TestMethod]
        public void TestControllerInheritsFromApplicationController()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            Assert.IsNotNull(controllerClass);
            Assert.IsNotNull(controllerClass.BaseType);
            var result = controllerClass.BaseType.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("ApplicationController", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has four attributes.
        /// </summary>
        [TestMethod]
        public void TestControllerHasFourAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(4, result.Count());
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has transaction attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasTransactionAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<UseTransactionsByDefaultAttribute>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "UseTransactionsByDefaultAttribute not found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has anti forgery token attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasAntiForgeryTokenAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<UseAntiForgeryTokenOnPostByDefault>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "UseAntiForgeryTokenOnPostByDefault not found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has students only attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasStudentsOnlyAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<StudentsOnly>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "StudentsOnly not found.");
            #endregion Assert
        }

        [TestMethod]
        public void TestControllerHasSessionExpirationFilterAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<SessionExpirationFilterAttribute>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "SessionExpirationFilterAttribute not found.");
            #endregion Assert
        }

        #endregion Controller Class Tests

        #region Controller Method Tests

        /// <summary>
        /// Tests the controller contains expected number of public methods.
        /// </summary>
        [TestMethod]
        public void TestControllerContainsExpectedNumberOfPublicMethods()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetMethods().Where(a => a.DeclaringType == controllerClass);
            #endregion Act

            #region Assert
            Assert.AreNotEqual(3, result.Count(), "It looks like a method was added or removed from the controller.");
            Assert.Inconclusive("Still writing tests");
            #endregion Assert
        }


        /// <summary>
        /// Tests the controller method index contains expected attributes.
        /// #1
        /// </summary>
        [TestMethod]
        public void TestControllerMethodIndexContainsExpectedAttributes1()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("Index");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method index contains expected attributes.
        /// #1
        /// </summary>
        [TestMethod]
        public void TestControllerMethodIndexContainsExpectedAttributes2()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("Index");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<IgnoreStudentsOnly>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "IgnoreStudentsOnly not found");
            Assert.AreEqual(2, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// #2
        /// </summary>
        [TestMethod]
        public void TestControllerMethodRegistrationRoutingContainsExpectedAttributes1()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("RegistrationRouting");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method register get contains expected attributes.
        /// #3
        /// </summary>
        [TestMethod]
        public void TestControllerMethodRegisterGetContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.ElementAt(0).GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method register post contains expected attributes.
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodRegisterPostContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<HttpPostAttribute>();
            var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "HttpPostAttribute not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }


        ///// <summary>
        ///// Tests the controller method choose ceremony contains expected attributes.
        ///// #2
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodChooseCeremonyContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("ChooseCeremony");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method display registration contains expected attributes.
        ///// #3
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodDisplayRegistrationContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("DisplayRegistration");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method registration confirmation contains expected attributes.
        ///// #4
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegistrationConfirmationContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("RegistrationConfirmation");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method register get contains expected attributes.
        ///// #5
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegisterGetContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(0).GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method register post contains expected attributes.
        ///// #6
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodRegisterPostContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "Register");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<HttpPostAttribute>();
        //    var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method edit registration get contains expected attributes.
        ///// #7
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodEditRegistrationGetContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "EditRegistration");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(0).GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method edit registration post contains expected attributes.
        ///// #8
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodEditRegistrationPostContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "EditRegistration");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<HttpPostAttribute>();
        //    var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        ///// <summary>
        ///// Tests the controller method no ceremony contains expected attributes.
        ///// #9 Note: this one is not being used.
        ///// </summary>
        //[TestMethod]
        //public void TestControllerMethodNoCeremonyContainsExpectedAttributes()
        //{
        //    #region Arrange
        //    var controllerClass = _controllerClass;
        //    var controllerMethod = controllerClass.GetMethod("NoCeremony");
        //    #endregion Arrange

        //    #region Act
        //    var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
        //    var allAttributes = controllerMethod.GetCustomAttributes(true);
        //    #endregion Act

        //    #region Assert
        //    Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
        //    Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
        //    #endregion Assert
        //}

        #endregion Controller Method Tests

        #endregion Reflection

    }
}
