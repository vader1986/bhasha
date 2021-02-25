using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Aggregation;
using Bhasha.Common.Extensions;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Aggregation
{
    [TestFixture]
    public class ChapterLoaderTests
    {
        private ILoadTranslations _translations;
        private ILoadProcedures _procedures;

        private ChapterLoader _loader;

        [SetUp]
        public void Before()
        {
            _translations = A.Fake<ILoadTranslations>();
            _procedures = A.Fake<ILoadProcedures>();

            _loader = new ChapterLoader(_translations, _procedures);
        }

        [Test]
        public async Task NextChapter_no_translations()
        {
            var userProgress = UserProgressBuilder
                .Create();

            A.CallTo(() => _translations.NextTranslations(userProgress))
                .Returns(new ValueTask<Translation[]>(new Translation[0]));

            var result = await _loader.NextChapter(userProgress);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task NextChapter_happy_path()
        {
            var userProgress = UserProgressBuilder
                .Create();

            var translations = TranslationBuilder
                .Create()
                .ToEnumeration()
                .ToArray();

            var procedures = new[] { new Procedure(new ProcedureId("p1"), "x", null, null, TokenTypeSupport.Words) };

            A.CallTo(() => _translations.NextTranslations(userProgress))
                .Returns(new ValueTask<Translation[]>(translations));

            A.CallTo(() => _procedures.NextProcedures(translations))
                .Returns(new ValueTask<Procedure[]>(procedures));

            var result = await _loader.NextChapter(userProgress);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Translations == translations);
            Assert.That(result.Procedures == procedures);
        }
    }
}
