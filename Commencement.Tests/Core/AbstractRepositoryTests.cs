﻿using System;
using System.Collections.Generic;
using System.Linq;
using Commencement.Core.Domain;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.DomainModel;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;



namespace Commencement.Tests.Core
{

    // ReSharper disable InconsistentNaming
    public abstract class AbstractRepositoryTests<T, IdT> : RepositoryTestBase where T : DomainObjectWithTypedId<IdT>
    // ReSharper restore InconsistentNaming
    {
        protected int EntriesAdded;
        protected string RestoreValue;
        protected bool BoolRestoreValue;
        protected int IntRestoreValue;
        protected bool ForceSave;
        private readonly IRepository<T> _intRepository;
        private readonly IRepositoryWithTypedId<T, string> _stringRepository;
        private readonly IRepositoryWithTypedId<T, Guid> _guidRepository;

        #region Init

        protected AbstractRepositoryTests()
        {
            //HibernatingRhinos.NHibernate.Profiler.Appender.NHibernateProfiler.Initialize();
            if (typeof(IdT) == typeof(int))
            {
                _intRepository = new Repository<T>();
            }
            if(typeof(IdT) == typeof(Guid))
            {
                _guidRepository = new RepositoryWithTypedId<T, Guid>();
            }
            if (typeof(IdT) == typeof(string))
            {
                _stringRepository = new RepositoryWithTypedId<T, string>();
            }
        }

        /// <summary>
        /// Gets the valid entity of type T
        /// </summary>
        /// <param name="counter">The counter.</param>
        /// <returns>A valid entity of type T</returns>
        protected abstract T GetValid(int? counter);

        /// <summary>
        /// A way to compare the entities that were read.
        /// For example, this would have the assert.AreEqual("Comment" + counter, entity.Comment);
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="counter"></param>
        protected abstract void FoundEntityComparison(T entity, int counter);

        /// <summary>
        /// Updates , compares, restores.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="action">The action.</param>
        protected abstract void UpdateUtility(T entity, ARTAction action);

        /// <summary>
        /// A Query which will return a single record
        /// </summary>
        /// <param name="numberAtEnd"></param>
        /// <returns></returns>
        protected abstract IQueryable<T> GetQuery(int numberAtEnd);

        /// <summary>
        /// Loads the records for CRUD Tests.
        /// </summary>
        /// <returns></returns>
        protected virtual void LoadRecords(int entriesToAdd)
        {
            EntriesAdded += entriesToAdd;
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = GetValid(i + 1);
                if (typeof(IdT) == typeof(int))
                {
                    _intRepository.EnsurePersistent(validEntity);
                }
                else if(typeof(IdT) == typeof(Guid))
                {
                    _guidRepository.EnsurePersistent(validEntity, true);
                }
                else
                {
                    if (ForceSave)
                    {
                        _stringRepository.EnsurePersistent(validEntity, true);
                    }
                    else
                    {
                        _stringRepository.EnsurePersistent(validEntity);
                    }
                }
            }
        }

        #endregion Init

        #region CRUD Tests

        /// <summary>
        /// Determines whether this instance [can save valid entity].
        /// </summary>
        [TestMethod]
        public void CanSaveValidEntity()
        {
            var validEntity = GetValid(null);
            if (typeof(IdT) == typeof(int))
            {
                _intRepository.EnsurePersistent(validEntity);
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                _guidRepository.EnsurePersistent(validEntity, true);
            }
            else
            {
                if (ForceSave)
                {
                    _stringRepository.EnsurePersistent(validEntity, true);
                }
                else
                {
                    _stringRepository.EnsurePersistent(validEntity);
                }
            }

            Assert.AreEqual(false, validEntity.IsTransient());
        }


