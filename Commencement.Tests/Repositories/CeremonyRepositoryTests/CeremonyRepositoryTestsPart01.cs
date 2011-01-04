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

namespace Commencement.Tests.Repositories.CeremonyRepositoryTests
{
    partial class CeremonyRepositoryTests
    {
        #region Fluent Mapping Tests

        [TestMethod]
        public void TestCanCorrectlyMapCeremony1()
        {
            #region Arrange
            var id = CeremonyRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var compareDate1 = new DateTime(2010, 01, 01);
            var compareDate2 = new DateTime(2010, 01, 02);
            var compareDate3 = new DateTime(2010, 01, 03);
            var compareDate4 = new DateTime(2010, 01, 04);
            LoadColleges(3);
            var colleges = Repository.OfType<College>().GetAll();
            //LoadCeremonyEditors(2);
            LoadUsers(3);
            var ceremony = new Ceremony();
            ceremony.SetIdTo(id);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(1), true);
            ceremony.AddEditor(Repository.OfType<vUser>().GetById(2), false);
            LoadMajorCode(3);
            var majors = Repository.OfType<MajorCode>().GetAll();
            LoadRegistrationPetitions(3);
            var registrationPetitions = Repository.OfType<RegistrationPetition>().GetAll();
            foreach (var registrationPetition in registrationPetitions)
            {
                registrationPetition.Ceremony = ceremony; //This would be set when the student creates the RegistrationPetition
            }

            LoadTemplateType(1);
            LoadTemplate(3);
            var templates = Repository.OfType<Template>().GetAll();
            foreach (var template in templates)
            {
                template.Ceremony = ceremony;
            }
            //LoadTermCode(1);
            var termCode = Repository.OfType<TermCode>().Queryable.First();
            LoadRegistrationParticipations(3,3);
            var registrationParticipations = Repository.OfType<RegistrationParticipation>().GetAll();
            foreach (var registrationParticipation in registrationParticipations)
            {
                registrationParticipation.Ceremony = ceremony;
            }

            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Ceremony>(session, new CeremonyEqualityComparer())
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Colleges, colleges)
                .CheckProperty(c => c.DateTime, compareDate1)
                .CheckProperty(c => c.Editors, ceremony.Editors)
                .CheckProperty(c => c.ExtraTicketDeadline, compareDate2)
                .CheckProperty(c => c.ExtraTicketPerStudent, 5)
                .CheckProperty(c => c.Location, "Location")
                .CheckProperty(c => c.Majors, majors)
                .CheckProperty(c => c.PrintingDeadline, compareDate3)
                .CheckProperty(c => c.RegistrationDeadline, compareDate4)
                .CheckProperty(c => c.RegistrationPetitions, registrationPetitions)
                .CheckProperty(c => c.Templates, templates)
                .CheckProperty(c => c.TermCode, termCode)
                .CheckProperty(c => c.TicketsPerStudent, 6)
                .CheckProperty(c => c.TotalTickets, 1000)
                .CheckProperty(c => c.RegistrationParticipations, registrationParticipations) 
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapCeremony2()
        {
            #region Arrange
            var id = CeremonyRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var compareDate1 = new DateTime(2010, 01, 01);
            var compareDate2 = new DateTime(2010, 01, 02);
            var compareDate3 = new DateTime(2010, 01, 03);
            var compareDate4 = new DateTime(2010, 01, 04);
            var compareDate5 = new DateTime(2010, 01, 05);
            var compareDate6 = new DateTime(2010, 01, 06);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Ceremony>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Location, "Location")
                .CheckProperty(c => c.DateTime, compareDate1)
                .CheckProperty(c => c.TicketsPerStudent, 5)
                .CheckProperty(c => c.TotalTickets, 1000)
                .CheckProperty(c => c.TotalStreamingTickets, null) //todo
                .CheckProperty(c => c.PrintingDeadline, compareDate2)
                .CheckProperty(c => c.RegistrationBegin, compareDate3)
                .CheckProperty(c => c.RegistrationDeadline, compareDate4)
                .CheckProperty(c => c.ExtraTicketBegin, compareDate5)
                .CheckProperty(c => c.ExtraTicketDeadline, compareDate6)
                .CheckProperty(c => c.ExtraTicketPerStudent, 5)
                .CheckProperty(c => c.MinUnits, 5)
                .CheckProperty(c => c.PetitionThreshold, 5)
                .CheckProperty(c => c.HasStreamingTickets, true) //todo
                .CheckProperty(c => c.ConfirmationText, "ConfirmationText")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        [TestMethod]
        public void TestCanCorrectlyMapCeremony3()
        {
            #region Arrange
            var id = CeremonyRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            var compareDate1 = new DateTime(2010, 01, 01);
            var compareDate2 = new DateTime(2010, 01, 02);
            var compareDate3 = new DateTime(2010, 01, 03);
            var compareDate4 = new DateTime(2010, 01, 04);
            var compareDate5 = new DateTime(2010, 01, 05);
            var compareDate6 = new DateTime(2010, 01, 06);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Ceremony>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.Location, "Location")
                .CheckProperty(c => c.DateTime, compareDate1)
                .CheckProperty(c => c.TicketsPerStudent, 5)
                .CheckProperty(c => c.TotalTickets, 1000)
                .CheckProperty(c => c.TotalStreamingTickets, 66) 
                .CheckProperty(c => c.PrintingDeadline, compareDate2)
                .CheckProperty(c => c.RegistrationBegin, compareDate3)
                .CheckProperty(c => c.RegistrationDeadline, compareDate4)
                .CheckProperty(c => c.ExtraTicketBegin, compareDate5)
                .CheckProperty(c => c.ExtraTicketDeadline, compareDate6)
                .CheckProperty(c => c.ExtraTicketPerStudent, 5)
                .CheckProperty(c => c.MinUnits, 5)
                .CheckProperty(c => c.PetitionThreshold, 5)
                .CheckProperty(c => c.HasStreamingTickets, false) 
                .CheckProperty(c => c.ConfirmationText, "ConfirmationText")
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class CeremonyEqualityComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                if (x == null || y == null)
                {
                    return false;
                }

