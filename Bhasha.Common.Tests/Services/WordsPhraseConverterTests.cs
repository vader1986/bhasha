using System.Collections;
using System.Collections.Generic;
using Bhasha.Common.Services;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class WordsPhraseConverterTests
    {
        public static IEnumerable WordsAndPhrases
        {
            get
            {
                yield return new TestCaseData(
                    new[] { "Hello", "my", "dear", "!" }, "Hello my dear!");
                yield return new TestCaseData(
                    new[] { "Hello", "!", "who", "is", "it", "?" }, "Hello! Who is it?");
                yield return new TestCaseData(
                    new[] { "Who", "is", "it", "?", "!", "?" }, "Who is it?!?");
            }
        }

        [TestCaseSource(nameof(WordsAndPhrases))]
        public void Convert_WordsAndSigns_ReturnsExpectedPhrase(IEnumerable<string> words, string expected)
        {
            // setup
            var converter = new WordsPhraseConverter();

            // act
            var result = converter.Convert(words, Language.English);

            // assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
