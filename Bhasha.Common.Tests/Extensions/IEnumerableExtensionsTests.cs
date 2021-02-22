using System;
using NUnit.Framework;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class IEnumerableExtensionsTests
    {
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
    }
}
