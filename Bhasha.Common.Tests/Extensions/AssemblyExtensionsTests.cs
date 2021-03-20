using System;
using System.Reflection;
using Bhasha.Common.Extensions;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Extensions
{
    [TestFixture]
    public class AssemblyExtensionsTests
    {
        public class AssemblyTestAttribute : Attribute { }

        [AssemblyTest]
        public class ExpectedType { }

        [Test]
        public void GetTypesWithAttribute()
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypesWithAttribute<AssemblyTestAttribute>();

            Assert.That(types.Count == 1);
            Assert.That(types.ContainsKey(typeof(ExpectedType)));
            Assert.That(types[typeof(ExpectedType)] != null);
        }
    }
}
