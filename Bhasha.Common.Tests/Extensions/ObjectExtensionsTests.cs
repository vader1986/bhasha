using System.Linq;
using Bhasha.Common.Extensions;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void ToEnumeration_for_an_object()
        {
            var obj = new object();
            var result = obj.ToEnumeration().ToList();

            Assert.That(result.Count == 1);
            Assert.Contains(obj, result);
        }

        [Test]
        public void Stringify_value_type()
        {
            var integerValue = 1;

            Assert.That(integerValue.Stringify(), Is.EqualTo("1"));
        }

        [Test]
        public void Stringify_anonymous_type()
        {
            var integerValue = new {
                Id = 1,
                Value = "hello"
            };

            var result = integerValue.Stringify();
            var expectedResult = "{\"Id\":1,\"Value\":\"hello\"}";

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
