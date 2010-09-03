using Commencement.Controllers.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commencement.Tests.Misc
{
    [TestClass]
    public class StringExtensionTests
    {

        /// <summary>
        /// Tests the upper first letter.
        /// </summary>
        [TestMethod]
        public void TestUpperFirstLetter1()
        {
            #region Arrange
            const string source = "this is The blest.of the blurst";            
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UpperWordsFirstLetterLowerOthers);
            #endregion Act

            #region Assert
            Assert.AreEqual("This Is The Blest.Of The Blurst", result);
            #endregion Assert		
        }

        /// <summary>
        /// Tests the upper first letter.
        /// </summary>
        [TestMethod]
        public void TestUpperFirstLetter2()
        {
            #region Arrange
            string source = string.Empty;
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UpperWordsFirstLetterLowerOthers);
            #endregion Act

            #region Assert
            Assert.AreEqual(string.Empty, result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the upper first letter.
        /// </summary>
        [TestMethod]
        public void TestUpperFirstLetter3()
        {
            #region Arrange
            string source = null;
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UpperWordsFirstLetterLowerOthers);
            #endregion Act

            #region Assert
            Assert.IsNull(result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the upper first letter.
        /// </summary>
        [TestMethod]
        public void TestUpperFirstLetter4()
        {
            #region Arrange
            string source = "TEST";
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UpperWordsFirstLetter);
            #endregion Act

            #region Assert
            Assert.AreEqual("TEST",result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the upper first letter.
        /// </summary>
        [TestMethod]
        public void TestUpperFirstLetter5()
        {
            #region Arrange
            string source = "TEST";
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UpperWordsFirstLetterLowerOthers);
            #endregion Act

            #region Assert
            Assert.AreEqual("Test", result);
            #endregion Assert
        }

        /// <summary>
        /// Tests the upper first letter.
        /// </summary>
        [TestMethod]
        public void TestUpperFirstLetter6()
        {
            #region Arrange
            string source = "McGanical street.";
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UpperWordsFirstLetterLowerOthers);
            #endregion Act

            #region Assert
            Assert.AreEqual("Mcganical Street.", result);
            #endregion Assert
        }

        [TestMethod]
        public void TestTitleCase()
        {
            #region Arrange
            string source = "McGanical street.";
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UseToTitle);
            #endregion Act

            #region Assert
            Assert.AreEqual("Mcganical Street.", result);
            #endregion Assert		
        }

        [TestMethod]
        public void TestTitleCase2()
        {
            #region Arrange
            string source = "MacGinter john JAMES";
            #endregion Arrange

            #region Act
            var result = source.UpperFirstLetter(StringExtension.UpperFirstLetterOptions.UseToTitle);
            #endregion Act

            #region Assert
            Assert.AreEqual("Macginter John JAMES", result);
            #endregion Assert
        }
    }
}
