using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class UpdateStatsOnSubmitTests
    {
        private Mock<IDatabase> _database;
        private Mock<IStore<DbStats>> _stats;
        private Mock<IStore<DbUserProfile>> _profiles;
        private UpdateStatsOnSubmit _statsUpdater;

        [SetUp]
        public void Before()
        {
            _database = new Mock<IDatabase>();
            _stats = new Mock<IStore<DbStats>>();
            _profiles = new Mock<IStore<DbUserProfile>>();
            _statsUpdater = new UpdateStatsOnSubmit(
                _database.Object,
                _stats.Object,
                _profiles.Object);
        }

        [Test]
        public async Task Update_EvaluationForMissingStats_AddsNewStats()
        {
            // setup
            var submit = SubmitBuilder
                .Default
                .WithPageIndex(0)
                .Build();

            var profile = ProfileBuilder.Default.Build();
            var evaluation = new Evaluation(Result.Correct, submit, profile);

            _database
                .Setup(x => x.QueryStats(submit.ChapterId, profile.Id))
                .ReturnsAsync(default(DbStats));

            _stats
                .Setup(x => x.Add(It.IsAny<DbStats>()))
                .ReturnsAsync((DbStats stats) => stats);

            var pages = submit.PageIndex + 1;

            // act
            await _statsUpdater.Update(evaluation, pages);

            // assert
            _stats
                .Verify(x => x.Add(
                    It.Is<DbStats>(y => y.Tips.Length == pages &&
                                        y.Submits.Length == pages &&
                                        y.Failures.Length == pages)), Times.Once);                
        }

        [Test]
        public async Task Update_ForCorrectResult_UpdatesStats()
        {
            // setup
            var pageIndex = 0;
            var submit = SubmitBuilder
                .Default
                .WithPageIndex(pageIndex)
                .Build();

            var profile = ProfileBuilder.Default.Build();
            var evaluation = new Evaluation(Result.Correct, submit, profile);
            var stats = DbStatsBuilder.Default.Build();

            _database
                .Setup(x => x.QueryStats(submit.ChapterId, profile.Id))
                .ReturnsAsync(stats);

            var pages = pageIndex + 1;

            var oldSubmits = stats.Submits[pageIndex];
            var oldFailures = stats.Submits[pageIndex];
            var oldTips = stats.Submits[pageIndex];

            // act
            await _statsUpdater.Update(evaluation, pages);

            // assert
            _stats
                .Verify(x => x.Replace(It.Is<DbStats>(
                    y => y.Submits[pageIndex] == oldSubmits + 1 &&
                         y.Failures[pageIndex] == oldFailures &&
                         y.Tips[pageIndex] == oldTips)), Times.Once);
        }

        [Test]
        public async Task Update_ForWrongResult_UpdatesStats()
        {
            // setup
            var pageIndex = 0;
            var submit = SubmitBuilder
                .Default
                .WithPageIndex(pageIndex)
                .Build();

            var profile = ProfileBuilder.Default.Build();
            var evaluation = new Evaluation(Result.Wrong, submit, profile);
            var stats = DbStatsBuilder.Default.Build();

            _database
                .Setup(x => x.QueryStats(submit.ChapterId, profile.Id))
                .ReturnsAsync(stats);

            var pages = pageIndex + 1;

            var oldSubmits = stats.Submits[pageIndex];
            var oldFailures = stats.Submits[pageIndex];
            var oldTips = stats.Submits[pageIndex];

            // act
            await _statsUpdater.Update(evaluation, pages);

            // assert
            _stats
                .Verify(x => x.Replace(It.Is<DbStats>(
                    y => y.Submits[pageIndex] == oldSubmits + 1 &&
                         y.Failures[pageIndex] == oldFailures + 1 &&
                         y.Tips[pageIndex] == oldTips)), Times.Once);
        }

        [Test]
        public async Task Update_ForCompletedChapter_UpdatesProfile()
        {
            // setup
            var pageIndex = 0;
            var submit = SubmitBuilder
                .Default
                .WithPageIndex(pageIndex)
                .Build();

            var profile = ProfileBuilder
                .Default
                .WithCompletedChapters(0)
                .WithLevel(1)
                .Build();

            var evaluation = new Evaluation(Result.Correct, submit, profile);

            var stats = DbStatsBuilder
                .Default
                .WithSubmits(new byte[] { 0 })
                .WithFailures(new byte[] { 0 })
                .WithTips(new byte[] { 0 })
                .WithCompleted(false)
                .Build();

            _database
                .Setup(x => x.QueryStats(submit.ChapterId, profile.Id))
                .ReturnsAsync(stats);

            _database
                .Setup(x => x.QueryChapters(profile.Level))
                .ReturnsAsync(new[] {
                    DbChapterBuilder.Default.Build(),
                    DbChapterBuilder.Default.Build()});

            var pages = pageIndex + 1;

            var oldCompletedChapters = profile.CompletedChapters;
            var oldLevel = profile.Level;

            // act
            await _statsUpdater.Update(evaluation, pages);

            // assert
            _profiles
                .Verify(x => x.Replace(It.Is<DbUserProfile>(
                    y => y.CompletedChapters == oldCompletedChapters + 1 &&
                         y.Level == oldLevel)), Times.Once);
            _stats
                .Verify(x => x.Replace(It.Is<DbStats>(y => y.Completed)), Times.Once);
        }

        [Test]
        public async Task Update_ForCompletedLevel_UpdatesProfile()
        {
            // setup
            var pageIndex = 0;
            var submit = SubmitBuilder
                .Default
                .WithPageIndex(pageIndex)
                .Build();

            var profile = ProfileBuilder
                .Default
                .WithCompletedChapters(0)
                .WithLevel(1)
                .Build();

            var evaluation = new Evaluation(Result.Correct, submit, profile);

            var stats = DbStatsBuilder
                .Default
                .WithSubmits(new byte[] { 0 })
                .WithFailures(new byte[] { 0 })
                .WithTips(new byte[] { 0 })
                .WithCompleted(false)
                .Build();

            _database
                .Setup(x => x.QueryStats(submit.ChapterId, profile.Id))
                .ReturnsAsync(stats);

            _database
                .Setup(x => x.QueryChapters(profile.Level))
                .ReturnsAsync(new[] { DbChapterBuilder.Default.Build() });

            var pages = pageIndex + 1;

            var oldCompletedChapters = profile.CompletedChapters;
            var oldLevel = profile.Level;

            // act
            await _statsUpdater.Update(evaluation, pages);

            // assert
            _profiles
                .Verify(x => x.Replace(It.Is<DbUserProfile>(
                    y => y.CompletedChapters == oldCompletedChapters + 1 &&
                         y.Level == oldLevel + 1)), Times.Once);
            _stats
                .Verify(x => x.Replace(It.Is<DbStats>(y => y.Completed)), Times.Once);
        }
    }
}
