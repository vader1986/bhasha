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
    public class EvaluateSubmitTests
    {
        private Mock<ICheckResult> _checker;
        private Mock<IUpdateStatsOnSubmit> _statsUpdater;
        private Mock<ITranslate<Guid, TranslatedExpression>> _translator;
        private Mock<IStore<DbChapter>> _chapters;
        private EvaluateSubmit _evaluator;

        [SetUp]
        public void Before()
        {
            _checker = new Mock<ICheckResult>();
            _statsUpdater = new Mock<IUpdateStatsOnSubmit>();
            _translator = new Mock<ITranslate<Guid, TranslatedExpression>>();
            _chapters = new Mock<IStore<DbChapter>>();
            _evaluator = new EvaluateSubmit(
                _checker.Object,
                _statsUpdater.Object,
                _translator.Object,
                _chapters.Object);
        }

        [Test]
        public void Evaluate_SubmitForNonExistingChapter_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var submit = SubmitBuilder.Default.Build();

            _chapters
                .Setup(x => x.Get(submit.ChapterId))
                .ReturnsAsync(default(DbChapter));

            // act & assert
            Assert.ThrowsAsync<ObjectNotFoundException>(async () => await _evaluator.Evaluate(profile, submit));
        }

        [Test]
        public void Evaluate_SubmitForChapterWithoutPages_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var submit = SubmitBuilder.Default.Build();

            _chapters
                .Setup(x => x.Get(submit.ChapterId))
                .ReturnsAsync(DbChapterBuilder.Default.WithPages(null).Build());

            // act & assert
            Assert.ThrowsAsync<InvalidObjectException>(async () => await _evaluator.Evaluate(profile, submit));
        }

        [Test]
        public void Evaluate_SubmitForInvalidPageIndex_ThrowsException()
        {
            // setup
            var chapter = DbChapterBuilder.Default.Build();
            var profile = ProfileBuilder.Default.Build();
            var submit = SubmitBuilder.Default.WithPageIndex(chapter.Pages.Length).Build();

            _chapters
                .Setup(x => x.Get(submit.ChapterId))
                .ReturnsAsync(chapter);

            // act & assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _evaluator.Evaluate(profile, submit));
        }

        [Test]
        public void Evaluate_SubmitForPageWithoutTranslation_ThrowsException()
        {
            // setup
            var chapter = DbChapterBuilder.Default.Build();
            var profile = ProfileBuilder.Default.Build();
            var submit = SubmitBuilder.Default.WithPageIndex(0).Build();

            _chapters
                .Setup(x => x.Get(submit.ChapterId))
                .ReturnsAsync(chapter);

            _translator
                .Setup(x => x.Translate(chapter.Pages[0].ExpressionId, profile.Target))
                .ReturnsAsync(default(TranslatedExpression));

            // act & assert
            Assert.ThrowsAsync<ObjectNotFoundException>(async () => await _evaluator.Evaluate(profile, submit));
        }

        [Test]
        public async Task Evaluate_Submit_UpdatesStats()
        {
            // setup
            var chapter = DbChapterBuilder.Default.Build();
            var profile = ProfileBuilder.Default.Build();
            var submit = SubmitBuilder.Default.WithPageIndex(0).Build();
            var translation = TranslatedExpressionBuilder.Default.Build();

            _chapters
                .Setup(x => x.Get(submit.ChapterId))
                .ReturnsAsync(chapter);

            _translator
                .Setup(x => x.Translate(chapter.Pages[0].ExpressionId, profile.Target))
                .ReturnsAsync(translation);

            _checker
                .Setup(x => x.Evaluate(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Result.Correct);

            // act
            await _evaluator.Evaluate(profile, submit);

            // assert
            var expectedEvaluation = new Evaluation(Result.Correct, submit, profile);
            _statsUpdater.Verify(x => x.Update(expectedEvaluation, chapter.Pages.Length), Times.Exactly(1));
        }
    }
}
