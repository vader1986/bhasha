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
    public class UpdateStatsOnTipRequestTests
    {
        private Mock<IDatabase> _database;
        private Mock<IStore<DbStats>> _stats;
        private Mock<IStore<DbChapter>> _chapters;
        private UpdateStatsOnTipRequest _tipsUpdater;

        [SetUp]
        public void Before()
        {
            _database = new Mock<IDatabase>();
            _stats = new Mock<IStore<DbStats>>();
            _chapters = new Mock<IStore<DbChapter>>();
            _tipsUpdater = new UpdateStatsOnTipRequest(
                _database.Object,
                _stats.Object,
                _chapters.Object);
        }

        [Test]
        public void Update_TipsForMissingChapter_ThrowsException()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapterId = Guid.NewGuid();

            // act & assert
            Assert.ThrowsAsync<ObjectNotFoundException>(
                async () =>  await _tipsUpdater.Update(profile, chapterId, 0));
        }

        [Test]
        public async Task Update_TipsForMissingStats_AddsStats()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapterId = Guid.NewGuid();
            var pageIndex = 0;

            _database
                .Setup(x => x.QueryStats(chapterId, profile.Id))
                .ReturnsAsync(default(DbStats));

            _stats
                .Setup(x => x.Add(It.IsAny<DbStats>()))
                .ReturnsAsync((DbStats stats) => stats);

            var chapter = DbChapterBuilder.Default.Build();

            _chapters
                .Setup(x => x.Get(chapterId))
                .ReturnsAsync(chapter);

            // act
            await _tipsUpdater.Update(profile, chapterId, pageIndex);

            // assert
            _stats
                .Verify(x => x.Add(It.Is<DbStats>(y => y.Tips[pageIndex] == 1)), Times.Once);
        }

        [Test]
        public async Task Update_TipsForExistingStats_UpdatesStats()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();
            var chapterId = Guid.NewGuid();
            var pageIndex = 0;

            var stats = DbStatsBuilder
                .Default
                .WithTips(new byte[] { 2 })
                .Build();

            _database
                .Setup(x => x.QueryStats(chapterId, profile.Id))
                .ReturnsAsync(stats);

            // act
            await _tipsUpdater.Update(profile, chapterId, pageIndex);

            // assert
            _stats
                .Verify(x => x.Replace(It.Is<DbStats>(y => y.Tips[pageIndex] == 3)), Times.Once);
        }
    }
}
