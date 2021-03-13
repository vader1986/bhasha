using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Web.Controllers;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private IDatabase _database;
        private UserController _controller;

        [SetUp]
        public void Before()
        {
            _database = A.Fake<IDatabase>();
            _controller = new UserController(_database);
        }

        [Test]
        public async Task Delete_removes_profiles_user_and_stats()
        {
            var userId = _controller.UserId;
            var profiles = new[] {
                new Profile(Guid.NewGuid(), userId, Language.Bengoli, Language.English, 1),
                new Profile(Guid.NewGuid(), userId, Language.English, Language.Bengoli, 3),
            };

            A.CallTo(() => _database.GetProfiles(userId))
                .Returns(Task.FromResult<IEnumerable<Profile>>(profiles));

            await _controller.Delete();


            A.CallTo(() => _database.DeleteUser(userId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _database.DeleteProfiles(userId)).MustHaveHappenedOnceExactly();

            foreach (var profile in profiles)
            {
                A.CallTo(() => _database.DeleteChapterStatsForProfile(profile.Id));
            }
        }
    }
}
