using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
using FluentNHibernate.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories
{
	/// <summary>
	/// Entity Name:		Template
	/// LookupFieldName:	BodyText
	/// </summary>
    [TestClass, Ignore]
	public class TemplateRepositoryTests : AbstractRepositoryTests<Template, int, TemplateMap>
	{
		/// <summary>
		/// Gets or sets the Template repository.
		/// </summary>
		/// <value>The Template repository.</value>
		public IRepository<Template> TemplateRepository { get; set; }
		
		#region Init and Overrides

		/// <summary>
		/// Initializes a new instance of the <see cref="TemplateRepositoryTests"/> class.
		/// </summary>
		public TemplateRepositoryTests()
		{
			TemplateRepository = new Repository<Template>();
		}

		/// <summary>
		/// Gets the valid entity of type T
		/// </summary>
		/// <param name="counter">The counter.</param>
		/// <returns>A valid entity of type T</returns>
		protected override Template GetValid(int? counter)
		{
			var rtValue = CreateValidEntities.Template(counter);
			rtValue.TemplateType = Repository.OfType<TemplateType>().GetById(1);
		    rtValue.Ceremony = Repository.OfType<Ceremony>().GetById(1);
			return rtValue;
		}

		/// <summary>
		/// A Query which will return a single record
		/// </summary>
		/// <param name="numberAtEnd"></param>
		/// <returns></returns>
		protected override IQueryable<Template> GetQuery(int numberAtEnd)
		{
			return TemplateRepository.Queryable.Where(a => a.BodyText.EndsWith(numberAtEnd.ToString()));
		}

		/// <summary>
		/// A way to compare the entities that were read.
		/// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="counter"></param>
		protected override void FoundEntityComparison(Template entity, int counter)
		{
			Assert.AreEqual("BodyText" + counter, entity.BodyText);
		}

		/// <summary>
		/// Updates , compares, restores.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="action">The action.</param>
		protected override void UpdateUtility(Template entity, ARTAction action)
		{
			const string updateValue = "Updated";
			switch (action)
			{
				case ARTAction.Compare:
					Assert.AreEqual(updateValue, entity.BodyText);
					break;
				case ARTAction.Restore:
					entity.BodyText = RestoreValue;
					break;
				case ARTAction.Update:
					RestoreValue = entity.BodyText;
					entity.BodyText = updateValue;
					break;
			}
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		protected override void LoadData()
		{
			TemplateRepository.DbContext.BeginTransaction();
		    LoadTermCode(3);
		    LoadCeremony(3);
			LoadTemplateType(3);
			LoadRecords(5);
			TemplateRepository.DbContext.CommitTransaction();
		}

		#endregion Init and Overrides	

        #region Fluent Mapping Tests
        [TestMethod]
        public void TestCanCorrectlyMapAttachment()
        {
            #region Arrange
            var id = TemplateRepository.Queryable.Max(x => x.Id) + 1;
            var session = NHibernateSessionManager.Instance.GetSession();
            LoadTemplateType(2);
            var templateType = Repository.OfType<TemplateType>().GetNullableById(1);
            Assert.IsNotNull(templateType);
            var ceremony = Repository.OfType<Ceremony>().GetById(1);
            #endregion Arrange

            #region Act/Assert
            new PersistenceSpecification<Template>(session)
                .CheckProperty(c => c.Id, id)
                .CheckProperty(c => c.BodyText, "Body Text")
                .CheckReference(c => c.TemplateType, templateType)
                .CheckProperty(c => c.Ceremony, ceremony)
                .CheckProperty(c => c.Subject, "Subject")
                .CheckProperty(c => c.IsActive, true)
                .VerifyTheMappings();
            #endregion Act/Assert
        }

        public class TemplateEqualityComparer : IEqualityComparer
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

                if (x is TemplateType && y is TemplateType)
                {
                    if (((TemplateType)x).Id == ((TemplateType)y).Id && ((TemplateType)x).Name == ((TemplateType)y).Name)
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
		
		#region BodyText Tests
		#region Invalid Tests

		/// <summary>
		/// Tests the BodyText with null value does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestBodyTextWithNullValueDoesNotSave()
		{
			Template template = null;
			try
			{
				#region Arrange
				template = GetValid(9);
				template.BodyText = null;
				#endregion Arrange

				#region Act
				TemplateRepository.DbContext.BeginTransaction();
				TemplateRepository.EnsurePersistent(template);
				TemplateRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(template);
				var results = template.ValidationResults().AsMessageList();
				results.AssertErrorsAre("BodyText: may not be null or empty");
				Assert.IsTrue(template.IsTransient());
				Assert.IsFalse(template.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the BodyText with empty string does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestBodyTextWithEmptyStringDoesNotSave()
		{
			Template template = null;
			try
			{
				#region Arrange
				template = GetValid(9);
				template.BodyText = string.Empty;
				#endregion Arrange

				#region Act
				TemplateRepository.DbContext.BeginTransaction();
				TemplateRepository.EnsurePersistent(template);
				TemplateRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(template);
				var results = template.ValidationResults().AsMessageList();
				results.AssertErrorsAre("BodyText: may not be null or empty");
				Assert.IsTrue(template.IsTransient());
				Assert.IsFalse(template.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the BodyText with spaces only does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestBodyTextWithSpacesOnlyDoesNotSave()
		{
			Template template = null;
			try
			{
				#region Arrange
				template = GetValid(9);
				template.BodyText = " ";
				#endregion Arrange

				#region Act
				TemplateRepository.DbContext.BeginTransaction();
				TemplateRepository.EnsurePersistent(template);
				TemplateRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(template);
				var results = template.ValidationResults().AsMessageList();
				results.AssertErrorsAre("BodyText: may not be null or empty");
				Assert.IsTrue(template.IsTransient());
				Assert.IsFalse(template.IsValid());
				throw;
			}
		}


		#endregion Invalid Tests

		#region Valid Tests

		/// <summary>
		/// Tests the BodyText with one character saves.
		/// </summary>
		[TestMethod]
		public void TestBodyTextWithOneCharacterSaves()
		{
			#region Arrange
			var template = GetValid(9);
			template.BodyText = "x";
			#endregion Arrange

			#region Act
			TemplateRepository.DbContext.BeginTransaction();
			TemplateRepository.EnsurePersistent(template);
			TemplateRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(template.IsTransient());
			Assert.IsTrue(template.IsValid());
			#endregion Assert
		}

		/// <summary>
		/// Tests the BodyText with long value saves.
		/// </summary>
		[TestMethod]
		public void TestBodyTextWithLongValueSaves()
		{
			#region Arrange
			var template = GetValid(9);
			template.BodyText = "x".RepeatTimes(999);
			#endregion Arrange

			#region Act
			TemplateRepository.DbContext.BeginTransaction();
			TemplateRepository.EnsurePersistent(template);
			TemplateRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.AreEqual(999, template.BodyText.Length);
			Assert.IsFalse(template.IsTransient());
			Assert.IsTrue(template.IsValid());
			#endregion Assert
		}

		#endregion Valid Tests
		#endregion BodyText Tests

        #region Ceremony Tests

        #region Invalid Tests

        /// <summary>
        /// Tests the Template with null ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestTemplateWithNullCeremonyDoesNotSave()
        {
            Template template = null;
            try
            {
                #region Arrange
                template = GetValid(9);
                template.Ceremony = null;
                #endregion Arrange

                #region Act
                TemplateRepository.DbContext.BeginTransaction();
                TemplateRepository.EnsurePersistent(template);
                TemplateRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception)
            {
                Assert.IsNotNull(template);
                var results = template.ValidationResults().AsMessageList();
                results.AssertErrorsAre("Ceremony: may not be null");
                Assert.IsTrue(template.IsTransient());
                Assert.IsFalse(template.IsValid());
                throw;
            }
        }

        /// <summary>
        /// Tests the Template with new ceremony does not save.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NHibernate.TransientObjectException))]
        public void TestTemplateWithNewCeremonyDoesNotSave()
        {
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            Template template;
            try
            {
                #region Arrange
                template = GetValid(9);
                template.Ceremony = CreateValidEntities.Ceremony(9);
                template.Ceremony.TermCode = termCodeRepository.GetById("1");
                #endregion Arrange

                #region Act
                TemplateRepository.DbContext.BeginTransaction();
                TemplateRepository.EnsurePersistent(template);
                TemplateRepository.DbContext.CommitTransaction();
                #endregion Act
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
                Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.Ceremony, Entity: Commencement.Core.Domain.Ceremony", ex.Message);
                throw;
            }
        }

        #endregion Invalid Tests

        #region Valid Tests

        /// <summary>
        /// Tests the Template with valid ceremony saves.
        /// </summary>
        [TestMethod]
        public void TestTemplateWithValidCeremonySaves()
        {
            #region Arrange
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            Repository.OfType<Ceremony>().DbContext.BeginTransaction();
            var ceremony = CreateValidEntities.Ceremony(9);
            ceremony.TermCode = termCodeRepository.GetById("1");
            Repository.OfType<Ceremony>().EnsurePersistent(ceremony);
            Repository.OfType<Ceremony>().DbContext.CommitTransaction();
            var template = CreateValidEntities.Template(9);
            template.TemplateType = Repository.OfType<TemplateType>().GetById(1);
            template.Ceremony = ceremony;
            #endregion Arrange

            #region Act
            TemplateRepository.DbContext.BeginTransaction();
            TemplateRepository.EnsurePersistent(template);
            TemplateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Assert.IsFalse(template.IsTransient());
            Assert.IsTrue(template.IsValid());
            #endregion Assert
        }
        #endregion Valid Tests
        #endregion Ceremony Tests

	    #region IsActive Tests

	    /// <summary>
	    /// Tests the IsActive is false saves.
	    /// </summary>
	    [TestMethod]
	    public void TestIsActiveIsFalseSaves()
	    {
	        #region Arrange

	        Template template = GetValid(9);
	        template.IsActive = false;

	        #endregion Arrange

	        #region Act

	        TemplateRepository.DbContext.BeginTransaction();
	        TemplateRepository.EnsurePersistent(template);
	        TemplateRepository.DbContext.CommitTransaction();

	        #endregion Act

	        #region Assert

	        Assert.IsFalse(template.IsActive);
	        Assert.IsFalse(template.IsTransient());
	        Assert.IsTrue(template.IsValid());

	        #endregion Assert
	    }

	    /// <summary>
	    /// Tests the IsActive is true saves.
	    /// </summary>
	    [TestMethod]
	    public void TestIsActiveIsTrueSaves()
	    {
	        #region Arrange

	        var template = GetValid(9);
	        template.IsActive = true;

	        #endregion Arrange

	        #region Act

	        TemplateRepository.DbContext.BeginTransaction();
	        TemplateRepository.EnsurePersistent(template);
	        TemplateRepository.DbContext.CommitTransaction();

	        #endregion Act

	        #region Assert

	        Assert.IsTrue(template.IsActive);
	        Assert.IsFalse(template.IsTransient());
	        Assert.IsTrue(template.IsValid());

	        #endregion Assert
	    }

	    #endregion IsActive Tests

	    #region Subject Tests
	    #region Invalid Tests

	    /// <summary>
	    /// Tests the Subject with too long value does not save.
	    /// </summary>
	    [TestMethod]
	    [ExpectedException(typeof(ApplicationException))]
	    public void TestSubjectWithTooLongValueDoesNotSave()
	    {
	        Template template = null;
	        try
	        {
	            #region Arrange
	            template = GetValid(9);
	            template.Subject = "x".RepeatTimes((100 + 1));
	            #endregion Arrange

	            #region Act
	            TemplateRepository.DbContext.BeginTransaction();
	            TemplateRepository.EnsurePersistent(template);
	            TemplateRepository.DbContext.CommitTransaction();
	            #endregion Act
	        }
	        catch (Exception)
	        {
	            Assert.IsNotNull(template);
	            Assert.AreEqual(100 + 1, template.Subject.Length);
	            var results = template.ValidationResults().AsMessageList();
	            results.AssertErrorsAre("Subject: length must be between 0 and 100");
	            Assert.IsTrue(template.IsTransient());
	            Assert.IsFalse(template.IsValid());
	            throw;
	        }
	    }
	    #endregion Invalid Tests

	    #region Valid Tests

	    /// <summary>
	    /// Tests the Subject with null value saves.
	    /// </summary>
	    [TestMethod]
	    public void TestSubjectWithNullValueSaves()
	    {
	        #region Arrange
	        var template = GetValid(9);
	        template.Subject = null;
	        #endregion Arrange

	        #region Act
	        TemplateRepository.DbContext.BeginTransaction();
	        TemplateRepository.EnsurePersistent(template);
	        TemplateRepository.DbContext.CommitTransaction();
	        #endregion Act

	        #region Assert
	        Assert.IsFalse(template.IsTransient());
	        Assert.IsTrue(template.IsValid());
	        #endregion Assert
	    }

	    /// <summary>
	    /// Tests the Subject with empty string saves.
	    /// </summary>
	    [TestMethod]
	    public void TestSubjectWithEmptyStringSaves()
	    {
	        #region Arrange
	        var template = GetValid(9);
	        template.Subject = string.Empty;
	        #endregion Arrange

	        #region Act
	        TemplateRepository.DbContext.BeginTransaction();
	        TemplateRepository.EnsurePersistent(template);
	        TemplateRepository.DbContext.CommitTransaction();
	        #endregion Act

	        #region Assert
	        Assert.IsFalse(template.IsTransient());
	        Assert.IsTrue(template.IsValid());
	        #endregion Assert
	    }

	    /// <summary>
	    /// Tests the Subject with one space saves.
	    /// </summary>
	    [TestMethod]
	    public void TestSubjectWithOneSpaceSaves()
	    {
	        #region Arrange
	        var template = GetValid(9);
	        template.Subject = " ";
	        #endregion Arrange

	        #region Act
	        TemplateRepository.DbContext.BeginTransaction();
	        TemplateRepository.EnsurePersistent(template);
	        TemplateRepository.DbContext.CommitTransaction();
	        #endregion Act

	        #region Assert
	        Assert.IsFalse(template.IsTransient());
	        Assert.IsTrue(template.IsValid());
	        #endregion Assert
	    }

	    /// <summary>
	    /// Tests the Subject with one character saves.
	    /// </summary>
	    [TestMethod]
	    public void TestSubjectWithOneCharacterSaves()
	    {
	        #region Arrange
	        var template = GetValid(9);
	        template.Subject = "x";
	        #endregion Arrange

	        #region Act
	        TemplateRepository.DbContext.BeginTransaction();
	        TemplateRepository.EnsurePersistent(template);
	        TemplateRepository.DbContext.CommitTransaction();
	        #endregion Act

	        #region Assert
	        Assert.IsFalse(template.IsTransient());
	        Assert.IsTrue(template.IsValid());
	        #endregion Assert
	    }

	    /// <summary>
	    /// Tests the Subject with long value saves.
	    /// </summary>
	    [TestMethod]
	    public void TestSubjectWithLongValueSaves()
	    {
	        #region Arrange
	        var template = GetValid(9);
	        template.Subject = "x".RepeatTimes(100);
	        #endregion Arrange

	        #region Act
	        TemplateRepository.DbContext.BeginTransaction();
	        TemplateRepository.EnsurePersistent(template);
	        TemplateRepository.DbContext.CommitTransaction();
	        #endregion Act

	        #region Assert
	        Assert.AreEqual(100, template.Subject.Length);
	        Assert.IsFalse(template.IsTransient());
	        Assert.IsTrue(template.IsValid());
	        #endregion Assert
	    }

	    #endregion Valid Tests
	    #endregion Subject Tests

		#region TemplateType Tests

		#region Invalid Test
		/// <summary>
		/// Tests the TemplateType with A value of null does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestTemplateTypeWithAValueOfNullDoesNotSave()
		{
			Template template = null;
			try
			{
				#region Arrange
				template = GetValid(9);
				template.TemplateType = null;
				#endregion Arrange

				#region Act
				TemplateRepository.DbContext.BeginTransaction();
				TemplateRepository.EnsurePersistent(template);
				TemplateRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(template);
				Assert.IsNull(template.TemplateType);
				var results = template.ValidationResults().AsMessageList();
				results.AssertErrorsAre("TemplateType: may not be null");
				Assert.IsTrue(template.IsTransient());
				Assert.IsFalse(template.IsValid());
				throw;
			}	
		}
		
		/// <summary>
		/// Tests the TemplateType with A value of New does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(NHibernate.TransientObjectException))]
		public void TestTemplateTypeWithAValueOfNewDoesNotSave()
		{
			Template template;
			try
			{
				#region Arrange
				template = GetValid(9);
				template.TemplateType = new TemplateType();
				#endregion Arrange

				#region Act
				TemplateRepository.DbContext.BeginTransaction();
				TemplateRepository.EnsurePersistent(template);
				TemplateRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception ex)
			{
				Assert.IsNotNull(ex);
				Assert.AreEqual("object references an unsaved transient instance - save the transient instance before flushing. Type: Commencement.Core.Domain.TemplateType, Entity: Commencement.Core.Domain.TemplateType", ex.Message);
				throw;
			}	
		}

		#endregion Invalid Test

		#region Valid Tests

		/// <summary>
		/// Tests the template type with existing value saves.
		/// </summary>
		[TestMethod]
		public void TestTemplateTypeWithExistingValueSaves()
		{
			#region Arrange
			var template = GetValid(9);
			template.TemplateType = Repository.OfType<TemplateType>().GetById(2);
			#endregion Arrange

			#region Act
			TemplateRepository.DbContext.BeginTransaction();
			TemplateRepository.EnsurePersistent(template);
			TemplateRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(template.IsTransient());
			Assert.IsTrue(template.IsValid());
			Assert.AreSame(template.TemplateType, Repository.OfType<TemplateType>().GetById(2));
			#endregion Assert		
		}


		/// <summary>
		/// Tests the template type with different value saves.
		/// </summary>
		[TestMethod]
		public void TestTemplateTypeWithDifferentValueSaves()
		{
			#region Arrange
			var template = TemplateRepository.GetById(1);
			Assert.AreNotSame(template.TemplateType,Repository.OfType<TemplateType>().GetById(2));
			#endregion Arrange

			#region Act

			template.TemplateType = Repository.OfType<TemplateType>().GetById(2);
			TemplateRepository.DbContext.BeginTransaction();
			TemplateRepository.EnsurePersistent(template);
			TemplateRepository.DbContext.CommitTransaction();

			#endregion Act

			#region Assert
			Assert.IsFalse(template.IsTransient());
			Assert.IsTrue(template.IsValid());
			Assert.AreSame(template.TemplateType, Repository.OfType<TemplateType>().GetById(2));
			#endregion Assert		
		}

		#endregion Valid Tests

		#endregion TemplateType Tests

		#region Constructor Tests

		/// <summary>
		/// Tests the constructor with no parameters does not set any values.
		/// </summary>
		[TestMethod]
		public void TestConstructorWithNoParametersDoesNotSetAnyValues()
		{
			#region Arrange
			var record = new Template();            
			#endregion Arrange

			#region Act

			#endregion Act

			#region Assert
			Assert.IsNull(record.BodyText);
			Assert.IsNull(record.TemplateType);
			#endregion Assert		
		}

		/// <summary>
		/// Tests the constructor with parameters does sets expected values.
		/// </summary>
		[TestMethod]
		public void TestConstructorWithParametersDoesSetsExpectedValues()
		{
            #region Arrange
            var record = new Template("Test", CreateValidEntities.TemplateType(9),CreateValidEntities.Ceremony(9));
            #endregion Arrange

            #region Act

            #endregion Act

            #region Assert
            Assert.AreEqual("Test", record.BodyText);
            Assert.IsNotNull(record.TemplateType);
            Assert.AreEqual("Name9", record.TemplateType.Name);
            Assert.IsNotNull(record.Ceremony);
            Assert.AreEqual("Location9", record.Ceremony.Location);
            #endregion Assert
		}
		#endregion Constructor 

		#region CascadeTests

		/// <summary>
		/// Tests the type of the delete template does not cascade to template.
		/// </summary>
		[TestMethod]
		public void TestDeleteTemplateDoesNotCascadeToTemplateType()
		{
            #region Arrange
            var record = new Template("Test", Repository.OfType<TemplateType>().GetById(2), Repository.OfType<Ceremony>().GetById(2));
            TemplateRepository.DbContext.BeginTransaction();
            TemplateRepository.EnsurePersistent(record);
            TemplateRepository.DbContext.CommitTransaction();
            var saveTemplateTypeId = record.TemplateType.Id;
            Console.WriteLine("Exiting Arrange...");
            #endregion Arrange

            #region Act
            var templateType = Repository.OfType<TemplateType>().GetById(saveTemplateTypeId);
            TemplateRepository.DbContext.BeginTransaction();
            TemplateRepository.Remove(record);
            TemplateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Console.WriteLine("Evicting...");
            NHibernateSessionManager.Instance.GetSession().Evict(templateType);
            templateType = Repository.OfType<TemplateType>().Queryable.Where(a => a.Id == saveTemplateTypeId).Single();
            Assert.IsNotNull(templateType);
            #endregion Assert		
		}

        [TestMethod]
        public void TestDeleteTemplateDoesNotCascadeToCeremony()
        {
            #region Arrange
            var record = new Template("Test", Repository.OfType<TemplateType>().GetById(2), Repository.OfType<Ceremony>().GetById(2));
            TemplateRepository.DbContext.BeginTransaction();
            TemplateRepository.EnsurePersistent(record);
            TemplateRepository.DbContext.CommitTransaction();
            var saveCeremonyId = record.Ceremony.Id;
            Console.WriteLine("Exiting Arrange...");
            #endregion Arrange

            #region Act
            var ceremony = Repository.OfType<Ceremony>().GetById(saveCeremonyId);
            TemplateRepository.DbContext.BeginTransaction();
            TemplateRepository.Remove(record);
            TemplateRepository.DbContext.CommitTransaction();
            #endregion Act

            #region Assert
            Console.WriteLine("Evicting...");
            NHibernateSessionManager.Instance.GetSession().Evict(ceremony);
            ceremony = Repository.OfType<Ceremony>().Queryable.Where(a => a.Id == saveCeremonyId).Single();
            Assert.IsNotNull(ceremony);
            #endregion Assert
        }
		#endregion CascadeTests



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

			expectedFields.Add(new NameAndType("BodyText", "System.String", new List<string>
			{
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
            expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>
			{
				 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
			expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
			{
				"[Newtonsoft.Json.JsonPropertyAttribute()]", 
				"[System.Xml.Serialization.XmlIgnoreAttribute()]"
			}));
            expectedFields.Add(new NameAndType("IsActive", "System.Boolean", new List<string>()));
            expectedFields.Add(new NameAndType("Subject", "System.String", new List<string>
            {
                 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)100)]"
            }));
			expectedFields.Add(new NameAndType("TemplateType", "Commencement.Core.Domain.TemplateType", new List<string>
			{
				 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
			#endregion Arrange

			AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(Template));

		}

		#endregion Reflection of Database.	
		
		
	}
}