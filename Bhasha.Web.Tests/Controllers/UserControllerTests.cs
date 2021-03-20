using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Controllers;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private IDatabase _database;
        private IStore<ChapterStats> _stats;
        private IStore<Profile> _profiles;
        private UserController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _profiles = A.Fake<IStore<Profile>>();
            _controller = new UserController(_database, _stats, _profiles);
        }

        [Test]
        public async Task Delete()
        {
            var profiles = new[] {
                ProfileBuilder.Default.Build()
            };

            A.CallTo(() => _database.QueryProfilesByUserId(_controller.UserId))
                .Returns(Task.FromResult<IEnumerable<Profile>>(profiles));

            var stats = new[] {
                ChapterStatsBuilder.Default.Build()
            };

            A.CallTo(() => _database.QueryStatsByProfileId(profiles[0].Id))
                .Returns(Task.FromResult<IEnumerable<ChapterStats>>(stats));

            await _controller.Delete();

            A.CallTo(() => _profiles.Remove(profiles[0])).MustHaveHappenedOnceExactly();
            A.CallTo(() => _stats.Remove(stats[0])).MustHaveHappenedOnceExactly();
        }
    }
}
