using System;
using NUnit.Framework;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        [Test]
        public void Shuffle_Array_KeepsArrayEquivalent()
        {
            // setup
            var source = new[] { 1, 2, 3, 4, 5 };

            // act
            source.Shuffle();

            // assert
            Assert.That(source, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void Random_ElementOfEmptyArray_ThrowsException()
        {
            // act & assert
            Assert.Throws<ArgumentOutOfRangeException>(() => Array.Empty<int>().Random());
        }

        [Test]
        public void Random_ElementOfSingleElementArray_ReturnsElement()
        {
            // setup
            var expected = new object();
            var array = new[] { expected };

            // act
            var result = array.Random();

            // assert
            Assert.That(result == expected);
        }

        [Test]
        public void RandomOrDefault_FromEmptyArray_ReturnsDefault()
        {
            // setup
            var array = Array.Empty<object>();

            // act
            var result = array.RandomOrDefault();

            // assert
            Assert.That(result == default);
        }

        [Test]
        public void RandomOrDefault_OfSingleElement_ReturnsElement()
        {
            // setup
            var expected = new object();
            var array = new[] { expected };

            // act
            var result = array.RandomOrDefault();

            // assert
            Assert.That(result == expected);
        }

        [Test]
        public void IsEmpty_EmptyArray_ReturnsTrue()
        {
            // setup
            var source = Array.Empty<int>();

            // act
            var isEmpty = source.IsEmpty();

            // assert
            Assert.That(isEmpty);
        }

        [Test]
        public void IsEmpty_ForNoneEmptyArray_ReturnsFalse()
        {
            // setup
            var source = new[] { 1 };

            // act
            var isEmpty = source.IsEmpty();

            // assert
            Assert.That(isEmpty, Is.False);
        }

        [Test]
        public void IndexOf_MatchingElement_ReturnsExpectedIndex()
        {
            // setup
            var source = new[] { 1, 2, 3 };

            // act
            var result = source.IndexOf(2);

            // assert
            Assert.That(result == 1);
        }

        [Test]
        public void IndexOf_MultipleMatchingElements_ReturnsFirstIndex()
        {
            // setup
            var source = new[] { 1, 2, 3, 2 };

            // act
            var result = source.IndexOf(2);

            // assert
            Assert.That(result == 1);
        }

        [Test]
        public void IndexOf_NoMatchingElement_ReturnsMinusOne()
        {
            // setup
            var source = new[] { 1, 2, 3, 2 };

            // act
            var result = source.IndexOf(5);

            // assert
            Assert.That(result == -1);
        }
    }
}
