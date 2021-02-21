using System;
using NUnit.Framework;

namespace Bhasha.Common.Tests
{
    [TestFixture]
    public class TranslationTests
    {
        private static Token AToken = new Token("cat", new[] { new Category("Animal") }, LanguageLevel.A1, TokenType.Noun);
        private static LanguageToken ALanguageToken = new LanguageToken(Languages.English, "cat", "cat");
        private static LanguageToken AnotherLanguageToken = new LanguageToken(Languages.Bengoli, "বিড়াল", "Biṛāla");

        [Test]
        public void Constructor_for_single_language()
        {
            Assert.Throws<ArgumentException>(() => new Translation(AToken, ALanguageToken, ALanguageToken));
        }

        [Test]
        public void Constructor_for_different_languages()
        {
            Assert.DoesNotThrow(() => new Translation(AToken, ALanguageToken, AnotherLanguageToken));
        }
    }
}