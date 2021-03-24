using System;
using System.Threading.Tasks;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class SubmitEvaluatorTests
    {
        private ICheckResult _checker;
        private IUpdateStatsForEvaluation _updateStats;
        private IDatabase _database;
        private IStore<GenericChapter> _chapters;
        private SubmitEvaluator _submitEvaluator;

        [SetUp]
        public void Before()
        {
            _checker = A.Fake<ICheckResult>();
            _updateStats = A.Fake<IUpdateStatsForEvaluation>();
            _database = A.Fake<IDatabase>();
            _chapters = A.Fake<IStore<GenericChapter>>();
            _submitEvaluator = new SubmitEvaluator(_checker, _updateStats, _database, _chapters);
        }

        [Test]
        public async Task Evaluate_submit_for_profile([Values]Result result)
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 0, "test");

            AssumeResult(profile, submit, result);

            var evaluation = await _submitEvaluator.Evaluate(profile, submit);

            A.CallTo(() => _updateStats.UpdateStats(evaluation, profile, A<GenericChapter>._))
                .MustHaveHappenedOnceExactly();

            Assert.That(evaluation.Result == result);
        }

        [Test]
        public void Evaluate_missing_chapter_throws()
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 0, "test");

            A.CallTo(() => _chapters.Get(submit.ChapterId))
                .Returns(Task.FromResult<GenericChapter>(null));

            Assert.ThrowsAsync<ObjectNotFoundException>(async () => await _submitEvaluator.Evaluate(profile, submit));
        }

        private void AssumeResult(Profile profile, Submit submit, Result result)
        {
            var page = GenericPageBuilder
                .Default
                .Build();

            var genericChapter = GenericChapterBuilder
                .Default
                .WithId(submit.ChapterId)
                .WithPages(new[] { page })
                .Build();

            A.CallTo(() => _chapters.Get(genericChapter.Id))
                .Returns(Task.FromResult(genericChapter));

            var translation = TranslationBuilder
                .Default
                .WithTokenId(page.TokenId)
                .WithLanguage(profile.To)
                .Build();

            A.CallTo(() => _database.QueryTranslationByTokenId(page.TokenId, profile.To))
                .Returns(Task.FromResult(translation));

            A.CallTo(() => _checker.Evaluate(translation.Native, submit.Solution))
                .Returns(result);
        }
    }
}