        /// <summary>
        /// Determines whether this instance [can commit valid entity].
        /// </summary>
        [TestMethod]
        public void CanCommitValidEntity()
        {
            var validEntity = GetValid(null);
            if (typeof(IdT) == typeof(int))
            {
                _intRepository.DbContext.BeginTransaction();
                _intRepository.EnsurePersistent(validEntity);
                Assert.IsFalse(validEntity.IsTransient());
                _intRepository.DbContext.CommitTransaction();
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                _guidRepository.DbContext.BeginTransaction();
                _guidRepository.EnsurePersistent(validEntity, true);
                Assert.IsFalse(validEntity.IsTransient());
                _guidRepository.DbContext.CommitTransaction();
            }
            else
            {
                _stringRepository.DbContext.BeginTransaction();
                if (ForceSave)
                {
                    _stringRepository.EnsurePersistent(validEntity, true);
                }
                else
                {
                    _stringRepository.EnsurePersistent(validEntity);
                }
                Assert.IsFalse(validEntity.IsTransient());
                _stringRepository.DbContext.CommitTransaction();
            }

        }


        /// <summary>
        /// Determines whether this instance [can get all entities].
        /// </summary>
        [TestMethod]
        public virtual void CanGetAllEntities()
        {
            List<T> foundEntities;
            if (typeof(IdT) == typeof(int))
            {
                foundEntities = _intRepository.GetAll().ToList();
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                foundEntities = _guidRepository.GetAll().ToList();
            }
            else
            {
                foundEntities = _stringRepository.GetAll().ToList();
            }
            Assert.AreEqual(EntriesAdded, foundEntities.Count, "GetAll() returned a different number of records");
            for (int i = 0; i < EntriesAdded; i++)
            {
                FoundEntityComparison(foundEntities[i], i + 1);
            }
        }

        /// <summary>
        /// Determines whether this instance [can query entities].
        /// </summary>
        [TestMethod]
        public virtual void CanQueryEntities()
        {
            List<T> foundEntry = GetQuery(3).ToList();
            Assert.AreEqual(1, foundEntry.Count);
            FoundEntityComparison(foundEntry[0], 3);
        }


        /// <summary>
        /// Determines whether this instance [can get entity using get by id where id is int].
        /// </summary>
        [TestMethod]
        public virtual void CanGetEntityUsingGetById()
        {
            if (typeof(IdT) == typeof(int))
            {
                Assert.IsTrue(EntriesAdded >= 2, "There are not enough entries to complete this test.");
                var foundEntity = Repository.OfType<T>().GetById(2);
                FoundEntityComparison(foundEntity, 2);
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                Assert.IsTrue(EntriesAdded >= 2, "There are not enough entries to complete this test.");
                var guidFor2 = _guidRepository.GetAll()[1].Id.ToString();
                var foundEntity = _guidRepository.GetById(new Guid(guidFor2));
                FoundEntityComparison(foundEntity, 2);
            }
            else
            {
                Assert.IsTrue(EntriesAdded >= 2, "There are not enough entries to complete this test.");
                var foundEntity = _stringRepository.GetById("2");
                FoundEntityComparison(foundEntity, 2);
            }
        }


        /// <summary>
        /// Determines whether this instance [can get entity using get by nullable with valid id where id is int].
        /// </summary>
        [TestMethod]
        public virtual void CanGetEntityUsingGetByNullableWithValidId()
        {
            if (typeof(IdT) == typeof(int))
            {
                Assert.IsTrue(EntriesAdded >= 2, "There are not enough entries to complete this test.");
                var foundEntity = _intRepository.GetNullableById(2);
                FoundEntityComparison(foundEntity, 2);
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                Assert.IsTrue(EntriesAdded >= 2, "There are not enough entries to complete this test.");
                var guidFor2 = _guidRepository.GetAll()[1].Id.ToString();
                var foundEntity = _guidRepository.GetNullableById(new Guid(guidFor2));
                FoundEntityComparison(foundEntity, 2);
            }
            else
            {
                Assert.IsTrue(EntriesAdded >= 2, "There are not enough entries to complete this test.");
                var foundEntity = _stringRepository.GetNullableById("2");
                FoundEntityComparison(foundEntity, 2);
            }
        }

        /// <summary>
        /// Determines whether this instance [can get null value using get by nullable with invalid id where id is int].
        /// </summary>
        [TestMethod]
        public virtual void CanGetNullValueUsingGetByNullableWithInvalidId()
        {
            if (typeof(IdT) == typeof(int))
            {
                var foundEntity = _intRepository.GetNullableById(EntriesAdded + 1);
                Assert.IsNull(foundEntity);
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                var foundEntity = _guidRepository.GetNullableById(Guid.NewGuid());
                Assert.IsNull(foundEntity);
            }
            else
            {
                var foundEntity = _stringRepository.GetNullableById((EntriesAdded + 1).ToString());
                Assert.IsNull(foundEntity);
            }
        }

