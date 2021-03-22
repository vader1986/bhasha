using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class TipStatsUpdaterTests
    {
        private IDatabase _database;
        private IStore<ChapterStats> _stats;
        private IStore<GenericChapter> _chapters;
        private TipStatsUpdater _statsUpdater;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _chapters = A.Fake<IStore<GenericChapter>>();

            _statsUpdater = new TipStatsUpdater(_database, _stats, _chapters);
        }

        private void AssumeNoChapterStatsFor(Guid chapterId, Guid profileId)
        {
            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(chapterId, profileId))
                .Returns(Task.FromResult<ChapterStats>(null));
        }

        private void AssumeGenericChapter(GenericChapter chapter)
        {
            A.CallTo(() => _chapters.Get(chapter.Id))
                .Returns(Task.FromResult(chapter));
        }

        private void AssumeStatsAdded(ChapterStats stats)
        {
            A.CallTo(() => _stats.Add(A<ChapterStats>._))
                .Returns(Task.FromResult(stats));
        }

        private void AssumeChapterStats(ChapterStats stats)
        {
            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(
                stats.ChapterId,
                stats.ProfileId))
                .Returns(Task.FromResult(stats));
        }

        [Test]
        public async Task UpdateStats_creates_new_stats()
        {
            var profile = ProfileBuilder
                .Default
                .Build();

            var chapter = GenericChapterBuilder
                .Default
                .Build();

            var tip = TipBuilder
                .Default
                .WithChapterId(chapter.Id)
                .WithPageIndex(1)
                .Build();


            var stats = ChapterStats.Create(profile.Id, chapter);

            AssumeNoChapterStatsFor(tip.ChapterId, profile.Id);
            AssumeGenericChapter(chapter);
            AssumeStatsAdded(stats);

            await _statsUpdater.UpdateStats(tip, profile);

            A.CallTo(() => _stats.Add(A<ChapterStats>.That
                .Matches(x => x.Completed == false &&
                              x.Tips.Length == chapter.Pages.Length &&
                              x.Tips.All(x => x == 0))))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.Completed == false &&
                              x.Tips[tip.PageIndex] == 1)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateStats_existing_stats()
        {
            var profile = ProfileBuilder
                .Default
                .Build();

            var tip = TipBuilder
                .Default
                .WithPageIndex(1)
                .Build();

            var stats = ChapterStatsBuilder
                .Default
                .WithProfileId(profile.Id)
                .WithChapterId(tip.ChapterId)
                .Build();

            AssumeChapterStats(stats);

            var expectedTips = stats.Tips[tip.PageIndex] + 1;

            await _statsUpdater.UpdateStats(tip, profile);

            A.CallTo(() => _stats.Add(A<ChapterStats>._))
                .MustNotHaveHappened();

            A.CallTo(() => _stats.Replace(A<ChapterStats>.That
                .Matches(x => x.Completed == stats.Completed &&
                              x.Tips[tip.PageIndex] == expectedTips)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateStats_missing_chapter_throws()
        {
            var chapterId = Guid
                .NewGuid();

            var profile = ProfileBuilder
                .Default
                .Build();

            var tip = TipBuilder
                .Default
                .WithChapterId(chapterId)
                .WithPageIndex(1)
                .Build();

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(chapterId, profile.Id))
                .Returns(Task.FromResult<ChapterStats>(null));

            A.CallTo(() => _chapters.Get(tip.ChapterId))
                .Returns(Task.FromResult<GenericChapter>(null));

            Assert.ThrowsAsync<ObjectNotFoundException>(
                async () => await _statsUpdater.UpdateStats(tip, profile));
        }
    }
}
