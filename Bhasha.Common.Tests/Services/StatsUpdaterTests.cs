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
    public class StatsUpdaterTests
    {
        private IDatabase _database;
        private IStore<ChapterStats> _stats;
        private IStore<Profile> _profiles;
        private StatsUpdater _statsUpdater;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _profiles = A.Fake<IStore<Profile>>();
            _statsUpdater = new StatsUpdater(_database, _stats, _profiles);
        }

        private static bool IsInitialized(byte[] bytes, GenericChapter chapter)
        {
            return
                bytes != null &&
                bytes.Length == chapter.Pages.Length &&
                bytes.All(x => x == 0);
        }

        [Test]
        public async Task FromEvaluation_creates_chapter_stats()
        {
            var genericChapter = GenericChapterBuilder.Default.Build();
            var submit = new Submit(genericChapter.Id, 1, "test");
            var evaluation = new Evaluation(Result.Correct, submit);
            var profile = ProfileBuilder.Default.Build();

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult<ChapterStats>(null));

            A.CallTo(() => _stats.Add(A<ChapterStats>.Ignored))
                .Returns(ChapterStats.Create(profile.Id, genericChapter));

            await _statsUpdater.FromEvaluation(evaluation, profile, genericChapter);

            A.CallTo(() => _stats.Add(A<ChapterStats>.That
                .Matches(x => x.Completed == false &&
                              x.ChapterId == genericChapter.Id &&
                              IsInitialized(x.Failures, genericChapter) &&
                              IsInitialized(x.Submits, genericChapter) &&
                              IsInitialized(x.Tips, genericChapter))))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task FromEvaluation_with_wrong_result()
        {
            var genericChapter = GenericChapterBuilder
                .Default
                .Build();

            var profile = ProfileBuilder
                .Default
                .Build();

            var stats = ChapterStatsBuilder
                .Default
                .WithFailures(new byte[] { 0, 0, 0, 0, 0 })
                .WithSubmits(new byte[] { 0, 0, 0, 0, 0 })
                .WithChapterId(genericChapter.Id)
                .Build();

            var submit = new Submit(genericChapter.Id, 1, "test");
            var evaluation = new Evaluation(Result.Wrong, submit);

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult(stats));

            await _statsUpdater.FromEvaluation(evaluation, profile, genericChapter);

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.ChapterId == genericChapter.Id &&
                              x.Failures[submit.PageIndex] == 1 &&
                              x.Submits[submit.PageIndex] == 1)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task FromEvaluation_with_correct_result()
        {
            var genericChapter = GenericChapterBuilder
                .Default
                .Build();

            var profile = ProfileBuilder
                .Default
                .Build();

            var stats = ChapterStatsBuilder
                .Default
                .WithCompleted(false)
                .WithFailures(new byte[] { 0, 0, 0, 0, 0 })
                .WithSubmits(new byte[] { 0, 0, 0, 0, 0 })
                .WithChapterId(genericChapter.Id)
                .Build();

            var submit = new Submit(genericChapter.Id, 1, "test");
            var evaluation = new Evaluation(Result.Correct, submit);

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult(stats));

            await _statsUpdater.FromEvaluation(evaluation, profile, genericChapter);

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.ChapterId == genericChapter.Id &&
                              x.Completed == false &&
                              x.Failures[submit.PageIndex] == 0 &&
                              x.Submits[submit.PageIndex] == 1)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task FromEvaluation_with_correct_result_and_completed_chapter()
        {
            var genericChapter = GenericChapterBuilder
                .Default
                .Build();

            var profile = ProfileBuilder
                .Default
                .Build();

            var stats = ChapterStatsBuilder
                .Default
                .WithCompleted(false)
                .WithFailures(new byte[] { 0, 0, 0, 0, 0 })
                .WithSubmits(new byte[] { 1, 0, 1, 1, 1 })
                .WithChapterId(genericChapter.Id)
                .Build();

            var submit = new Submit(genericChapter.Id, 1, "test");
            var evaluation = new Evaluation(Result.Correct, submit);

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult(stats));

            await _statsUpdater.FromEvaluation(evaluation, profile, genericChapter);

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.ChapterId == genericChapter.Id &&
                              x.Completed == true &&
                              x.Failures[submit.PageIndex] == 0 &&
                              x.Submits[submit.PageIndex] == 1)))
                .MustHaveHappenedOnceExactly();
        }
    }
}
