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
        protected IRepository<Ceremony> CeremonyRepository;

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

            CeremonyRepository = FakeRepository<Ceremony>();
            Controller.Repository.Expect(a => a.OfType<Ceremony>()).Return(CeremonyRepository).Repeat.Any();
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

        /// <summary>
        /// Tests the add student mapping.
        /// </summary>
        [TestMethod]
        public void TestAddStudentMapping()
        {
            "~/Admin/AddStudent/".ShouldMapTo<AdminController>(a => a.AddStudent(null), true);
        }

        /// <summary>
        /// Tests the add student confirm get mapping.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmGetMapping()
        {
            "~/Admin/AddStudentConfirm/".ShouldMapTo<AdminController>(a => a.AddStudentConfirm(null,null), true);
        }

        /// <summary>
        /// Tests the add student confirm post mapping.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostMapping()
        {
            "~/Admin/AddStudentConfirm/".ShouldMapTo<AdminController>(a => a.AddStudentConfirm(null, null,null), true);
        }

        /// <summary>
        /// Tests the change major get mapping.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorGetMapping()
        {
            "~/Admin/ChangeMajor/5".ShouldMapTo<AdminController>(a => a.ChangeMajor(5));
        }

        /// <summary>
        /// Tests the change major post mapping.
        /// </summary>
        [TestMethod]
        public void TestChangeMajorPostMapping()
        {
            "~/Admin/ChangeMajor/5".ShouldMapTo<AdminController>(a => a.ChangeMajor(5, "123"), true);
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

        #region AddStudentConfirm Tests

        #region Get Tests

        /// <summary>
        /// Tests the add student confirm get redirects when student id is null.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmGetRedirectsWhenStudentIdIsNull()
        {
            #region Arrange
            string studentId = null;
            string majorId = "Test";     
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId)
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.AddStudent(studentId));
            #endregion Act

            #region Assert

            #endregion Assert		
        }

        /// <summary>
        /// Tests the add student confirm get redirects when student id is empty.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmGetRedirectsWhenStudentIdIsEmpty()
        {
            #region Arrange
            string studentId = string.Empty;
            string majorId = "Test";
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId)
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.AddStudent(studentId));
            #endregion Act

            #region Assert

            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm get redirects when major id is null.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmGetRedirectsWhenMajorIdIsNull()
        {
            #region Arrange
            string studentId = "Test";
            string majorId = null;
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId)
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.AddStudent(studentId));
            #endregion Act

            #region Assert

            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm get redirects when major id is empty.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmGetRedirectsWhenMajorIdIsEmpty()
        {
            #region Arrange
            string studentId = "Test";
            string majorId = string.Empty;
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId)
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.AddStudent(studentId));
            #endregion Act

            #region Assert

            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm throws exception if no students found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddStudentConfirmGetThrowsExceptionIfNoStudentsFound1()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            _studentService.Expect(a => a.SearchStudent(studentId, termCode)).Return(new List<SearchStudent>()).Repeat.Any();

            #endregion Arrange
            try
            {
                #region Act
                Controller.AddStudentConfirm(studentId, majorId);
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("Unable to find requested record.", ex.Message);
                _studentService.AssertWasCalled(a => a.SearchStudent(studentId, termCode));
                throw;
            }	
        }

        /// <summary>
        /// Tests the add student confirm throws exception if no students found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddStudentConfirmGetThrowsExceptionIfNoStudentsFound2()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var searchStudents = new List<SearchStudent>();
            searchStudents.Add(CreateValidEntities.SearchStudent(1));
            searchStudents.Add(CreateValidEntities.SearchStudent(2));
            _studentService.Expect(a => a.SearchStudent(studentId, termCode)).Return(searchStudents).Repeat.Any();

            #endregion Arrange
            try
            {
                #region Act
                Controller.AddStudentConfirm(studentId, majorId);
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("Unable to find requested record.", ex.Message);
                _studentService.AssertWasCalled(a => a.SearchStudent(studentId, termCode));
                throw;
            }
        }


        /// <summary>
        /// Tests the add student confirm get returns view if A student is found.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmGetReturnsViewIfAStudentIsFound1()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var searchStudents = new List<SearchStudent>();
            searchStudents.Add(CreateValidEntities.SearchStudent(1));
            searchStudents.Add(CreateValidEntities.SearchStudent(2));
            searchStudents[0].MajorCode = majorId;
            searchStudents[1].MajorCode = majorId;
            _studentService.Expect(a => a.SearchStudent(studentId, termCode)).Return(searchStudents).Repeat.Any();           
            #endregion Arrange

            #region Act
            var result = Controller.AddStudentConfirm(studentId, majorId)
                .AssertViewRendered()
                .WithViewData<Student>();

            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pidm1", result.Pidm);
            Assert.AreEqual("Id1", result.StudentId);
            Assert.AreEqual("FirstName1", result.FirstName);
            Assert.AreEqual("MI1", result.MI);
            Assert.AreEqual("LastName1", result.LastName);
            Assert.AreEqual(100m, result.Units);
            Assert.AreEqual("Email1", result.Email);
            Assert.AreEqual("LoginId1", result.Login);
            Assert.AreEqual("201003", result.TermCode.Id);
            Assert.AreSame(majors[0], result.Majors[0]);
            #endregion Assert		
        }

        #endregion Get Tests

        #region Post Tests
        /// <summary>
        /// Tests the add student confirm post throws exception if student is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddStudentConfirmPostThrowsExceptionIfStudentIsNull()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange
            try
            {
                #region Act
                Controller.AddStudentConfirm(studentId, majorId, null);
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("Student cannot be null.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddStudentConfirmPostThrowsExceptionIfMajorCodeNull()
        {
            #region Arrange
            string studentId = "1";
            string majorId = null;
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange
            try
            {
                #region Act
                Controller.AddStudentConfirm(studentId, majorId, new Student());
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("Major code is required.", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Tests the add student confirm post throws exception if major code not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(UCDArch.Core.Utils.PreconditionException))]
        public void TestAddStudentConfirmPostThrowsExceptionIfMajorCodeNotFound()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "2";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange
            try
            {
                #region Act
                Controller.AddStudentConfirm(studentId, majorId, new Student());
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("Unable to find major.", ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Tests the add student confirm post does not save if ceremony not found.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesNotSaveIfCeremonyNotFound1()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = CreateValidEntities.TermCode(9); //TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange

            #region Act
            var result = Controller.AddStudentConfirm(studentId, majorId, new Student())
                .AssertViewRendered()
                .WithViewData<Student>();
            #endregion Act

            #region Assert
            Controller.ModelState.AssertErrorsAre("No ceremony exists for this major for the current term.");
            Assert.IsNotNull(result);
            Assert.AreEqual(termCode, result.TermCode.Id);
            StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            #endregion Assert		
        }

        /// <summary>
        /// Tests the add student confirm post does not save if ceremony not found.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesNotSaveIfCeremonyNotFound2()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange

            #region Act
            var result = Controller.AddStudentConfirm(studentId, majorId, new Student())
                .AssertViewRendered()
                .WithViewData<Student>();
            #endregion Act

            #region Assert
            Controller.ModelState.AssertErrorsAre("No ceremony exists for this major for the current term.");
            Assert.IsNotNull(result);
            Assert.AreEqual(termCode, result.TermCode.Id);
            StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm post does not save if student already exists and has that major.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesNotSaveIfStudentAlreadyExistsAndHasThatMajor()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            students[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange

            #region Act
            var result = Controller.AddStudentConfirm(studentId, majorId, new Student())
                .AssertViewRendered()
                .WithViewData<Student>();
            #endregion Act

            #region Assert
            Assert.AreEqual("Student already exists.", Controller.Message);
            Assert.IsNotNull(result);
            Assert.AreEqual(termCode, result.TermCode.Id);
            StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm post does save if student already exists and does not have that major.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesSaveIfStudentAlreadyExistsAndDoesNotHaveThatMajor()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            students[0].Majors.Add(ceremonies[0].Majors[0]);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId, new Student())
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            _emailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
            var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual(2, args.Majors.Count);
            Assert.AreSame(ceremonies[0].Majors[0], args.Majors[0]);
            Assert.AreSame(majors[0], args.Majors[1]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm post does save if student already exists and does not have that major notifies users if email did not work.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesSaveIfStudentAlreadyExistsAndDoesNotHaveThatMajorNotifiesUsersIfEmailDidNotWork()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            students[0].Majors.Add(ceremonies[0].Majors[0]);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            _emailService.Expect(a =>a.SendAddPermission(Arg<IRepository>.Is.Anything, 
                Arg<Student>.Is.Anything, 
                Arg<Ceremony>.Is.Anything)).Throw(new Exception("An Exception."));
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId, new Student())
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
            #endregion Act

            #region Assert
            Assert.AreEqual("There was a problem sending FirstName1 LastName1 an email.", Controller.Message);
            StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            _emailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
            var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual(2, args.Majors.Count);
            Assert.AreSame(ceremonies[0].Majors[0], args.Majors[0]);
            Assert.AreSame(majors[0], args.Majors[1]);
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm post does not save if student has validation errors.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesNotSaveIfStudentHasValidationErrors()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students[0].StudentId = studentId;
            students[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            students[0].Majors.Add(ceremonies[0].Majors[0]);
            students[0].Pidm = null; //Invalid
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId, new Student())
                .AssertViewRendered()
                .WithViewData<Student>();
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            StudentRepository2.AssertWasNotCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            Controller.ModelState.AssertErrorsAre("Pidm: may not be null or empty");
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm post does save if student does not exist.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesSaveIfStudentDoesNotExist()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(99));
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(studentId, majorId, CreateValidEntities.Student(3))
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            _emailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
            var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual(1, args.Majors.Count);
            Assert.AreSame(majors[0], args.Majors[0]);
            Assert.AreEqual("FirstName3", args.FirstName);
            Assert.AreNotEqual(Guid.Empty, args.Id);
            Console.WriteLine(args.Id);
            #endregion Assert
        }

        /// <summary>
        /// Tests the add student confirm post does save if student does not exist for that term.
        /// </summary>
        [TestMethod]
        public void TestAddStudentConfirmPostDoesSaveIfStudentDoesNotExistForThatTerm()
        {
            #region Arrange
            string studentId = "1";
            string majorId = "1";
            string termCode = "201003";
            LoadTermCodes(termCode);
            var majors = new List<MajorCode>();
            majors.Add(CreateValidEntities.MajorCode(1));
            ControllerRecordFakes.FakeMajors(0, _majorRepository, majors);
            var ceremonies = new List<Ceremony>();
            ceremonies.Add(CreateValidEntities.Ceremony(1));
            ceremonies[0].TermCode = TermCodeRepository.Queryable.FirstOrDefault();
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(98));
            ceremonies[0].Majors.Add(CreateValidEntities.MajorCode(99));
            ceremonies[0].Majors.Add(majors[0]);
            ControllerRecordFakes.FakeCeremony(0, CeremonyRepository, ceremonies);
            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = CreateValidEntities.TermCode(9);
            ControllerRecordFakes.FakeStudent(0, _studentRepository, students, StudentRepository2);
            #endregion Arrange

            #region Act
            Controller.AddStudentConfirm(students[0].StudentId, majorId, CreateValidEntities.Student(3))
                .AssertActionRedirect()
                .ToAction<AdminController>(a => a.Students(studentId, null, null, null));
            #endregion Act

            #region Assert
            Assert.IsNull(Controller.Message);
            StudentRepository2.AssertWasCalled(a => a.EnsurePersistent(Arg<Student>.Is.Anything));
            _emailService.AssertWasCalled(a => a.SendAddPermission(Arg<IRepository>.Is.Anything, Arg<Student>.Is.Anything, Arg<Ceremony>.Is.Anything));
            var args = (Student)StudentRepository2.GetArgumentsForCallsMadeOn(a => a.EnsurePersistent(Arg<Student>.Is.Anything))[0][0];
            Assert.IsNotNull(args);
            Assert.AreEqual(1, args.Majors.Count);
            Assert.AreSame(majors[0], args.Majors[0]);
            Assert.AreEqual("FirstName3", args.FirstName);
            Assert.AreNotEqual(Guid.Empty, args.Id);
            Assert.AreEqual(students[0].StudentId, args.StudentId);
            Assert.AreNotSame(students[0].TermCode, args.TermCode);
            #endregion Assert
        }
        #endregion Post Tests

        #endregion AddStudentConfirm Tests

        #region ChangeMajor Tests
        #region Get Tests

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
        #endregion Get Tests
        #region Post Tests

        #endregion Post Tests
        #endregion ChangeMajor Tests

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
            Assert.AreEqual(6, result.Count(), "It looks like a method was added or removed from the controller.");
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

        /// <summary>
        /// Tests the controller method add student confirm get contains expected attributes.
        /// #5
        /// </summary>
        [TestMethod]
        public void TestControllerMethodAddStudentConfirmGetContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "AddStudentConfirm");
            #endregion Arrange

            #region Act
            //var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
            var allAttributes = controllerMethod.ElementAt(0).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            //Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
            Assert.AreEqual(0, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }

        /// <summary>
        /// Tests the controller method add student confirm post contains expected attributes.
        /// #6
        /// </summary>
        [TestMethod]
        public void TestControllerMethodAddStudentConfirmPostContainsExpectedAttributes()
        {
            #region Arrange
            var controllerClass = _controllerClass;
            var controllerMethod = controllerClass.GetMethods().Where(a => a.Name == "AddStudentConfirm");
            #endregion Arrange

            #region Act
            var expectedAttribute = controllerMethod.ElementAt(1).GetCustomAttributes(true).OfType<AcceptPostAttribute>();
            var allAttributes = controllerMethod.ElementAt(1).GetCustomAttributes(true);
            #endregion Act

            #region Assert
            Assert.AreEqual(1, expectedAttribute.Count(), "AcceptPostAttribute not found");
            Assert.AreEqual(1, allAttributes.Count(), "More than expected custom attributes found.");
            #endregion Assert
        }


        #endregion Controller Method Tests

        #endregion Reflection

        #region Helpers

        private void LoadTermCodes(string termCode)
        {
            var termCodes = new List<TermCode>();
            termCodes.Add(CreateValidEntities.TermCode(1));
            termCodes[0].IsActive = true;            
            ControllerRecordFakes.FakeTermCode(0, TermCodeRepository, termCodes);
            termCodes[0].SetIdTo(termCode);
        }

        #endregion Helpers
    }
}