        public void CanUpdateEntity(bool doesItAllowUpdate)
        {
            //Get an entity to update
            T foundEntity;
            if (typeof(IdT) == typeof(int))
            {
                foundEntity = _intRepository.GetAll()[2];
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                foundEntity = _guidRepository.GetAll()[2];
            }
            else
            {
                foundEntity = _stringRepository.GetAll()[2];
            }


            //Update and commit entity
            if (typeof(IdT) == typeof(int))
            {
                _intRepository.DbContext.BeginTransaction();
                UpdateUtility(foundEntity, ARTAction.Update);
                _intRepository.EnsurePersistent(foundEntity);
                _intRepository.DbContext.CommitTransaction();
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                _guidRepository.DbContext.BeginTransaction();
                UpdateUtility(foundEntity, ARTAction.Update);
                _guidRepository.EnsurePersistent(foundEntity, true);
                _guidRepository.DbContext.CommitTransaction();
            }
            else
            {
                _stringRepository.DbContext.BeginTransaction();
                UpdateUtility(foundEntity, ARTAction.Update);
                _stringRepository.EnsurePersistent(foundEntity);
                _stringRepository.DbContext.CommitTransaction();
            }

            NHibernateSessionManager.Instance.GetSession().Evict(foundEntity);

            if (doesItAllowUpdate)
            {
                //Compare entity
                T compareEntity;
                if (typeof(IdT) == typeof(int))
                {
                    compareEntity = _intRepository.GetAll()[2];
                }
                else if (typeof(IdT) == typeof(Guid))
                {
                    compareEntity = _guidRepository.GetAll()[2];
                }
                else
                {
                    compareEntity = _stringRepository.GetAll()[2];
                }
                UpdateUtility(compareEntity, ARTAction.Compare);

                //Restore entity, do not commit, then get entity to make sure it isn't restored.            
                UpdateUtility(compareEntity, ARTAction.Restore);
                NHibernateSessionManager.Instance.GetSession().Evict(compareEntity);
                //For testing at least, this is required to clear the changes from memory.
                T checkNotUpdatedEntity;
                if (typeof(IdT) == typeof(int))
                {
                    checkNotUpdatedEntity = _intRepository.GetAll()[2];
                }
                else if (typeof(IdT) == typeof(Guid))
                {
                    checkNotUpdatedEntity = _guidRepository.GetAll()[2];
                }
                else
                {
                    checkNotUpdatedEntity = _stringRepository.GetAll()[2];
                }
                UpdateUtility(checkNotUpdatedEntity, ARTAction.Compare);
            }
            else
            {
                //Compare entity
                T compareEntity;
                if (typeof(IdT) == typeof(int))
                {
                    compareEntity = _intRepository.GetAll()[2];
                }
                else if (typeof(IdT) == typeof(Guid))
                {
                    compareEntity = _guidRepository.GetAll()[2];
                }
                else
                {
                    compareEntity = _stringRepository.GetAll()[2];
                }
                UpdateUtility(compareEntity, ARTAction.CompareNotUpdated);
            }
        }

        /// <summary>
        /// Determines whether this instance [can update entity].
        /// Defaults to true unless overridden
        /// </summary>
        [TestMethod]
        public virtual void CanUpdateEntity()
        {
            CanUpdateEntity(true);
        }


