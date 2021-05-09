using Bhasha.Common.Extensions;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void Stringify_SinlgeValue_ReturnsValueAsString()
        {
            // setup
            var integerValue = 1;

            // act
            var result = integerValue.Stringify();

            // assert
            Assert.That(result, Is.EqualTo("1"));
        }

        [Test]
        public void Stringify_AnonymousType_ReturnsJsonString()
        {
            // setup
            var integerValue = new {
                Id = 1,
                Value = "hello"
            };

            // act
            var result = integerValue.Stringify();

            // assert
            Assert.That(result, Is.EqualTo("{\"Id\":1,\"Value\":\"hello\"}"));
        }
    }
}
