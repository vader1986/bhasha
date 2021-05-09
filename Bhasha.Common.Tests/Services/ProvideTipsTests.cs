using System;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class ProvideTipsTests
    {
        private Mock<IStore<DbChapter>> _chapters;
        private Mock<ITranslate<Guid, TranslatedExpression>> _expressions;
        private Mock<ITranslate<Guid, TranslatedWord>> _words;
        private Mock<IUpdateStatsOnTipRequest> _tipsUpdater;
        private ProvideTips _tipsProvider;

        [SetUp]
        public void Before()
        {
            _chapters = new Mock<IStore<DbChapter>>();
            _expressions = new Mock<ITranslate<Guid, TranslatedExpression>>();
            _words = new Mock<ITranslate<Guid, TranslatedWord>>();
            _tipsUpdater = new Mock<IUpdateStatsOnTipRequest>();
            _tipsProvider = new ProvideTips(
                _chapters.Object,
                _expressions.Object,
                _words.Object,
                _tipsUpdater.Object);
        }

        [Test]
        public void GetTip_ForNonExistingChapter_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapterId = Guid.NewGuid();

            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(default(DbChapter));

            // act & assert
            Assert.ThrowsAsync<ObjectNotFoundException>(
                async () => await _tipsProvider.GetTip(profile, chapterId, 0));
        }

        [Test]
        public void GetTip_ForInvalidChapter_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapterId = Guid.NewGuid();
            
            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(DbChapterBuilder.Default.WithPages(null).Build());

            // act & assert
            Assert.ThrowsAsync<InvalidObjectException>(
                async () => await _tipsProvider.GetTip(profile, chapterId, 0));
        }

        [Test]
        public void GetTip_ForInvalidPageIndex_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapter = DbChapterBuilder.Default.Build();
            var chapterId = Guid.NewGuid();

            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(chapter);

            // act & assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                async () => await _tipsProvider.GetTip(profile, chapterId, chapter.Pages.Length));
        }

        [Test]
        public void GetTip_ForPageWithoutNativeTranslation_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapter = DbChapterBuilder.Default.Build();
            var chapterId = Guid.NewGuid();

            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(chapter);

            _expressions
                .Setup(x => x.Translate(chapter.Pages[0].ExpressionId, profile.Native))
                .ReturnsAsync(default(TranslatedExpression));

            // act & assert
            Assert.ThrowsAsync<ObjectNotFoundException>(
                async () => await _tipsProvider.GetTip(profile, chapterId, 0));
        }

        [Test]
        public void GetTip_ForPageWithoutTargetTranslation_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapter = DbChapterBuilder.Default.Build();
            var chapterId = Guid.NewGuid();
            var translatedExpression = TranslatedExpressionBuilder.Default.Build();

            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(chapter);

            _expressions
                .Setup(x => x.Translate(chapter.Pages[0].ExpressionId, profile.Native))
                .ReturnsAsync(translatedExpression);

            _words
                .Setup(x => x.Translate(It.IsAny<Guid>(), profile.Target))
                .ReturnsAsync(default(TranslatedWord));

            // act & assert
            Assert.ThrowsAsync<ObjectNotFoundException>(
                async () => await _tipsProvider.GetTip(profile, chapterId, 0));
        }

        [Test]
        public async Task GetTips_ForValidParameters_ReturnsExpectedTip()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapter = DbChapterBuilder.Default.Build();
            var chapterId = Guid.NewGuid();
            var pageIndex = chapter.Pages.Length - 1;
            var expressionId = chapter.Pages[pageIndex].ExpressionId;

            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(chapter);

            var native = TranslatedExpressionBuilder.Default.Build();

            _expressions
                .Setup(x => x.Translate(expressionId, profile.Native))
                .ReturnsAsync(native);

            var target = TranslatedWordBuilder.Default.Build();

            _words
                .Setup(x => x.Translate(It.IsAny<Guid>(), profile.Target))
                .ReturnsAsync(target);

            // act
            var result = await _tipsProvider.GetTip(profile, chapterId, pageIndex);

            // assert
            Assert.That(result.EndsWith($" = {target.Native} [{target.Spoken}]"));
        }

        [Test]
        public async Task GetTips_ForValidParameters_UpdatesStats()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapter = DbChapterBuilder.Default.Build();
            var chapterId = Guid.NewGuid();
            var pageIndex = chapter.Pages.Length - 1;
            var expressionId = chapter.Pages[pageIndex].ExpressionId;

            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(chapter);

            var native = TranslatedExpressionBuilder.Default.Build();

            _expressions
                .Setup(x => x.Translate(expressionId, profile.Native))
                .ReturnsAsync(native);

            var target = TranslatedWordBuilder.Default.Build();

            _words
                .Setup(x => x.Translate(It.IsAny<Guid>(), profile.Target))
                .ReturnsAsync(target);

            // act
            var result = await _tipsProvider.GetTip(profile, chapterId, pageIndex);

            // assert
            _tipsUpdater.Verify(x => x.Update(profile, chapterId, pageIndex), Times.Exactly(1));
        }
    }
}
