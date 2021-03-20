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
        private IAssembleChapters _chapters;
        private IAuthorizedProfileLookup _profiles;
        private ChapterController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _chapters = A.Fake<IAssembleChapters>();
            _profiles = A.Fake<IAuthorizedProfileLookup>();
            _controller = new ChapterController(_database, _chapters, _profiles);
        }

        [Test]
        public async Task List()
        {
            var profile = ProfileBuilder.Default.Build();

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var chapters = new[] { GenericChapterBuilder.Default.Build() };

            A.CallTo(() => _database.QueryChaptersByLevel(profile.Level))
                .Returns(Task.FromResult<IEnumerable<GenericChapter>>(chapters));

            var result = await _controller.List(profile.Id);

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .MustHaveHappenedOnceExactly();

            Assert.That(result, Is.EquivalentTo(chapters));
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

        [Test]
        public async Task Get()
        {
            var profile = ProfileBuilder.Default.Build();

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var chapter = new Chapter(Guid.NewGuid(), 1, "asf", "asd", Array.Empty<Page>(), null);

            A.CallTo(() => _chapters.Assemble(chapter.Id, profile))
                .Returns(Task.FromResult(chapter));

            var result = await _controller.Get(profile.Id, chapter.Id);

            Assert.That(result, Is.EqualTo(chapter));
        }
    }
}
