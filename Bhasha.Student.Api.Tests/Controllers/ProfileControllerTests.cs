using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Database;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Tests.Support;
using Bhasha.Student.Api.Controllers;
using Bhasha.Student.Api.Services;
using Moq;
using NUnit.Framework;

namespace Bhasha.Student.Api.Tests.Controllers
{
    [TestFixture]
    public class ProfileControllerTests
    {
        private Mock<IDatabase> _database;
        private Mock<IStore<DbUserProfile>> _store;
        private Mock<IStore<DbStats>> _stats;
        private Mock<IAuthorizedProfileLookup> _profiles;
        private Mock<IConvert<DbUserProfile, Profile>> _converter;
        private ProfileController _controller;

        [SetUp]
        public void Before()
        {
            _database = new Mock<IDatabase>();
            _store = new Mock<IStore<DbUserProfile>>();
            _stats = new Mock<IStore<DbStats>>();
            _profiles = new Mock<IAuthorizedProfileLookup>();
            _converter = new Mock<IConvert<DbUserProfile, Profile>>();
            _controller = new ProfileController(
                _database.Object,
                _store.Object,
                _stats.Object,
                _profiles.Object,
                _converter.Object);
        }

        [Test]
        public async Task Create_ProfileForNativeAndTargetLanguage()
        {
            // setup
            var profile = DbUserProfileBuilder.Default.Build();

            _database
                .Setup(x => x.QueryProfiles(_controller.UserId))
                .ReturnsAsync(Array.Empty<DbUserProfile>());

            _store
                .Setup(x => x.Add(It.IsAny<DbUserProfile>()))
                .ReturnsAsync(profile);

            // act
            var result = await _controller.Create(Language.English, Language.Bengali);

            // assert
            _store
                .Verify(x => x.Add(It.Is<DbUserProfile>(
                    y => y.Id == default &&
                    y.Languages.Native == Language.English &&
                    y.Languages.Target == Language.Bengali &&
                    y.Level == 1)), Times.Once);
        }

        [Test]
        public void Create_ForEqualNativeAndTargetLanguage_ThrowsException()
        {
            // setup
            var profile = DbUserProfileBuilder.Default.Build();

            _database
                .Setup(x => x.QueryProfiles(_controller.UserId))
                .ReturnsAsync(Array.Empty<DbUserProfile>());

            _store
                .Setup(x => x.Add(It.IsAny<DbUserProfile>()))
                .ReturnsAsync(profile);

            // act & assert
            Assert.ThrowsAsync<BadRequestException>(
                async () => await _controller.Create(Language.English, Language.English));
        }

        [Test]
        public void Create_ForAlreadyExistingProfile_ThrowsException()
        {
            // setup
            var profile = DbUserProfileBuilder.Default.Build();

            _database
                .Setup(x => x.QueryProfiles(_controller.UserId))
                .ReturnsAsync(new[] { profile });

            _store
                .Setup(x => x.Add(It.IsAny<DbUserProfile>()))
                .ReturnsAsync(profile);

            // act & assert
            Assert.ThrowsAsync<BadRequestException>(
                async () => await _controller.Create(
                    profile.Languages.Native,
                    profile.Languages.Target));
        }

        [Test]
        public async Task List_UserProfiles()
        {
            // setup
            var profiles = new[] { DbUserProfileBuilder.Default.Build() };

            _database
                .Setup(x => x.QueryProfiles(_controller.UserId))
                .ReturnsAsync(profiles);

            var profile = ProfileBuilder.Default.Build();

            _converter
                .Setup(x => x.Convert(It.IsAny<DbUserProfile>()))
                .Returns(profile);

            // act
            var result = await _controller.List();

            // assert
            Assert.That(result, Is.EqualTo(new[] { profile }));
        }

        [Test]
        public async Task Delete_ExistingUserProfile_AlsoRemovesStats()
        {
            // setup
            var profile = ProfileBuilder.Default.Build();

            _profiles
                .Setup(x => x.Get(profile.Id, _controller.UserId))
                .ReturnsAsync(profile);

            var stats = DbStatsBuilder.Default.Build();

            _database
                .Setup(x => x.QueryStats(profile.Id))
                .ReturnsAsync(new[] { stats });

            // act
            await _controller.Delete(profile.Id);

            // assert
            _store
                .Verify(x => x.Remove(profile.Id), Times.Once);
            _stats
                .Verify(x => x.Remove(stats.Id), Times.Once);
        }
    }
}
