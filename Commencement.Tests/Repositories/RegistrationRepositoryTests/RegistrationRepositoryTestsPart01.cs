using System.Collections.Generic;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {
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
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("City", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
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
