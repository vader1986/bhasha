using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Importers;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Importers
{
    [TestFixture]
    public class ChapterImporterTests
    {
        private Mock<IStore<DbTranslatedChapter>> _translatedChapters;
        private Mock<IStore<DbChapter>> _chapters;
        private Mock<IStore<DbExpression>> _expressions;
        private Mock<IStore<DbWord>> _words;
        private Mock<ITranslate<Guid, TranslatedExpression>> _expressionTranslator;
        private Mock<ITranslate<Guid, TranslatedWord>> _wordTranslator;
        private ChapterImporter _importer;

        [SetUp]
        public void Before()
        {
            _translatedChapters = new Mock<IStore<DbTranslatedChapter>>();
            _chapters = new Mock<IStore<DbChapter>>();
            _expressions = new Mock<IStore<DbExpression>>();
            _words = new Mock<IStore<DbWord>>();
            _expressionTranslator = new Mock<ITranslate<Guid, TranslatedExpression>>();
            _wordTranslator = new Mock<ITranslate<Guid, TranslatedWord>>();
            _importer = new ChapterImporter(
                _translatedChapters.Object,
                _chapters.Object,
                _expressions.Object,
                _words.Object,
                _expressionTranslator.Object,
                _wordTranslator.Object);
        }

        [Test]
        public async Task Import_FullyPopulatedDbTranslatedChapter_AddDatabaseEntries()
        {
            // setup
            var chapter = DbTranslatedChapterBuilder.Default.Build();

            _words
                .Setup(x => x.Add(It.IsAny<DbWord>()))
                .Returns<DbWord>(Task.FromResult);

            _expressions
                .Setup(x => x.Add(It.IsAny<DbExpression>()))
                .Returns<DbExpression>(Task.FromResult);

            _translatedChapters
                .Setup(x => x.Add(It.IsAny<DbTranslatedChapter>()))
                .Returns<DbTranslatedChapter>(Task.FromResult);

            _chapters
                .Setup(x => x.Add(It.IsAny<DbChapter>()))
                .Returns<DbChapter>(Task.FromResult);

            // act
            await _importer.Import(chapter);

            // assert
            var expectedExpressions = chapter.Pages.Length * 2 + 2;
            var expectedWords = chapter.Name.Words.Length + chapter.Description.Words.Length + chapter.Pages.Select(x => x.Native.Words.Length + x.Target.Words.Length).Sum();

            _chapters.Verify(x => x.Add(It.Is<DbChapter>(c => c.Id == default)), Times.Exactly(1));
            _expressions.Verify(x => x.Add(It.Is<DbExpression>(c => c.Id == default)), Times.Exactly(expectedExpressions));
            _words.Verify(x => x.Add(It.Is<DbWord>(c => c.Id == default)), Times.Exactly(expectedWords));
        }
    }
}
