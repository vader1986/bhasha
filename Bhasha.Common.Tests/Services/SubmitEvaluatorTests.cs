using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class SubmitEvaluatorTests
    {
        private IEvaluateSolution _evaluator;
        private IDatabase _database;
        private IStore<GenericChapter> _chapters;
        private IStore<ChapterStats> _stats;
        private SubmitEvaluator _submitEvaluator;

        [SetUp]
        public void Before()
        {
            _evaluator = A.Fake<IEvaluateSolution>();
            _database = A.Fake<IDatabase>();
            _chapters = A.Fake<IStore<GenericChapter>>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _submitEvaluator = new SubmitEvaluator(_evaluator, _database, _chapters, _stats);
        }

        [Test]
        public async Task Evaluate_correct_submit_for_profile()
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 0, "test");
            var stats = AssumeResult(profile, submit, Result.Correct);
            
            var beforeSubmits = stats.Submits[submit.PageIndex];
            var beforeFailures = stats.Failures[submit.PageIndex];

            var evaluation = await _submitEvaluator.Evaluate(profile, submit);

            A.CallTo(() => _stats
                .Update(A<ChapterStats>.That
                .Matches(x => x.Submits[submit.PageIndex] == beforeSubmits + 1 &&
                              x.Failures[submit.PageIndex] == beforeFailures &&
                              x.Completed)))
                .MustHaveHappenedOnceExactly();

            Assert.That(evaluation.Result == Result.Correct);
        }

        [Test]
        public async Task Evaluate_wrong_submit_for_profile()
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 0, "test");
            var stats = AssumeResult(profile, submit, Result.Wrong);

            var beforeSubmits = stats.Submits[submit.PageIndex];
            var beforeFailures = stats.Failures[submit.PageIndex];

            var evaluation = await _submitEvaluator.Evaluate(profile, submit);

            A.CallTo(() => _stats
                .Update(A<ChapterStats>.That
                .Matches(x => x.Submits[submit.PageIndex] == beforeSubmits + 1 &&
                              x.Failures[submit.PageIndex] == beforeFailures + 1 &&
                              x.Completed == false)))
                .MustHaveHappenedOnceExactly();

            Assert.That(evaluation.Result == Result.Wrong);
        }

        [Test]
        public async Task Evaluate_first_submit_for_chapter()
        {
            var profile = ProfileBuilder.Default.Build();
            var submit = new Submit(Guid.NewGuid(), 0, "test");
            var stats = AssumeResult(profile, submit, Result.Correct);

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(A<Guid>._, A<Guid>._))
                .Returns(Task.FromResult<ChapterStats>(default));
            A.CallTo(() => _stats.Add(A<ChapterStats>._))
                .Returns(Task.FromResult(stats));

            var beforeSubmits = stats.Submits[submit.PageIndex];
            var beforeFailures = stats.Failures[submit.PageIndex];

            var evaluation = await _submitEvaluator.Evaluate(profile, submit);

            A.CallTo(() => _stats
                .Add(A<ChapterStats>.That
                .Matches(x => x.ChapterId == submit.ChapterId &&
                              x.ProfileId == profile.Id &&
                              x.Completed == false &&
                              x.Submits.All(s => s == 0) &&
                              x.Failures.All(f => f == 0) &&
                              x.Tips.All(t => t == 0))))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _stats
                .Update(A<ChapterStats>.That
                .Matches(x => x.Submits[submit.PageIndex] == beforeSubmits + 1 &&
                              x.Failures[submit.PageIndex] == beforeFailures &&
                              x.Completed)))
                .MustHaveHappenedOnceExactly();

            Assert.That(evaluation.Result == Result.Correct);
        }

        private ChapterStats AssumeResult(Profile profile, Submit submit, Result result)
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

            A.CallTo(() => _evaluator.Evaluate(translation.Native, submit.Solution))
                .Returns(new Evaluation(result));

            var stats = ChapterStatsBuilder
                .Default
                .WithCompleted(false)
                .WithSubmits(new byte[] { 0 })
                .WithFailures(new byte[] { 0 })
                .Build();

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult(stats));

            return stats;
        }
    }
}
