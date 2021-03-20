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
        public void NextString()
        {
            var result = _random.NextString();

            Assert.NotNull(result);
            Assert.That(result.Contains(' '), Is.False);
        }

        [Test]
        public void NextStrings()
        {
            var result = _random.NextStrings();

            Assert.That(result.Contains(null), Is.False);
            Assert.That(result.All(x => !x.Contains(' ')));
        }

        [Test]
        public void NextPhrase()
        {
            var result = _random.NextPhrase(5, 5, 7);
            var words = result.Split(' ');

            Assert.NotNull(result);
            Assert.That(words.Length == 5);
            Assert.That(words.All(x => x.Length == 7));
        }

        [Test]
        public void Choose_returns_element_from_source()
        {
            var source = new[] { 1, 2, 3, 4, 5 };

            var result = _random.Choose(source);

            Assert.That(source.Contains(result));
        }
    }
}
