using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.StudentRepositoryTests
{
    /// <summary>
    /// Entity Name:		Student
    /// LookupFieldName:	FirstName
    /// </summary>
    public partial class StudentRepositoryTests
    {

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapStudent1()
        {
            #region Arrange
            var id = Guid.NewGuid();
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            var dateToCheck2 = new DateTime(2010, 01, 02);
            LoadCeremony(1);
            var ceremony = Repository.OfType<Ceremony>().GetById(1);

            LoadMajorCode(3);
            var majors = Repository.OfType<MajorCode>().GetAll().ToList();
            Assert.IsNotNull(majors);
            Assert.IsTrue(majors.Count > 1);

            LoadTermCode(1);
            var termCode = TermCodeRepository.GetById("1");

            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Student>(session, new StudentEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Pidm, "PIDM")
                .CheckProperty(c => c.StudentId, "123456789")
                .CheckProperty(c => c.FirstName, "FirstName")
                .CheckProperty(c => c.MI, "I")
                .CheckProperty(c => c.LastName, "LastName")
                .CheckProperty(c => c.EarnedUnits, 201m)
                .CheckProperty(c => c.CurrentUnits, 200m)
                .CheckProperty(c => c.Email, "test@testy.com")
                .CheckProperty(c => c.Login, "Login")
                .CheckProperty(c => c.DateAdded, dateToCheck1)
                .CheckProperty(c => c.DateUpdated, dateToCheck2)
                .CheckProperty(c => c.SjaBlock, false)
                .CheckProperty(c => c.Blocked, false)
                .CheckProperty(c => c.AddedBy, "Me")
                .CheckProperty(c => c.TermCode, termCode)
                .CheckProperty(c => c.Ceremony, ceremony)                
                .CheckProperty(c => c.Majors, majors)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapStudent2()
        {
            #region Arrange
            var id = Guid.NewGuid();
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Student>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.SjaBlock, true)
                .CheckProperty(c => c.Blocked, true)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class StudentEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                if (x is Guid && y is Guid)
                {
                    if ((Guid)y == Guid.Empty)
                    {
                        return false;
                    }
                    if (((Guid)x) == ((Guid)y))
                    {
                        return true;
                    }
                    return false; //They should never match
                }

                if (x is Ceremony && y is Ceremony)
                {
                    if (((Ceremony)x).Id == ((Ceremony)y).Id && ((Ceremony)x).Location == ((Ceremony)y).Location)
                    {
                        return true;
                    }
                    return false;
                }

                if (x is IList<MajorCode> && y is IList<MajorCode>)
                {
                    var xVal = (IList<MajorCode>)x;
                    var yVal = (IList<MajorCode>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Name, yVal[i].Name);
                        Assert.AreEqual(xVal[i].DisciplineCode, yVal[i].DisciplineCode);
                    }
                    return true;
                }

                if (x is TermCode && y is TermCode)
                {
                    if (((TermCode)x).Id == ((TermCode)y).Id && ((TermCode)x).Name == ((TermCode)y).Name)
                    {
                        return true;
                    }
                    return false;
                }
                return x.Equals(y);
            }

            public int GetHashCode(object obj)
            {
                throw new NotImplementedException();
            }
        }

        #endregion Fluent Mapping Tests

        #region Reflection of Database.

        /// <summary>
        /// Tests all fields in the database have been tested.
        /// If this fails and no other tests, it means that a field has been added which has not been tested above.
        /// </summary>
        [TestMethod]
        public void TestAllFieldsInTheDatabaseHaveBeenTested()
        {
            #region Arrange
            var expectedFields = new List<NameAndType>();
            expectedFields.Add(new NameAndType("AddedBy", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Blocked", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>()));
            expectedFields.Add(new NameAndType("CurrentUnits", "System.Decimal", new List<string>()));
            expectedFields.Add(new NameAndType("DateAdded", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("DateUpdated", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("EarnedUnits", "System.Decimal", new List<string>()));
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
            }));
            expectedFields.Add(new NameAndType("FirstName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("FullName", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Guid", new List<string>
            {
                "[Newtonsoft.Json.JsonPropertyAttribute()]", 
                "[System.Xml.Serialization.XmlIgnoreAttribute()]"
            }));
            expectedFields.Add(new NameAndType("LastName", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("Login", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("Majors", "System.Collections.Generic.IList`1[Commencement.Core.Domain.MajorCode]", new List<string>()));
            expectedFields.Add(new NameAndType("MI", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
            }));
            expectedFields.Add(new NameAndType("Pidm", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)8)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("SjaBlock", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("StrMajorCodes", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("StrMajors", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("StudentId", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)9)]", 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("TermCode", "Commencement.Core.Domain.TermCode", new List<string>()));
            expectedFields.Add(new NameAndType("TotalUnits", "System.Decimal", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Student));

        }

        #endregion Reflection of Database.	
    }
}