        /// <summary>
        /// Determines whether this instance [can delete entity].
        /// </summary>
        [TestMethod]
        public virtual void CanDeleteEntity()
        {

            if (typeof(IdT) == typeof(int))
            {
                var count = _intRepository.GetAll().ToList().Count();
                var foundEntity = _intRepository.GetAll().ToList()[2];

                //Update and commit entity
                _intRepository.DbContext.BeginTransaction();
                _intRepository.Remove(foundEntity);
                _intRepository.DbContext.CommitTransaction();
                Assert.AreEqual(count - 1, _intRepository.GetAll().ToList().Count());
                foundEntity = Repository.OfType<T>().GetNullableById(3);
                Assert.IsNull(foundEntity);
            }
            else if (typeof(IdT) == typeof(Guid))
            {
                var count = _guidRepository.GetAll().ToList().Count();
                var foundEntity = _guidRepository.GetAll().ToList()[2];

                //Update and commit entity
                _guidRepository.DbContext.BeginTransaction();
                _guidRepository.Remove(foundEntity);
                _guidRepository.DbContext.CommitTransaction();
                Assert.AreEqual(count - 1, _guidRepository.GetAll().ToList().Count());
                foundEntity = _guidRepository.GetNullableById(SpecificGuid.GetGuid(3));
                Assert.IsNull(foundEntity);
            }
            else
            {
                var count = _stringRepository.GetAll().ToList().Count();
                var foundEntity = _stringRepository.GetAll().ToList()[2];

                //Update and commit entity
                _stringRepository.DbContext.BeginTransaction();
                _stringRepository.Remove(foundEntity);
                _stringRepository.DbContext.CommitTransaction();
                Assert.AreEqual(count - 1, _stringRepository.GetAll().ToList().Count());
                foundEntity = _stringRepository.GetNullableById("3");
                Assert.IsNull(foundEntity);
            }
        }

        #endregion CRUD Tests

        #region Utilities


        /// <summary>
        /// Loads the TermCode.
        /// </summary>
        /// <param name="entriesToAdd">The entries to add.</param>
        protected void LoadTermCode(int entriesToAdd)
        {
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = CreateValidEntities.TermCode(i + 1);
                validEntity.SetIdTo((i + 1).ToString());
                termCodeRepository.EnsurePersistent(validEntity, true);
            }     
        }

        protected void LoadTemplateType(int entriesToAdd)
        {
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = CreateValidEntities.TemplateType(i + 1);
                //validEntity.SetIdTo((i + 1).ToString());
                Repository.OfType<TemplateType>().EnsurePersistent(validEntity);
            }
        }

        /// <summary>
        /// Loads the state.
        /// </summary>
        /// <param name="entriesToAdd">The entries to add.</param>
        protected void LoadState(int entriesToAdd)
        {
            var stateRepository = new RepositoryWithTypedId<State, string>();
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = CreateValidEntities.State(i + 1);
                validEntity.SetIdTo((i + 1).ToString());
                stateRepository.EnsurePersistent(validEntity, true);
            }
        }

        protected void LoadMajorCode(int entriesToAdd)
        {
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = CreateValidEntities.MajorCode(i + 1);
                validEntity.SetIdTo((i + 1).ToString());
                majorCodeRepository.EnsurePersistent(validEntity, true);
            }
        }

        protected void LoadStudent(int entriesToAdd)
        {
            var studentRepository = new RepositoryWithTypedId<Student, Guid>();
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = CreateValidEntities.Student(i + 1);
                validEntity.SetIdTo(SpecificGuid.GetGuid(i + 1));
                studentRepository.EnsurePersistent(validEntity);
            }
        }

        /// <summary>
        /// Loads the ceremony.
        /// Needs TermCode
        /// </summary>
        /// <param name="entriesToAdd">The entries to add.</param>
        protected void LoadCeremony(int entriesToAdd)
        {
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = CreateValidEntities.Ceremony(i + 1);
                validEntity.TermCode = termCodeRepository.GetById("1");
                Repository.OfType<Ceremony>().EnsurePersistent(validEntity);
            }     
        }

        /// <summary>
        /// Loads the registration petitions.
        /// Needs MajorCode
        /// Needs TermCode
        /// </summary>
        /// <param name="entriesToAdd">The entries to add.</param>
        protected void LoadRegistrationPetitions(int entriesToAdd)
        {
            var majorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            var termCodeRepository = new RepositoryWithTypedId<TermCode, string>();
            for (int i = 0; i < entriesToAdd; i++)
            {
                var validEntity = CreateValidEntities.RegistrationPetition(i + 1);
                validEntity.MajorCode = majorCodeRepository.GetById("1");
                validEntity.TermCode = termCodeRepository.GetById("1");
                Repository.OfType<RegistrationPetition>().EnsurePersistent(validEntity);
            } 
        }


        /// <summary>
        /// Abstract Repository Tests Action
        /// </summary>
        public enum ARTAction
        {
            Compare = 1,
            Update,
            Restore,
            CompareNotUpdated
        }
        #endregion Utilities
    }
}
