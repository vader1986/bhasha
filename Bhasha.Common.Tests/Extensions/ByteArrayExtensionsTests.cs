using Bhasha.Common.Extensions;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class ByteArrayExtensionsTests
    {
        [Test]
        public void Increment_ByteAtIndex_UpdatesOnlyIndexedByte()
        {
            // setup
            var bytes = new byte[]
            {
                1, 2, 3, 4
            };

            // act
            bytes.Increment(2);

            // assert
            Assert.That(bytes[0] == 1);
            Assert.That(bytes[1] == 2);
            Assert.That(bytes[2] == 4);
            Assert.That(bytes[3] == 4);
        }

        [Test]
        public void Increment_ByteWithMaximumValue_KeepsMaximumValue()
        {
            // setup
            var bytes = new byte[]
            {
                1, byte.MaxValue, 3, 4
            };

            // act
            bytes.Increment(1);

            // assert
            Assert.AreEqual(bytes, new byte[] {
                1, byte.MaxValue, 3, 4
            });
        }
    }
}
