using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Castle.MicroKernel.Registration;
using Commencement.Controllers;
using Commencement.Controllers.Filters;
using Commencement.Controllers.Helpers;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.Attributes;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;
using UCDArch.Web.Attributes;
using Castle.Windsor;
using UCDArch.Web.IoC;

//using Microsoft.Practices.ServiceLocation;

namespace Commencement.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests : ControllerTestBase<AdminController>
    {
        private readonly Type _controllerClass = typeof(AdminController);
        private IRepositoryWithTypedId<Student, Guid> _studentRepository;
        private IRepositoryWithTypedId<MajorCode, string> _majorRepository;
        private IStudentService _studentService;
        private IEmailService _emailService;
        private IMajorService _majorService;

        protected IRepository<Student> StudentRepository2;
        protected IRepository<Registration> RegistrationRepository;
        protected IRepository<State> StateRepository;

        public IRepository<TermCode> TermCodeRepository;
        #region Init

        public AdminControllerTests()
        {
            StudentRepository2 = FakeRepository<Student>();
            Controller.Repository.Expect(a => a.OfType<Student>()).Return(StudentRepository2).Repeat.Any();

            RegistrationRepository = FakeRepository<Registration>();
            Controller.Repository.Expect(a => a.OfType<Registration>()).Return(RegistrationRepository).Repeat.Any();

            StateRepository = FakeRepository<State>();
            Controller.Repository.Expect(a => a.OfType<State>()).Return(StateRepository).Repeat.Any();
        }

        protected override void SetupController()
        {
            _studentRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<Student, Guid>>();
            _majorRepository = MockRepository.GenerateStub<IRepositoryWithTypedId<MajorCode, string>>();
            _studentService = MockRepository.GenerateStub<IStudentService>();
            _emailService = MockRepository.GenerateStub<IEmailService>();
            _majorService = MockRepository.GenerateStub<IMajorService>();

            Controller = new TestControllerBuilder().CreateController<AdminController>(_studentRepository, _majorRepository, _studentService, _emailService, _majorService);
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

        #region Mapping Tests

        /// <summary>
        /// Tests the index mapping.
        /// </summary>
        [TestMethod]
        public void TestIndexMapping()
        {
            "~/Admin/Index".ShouldMapTo<AdminController>(a => a.Index());
        }

        /// <summary>
        /// Tests the students mapping.
        /// </summary>
        [TestMethod]
        public void TestStudentsMapping()
        {
            "~/Admin/Students/".ShouldMapTo<AdminController>(a => a.Students("1", null,null,null), true);
        }

        /// <summary>
        /// Tests the student details mapping.
        /// </summary>
        [TestMethod]
        public void TestStudentDetailsMapping()
        {
            "~/Admin/StudentDetails/".ShouldMapTo<AdminController>(a => a.StudentDetails(Guid.Empty,true), true);
        }
        #endregion Mapping Tests

        #region Index Tests

        /// <summary>
        /// Tests the index returns view.
        /// </summary>
        [TestMethod]
        public void TestIndexReturnsView()
        {
            #region Arrange

            #endregion Arrange

            #region Act
            Controller.Index()
                .AssertViewRendered();
            #endregion Act

            #region Assert

            #endregion Assert		
        }
        #endregion Index Tests

        #region Students Tests

        /// <summary>
        /// Tests the students returns view.
        /// </summary>
        [TestMethod]
        public void TestStudentsReturnsView()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[1].TermCode = termCodes[0];
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);

            #endregion Arrange

            #region Act
            var result = Controller.Students("1", null, null, null)
                .AssertViewRendered();
            #endregion Act

            #region Assert

            #endregion Assert		
        }


        /// <summary>
        /// Tests the students calls get AES majors.
        /// </summary>
        [TestMethod]
        public void TestStudentsCallsGetAESMajors()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[1].TermCode = termCodes[0];
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            Controller.Students("1", null, null, null)
                .AssertViewRendered();
            #endregion Act

            #region Assert
            _majorService.AssertWasCalled(a => a.GetAESMajors());
            #endregion Assert		
        }

        /// <summary>
        /// Tests the students filters out students where the student id does not contain the passed student id.
        /// </summary>
        [TestMethod]
        public void TestStudentsFiltersOutStudentsWhereTheStudentIdDoesNotContainThePassedStudentId()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].StudentId = "123456789";
            students[1].StudentId = "111156789";
            students[2].StudentId = "123456799";

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students("234", null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[0].Id ||
                              studentRegistrationModel.Student.Id == students[2].Id);
            }
            #endregion Assert		
        }

        /// <summary>
        /// Tests the last name of the students filters out students where the student last name does not contain the passed.
        /// </summary>
        [TestMethod]
        public void TestStudentsFiltersOutStudentsWhereTheStudentLastNameDoesNotContainThePassedLastName()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].LastName = "John";
            students[1].LastName = "Jimbo";
            students[2].LastName = "Johny";

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, "john", null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[0].Id ||
                              studentRegistrationModel.Student.Id == students[2].Id);
            }
            #endregion Assert
        }
        /// <summary>
        /// Tests the last name of the students filters out students where the student last name does not contain the passed.
        /// </summary>
        [TestMethod]
        public void TestStudentsFiltersOutStudentsWhereTheStudentFirstNameDoesNotContainThePassedFirstName()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].FirstName = "John";
            students[1].FirstName = "Jimbo";
            students[2].FirstName = "Johny";

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, null, "JOHN", null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[0].Id ||
                              studentRegistrationModel.Student.Id == students[2].Id);
            }
            #endregion Assert
        }

        /// <summary>
        /// Tests the last name of the students filters out students where the student last name does not contain the passed.
        /// </summary>
        [TestMethod]
        public void TestStudentsFiltersOutStudentsWhereTheStudentHasAndLogic()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].LastName = "John";
            students[1].LastName = "Jimbo";
            students[2].LastName = "Johny";
            students[0].FirstName = "Mark";
            students[1].FirstName = "John";
            students[2].FirstName = "Markus";

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, "john", "US", null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[2].Id);
            }
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentsFiltersOutStudentsWhereTheStudentHasAndLogic2()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].LastName = "John";
            students[1].LastName = "Jimbo";
            students[2].LastName = "Johny";
            students[0].StudentId = "Mark";
            students[1].StudentId = "John";
            students[2].StudentId = "Markus";

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students("us", "john", null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[2].Id);
            }
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentsFiltersOutStudentsDuplicateStudents()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            
            students[1].LastName = "Jimbo";
            


            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            students[0].SetIdTo(SpecificGuid.GetGuid(9));
            students[2].SetIdTo(SpecificGuid.GetGuid(9));

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[0].Id ||studentRegistrationModel.Student.Id == students[1].Id);
            }
            #endregion Assert
        }

        [TestMethod]
        public void TestStudentsFiltersOutStudentsWithMajorCode()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            majorCodes.Add(CreateValidEntities.MajorCode(2));
            majorCodes.Add(CreateValidEntities.MajorCode(3));
            majorCodes[0].SetIdTo("1");
            majorCodes[1].SetIdTo("2");
            majorCodes[2].SetIdTo("3");
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].Majors.Add(majorCodes[0]);
            students[0].Majors.Add(majorCodes[1]);
            students[0].Majors.Add(majorCodes[2]);
            students[1].Majors.Add(majorCodes[1]);
            students[2].Majors.Add(majorCodes[2]);

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, null, null, majorCodes[2].Id)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[0].Id || studentRegistrationModel.Student.Id == students[2].Id);
            }
            #endregion Assert
        }

        /// <summary>
        /// Tests the students checks if it is registered.
        /// </summary>
        [TestMethod]
        public void TestStudentsChecksIfItIsRegistered()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            majorCodes.Add(CreateValidEntities.MajorCode(2));
            majorCodes.Add(CreateValidEntities.MajorCode(3));
            majorCodes[0].SetIdTo("1");
            majorCodes[1].SetIdTo("2");
            majorCodes[2].SetIdTo("3");
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].Majors.Add(majorCodes[0]);
            students[0].Majors.Add(majorCodes[1]);
            students[0].Majors.Add(majorCodes[2]);
            students[1].Majors.Add(majorCodes[1]);
            students[2].Majors.Add(majorCodes[2]);

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                Assert.IsFalse(studentRegistrationModel.Registration);
            }
            #endregion Assert
        }

        /// <summary>
        /// Tests the students checks if it is registered.
        /// </summary>
        [TestMethod]
        public void TestStudentsChecksIfItIsRegistered2()
        {
            #region Arrange
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("1");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);

            var majorCodes = new List<MajorCode>();
            majorCodes.Add(CreateValidEntities.MajorCode(1));
            majorCodes.Add(CreateValidEntities.MajorCode(2));
            majorCodes.Add(CreateValidEntities.MajorCode(3));
            majorCodes[0].SetIdTo("1");
            majorCodes[1].SetIdTo("2");
            majorCodes[2].SetIdTo("3");
            _majorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].Majors.Add(majorCodes[0]);
            students[0].Majors.Add(majorCodes[1]);
            students[0].Majors.Add(majorCodes[2]);
            students[1].Majors.Add(majorCodes[1]);
            students[2].Majors.Add(majorCodes[2]);

            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations.Add(CreateValidEntities.Registration(2));
            registrations.Add(CreateValidEntities.Registration(3));
            registrations[0].Ceremony = ceremony;
            registrations[1].Ceremony = ceremony;
            registrations[2].Ceremony = ceremony;
            registrations[1].Student = students[1];
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, null, null, null)
                .AssertViewRendered()
                .WithViewData<AdminStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.StudentRegistrationModels.Count);
            foreach (var studentRegistrationModel in result.StudentRegistrationModels)
            {
                if(studentRegistrationModel.Student.Id == students[1].Id)
                {
                    Assert.IsTrue(studentRegistrationModel.Registration);
                }
                else 
                {
                Assert.IsFalse(studentRegistrationModel.Registration);
                }
            }
            #endregion Assert
        }
        #endregion Students Tests 

        #region StudentDetails Tests

        /// <summary>
        /// Tests the student details redirects to index if the student is not found.
        /// </summary>
        [TestMethod]
        public void TestStudentDetailsRedirectsToIndexIfTheStudentIsNotFound()
        {
            #region Arrange
            ControllerRecordFakes.FakeStudent(3, _studentRepository, null, null);
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
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            ControllerRecordFakes.FakeStudent(3, _studentRepository, students, null);
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Student = students[0];
            ControllerRecordFakes.FakeRegistration(3, RegistrationRepository, registrations);
            ControllerRecordFakes.FakeState(3, Controller.Repository);
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

        #region AddStudent Tests

        /// <summary>
        /// Tests the add student with null parameter returns view.
        /// </summary>
        [TestMethod]
        public void TestAddStudentWithNullParameterReturnsView()
        {
            #region Arrange
            string studentId = null;
            #endregion Arrange

            #region Act
            var result = Controller.AddStudent(studentId)
                .AssertViewRendered()
                .WithViewData<SearchStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            _studentService.AssertWasNotCalled(a => a.SearchStudent(Arg<string>.Is.Anything, Arg<string>.Is.Anything));
            #endregion Assert		
        }
        /// <summary>
        /// Tests the add student with empty string parameter returns view.
        /// </summary>
        [TestMethod]
        public void TestAddStudentWithEmptyStringParameterReturnsView()
        {
            #region Arrange
            string studentId = string.Empty;
            #endregion Arrange

            #region Act
            var result = Controller.AddStudent(studentId)
                .AssertViewRendered()
                .WithViewData<SearchStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            _studentService.AssertWasNotCalled(a => a.SearchStudent(Arg<string>.Is.Anything, Arg<string>.Is.Anything));
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student with student id parameter returns view.
        /// </summary>
        [TestMethod]
        public void TestAddStudentWithStudentIdParameterReturnsView1()
        {
            #region Arrange
            string studentId = "123456789";
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("201003");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);
            var searchStudents = new List<SearchStudent>();
            searchStudents.Add(CreateValidEntities.SearchStudent(1));
            searchStudents.Add(CreateValidEntities.SearchStudent(2));
            searchStudents.Add(CreateValidEntities.SearchStudent(3));
            _studentService.Expect(a => a.SearchStudent(studentId, termCodes[0].Id)).Return(searchStudents).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.AddStudent(studentId)
                .AssertViewRendered()
                .WithViewData<SearchStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.SearchStudents.Count);
            Assert.IsNull(Controller.Message);
            Assert.AreEqual(studentId, result.StudentId);
            _studentService.AssertWasCalled(a => a.SearchStudent(studentId, termCodes[0].Id));
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student with student id parameter returns view.
        /// </summary>
        [TestMethod]
        public void TestAddStudentWithStudentIdParameterReturnsView2()
        {
            #region Arrange
            string studentId = "123456789";
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;
            termCodes[0].SetIdTo("201003");
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);
            var searchStudents = new List<SearchStudent>();
            _studentService.Expect(a => a.SearchStudent(studentId, termCodes[0].Id)).Return(searchStudents).Repeat.Any();
            #endregion Arrange

            #region Act
            var result = Controller.AddStudent(studentId)
                .AssertViewRendered()
                .WithViewData<SearchStudentViewModel>();
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.SearchStudents.Count);
            Assert.AreEqual("No results for the student were found.", Controller.Message);
            Assert.AreEqual(studentId, result.StudentId);
            _studentService.AssertWasCalled(a => a.SearchStudent(studentId, termCodes[0].Id));
            #endregion Assert
        }

        #endregion AddStudent Tests

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
            Assert.IsNotNull(controllerClass.BaseType);
            var result = controllerClass.BaseType.Name;
            #endregion Act

            #region Assert
            Assert.AreEqual("ApplicationController", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller has only three attributes.
        /// </summary>
        [TestMethod]
        public void TestControllerHasOnlyThreeAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(3, result.Count());
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
        /// Tests the controller has Anyone With Role Attribute.
        /// </summary>
        [TestMethod]
        public void TestControllerHasAnyoneWithRoleAttribute()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            #endregion Arrange

            #region Act
            var result = controllerClass.GetCustomAttributes(true).OfType<AnyoneWithRoleAttribute>();
            #endregion Act

            #region Assert
            Assert.IsTrue(result.Count() > 0, "AnyoneWithRoleAttribute not found.");
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
            Assert.AreEqual(4, result.Count(), "It looks like a method was added or removed from the controller.");
            #endregion Assert
        }


        /// <summary>
        /// Tests the controller method index contains expected attributes.
        /// #1
        /// </summary>
        [TestMethod]
        public void TestControllerMethodIndexContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("Index");
            #endregion Arrange

            #region Act
            //var expectedAttribute = controllerMethod.GetCustomAttributes(true).OfType<PageTrackingFilter>();
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
           // Assert.AreEqual(1, expectedAttribute.Count(), "PageTrackingFilter not found");
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method students contains expected attributes.
        /// #2
        /// </summary>
        [TestMethod]
        public void TestControllerMethodStudentsContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("Students");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method student details contains expected attributes.
        /// #3
        /// </summary>
        [TestMethod]
        public void TestControllerMethodStudentDetailsContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("StudentDetails");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method add student contains expected attributes.
        /// #4
        /// </summary>
        [TestMethod]
        public void TestControllerMethodAddStudentContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethod("AddStudent");
            #endregion Arrange

            #region Act
            var allAttributes = controllerMethod.GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
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
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
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
        //    var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
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