                if (x is IList<College> && y is IList<College>)
                {
                    var xVal = (IList<College>)x;
                    var yVal = (IList<College>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Name, yVal[i].Name);
                        Assert.AreEqual(xVal[i].Display, yVal[i].Display);
                    }
                    return true;
                }
                if (x is IList<CeremonyEditor> && y is IList<CeremonyEditor>)
                {
                    var xVal = (IList<CeremonyEditor>)x;
                    var yVal = (IList<CeremonyEditor>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Owner, yVal[i].Owner);
                        Assert.AreEqual(xVal[i].User.FirstName, yVal[i].User.FirstName);
                    }
                    return true;
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
                if (x is IList<RegistrationPetition> && y is IList<RegistrationPetition>)
                {
                    var xVal = (IList<RegistrationPetition>)x;
                    var yVal = (IList<RegistrationPetition>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].ExceptionReason, yVal[i].ExceptionReason);
                    }
                    return true;
                }
                if (x is IList<RegistrationParticipation> && y is IList<RegistrationParticipation>)
                {
                    var xVal = (IList<RegistrationParticipation>)x;
                    var yVal = (IList<RegistrationParticipation>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].NumberTickets, yVal[i].NumberTickets);
                    }
                    return true;
                }
                if (x is IList<Registration> && y is IList<Registration>)
                {
                    var xVal = (IList<Registration>)x;
                    var yVal = (IList<Registration>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].Address1, yVal[i].Address1);
                    }
                    return true;
                }
                if (x is IList<Template> && y is IList<Template>)
                {
                    var xVal = (IList<Template>)x;
                    var yVal = (IList<Template>)y;
                    Assert.AreEqual(xVal.Count, yVal.Count);
                    for (int i = 0; i < xVal.Count; i++)
                    {
                        Assert.AreEqual(xVal[i].BodyText, yVal[i].BodyText);
                        Assert.AreEqual(xVal[i].IsActive, yVal[i].IsActive);
                        Assert.AreEqual(xVal[i].Subject, yVal[i].Subject);
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
            expectedFields.Add(new NameAndType("AvailableStreamingTickets", "System.Nullable`1[System.Int32]", new List<string>()));
            expectedFields.Add(new NameAndType("AvailableTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Colleges", "System.Collections.Generic.IList`1[Commencement.Core.Domain.College]", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("ConfirmationText", "System.String", new List<string>
            { 
                 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
            }));
            expectedFields.Add(new NameAndType("DateTime", "System.DateTime", new List<string>
			{
				"[NHibernate.Validator.Constraints.FutureAttribute()]", 
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Editors", "System.Collections.Generic.IList`1[Commencement.Core.Domain.CeremonyEditor]", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("ExtraTicketBegin", "System.DateTime", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("ExtraTicketDeadline", "System.DateTime", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("ExtraTicketPerStudent", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("HasStreamingTickets", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
			{
				"[Newtonsoft.Json.JsonPropertyAttribute()]", 
				"[System.Xml.Serialization.XmlIgnoreAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Location", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)200)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Majors", "System.Collections.Generic.IList`1[Commencement.Core.Domain.MajorCode]", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("MinUnits", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("Name", "System.String", new List<string>()));
            expectedFields.Add(new NameAndType("PetitionThreshold", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("PrintingDeadline", "System.DateTime", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("ProjectedAvailableStreamingTickets", "System.Nullable`1[System.Int32]", new List<string>()));
            expectedFields.Add(new NameAndType("ProjectedAvailableTickets", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("ProjectedTicketCount", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("ProjectedTicketStreamingCount", "System.Nullable`1[System.Int32]", new List<string>()));
            expectedFields.Add(new NameAndType("RegistrationBegin", "System.DateTime", new List<string>
            {
                "[NHibernate.Validator.Constraints.NotNullAttribute()]"
            }));
            expectedFields.Add(new NameAndType("RegistrationDeadline", "System.DateTime", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("RegistrationParticipations", "System.Collections.Generic.IList`1[Commencement.Core.Domain.RegistrationParticipation]", new List<string>{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("RegistrationPetitions", "System.Collections.Generic.IList`1[Commencement.Core.Domain.RegistrationPetition]", new List<string>{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));            
            expectedFields.Add(new NameAndType("Templates", "System.Collections.Generic.IList`1[Commencement.Core.Domain.Template]", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));

            expectedFields.Add(new NameAndType("TermCode", "Commencement.Core.Domain.TermCode", new List<string>
			{
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
            expectedFields.Add(new NameAndType("TicketCount", "System.Int32", new List<string>()));
            expectedFields.Add(new NameAndType("TicketsPerStudent", "System.Int32", new List<string>
			{
				"[NHibernate.Validator.Constraints.MinAttribute((Int64)1)]",
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"                
			}));
            expectedFields.Add(new NameAndType("TicketStreamingCount", "System.Nullable`1[System.Int32]", new List<string>()));
            expectedFields.Add(new NameAndType("TotalStreamingTickets", "System.Nullable`1[System.Int32]", new List<string>()));            
            expectedFields.Add(new NameAndType("TotalTickets", "System.Int32", new List<string>
			{
				"[NHibernate.Validator.Constraints.MinAttribute((Int64)1)]",
				"[NHibernate.Validator.Constraints.NotNullAttribute()]"     
			}));


            #endregion Arrange

            AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Ceremony));

        }

        #endregion Reflection of Database.	
    }
}
