using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Moq;
using NUnit.Framework;

namespace Bhasha.Common.Tests.Services
{
    [TestFixture]
    public class ChaptersLookupTests
    {
        private Mock<IDatabase> _database;
        private Mock<IStore<DbTranslatedChapter>> _chapters;
        private Mock<IStore<DbStats>> _stats;
        private Mock<IConvert<DbTranslatedChapter, Chapter>> _convertChapters;
        private Mock<IConvert<DbStats, Stats>> _convertStats;
        private ChaptersLookup _lookup;

        [SetUp]
        public void Before()
        {
            
            _database = new Mock<IDatabase>();
            _chapters = new Mock<IStore<DbTranslatedChapter>>();
            _stats = new Mock<IStore<DbStats>>();
            _convertChapters = new Mock<IConvert<DbTranslatedChapter, Chapter>>();
            _convertStats = new Mock<IConvert<DbStats, Stats>>();
            _lookup = new ChaptersLookup(
                _database.Object,
                _chapters.Object,
                _stats.Object,
                _convertChapters.Object,
                _convertStats.Object);
        }

        private void AssumePopulatedDatabase(int expectedChapters, int expectedLevel)
        {
            var chapters = Enumerable
                .Range(1, expectedChapters)
                .Select(level => DbChapterBuilder.Default.Build())
                .ToList();

            _database
                .Setup(x => x.QueryChapters(expectedLevel))
                .ReturnsAsync(chapters);

            var stats = DbStatsBuilder.Default.Build();

            _database
                .Setup(x => x.QueryStats(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(stats);

            var chapter = DbTranslatedChapterBuilder.Default.Build();

            _chapters
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(chapter);

            _stats
                .Setup(x => x.Add(It.IsAny<DbStats>()))
                .ReturnsAsync(stats);

            var convertedChapter = ChapterBuilder.Default.Build();
            var convertedStats = StatsBuilder.Default.Build();

            _convertChapters
                .Setup(x => x.Convert(chapter))
                .Returns(convertedChapter);

            _convertStats
                .Setup(x => x.Convert(stats))
                .Returns(convertedStats);
        }

        [Test]
        public async Task GetChapters_ForProfileAndLevel_ReturnsExpectedChapters()
        {
            // setup
            const int expectedChapters = 10;
            const int expectedLevel = 5;

            AssumePopulatedDatabase(expectedChapters, expectedLevel);

            var profile = ProfileBuilder
                .Default
                .WithLevel(expectedLevel)
                .Build();

            // act
            var result = await _lookup.GetChapters(profile, expectedLevel);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(expectedChapters));
        }

        [Test]
        public async Task GetChapters_ForMissingStats_CreatesNewStats()
        {
            // setup
            const int expectedChapters = 10;
            const int expectedLevel = 5;

            AssumePopulatedDatabase(expectedChapters, expectedLevel);

            _database
                .Setup(x => x.QueryStats(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(default(DbStats));

            var profile = ProfileBuilder
                .Default
                .WithLevel(expectedLevel)
                .Build();

            // act
            var result = await _lookup.GetChapters(profile, expectedLevel);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(expectedChapters));

            _stats.Verify(x => x.Add(It.IsAny<DbStats>()), Times.Exactly(expectedChapters));
        }

        [Test]
        public async Task GetChapters_ForProfileWithLowerLevel_ReturnsLowerLevelChapters()
        {
            // setup
            const int expectedChapters = 10;
            const int expectedLevel = 5;

            AssumePopulatedDatabase(expectedChapters, expectedLevel);

            var profile = ProfileBuilder
                .Default
                .WithLevel(expectedLevel)
                .Build();

            // act
            var result = await _lookup.GetChapters(profile, int.MaxValue);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(expectedChapters));
        }


        [Test]
        public async Task GetChapters_ForMissingChapter_DoesNotThrow()
        {
            // setup
            const int expectedChapters = 10;
            const int expectedLevel = 5;

            AssumePopulatedDatabase(expectedChapters, expectedLevel);

            _chapters
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .ReturnsAsync(default(DbTranslatedChapter));

            var profile = ProfileBuilder
                .Default
                .WithLevel(expectedLevel)
                .Build();

            // act
            var result = await _lookup.GetChapters(profile, int.MaxValue);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.EqualTo(0));
        }
    }
}
