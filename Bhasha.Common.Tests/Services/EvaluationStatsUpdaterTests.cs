using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class EvaluationStatsUpdaterTests
    {
        private IDatabase _database;
        private IStore<ChapterStats> _stats;
        private IStore<Profile> _profiles;
        private EvaluationStatsUpdater _statsUpdater;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _profiles = A.Fake<IStore<Profile>>();

            _statsUpdater = new EvaluationStatsUpdater(_database, _stats, _profiles);
        }

        private static bool IsInitialized(byte[] bytes, GenericChapter chapter)
        {
            return
                bytes != null &&
                bytes.Length == chapter.Pages.Length &&
                bytes.All(x => x == 0);
        }

        [Test]
        public async Task UpdateStats_creates_chapter_stats()
        {
            var genericChapter = GenericChapterBuilder.Default.Build();
            var profile = ProfileBuilder.Default.Build();

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult<ChapterStats>(null));

            A.CallTo(() => _stats.Add(A<ChapterStats>.Ignored))
                .Returns(ChapterStats.Create(profile.Id, genericChapter));

            await _statsUpdater.UpdateStats(Result.Correct, 1, profile, genericChapter);

            A.CallTo(() => _stats.Add(A<ChapterStats>.That
                .Matches(x => x.Completed == false &&
                              x.ChapterId == genericChapter.Id &&
                              x.Tips == 0 &&
                              IsInitialized(x.Failures, genericChapter) &&
                              IsInitialized(x.Submits, genericChapter))))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateStats_with_wrong_result()
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

            var pageIndex = 1;

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult(stats));

            await _statsUpdater.UpdateStats(Result.Wrong, pageIndex, profile, genericChapter);

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.ChapterId == genericChapter.Id &&
                              x.Failures[pageIndex] == 1 &&
                              x.Submits[pageIndex] == 1)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateStats_with_correct_result()
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

            var pageIndex = 1;

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult(stats));

            await _statsUpdater.UpdateStats(Result.Correct, pageIndex, profile, genericChapter);

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.ChapterId == genericChapter.Id &&
                              x.Completed == false &&
                              x.Failures[pageIndex] == 0 &&
                              x.Submits[pageIndex] == 1)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateStats_with_correct_result_and_completed_chapter()
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

            var pageIndex = 1;

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(genericChapter.Id, profile.Id))
                .Returns(Task.FromResult(stats));

            await _statsUpdater.UpdateStats(Result.Correct, pageIndex, profile, genericChapter);

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.ChapterId == genericChapter.Id &&
                              x.Completed == true &&
                              x.Failures[pageIndex] == 0 &&
                              x.Submits[pageIndex] == 1)))
                .MustHaveHappenedOnceExactly();
        }
    }
}
