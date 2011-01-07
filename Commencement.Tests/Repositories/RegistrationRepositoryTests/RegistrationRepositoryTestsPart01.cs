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
        public void TestCanCorrectlyMapRegistration()
        {
            Assert.Inconclusive("Todo Mapping tests");
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

                //Check if the date is the same (seconds may be different)
                if (x is ExtraTicketPetition && y is ExtraTicketPetition)
                {
                    if (((ExtraTicketPetition)x).DateSubmitted.Date == ((ExtraTicketPetition)y).DateSubmitted.Date &&
                        ((ExtraTicketPetition)x).Id == ((ExtraTicketPetition)y).Id)
                    {
                        return true;
                    }
                    return false;
                }

                if (x is MajorCode && y is MajorCode)
                {
                    if (((MajorCode)x).Name == ((MajorCode)y).Name && ((MajorCode)x).DisciplineCode == ((MajorCode)y).DisciplineCode)
                    {
                        return true;
                    }
                    return false;
                }

                if (x is State && y is State)
                {
                    if (((State)x).Id == ((State)y).Id && ((State)x).Name == ((State)y).Name)
                    {
                        return true;
                    }
                    return false;
                }

                if (x is Student && y is Student)
                {
                    if (((Student)x).Id == ((Student)y).Id && ((Student)x).FirstName == ((Student)y).FirstName)
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
            expectedFields.Add(new NameAndType("City", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
			{
				"[NHibernate.Validator.Constraints.EmailAttribute()]",
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
			}));
            expectedFields.Add(new NameAndType("GradTrack", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
			{
				"[Newtonsoft.Json.JsonPropertyAttribute()]", 
				"[System.Xml.Serialization.XmlIgnoreAttribute()]"
			}));
            expectedFields.Add(new NameAndType("MailTickets", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("MajorCodes", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Majors", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("RegistrationParticipations", "System.Collections.Generic.IList`1[Commencement.Core.Domain.RegistrationParticipation]", new List<string>()));
            expectedFields.Add(new NameAndType("RegistrationPetitions", "System.Collections.Generic.IList`1[Commencement.Core.Domain.RegistrationPetition]", new List<string>()));
            expectedFields.Add(new NameAndType("SpecialNeeds", "System.Collections.Generic.IList`1[Commencement.Core.Domain.SpecialNeed]", new List<string>()));
            expectedFields.Add(new NameAndType("State", "Commencement.Core.Domain.State", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Student", "Commencement.Core.Domain.Student", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("TermCode", "Commencement.Core.Domain.TermCode", new List<string>
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
