using Bhasha.Common.Extensions;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class ByteArrayExtensionsTests
    {
        [Test]
        public void Inc_byte_at_index()
        {
            var bytes = new byte[]
            {
                1, 2, 3, 4
            };

            bytes.Inc(2);

            Assert.That(bytes[0] == 1);
            Assert.That(bytes[1] == 2);
            Assert.That(bytes[2] == 4);
            Assert.That(bytes[3] == 4);
        }

        [Test]
        public void Inc_byte_at_maximum()
        {
            var bytes = new byte[]
            {
                1, byte.MaxValue, 3, 4
            };

            bytes.Inc(1);

            Assert.AreEqual(bytes, new byte[] {
                1, byte.MaxValue, 3, 4
            });
        }
    }
}
