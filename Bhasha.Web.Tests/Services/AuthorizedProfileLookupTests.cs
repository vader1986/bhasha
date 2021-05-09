using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Exceptions;
using Bhasha.Web.Services;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
    [TestFixture]
    public class AuthorizedProfileLookupTests
    {
        private Mock<IAppCache> _cache;
        private Mock<IStore<DbUserProfile>> _profiles;
        private Mock<IConvert<DbUserProfile, Profile>> _converter;
        private AuthorizedProfileLookup _authorizedLookup;

        [SetUp]
        public void Before()
        {
            _cache = new Mock<IAppCache>();
            _profiles = new Mock<IStore<DbUserProfile>>();
            _converter = new Mock<IConvert<DbUserProfile, Profile>>();
            _authorizedLookup = new AuthorizedProfileLookup(
                _cache.Object,
                _profiles.Object,
                _converter.Object);
        }

        private void AssumeCachedProfile(Profile profile)
        {
            _cache
                .Setup(x => x.GetOrAddAsync(
                    It.IsAny<string>(),
                    It.IsAny<Func<ICacheEntry, Task<Profile>>>(),
                    It.IsAny<MemoryCacheEntryOptions>()))
                .ReturnsAsync(profile);

            _cache
                .Setup(x => x.DefaultCachePolicy)
                .Returns(new CacheDefaults());
        }

        [Test]
        public async Task Get_ForCachedProfile_ReturnsCachedProfile()
        {
            // setup
            var profileId = Guid.NewGuid();
            var userId = Rnd.Create.NextString();

            var profile = ProfileBuilder
                .Default
                .WithId(profileId)
                .WithUserId(userId)
                .Build();

            AssumeCachedProfile(profile);

            // act
            var result = await _authorizedLookup.Get(profileId, userId);

            // assert
            Assert.That(result, Is.EqualTo(profile));
        }

        [Test]
        public void Get_ForDifferentUserId_ThrowsException()
        {
            // setup
            var profileId = Guid.NewGuid();
            var userId = Rnd.Create.NextString();

            var profile = ProfileBuilder
                .Default
                .WithId(profileId)
                .WithUserId(userId)
                .Build();

            var otherUserId = Rnd.Create.NextString();

            AssumeCachedProfile(profile);

            // act & assert
            Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _authorizedLookup.Get(profileId, otherUserId));
        }

        [Test]
        public void Get_ForNonExistingProfile_ThrowsException()
        {
            // setup
            var profileId = Guid.NewGuid();
            var userId = Rnd.Create.NextString();

            AssumeCachedProfile(null);

            // act & assert
            Assert.ThrowsAsync<NotFoundException>(async () =>
                await _authorizedLookup.Get(profileId, userId));
        }
    }
}