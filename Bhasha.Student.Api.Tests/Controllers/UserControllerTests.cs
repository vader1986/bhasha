using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Tests.Support;
using Bhasha.Student.Api.Controllers;
using Moq;
using NUnit.Framework;

namespace Bhasha.Student.Api.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IDatabase> _database;
        private Mock<IStore<DbStats>> _stats;
        private Mock<IStore<DbUserProfile>> _profiles;
        private UserController _controller;

        [SetUp]
        public void Before()
        {
            _database = new Mock<IDatabase>();
            _stats = new Mock<IStore<DbStats>>();
            _profiles = new Mock<IStore<DbUserProfile>>();
            _controller = new UserController(
                _database.Object,
                _stats.Object,
                _profiles.Object);
        }

        [Test]
        public async Task Delete()
        {
            // setup
            var profiles = new[] { DbUserProfileBuilder.Default.Build() };

            _database
                .Setup(x => x.QueryProfiles(_controller.UserId))
                .ReturnsAsync(profiles);

            var stats = new[] { DbStatsBuilder.Default.Build() };

            _database
                .Setup(x => x.QueryStats(profiles[0].Id))
                .ReturnsAsync(stats);

            // act
            await _controller.Delete();

            // assert
            _profiles
                .Verify(x => x.Remove(profiles[0].Id), Times.Once);
            _stats
                .Verify(x => x.Remove(stats[0].Id), Times.Once);
        }
    }
}
