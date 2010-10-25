using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapAttachment()
        {
            #region Arrange
            var session = NHibernateSessionManager.Instance.GetSession();
            var id = RegistrationRepository.Queryable.Max(x => x.Id) + 1;
            var dateToCompare1 = new DateTime(2010, 01, 01);
            LoadCeremony(3);
            var ceremony = Repository.OfType<Ceremony>().GetById(2);
            Assert.IsNotNull(ceremony);

            LoadColleges(3);
            var college = CollegeRepository.GetById("2");
            Assert.IsNotNull(college);

            var extraTicketPetition = CreateValidEntities.ExtraTicketPetition(9);

            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Registration>(session, new RegistrationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Address1, "Address1")
                .CheckProperty(c => c.Address2, "Address2")
                .CheckProperty(c => c.Address3, "Address3")
                .CheckProperty(c => c.Cancelled, false)
                .CheckProperty(c => c.Ceremony, ceremony)
                .CheckProperty(c => c.City, "City")
                .CheckProperty(c => c.College, college)
                .CheckProperty(c => c.Comments, "Comments")
                .CheckProperty(c => c.DateRegistered, dateToCompare1)
                .CheckProperty(c => c.Email, "test@testy.com")
                .CheckProperty(c => c.ExtraTicketPetition, extraTicketPetition)
                .CheckProperty(c => c.LabelPrinted, false)
                .CheckProperty(c => c.MailTickets, true)
                //.CheckProperty(c => c.Major)
                .CheckProperty(c => c.NumberTickets, 12)
                .CheckProperty(c => c.SjaBlock, true)
                //.CheckProperty(c => c.State, )
                //.CheckProperty(c => c.Student)
                .CheckProperty(c => c.Zip, "95616")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class RegistrationEqualityComparer : IEqualityComparer
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <returns>
            /// true if the specified objects are equal; otherwise, false.
            /// </returns>
            /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param><exception cref="T:System.ArgumentException"><paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other.</exception>
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x is Ceremony && y is Ceremony)
                {
                    if (((Ceremony)x).Id == ((Ceremony)y).Id && ((Ceremony)x).Location == ((Ceremony)y).Location)
                    {
                        return true;
                    }
                    return false;
                }
                if (x is College && y is College)
                {
                    if (((College)x).Name == ((College)y).Name)
                    {
                        return true;
                    }
                    return false;
                }

                if (x is ExtraTicketPetition && y is ExtraTicketPetition)
                {
                    if (((ExtraTicketPetition)x).DateSubmitted == ((ExtraTicketPetition)y).DateSubmitted)
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
            expectedFields.Add(new NameAndType("Address1", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Address2", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]"
			}));
            expectedFields.Add(new NameAndType("Address3", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]"
			}));
            expectedFields.Add(new NameAndType("Cancelled", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("City", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("College", "Commencement.Core.Domain.College", new List<string>()));
            expectedFields.Add(new NameAndType("Comments", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)1000, Message = \"Please enter less than 1,000 characters\")]"
			}));
            expectedFields.Add(new NameAndType("DateRegistered", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
			{
				"[NHibernate.Validator.Constraints.EmailAttribute()]",
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
			}));
            expectedFields.Add(new NameAndType("ExtraTicketPetition", "Commencement.Core.Domain.ExtraTicketPetition", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
			{
				"[Newtonsoft.Json.JsonPropertyAttribute()]", 
				"[System.Xml.Serialization.XmlIgnoreAttribute()]"
			}));
            expectedFields.Add(new NameAndType("LabelPrinted", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("MailTickets", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Major", "Commencement.Core.Domain.MajorCode", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("NumberTickets", "System.Int32", new List<string>
			{
				"[NHibernate.Validator.Constraints.MinAttribute((Int64)1)]"
			}));
            expectedFields.Add(new NameAndType("SjaBlock", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("State", "Commencement.Core.Domain.State", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Student", "Commencement.Core.Domain.Student", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("TicketDistributionMethod", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("TotalTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Zip", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)15)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Registration));

        }

        #endregion Reflection of Database.	
    }
}
