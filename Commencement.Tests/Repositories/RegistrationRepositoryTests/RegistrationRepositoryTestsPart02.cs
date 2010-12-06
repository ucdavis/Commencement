using System;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Testing.Extensions;

namespace Commencement.Tests.Repositories.RegistrationRepositoryTests
{
    public partial class RegistrationRepositoryTests
    {		
        #region Address1 Tests
		#region Invalid Tests

		/// <summary>
		/// Tests the Address1 with null value does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestAddress1WithNullValueDoesNotSave()
		{
			Registration registration = null;
			try
			{
				#region Arrange
				registration = GetValid(9);
				registration.Address1 = null;
				#endregion Arrange

				#region Act
				RegistrationRepository.DbContext.BeginTransaction();
				RegistrationRepository.EnsurePersistent(registration);
				RegistrationRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(registration);
				var results = registration.ValidationResults().AsMessageList();
				results.AssertErrorsAre("Address1: may not be null or empty");
				Assert.IsTrue(registration.IsTransient());
				Assert.IsFalse(registration.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the Address1 with empty string does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestAddress1WithEmptyStringDoesNotSave()
		{
			Registration registration = null;
			try
			{
				#region Arrange
				registration = GetValid(9);
				registration.Address1 = string.Empty;
				#endregion Arrange

				#region Act
				RegistrationRepository.DbContext.BeginTransaction();
				RegistrationRepository.EnsurePersistent(registration);
				RegistrationRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(registration);
				var results = registration.ValidationResults().AsMessageList();
				results.AssertErrorsAre("Address1: may not be null or empty");
				Assert.IsTrue(registration.IsTransient());
				Assert.IsFalse(registration.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the Address1 with spaces only does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestAddress1WithSpacesOnlyDoesNotSave()
		{
			Registration registration = null;
			try
			{
				#region Arrange
				registration = GetValid(9);
				registration.Address1 = " ";
				#endregion Arrange

				#region Act
				RegistrationRepository.DbContext.BeginTransaction();
				RegistrationRepository.EnsurePersistent(registration);
				RegistrationRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(registration);
				var results = registration.ValidationResults().AsMessageList();
				results.AssertErrorsAre("Address1: may not be null or empty");
				Assert.IsTrue(registration.IsTransient());
				Assert.IsFalse(registration.IsValid());
				throw;
			}
		}

		/// <summary>
		/// Tests the Address1 with too long value does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestAddress1WithTooLongValueDoesNotSave()
		{
			Registration registration = null;
			try
			{
				#region Arrange
				registration = GetValid(9);
				registration.Address1 = "x".RepeatTimes((200 + 1));
				#endregion Arrange

				#region Act
				RegistrationRepository.DbContext.BeginTransaction();
				RegistrationRepository.EnsurePersistent(registration);
				RegistrationRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(registration);
				Assert.AreEqual(200 + 1, registration.Address1.Length);
				var results = registration.ValidationResults().AsMessageList();
				results.AssertErrorsAre("Address1: length must be between 0 and 200");
				Assert.IsTrue(registration.IsTransient());
				Assert.IsFalse(registration.IsValid());
				throw;
			}
		}
		#endregion Invalid Tests

		#region Valid Tests

		/// <summary>
		/// Tests the Address1 with one character saves.
		/// </summary>
		[TestMethod]
		public void TestAddress1WithOneCharacterSaves()
		{
			#region Arrange
			var registration = GetValid(9);
			registration.Address1 = "x";
			#endregion Arrange

			#region Act
			RegistrationRepository.DbContext.BeginTransaction();
			RegistrationRepository.EnsurePersistent(registration);
			RegistrationRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(registration.IsTransient());
			Assert.IsTrue(registration.IsValid());
			#endregion Assert
		}

		/// <summary>
		/// Tests the Address1 with long value saves.
		/// </summary>
		[TestMethod]
		public void TestAddress1WithLongValueSaves()
		{
			#region Arrange
			var registration = GetValid(9);
			registration.Address1 = "x".RepeatTimes(200);
			#endregion Arrange

			#region Act
			RegistrationRepository.DbContext.BeginTransaction();
			RegistrationRepository.EnsurePersistent(registration);
			RegistrationRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.AreEqual(200, registration.Address1.Length);
			Assert.IsFalse(registration.IsTransient());
			Assert.IsTrue(registration.IsValid());
			#endregion Assert
		}

		#endregion Valid Tests
		#endregion Address1 Tests
		
		#region Address2 Tests
		#region Invalid Tests

		/// <summary>
		/// Tests the Address2 with too long value does not save.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ApplicationException))]
		public void TestAddress2WithTooLongValueDoesNotSave()
		{
			Registration registration = null;
			try
			{
				#region Arrange
				registration = GetValid(9);
				registration.Address2 = "x".RepeatTimes((200 + 1));
				#endregion Arrange

				#region Act
				RegistrationRepository.DbContext.BeginTransaction();
				RegistrationRepository.EnsurePersistent(registration);
				RegistrationRepository.DbContext.CommitTransaction();
				#endregion Act
			}
			catch (Exception)
			{
				Assert.IsNotNull(registration);
				Assert.AreEqual(200 + 1, registration.Address2.Length);
				var results = registration.ValidationResults().AsMessageList();
				results.AssertErrorsAre("Address2: length must be between 0 and 200");
				Assert.IsTrue(registration.IsTransient());
				Assert.IsFalse(registration.IsValid());
				throw;
			}
		}
		#endregion Invalid Tests

		#region Valid Tests

		/// <summary>
		/// Tests the Address2 with null value saves.
		/// </summary>
		[TestMethod]
		public void TestAddress2WithNullValueSaves()
		{
			#region Arrange
			var registration = GetValid(9);
			registration.Address2 = null;
			#endregion Arrange

			#region Act
			RegistrationRepository.DbContext.BeginTransaction();
			RegistrationRepository.EnsurePersistent(registration);
			RegistrationRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(registration.IsTransient());
			Assert.IsTrue(registration.IsValid());
			#endregion Assert
		}

		/// <summary>
		/// Tests the Address2 with empty string saves.
		/// </summary>
		[TestMethod]
		public void TestAddress2WithEmptyStringSaves()
		{
			#region Arrange
			var registration = GetValid(9);
			registration.Address2 = string.Empty;
			#endregion Arrange

			#region Act
			RegistrationRepository.DbContext.BeginTransaction();
			RegistrationRepository.EnsurePersistent(registration);
			RegistrationRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(registration.IsTransient());
			Assert.IsTrue(registration.IsValid());
			#endregion Assert
		}

		/// <summary>
		/// Tests the Address2 with one space saves.
		/// </summary>
		[TestMethod]
		public void TestAddress2WithOneSpaceSaves()
		{
			#region Arrange
			var registration = GetValid(9);
			registration.Address2 = " ";
			#endregion Arrange

			#region Act
			RegistrationRepository.DbContext.BeginTransaction();
			RegistrationRepository.EnsurePersistent(registration);
			RegistrationRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(registration.IsTransient());
			Assert.IsTrue(registration.IsValid());
			#endregion Assert
		}

		/// <summary>
		/// Tests the Address2 with one character saves.
		/// </summary>
		[TestMethod]
		public void TestAddress2WithOneCharacterSaves()
		{
			#region Arrange
			var registration = GetValid(9);
			registration.Address2 = "x";
			#endregion Arrange

			#region Act
			RegistrationRepository.DbContext.BeginTransaction();
			RegistrationRepository.EnsurePersistent(registration);
			RegistrationRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.IsFalse(registration.IsTransient());
			Assert.IsTrue(registration.IsValid());
			#endregion Assert
		}

		/// <summary>
		/// Tests the Address2 with long value saves.
		/// </summary>
		[TestMethod]
		public void TestAddress2WithLongValueSaves()
		{
			#region Arrange
			var registration = GetValid(9);
			registration.Address2 = "x".RepeatTimes(200);
			#endregion Arrange

			#region Act
			RegistrationRepository.DbContext.BeginTransaction();
			RegistrationRepository.EnsurePersistent(registration);
			RegistrationRepository.DbContext.CommitTransaction();
			#endregion Act

			#region Assert
			Assert.AreEqual(200, registration.Address2.Length);
			Assert.IsFalse(registration.IsTransient());
			Assert.IsTrue(registration.IsValid());
			#endregion Assert
		}

		#endregion Valid Tests
		#endregion Address2 Tests

    }
}
