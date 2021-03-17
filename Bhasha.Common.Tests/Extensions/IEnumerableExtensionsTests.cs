using System;
using NUnit.Framework;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
        [Test]
        public void Shuffle_keeps_elements()
        {
            var source = new[] { 1, 2, 3, 4, 5 };

            source.Shuffle();

            Assert.That(source, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void Random_element_from_empty_array()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new int[0].Random());
        }

        [Test]
        public void Random_element_of_array()
        {
            var expected = new object();
            var array = new[] { expected };

            Assert.That(array.Random() == expected);
        }

        [Test]
        public void RandomOrDefault_from_empty_array()
        {
            var array = new object[0];

            Assert.That(array.RandomOrDefault() == null);
        }

        [Test]
        public void RandomOrDefault_element_of_array()
        {
            var expected = new object();
            var array = new[] { expected };

            Assert.That(array.RandomOrDefault() == expected);
        }

        [Test]
        public void IsEmpty_no_elements()
        {
            var source = new int[0];

            Assert.That(source.IsEmpty());
        }

        [Test]
        public void IsEmpty_for_elements()
        {
            var source = new[] { 1 };

            Assert.That(source.IsEmpty(), Is.False);
        }

        [Test]
        public void IndexOf_matching_element()
        {
            var source = new[] { 1, 2, 3 };
            var result = source.IndexOf(2);

            Assert.That(result == 1);
        }

        [Test]
        public void IndexOf_multiple_matching_elements()
        {
            var source = new[] { 1, 2, 3, 2 };
            var result = source.IndexOf(2);

            Assert.That(result == 1);
        }

        [Test]
        public void IndexOf_no_matching_element()
        {
            var source = new[] { 1, 2, 3, 2 };
            var result = source.IndexOf(5);

            Assert.That(result == -1);
        }
    }
}
