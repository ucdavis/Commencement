using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapRegistration1()
        {
            #region Arrange
            var id = RegistrationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Registration>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Address1, "Address1")
                .CheckProperty(c => c.Address2, "Address2")
                .CheckProperty(c => c.City, "City")
                .CheckProperty(c => c.Zip, "95616")
                .CheckProperty(c => c.Email, "test@tester.com")
                .CheckProperty(c => c.MailTickets, true)
                .CheckProperty(c => c.GradTrack, true)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistration2()
        {
            #region Arrange
            var id = RegistrationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Registration>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Address1, "Address1")
                .CheckProperty(c => c.Address2, "Address2")
                .CheckProperty(c => c.City, "City")
                .CheckProperty(c => c.Zip, "95616")
                .CheckProperty(c => c.Email, "test@tester.com")
                .CheckProperty(c => c.MailTickets, false)
                .CheckProperty(c => c.GradTrack, false)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistration3()
        {
            #region Arrange
            var id = RegistrationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var student = Repository.OfType<Student>().Queryable.First();
            var state = Repository.OfType<State>().Queryable.First();
            var termCode = Repository.OfType<TermCode>().Queryable.First();
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Registration>(session)
                .CheckProperty(c => c.Id, id)
                .CheckReference(c => c.Student, student)
                .CheckReference(c => c.State, state)
                .CheckReference(c => c.TermCode, termCode)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistration4()
        {
            #region Arrange
            var id = RegistrationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var major = Repository.OfType<MajorCode>().Queryable.First();
            var ceremony = Repository.OfType<Ceremony>().Queryable.First();
            var registration = GetValid(9);
            registration.SetIdTo(id);
            registration.AddParticipation(major, ceremony, 4, null);
            registration.AddParticipation(major, ceremony, 5, null);
            registration.AddParticipation(major, ceremony, 6, null);
            var registrationParticipations = registration.RegistrationParticipations;
            Assert.AreEqual(3, registrationParticipations.Count);

            registration.AddPetition(CreateValidEntities.RegistrationPetition(7));
            registration.AddPetition(CreateValidEntities.RegistrationPetition(8));
            foreach (var registrationPetition in registration.RegistrationPetitions)
            {
                registrationPetition.MajorCode = major;
            }
            var petitions = registration.RegistrationPetitions;
            Assert.AreEqual(2, petitions.Count);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Registration>(session, new RegistrationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.RegistrationParticipations, registrationParticipations)
                .CheckProperty(c => c.RegistrationPetitions, petitions)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapRegistration5()
        {
            #region Arrange
            var id = RegistrationRepository.Queryable.Max(a => a.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            Repository.OfType<SpecialNeed>().DbContext.BeginTransaction();
            LoadSpecialNeeds(5);
            Repository.OfType<SpecialNeed>().DbContext.CommitTransaction();
            var registration = GetValid(9);
            registration.SetIdTo(id);
            registration.SpecialNeeds.Add(Repository.OfType<SpecialNeed>().GetNullableById(1));
            registration.SpecialNeeds.Add(Repository.OfType<SpecialNeed>().GetNullableById(3));
            registration.SpecialNeeds.Add(Repository.OfType<SpecialNeed>().GetNullableById(4));
            var specialNeeds = registration.SpecialNeeds;
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Registration>(session, new RegistrationEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.SpecialNeeds, specialNeeds)
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
                if (x is IList<RegistrationParticipation> && y is IList<RegistrationParticipation>)
                {
                    var xVal = (IList<RegistrationParticipation>)x;
                    var yVal = (IList<RegistrationParticipation>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].NumberTickets, yVal[i].NumberTickets);
                        Assert.AreEqual(xVal[i].Id, yVal[i].Id);
                    }
                    return true;
                }

                if (x is IList<RegistrationPetition> && y is IList<RegistrationPetition>)
                {
                    var xVal = (IList<RegistrationPetition>)x;
                    var yVal = (IList<RegistrationPetition>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].ExceptionReason, yVal[i].ExceptionReason);
                        Assert.AreEqual(xVal[i].Id, yVal[i].Id);
                    }
                    return true;
                }

                if (x is IList<SpecialNeed> && y is IList<SpecialNeed>)
                {
                    var xVal = (IList<SpecialNeed>)x;
                    var yVal = (IList<SpecialNeed>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Name, yVal[i].Name);
                        Assert.AreEqual(xVal[i].Id, yVal[i].Id);
                    }
                    return true;
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
