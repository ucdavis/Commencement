using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Data.NHibernate;

namespace Commencement.Tests.Repositories.RegistrationPetitionRepositoryTests
{
    /// <summary>
    /// Entity Name:		RegistrationPetition
    /// LookupFieldName:	LastName
    /// </summary>
    public partial class RegistrationPetitionRepositoryTests
    {
        #region Fluent Mapping Tests
        [TestMethod, Ignore]
        public void TestCanCorrectlyMapAttachment()
        {
            #region Arrange

            var id = RegistrationPetitionRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var dateToCheck1 = new DateTime(2010, 01, 01);
            var dateToCheck2 = new DateTime(2010, 01, 02);
            double? transferUnits = 201;

            LoadCeremony(1);
            var ceremony = Repository.OfType<Ceremony>().GetById(1);

            //LoadMajorCode(1);
            var majorCode = MajorCodeRepository.GetById("1");

            var termCode = TermCodeRepository.GetById("1");
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<RegistrationPetition>(session, new RegistrationPetitionEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Ceremony, ceremony)
                .CheckProperty(c => c.CompletionTerm, "CompletionTerm")
                .CheckProperty(c => c.DateDecision, dateToCheck1)
                .CheckProperty(c => c.DateSubmitted, dateToCheck2)
                .CheckProperty(c => c.Email, "test@testy.com")
                .CheckProperty(c => c.ExceptionReason, "Exception reason")
                .CheckProperty(c => c.FirstName, "FirstName")
                .CheckProperty(c => c.IsApproved, true)
                .CheckProperty(c => c.IsPending, false)
                .CheckProperty(c => c.LastName, "LastName")
                .CheckProperty(c => c.Login, "Login")
                .CheckProperty(c => c.MajorCode, majorCode)
                .CheckProperty(c => c.MI, "I")
                .CheckProperty(c => c.Pidm, "PIDM")
                .CheckProperty(c => c.StudentId, "123456789")
                .CheckProperty(c => c.TermCode, termCode)
                .CheckProperty(c => c.TransferUnits, transferUnits)
                .CheckProperty(c => c.TransferUnitsFrom, "FromSomewhere")
                .CheckProperty(c => c.Units, 10m)                
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class RegistrationPetitionEqualityComparer : IEqualityComparer
        {
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

                if (x is MajorCode && y is MajorCode)
                {
                    if (((MajorCode)x).Name == ((MajorCode)y).Name && ((MajorCode)x).DisciplineCode == ((MajorCode)y).DisciplineCode)
                    {
                        return true;
                    }
                    return false;
                }

                if (x is TermCode && y is TermCode)
                {
                    if (((TermCode)x).Name == ((TermCode)y).Name && ((TermCode)x).Id == ((TermCode)y).Id)
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
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>()));
            expectedFields.Add(new NameAndType("CompletionTerm", "System.String", new List<string>
			{
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("DateDecision", "System.Nullable`1[System.DateTime]", new List<string>()));
            expectedFields.Add(new NameAndType("DateSubmitted", "System.DateTime", new List<string>()));
            expectedFields.Add(new NameAndType("Email", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("ExceptionReason", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)1000)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("FirstName", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("FullName", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
			{
				"[Newtonsoft.Json.JsonPropertyAttribute()]", 
				"[System.Xml.Serialization.XmlIgnoreAttribute()]"
			}));
            expectedFields.Add(new NameAndType("IsApproved", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("IsPending", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("LastName", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Login", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("MajorCode", "Commencement.Core.Domain.MajorCode", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("MI", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]"
			}));
            expectedFields.Add(new NameAndType("Pidm", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)8)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("StudentId", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)9)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("TermCode", "Commencement.Core.Domain.TermCode", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("TransferUnits", "System.Nullable`1[System.Double]", new List<string>()));
            expectedFields.Add(new NameAndType("TransferUnitsFrom", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
			}));
            expectedFields.Add(new NameAndType("Units", "System.Decimal", new List<string>()));
            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(RegistrationPetition));

        }

        #endregion Reflection of Database.
    }
}
