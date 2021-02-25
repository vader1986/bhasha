using System;
using Bhasha.Common.Tests.Support;
using NUnit.Framework;

namespace Bhasha.Common.Tests
{
    [TestFixture]
    public class TranslationTests
    {
        private static readonly LanguageToken ALanguageToken = new LanguageToken(Languages.English, "cat", "cat");
        private static readonly LanguageToken AnotherLanguageToken = new LanguageToken(Languages.Bengoli, "বিড়াল", "Biṛāla");

        [Test]
        public void Constructor_for_single_language()
        {
            Assert.Throws<ArgumentException>(() => new Translation(TokenBuilder.Create(), ALanguageToken, ALanguageToken));
        }

        [Test]
        public void Constructor_for_different_languages()
        {
            Assert.DoesNotThrow(() => new Translation(TokenBuilder.Create(), ALanguageToken, AnotherLanguageToken));
        }
    }
}