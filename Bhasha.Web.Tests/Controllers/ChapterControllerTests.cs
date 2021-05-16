using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Database;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Controllers;
using Bhasha.Web.Services;
using Moq;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Controllers
{
    [TestFixture]
    public class ChapterControllerTests
    {
        private Mock<IDatabase> _database;
        private Mock<IChaptersLookup> _chapters;
        private Mock<IAuthorizedProfileLookup> _profiles;
        private Mock<IConvert<DbStats, Stats>> _stats;
        private ChapterController _controller;

        [SetUp]
        public void Before()
        {
            _database = new Mock<IDatabase>();
            _chapters = new Mock<IChaptersLookup>();
            _profiles = new Mock<IAuthorizedProfileLookup>();
            _stats = new Mock<IConvert<DbStats, Stats>>();
            _controller = new ChapterController(
                _database.Object,
                _chapters.Object,
                _profiles.Object,
                _stats.Object);
        }

        [Test]
        public async Task List_ChaptersForMaximumProfileLevel()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();

            _profiles
                .Setup(x => x.Get(profile.Id, _controller.UserId))
                .ReturnsAsync(profile);

            var chapterEnvelope = new ChapterEnvelope(
                ChapterBuilder.Default.Build(),
                StatsBuilder.Default.Build());

            var chapters = new[] { chapterEnvelope };

            _chapters
                .Setup(x => x.GetChapters(profile, int.MaxValue))
                .ReturnsAsync(chapters);

            // act
            var result = await _controller.List(profile.Id);

            // assert
            Assert.That(result, Is.EqualTo(chapters));
        }

        [Test]
        public async Task Stats_ForProfileAndChapterId()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();

            _profiles
                .Setup(x => x.Get(profile.Id, _controller.UserId))
                .ReturnsAsync(profile);

            var stats = DbStatsBuilder.Default
                .WithProfileId(profile.Id)
                .Build();

            _database
                .Setup(x => x.QueryStats(stats.ChapterId, profile.Id))
                .ReturnsAsync(stats);

            var expectedStats = StatsBuilder.Default.Build();

            _stats
                .Setup(x => x.Convert(stats))
                .Returns(expectedStats);

            // act
            var result = await _controller.Stats(stats.ProfileId, stats.ChapterId);

            // assert
            Assert.That(result, Is.EqualTo(expectedStats));
        }
    }
}
