using System;
using System.Linq;
using Bhasha.Common.Arguments;
using Bhasha.Common.Tests.Support;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Arguments
{
    [TestFixture]
    public class OneOutOfFourArgumentsAssemblyTests
    {
        private OneOutOfFourArgumentsAssembly _assembly;

        [SetUp]
        public void Before()
        {
            _assembly = new OneOutOfFourArgumentsAssembly();
        }

        [Test]
        public void Assemble_Translations_ReturnsExpectedArguments([Range(1, 10)] int length)
        {
            // setup
            var translations = Enumerable
                .Range(1, length)
                .Select(_ => TranslatedExpressionBuilder.Default.Build())
                .ToArray();

            var expressionId = translations[0].Expression.Id;

            // act
            var arguments = _assembly.Assemble(translations, expressionId);

            // assert
            var expectedItem = $"{translations[0].Native} ({translations[0].Spoken})";

            Assert.That(arguments is OneOutOfFourArguments args &&
                args.Options != null &&
                args.Options.Length == Math.Min(length, 4) &&
                args.Options.Any(x => x.DisplayName == expectedItem &&
                                      x.Value == translations[0].Native));
        }
    }
}
