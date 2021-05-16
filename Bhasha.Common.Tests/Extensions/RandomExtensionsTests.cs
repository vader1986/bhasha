using System;
using System.Linq;
using Bhasha.Common.Extensions;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class RandomExtensionsTests
    {
        private Random _random;

        [SetUp]
        public void Before()
        {
            _random = new Random(0);
        }

        [Test]
        public void NextString_ReturnsNonEmptyStringWithoutWhitespaces()
        {
            // act
            var result = _random.NextString();

            // assert
            Assert.That(string.IsNullOrWhiteSpace(result), Is.False);
            Assert.That(result.Contains(' '), Is.False);
        }

        [Test]
        public void NextStrings_ReturnsStringsWithoutWhitespaces()
        {
            // act
            var result = _random.NextStrings();

            // assert
            Assert.That(result.Contains(null), Is.False);
            Assert.That(result.All(x => !x.Contains(' ')));
        }

        [Test]
        public void NextPhrase_With5WordsOf7Letters_ReturnsExpectedStringsDevidedByWhitespaces()
        {
            // act
            var result = _random.NextPhrase(5, 5, 7);
            var words = result.Split(' ');

            // assert
            Assert.NotNull(result);
            Assert.That(words.Length == 5);
            Assert.That(words.All(x => x.Length == 7));
        }

        [Test]
        public void Choose_RandomElementFromEmptyArray_ThrowsException()
        {
            // setup
            var source = new int[0];

            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _random.Choose(source));
        }

        [Test]
        public void Choose_RandomElementFromArray_ReturnsElementFromArray()
        {
            // setup
            var source = new[] { 1, 2, 3, 4, 5 };

            // act
            var result = _random.Choose(source);

            // assert
            Assert.That(source.Contains(result));
        }
    }
}
