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
	/// Entity Name:		CeremonyEditor
	/// LookupFieldName:	LoginId
	/// </summary>
	[TestClass]
	public class CeremonyEditorRepositoryTests : AbstractRepositoryTests<CeremonyEditor, int>
	{
		/// <summary>
		/// Gets or sets the CeremonyEditor repository.
		/// </summary>
		/// <value>The CeremonyEditor repository.</value>
		public IRepository<CeremonyEditor> CeremonyEditorRepository { get; set; }
		
		#region Init and Overrides

		/// <summary>
		/// Initializes a new instance of the <see cref="CeremonyEditorRepositoryTests"/> class.
		/// </summary>
		public CeremonyEditorRepositoryTests()
		{
			CeremonyEditorRepository = new Repository<CeremonyEditor>();
		}

		/// <summary>
		/// Gets the valid entity of type T
		/// </summary>
		/// <param name="counter">The counter.</param>
		/// <returns>A valid entity of type T</returns>
		protected override CeremonyEditor GetValid(int? counter)
		{
			var rtValue = CreateValidEntities.CeremonyEditor(counter);
			rtValue.Ceremony = Repository.OfType<Ceremony>().GetById(1);
			return rtValue;
		}

		/// <summary>
		/// A Query which will return a single record
		/// </summary>
		/// <param name="numberAtEnd"></param>
		/// <returns></returns>
		protected override IQueryable<CeremonyEditor> GetQuery(int numberAtEnd)
		{
			return CeremonyEditorRepository.Queryable.Where(a => a.LoginId.EndsWith(numberAtEnd.ToString()));
		}

		/// <summary>
		/// A way to compare the entities that were read.
		/// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="counter"></param>
		protected override void FoundEntityComparison(CeremonyEditor entity, int counter)
		{
			Assert.AreEqual("LoginId" + counter, entity.LoginId);
		}

		/// <summary>
		/// Updates , compares, restores.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="action">The action.</param>
		protected override void UpdateUtility(CeremonyEditor entity, ARTAction action)
		{
			const string updateValue = "Updated";
			switch (action)
			{
				case ARTAction.Compare:
					Assert.AreEqual(updateValue, entity.LoginId);
					break;
				case ARTAction.Restore:
					entity.LoginId = RestoreValue;
					break;
				case ARTAction.Update:
					RestoreValue = entity.LoginId;
					entity.LoginId = updateValue;
					break;
			}
		}

		/// <summary>
		/// Loads the data.
		/// </summary>
		protected override void LoadData()
		{
			CeremonyEditorRepository.DbContext.BeginTransaction();
			LoadTermCode(1);
			LoadCeremony(1);
			LoadRecords(5);
			CeremonyEditorRepository.DbContext.CommitTransaction();
		}

		#endregion Init and Overrides	
		
		#region LoginId Tests
		#region Invalid Tests

		/// <summary>
		/// Tests the LoginId with null value does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestLoginIdWithNullValueDoesNotSave()
		{
			CeremonyEditor ceremonyEditor = null;
			try
			{
				#region Arrange
				ceremonyEditor = GetValid(9);
				ceremonyEditor.LoginId = null;
				#endregion Arrange

				#region Act
				CeremonyEditorRepository.DbContext.BeginTransaction();
				CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
				CeremonyEditorRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(ceremonyEditor);
				var results = ceremonyEditor.ValidationResults().AsMessageList();
				results.AssertErrorsAre("LoginId: may not be null or empty");
				Assert.IsTrue(ceremonyEditor.IsTransient());
				Assert.IsFalse(ceremonyEditor.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the LoginId with empty string does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestLoginIdWithEmptyStringDoesNotSave()
		{
			CeremonyEditor ceremonyEditor = null;
			try
			{
				#region Arrange
				ceremonyEditor = GetValid(9);
				ceremonyEditor.LoginId = string.Empty;
				#endregion Arrange

				#region Act
				CeremonyEditorRepository.DbContext.BeginTransaction();
				CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
				CeremonyEditorRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(ceremonyEditor);
				var results = ceremonyEditor.ValidationResults().AsMessageList();
				results.AssertErrorsAre("LoginId: may not be null or empty");
				Assert.IsTrue(ceremonyEditor.IsTransient());
				Assert.IsFalse(ceremonyEditor.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the LoginId with spaces only does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestLoginIdWithSpacesOnlyDoesNotSave()
		{
			CeremonyEditor ceremonyEditor = null;
			try
			{
				#region Arrange
				ceremonyEditor = GetValid(9);
				ceremonyEditor.LoginId = " ";
				#endregion Arrange

				#region Act
				CeremonyEditorRepository.DbContext.BeginTransaction();
				CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
				CeremonyEditorRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(ceremonyEditor);
				var results = ceremonyEditor.ValidationResults().AsMessageList();
				results.AssertErrorsAre("LoginId: may not be null or empty");
				Assert.IsTrue(ceremonyEditor.IsTransient());
				Assert.IsFalse(ceremonyEditor.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the LoginId with too long value does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestLoginIdWithTooLongValueDoesNotSave()
		{
			CeremonyEditor ceremonyEditor = null;
			try
			{
				#region Arrange
				ceremonyEditor = GetValid(9);
				ceremonyEditor.LoginId = "x".RepeatTimes((50 + 1));
				#endregion Arrange

				#region Act
				CeremonyEditorRepository.DbContext.BeginTransaction();
				CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
				CeremonyEditorRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(ceremonyEditor);
				Assert.AreEqual(50 + 1, ceremonyEditor.LoginId.Length);
				var results = ceremonyEditor.ValidationResults().AsMessageList();
				results.AssertErrorsAre("LoginId: length must be between 0 and 50");
				Assert.IsTrue(ceremonyEditor.IsTransient());
				Assert.IsFalse(ceremonyEditor.IsValid());
				throw;
			}
		}
		#endregion Invalid Tests

		#region Valid Tests

		/// <summary>
		/// Tests the LoginId with one character saves.
		/// </summary>
		[TestMethod]
		public void TestLoginIdWithOneCharacterSaves()
		{
			#region Arrange
			var ceremonyEditor = GetValid(9);
			ceremonyEditor.LoginId = "x";
			#endregion Arrange

			#region Act
			CeremonyEditorRepository.DbContext.BeginTransaction();
			CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
			CeremonyEditorRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(ceremonyEditor.IsTransient());
			Assert.IsTrue(ceremonyEditor.IsValid());
			#endregion Assert
		}

		/// <summary>
		/// Tests the LoginId with long value saves.
		/// </summary>
		[TestMethod]
		public void TestLoginIdWithLongValueSaves()
		{
			#region Arrange
			var ceremonyEditor = GetValid(9);
			ceremonyEditor.LoginId = "x".RepeatTimes(50);
			#endregion Arrange

			#region Act
			CeremonyEditorRepository.DbContext.BeginTransaction();
			CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
			CeremonyEditorRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.AreEqual(50, ceremonyEditor.LoginId.Length);
			Assert.IsFalse(ceremonyEditor.IsTransient());
			Assert.IsTrue(ceremonyEditor.IsValid());
			#endregion Assert
		}

		#endregion Valid Tests
		#endregion LoginId Tests

		#region Owner Tests

		/// <summary>
		/// Tests the Owner is false saves.
		/// </summary>
		[TestMethod]
		public void TestOwnerIsFalseSaves()
		{
			#region Arrange

			CeremonyEditor ceremonyEditor = GetValid(9);
			ceremonyEditor.Owner = false;

			#endregion Arrange

			#region Act

			CeremonyEditorRepository.DbContext.BeginTransaction();
			CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
			CeremonyEditorRepository.DbContext.CommitTransaction();

			#endregion Act

			#region Assert

			Assert.IsFalse(ceremonyEditor.Owner);
			Assert.IsFalse(ceremonyEditor.IsTransient());
			Assert.IsTrue(ceremonyEditor.IsValid());

			#endregion Assert
		}

		/// <summary>
		/// Tests the Owner is true saves.
		/// </summary>
		[TestMethod]
		public void TestOwnerIsTrueSaves()
		{
			#region Arrange

			var ceremonyEditor = GetValid(9);
			ceremonyEditor.Owner = true;

			#endregion Arrange

			#region Act

			CeremonyEditorRepository.DbContext.BeginTransaction();
			CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
			CeremonyEditorRepository.DbContext.CommitTransaction();

			#endregion Act

			#region Assert

			Assert.IsTrue(ceremonyEditor.Owner);
			Assert.IsFalse(ceremonyEditor.IsTransient());
			Assert.IsTrue(ceremonyEditor.IsValid());

			#endregion Assert
		}

		#endregion Owner Tests

		#region Ceremony Tests

		#region Invalid Tests
		
		/// <summary>
		/// Tests the ceremony editor with null ceremony does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestCeremonyEditorWithNullCeremonyDoesNotSave()
		{
			CeremonyEditor ceremonyEditor = null;
			try
			{
				#region Arrange
				ceremonyEditor = GetValid(9);
				ceremonyEditor.Ceremony = null;
				#endregion Arrange

				#region Act
				CeremonyEditorRepository.DbContext.BeginTransaction();
				CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
				CeremonyEditorRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(ceremonyEditor);
				var results = ceremonyEditor.ValidationResults().AsMessageList();
				results.AssertErrorsAre("Ceremony: may not be null");
				Assert.IsTrue(ceremonyEditor.IsTransient());
				Assert.IsFalse(ceremonyEditor.IsValid());
				throw;
			}	
		}

		/// <summary>
		/// Tests the ceremony editor with new ceremony does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(NHibernate.TransientObjectException))]
		public void TestCeremonyEditorWithNewCeremonyDoesNotSave()
		{
			var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
			CeremonyEditor ceremonyEditor;
			try
			{
				#region Arrange
				ceremonyEditor = GetValid(9);
				ceremonyEditor.Ceremony = CreateValidEntities.Ceremony(9);
				ceremonyEditor.Ceremony.TermCode = termCodeRepository.GetById("1");
				#endregion Arrange

				#region Act
				CeremonyEditorRepository.DbContext.BeginTransaction();
				CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
				CeremonyEditorRepository.DbContext.CommitTransaction();
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
		/// Tests the ceremony editor with valid ceremony saves.
		/// </summary>
		[TestMethod]
		public void TestCeremonyEditorWithValidCeremonySaves()
		{
			#region Arrange
			var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
			Repository.OfType<Ceremony>().DbContext.BeginTransaction();
			var ceremony = CreateValidEntities.Ceremony(9);
			ceremony.TermCode = termCodeRepository.GetById("1");
			Repository.OfType<Ceremony>().EnsurePersistent(ceremony);
			Repository.OfType<Ceremony>().DbContext.CommitTransaction();
			var ceremonyEditor = CreateValidEntities.CeremonyEditor(9);
			ceremonyEditor.Ceremony = ceremony;
			#endregion Arrange

			#region Act
			CeremonyEditorRepository.DbContext.BeginTransaction();
			CeremonyEditorRepository.EnsurePersistent(ceremonyEditor);
			CeremonyEditorRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(ceremonyEditor.IsTransient());
			Assert.IsTrue(ceremonyEditor.IsValid());
			#endregion Assert		
		}
		#endregion Valid Tests
		#endregion Ceremony Tests


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
			expectedFields.Add(new NameAndType("Ceremony", "Commencement.Core.Domain.Ceremony", new List<string>
			{ 
				 "[NHibernate.Validator.Constraints.NotNullAttribute()]"
			}));
			expectedFields.Add(new NameAndType("Id", "System.Int32", new List<string>
			{
				"[Newtonsoft.Json.JsonPropertyAttribute()]", 
				"[System.Xml.Serialization.XmlIgnoreAttribute()]"
			}));
			expectedFields.Add(new NameAndType("LoginId", "System.String", new List<string>
			{
				 "[NHibernate.Validator.Constraints.LengthAttribute((Int32)50)]", 
				 "[UCDArch.Core.NHibernateValidator.Extensions.RequiredAttribute()]"
			}));
			expectedFields.Add(new NameAndType("Owner", "System.Boolean", new List<string>()));
			#endregion Arrange

			AttributeAndFieldValidation.ValidateFieldsAndAttributes(expectedFields, typeof(CeremonyEditor));

		}

		#endregion Reflection of Database.	
		
		
	}
}