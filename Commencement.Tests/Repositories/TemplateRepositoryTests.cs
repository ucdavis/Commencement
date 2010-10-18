using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core;
using Commencement.Tests.Core.Extensions;
using Commencement.Tests.Core.Helpers;
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
	[TestClass]
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
			LoadTemplateType(3);
			LoadRecords(5);
			TemplateRepository.DbContext.CommitTransaction();
		}

		#endregion Init and Overrides	
		
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
			var record = new Template("Test", CreateValidEntities.TemplateType(9));
			#endregion Arrange

			#region Act

			#endregion Act

			#region Assert
			Assert.AreEqual("Test", record.BodyText);
			Assert.IsNotNull(record.TemplateType);
			Assert.AreEqual("Name9", record.TemplateType.Name);
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
			var record = new Template("Test", Repository.OfType<TemplateType>().GetById(2));
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
		
			expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
			{
				"[Newtonsoft.Json.JsonPropertyAttribute()]", 
				"[System.Xml.Serialization.XmlIgnoreAttribute()]"
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