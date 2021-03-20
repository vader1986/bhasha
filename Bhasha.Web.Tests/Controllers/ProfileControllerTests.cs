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
    public class ProfileControllerTests
    {
        private IDatabase _database;
        private IStore<Profile> _store;
        private IStore<ChapterStats> _stats;
        private IAuthorizedProfileLookup _profiles;
        private ProfileController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _store = A.Fake<IStore<Profile>>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _profiles = A.Fake<IAuthorizedProfileLookup>();
            _controller = new ProfileController(_database, _store, _stats, _profiles);
        }

        [Test]
        public async Task Create()
        {
            var profile = ProfileBuilder
                .Default
                .Build();

            A.CallTo(() => _store.Add(A<Profile>._)).Returns(profile);

            var result = await _controller.Create(Language.English, Language.Bengoli);

            A.CallTo(() => _store.Add(A<Profile>.That
                .Matches(x => x.Id == default &&
                              x.From == Language.English &&
                              x.To == Language.Bengoli &&
                              x.Level == 1)))
                .MustHaveHappenedOnceExactly();

            Assert.That(result, Is.EqualTo(profile));
        }

        [Test]
        public async Task List()
        {
            var profiles = new[] { ProfileBuilder.Default.Build() };

            A.CallTo(() => _database.QueryProfilesByUserId(_controller.UserId))
                .Returns(Task.FromResult<IEnumerable<Profile>>(profiles));

            var result = await _controller.List();

            Assert.That(result, Is.EqualTo(profiles));
        }

        [Test]
        public async Task Delete()
        {
            var profile = ProfileBuilder
                .Default
                .Build();

            A.CallTo(() => _profiles.Get(profile.Id, _controller.UserId))
                .Returns(Task.FromResult(profile));

            var stats = ChapterStatsBuilder
                .Default
                .Build();

            A.CallTo(() => _database.QueryStatsByProfileId(profile.Id))
                .Returns(Task.FromResult<IEnumerable<ChapterStats>>(new[] { stats }));

            await _controller.Delete(profile.Id);

            A.CallTo(() => _store.Remove(profile)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _stats.Remove(stats)).MustHaveHappenedOnceExactly();
        }
    }
}
