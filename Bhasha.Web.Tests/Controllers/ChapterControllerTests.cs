using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Controllers;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Controllers
{
    [TestFixture]
    public class ChapterControllerTests
    {
        private IDatabase _database;
        private IChaptersLookup _chapters;
        private IAuthorizedProfileLookup _profiles;
        private ChapterController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _chapters = A.Fake<IChaptersLookup>();
            _profiles = A.Fake<IAuthorizedProfileLookup>();
            _controller = new ChapterController(_database, _chapters, _profiles);
        }

        [Test]
        public async Task List()
        {
            var profile = ProfileBuilder.Default.Build();

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var stats = ChapterStatsBuilder.Default.WithCompleted(false).Build();

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(A<Guid>._, A<Guid>._))
                .Returns(Task.FromResult(stats));

            var chapters = new[] { GenericChapterBuilder.Default.WithId(stats.ChapterId).Build() };

            A.CallTo(() => _database.QueryChaptersByLevel(profile.Level))
                .Returns(Task.FromResult<IEnumerable<GenericChapter>>(chapters));

            var expectedChapter = new Chapter(Guid.NewGuid(), 1, "x", "x", new Page[0], default);

            A.CallTo(() => _chapters.GetChapters(profile, A<int>._))
                .Returns(Task.FromResult(new[] { expectedChapter }));

            var result = await _controller.List(profile.Id);

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .MustHaveHappenedOnceExactly();

            Assert.That(result, Is.EquivalentTo(new[] { expectedChapter }));
        }

        [Test]
        public async Task Stats()
        {
            var stats = ChapterStatsBuilder
                .Default
                .Build();

            var profile = ProfileBuilder
                .Default
                .WithId(stats.ProfileId)
                .Build();

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            A.CallTo(() => _database.QueryStatsByChapterAndProfileId(stats.ChapterId, stats.ProfileId))
                .Returns(Task.FromResult(stats));

            var result = await _controller.Stats(stats.ProfileId, stats.ChapterId);

            Assert.That(result, Is.EqualTo(stats));
        }
    }
}
