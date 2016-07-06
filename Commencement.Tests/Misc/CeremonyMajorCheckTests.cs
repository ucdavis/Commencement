using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commencement.Core.Domain;
using Commencement.Mvc.Controllers.Helpers;
using Commencement.Tests.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCDArch.Core.DomainModel;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using UCDArch.Testing;

namespace Commencement.Tests.Misc
{
    [TestClass]
    public class CeremonyMajorCheckTests : FluentRepositoryTestBase<CeremonyMap>
    {
        public IRepository<Ceremony> CeremonyRepository { get; set; }
        public IRepositoryWithTypedId<MajorCode, string> MajorCodeRepository { get; set; }
        public IRepositoryWithTypedId<TermCode, string> TermCodeRepository { get; set; }

        public CeremonyMajorCheckTests()
        {
            CeremonyRepository = new Repository<Ceremony>();
            MajorCodeRepository = new RepositoryWithTypedId<MajorCode, string>();
            TermCodeRepository = new RepositoryWithTypedId<TermCode, string>();
        }


        [TestMethod]
        public void TestCeremonyMajorCheckReturnsNullWhenNoDuplicateMajorsFound()
        {
            #region Arrange
            LoadMajors();
            LoadTermCodes();
            var majors = new List<MajorCode>();
            majors.Add(MajorCodeRepository.GetById("2"));
            majors.Add(MajorCodeRepository.GetById("3"));
            CeremonyRepository.DbContext.BeginTransaction();
            var ceremony = CreateValidEntities.Ceremony(1);
            ceremony.TermCode = TermCodeRepository.GetById("2");
            ceremony.Majors = majors;
            CeremonyRepository.EnsurePersistent(ceremony);
            CeremonyRepository.DbContext.CommitChanges();
            #endregion Arrange

            #region Act
            //var check = new CeremonyMajorCheck();
            var result = CeremonyMajorCheck.Check(ceremony, majors, CeremonyRepository);
            #endregion Act

            #region Assert
            Assert.IsNull(result);
            #endregion Assert		
        }

        [TestMethod]
        public void TestCeremonyMajorCheckReturnsListWhenDuplicateMajorsFound()
        {
            #region Arrange
            LoadMajors();
            LoadTermCodes();
            var majors = new List<MajorCode>();
            majors.Add(MajorCodeRepository.GetById("2"));
            majors.Add(MajorCodeRepository.GetById("3"));
            majors.Add(MajorCodeRepository.GetById("4"));
            CeremonyRepository.DbContext.BeginTransaction();

            var ceremony1 = CreateValidEntities.Ceremony(1);
            ceremony1.TermCode = TermCodeRepository.GetById("2");
            ceremony1.Majors = majors;
            CeremonyRepository.EnsurePersistent(ceremony1);

            var ceremony2 = CreateValidEntities.Ceremony(2);
            ceremony2.TermCode = TermCodeRepository.GetById("2");
            ceremony2.Majors.Add(MajorCodeRepository.GetById("2"));
            CeremonyRepository.EnsurePersistent(ceremony2);

            var ceremony3 = CreateValidEntities.Ceremony(3);
            ceremony3.TermCode = TermCodeRepository.GetById("2");
            ceremony3.Majors.Add(MajorCodeRepository.GetById("3"));
            CeremonyRepository.EnsurePersistent(ceremony3);

            var ceremony4 = CreateValidEntities.Ceremony(4);
            ceremony4.TermCode = TermCodeRepository.GetById("4");
            ceremony4.Majors.Add(MajorCodeRepository.GetById("1"));
            CeremonyRepository.EnsurePersistent(ceremony4);

            CeremonyRepository.DbContext.CommitChanges();
            #endregion Arrange

            #region Act
            //var check = new CeremonyMajorCheck();
            var result = CeremonyMajorCheck.Check(ceremony1, majors, CeremonyRepository);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(MajorCodeRepository.GetById("2")));
            Assert.IsTrue(result.Contains(MajorCodeRepository.GetById("3")));
            #endregion Assert
        }

        [TestMethod]
        // This one has one in a different term
        public void TestCeremonyMajorCheckReturnsListWhenDuplicateMajorsFound1()
        {
            #region Arrange
            LoadMajors();
            LoadTermCodes();
            var majors = new List<MajorCode>();
            majors.Add(MajorCodeRepository.GetById("2"));
            majors.Add(MajorCodeRepository.GetById("3"));
            majors.Add(MajorCodeRepository.GetById("4"));
            CeremonyRepository.DbContext.BeginTransaction();

            var ceremony1 = CreateValidEntities.Ceremony(1);
            ceremony1.TermCode = TermCodeRepository.GetById("2");
            ceremony1.Majors = majors;
            CeremonyRepository.EnsurePersistent(ceremony1);

            var ceremony2 = CreateValidEntities.Ceremony(2);
            ceremony2.TermCode = TermCodeRepository.GetById("2");
            ceremony2.Majors.Add(MajorCodeRepository.GetById("2"));
            CeremonyRepository.EnsurePersistent(ceremony2);

            var ceremony3 = CreateValidEntities.Ceremony(3);
            ceremony3.TermCode = TermCodeRepository.GetById("1");
            ceremony3.Majors.Add(MajorCodeRepository.GetById("3"));
            CeremonyRepository.EnsurePersistent(ceremony3);

            var ceremony4 = CreateValidEntities.Ceremony(4);
            ceremony4.TermCode = TermCodeRepository.GetById("4");
            ceremony4.Majors.Add(MajorCodeRepository.GetById("1"));
            CeremonyRepository.EnsurePersistent(ceremony4);

            CeremonyRepository.DbContext.CommitChanges();
            #endregion Arrange

            #region Act
            //var check = new CeremonyMajorCheck();
            var result = CeremonyMajorCheck.Check(ceremony1, majors, CeremonyRepository);
            #endregion Act

            #region Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(MajorCodeRepository.GetById("2")));
            //Assert.IsTrue(result.Contains(MajorCodeRepository.GetById("3")));
            #endregion Assert
        }

        private void LoadTermCodes()
        {
            TermCodeRepository.DbContext.BeginTransaction();
            for (int i = 0; i < 3; i++)
            {
                var termCode = CreateValidEntities.TermCode(i + 1);
                termCode.SetIdTo((i + 1).ToString());
                TermCodeRepository.EnsurePersistent(termCode);
            }
            TermCodeRepository.DbContext.CommitChanges();
        }

        private void LoadMajors()
        {
            MajorCodeRepository.DbContext.BeginTransaction();
            for (int i = 0; i < 20; i++)
            {
                var major = CreateValidEntities.MajorCode(i + 1);
                major.SetIdTo((i + 1).ToString());
                MajorCodeRepository.EnsurePersistent(major);
            }
            MajorCodeRepository.DbContext.CommitTransaction();
        }
    }
}
