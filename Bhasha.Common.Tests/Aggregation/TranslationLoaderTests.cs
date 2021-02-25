using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Aggregation;
using Bhasha.Common.Extensions;
using Bhasha.Common.Queries;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Aggregation
{
    [TestFixture]
    public class TranslationLoaderTests
    {
        private IQueryable<Translation, TranslationQuery> _translations;
        private TranslationLoader _loader;

        [SetUp]
        public void Before()
        {
            _translations = A.Fake<IQueryable<Translation, TranslationQuery>>();
            _loader = new TranslationLoader(_translations);
        }

        [Test]
        public async Task NextTranslations_happy_path()
        {
            var userProgress = UserProgressBuilder
                .Create();

            var translations = TranslationBuilder
                .Default
                .Build()
                .ToEnumeration()
                .ToArray();

            A.CallTo(() => _translations.Query(A<TranslationQuery>._))
                .Returns(new ValueTask<IEnumerable<Translation>>(translations));

            var result = await _loader.NextTranslations(userProgress);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Contains.Item(translations[0]));
        }
    }
}
