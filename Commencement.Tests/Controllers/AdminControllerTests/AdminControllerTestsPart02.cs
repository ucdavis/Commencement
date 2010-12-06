using System.Collections.Generic;
using System.Linq;
using Commencement.Controllers.ViewModels;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using UCDArch.Testing;

namespace Commencement.Tests.Controllers.AdminControllerTests
{
    public partial class AdminControllerTests
    {
        /*
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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[1].TermCode = termCodes[0];
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[1].TermCode = termCodes[0];
            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.AssertWasCalled(a => a.GetAESMajors());
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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

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

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].LastName = "john";
            students[1].LastName = "jim";
            students[2].LastName = "johny";

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].FirstName = "JOHN";
            students[1].FirstName = "JIM";
            students[2].FirstName = "JOHNY";

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].LastName = "john";
            students[1].LastName = "jim";
            students[2].LastName = "johny";
            students[0].FirstName = "mark";
            students[1].FirstName = "john";
            students[2].FirstName = "markus";

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = termCodes[0];
            var registrations = new List<Registration>();
            registrations.Add(CreateValidEntities.Registration(1));
            registrations[0].Ceremony = ceremony;
            ControllerRecordFakes.FakeRegistration(0, RegistrationRepository, registrations);
            #endregion Arrange

            #region Act
            var result = Controller.Students(null, "john", "us", null)
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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];
            students[0].LastName = "john";
            students[1].LastName = "jim";
            students[2].LastName = "johny";
            students[0].StudentId = "mark";
            students[1].StudentId = "john";
            students[2].StudentId = "markus";

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

            var students = new List<Student>();
            students.Add(CreateValidEntities.Student(1));
            students.Add(CreateValidEntities.Student(2));
            students.Add(CreateValidEntities.Student(3));
            students[0].TermCode = termCodes[0];
            students[1].TermCode = termCodes[0];
            students[2].TermCode = termCodes[0];

            //students[1].LastName = "Jim";



            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);
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
                Assert.IsTrue(studentRegistrationModel.Student.Id == students[0].Id || studentRegistrationModel.Student.Id == students[1].Id);
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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

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

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

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

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
            MajorService.Expect(a => a.GetAESMajors()).Return(majorCodes.AsEnumerable()).Repeat.Any();

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

            ControllerRecordFakes.FakeStudent(0, StudentRepository, students, StudentRepository2);

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
                if (studentRegistrationModel.Student.Id == students[1].Id)
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
        */
    }
}
