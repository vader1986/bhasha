using System;
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
        private IStore<User> _users;
        private IStore<ChapterStats> _stats;
        private IStore<Profile> _profiles;
        private UserController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _users = A.Fake<IStore<User>>();
            _stats = A.Fake<IStore<ChapterStats>>();
            _profiles = A.Fake<IStore<Profile>>();

            _controller = new UserController(_database, _users, _stats, _profiles);
        }

        [Test]
        public async Task Create()
        {
            await _controller.Create("user", "email");

            A.CallTo(() => _users.Add(A<User>.That
                .Matches(x => x.UserName == "user" &&
                              x.Email == "email")))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Update()
        {
            await _controller.Update("user", "email");

            A.CallTo(() => _users.Replace(A<User>.That
                .Matches(x => x.Id == _controller.UserId &&
                              x.UserName == "user" &&
                              x.Email == "email")))
                .MustHaveHappenedOnceExactly();
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

            var user = UserBuilder.Default.Build();

            A.CallTo(() => _users.Get(user.Id))
                .Returns(Task.FromResult(user));

            await _controller.Delete();

            A.CallTo(() => _profiles.Remove(profiles[0])).MustHaveHappenedOnceExactly();
            A.CallTo(() => _stats.Remove(stats[0])).MustHaveHappenedOnceExactly();
            A.CallTo(() => _users.Remove(user)).MustHaveHappenedOnceExactly();
        }
    }
}
