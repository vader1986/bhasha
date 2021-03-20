using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Exceptions;
using Bhasha.Web.Services;
using FakeItEasy;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
    using AFactory = A<Func<ICacheEntry, Task<Profile>>>;
    using AString = A<string>;
    using ACacheOption = A<MemoryCacheEntryOptions>;

    [TestFixture]
    public class AuthorizedProfileLookupTests
    {
        private IAppCache _cache;
        private IStore<Profile> _profiles;
        private AuthorizedProfileLookup _authorizedLookup;

        [SetUp]
        public void Before()
        {
            _cache = A.Fake<IAppCache>();
            _profiles = A.Fake<IStore<Profile>>();
            _authorizedLookup = new AuthorizedProfileLookup(_cache, _profiles);
        }

        [Test]
        public async Task Get_profile_by_profile_id_for_user_id()
        {
            var profileId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var profile = ProfileBuilder
                .Default
                .WithId(profileId)
                .WithUserId(userId)
                .Build();

            A.CallTo(() => _cache.GetOrAddAsync(AString._, AFactory._, ACacheOption._))
                .Returns(Task.FromResult(profile));

            var result = await _authorizedLookup.Get(profileId, userId);

            Assert.That(result, Is.EqualTo(profile));
        }

        [Test]
        public void Get_profile_for_different_user()
        {
            var profileId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var profile = ProfileBuilder
                .Default
                .WithId(profileId)
                .WithUserId(userId)
                .Build();

            A.CallTo(() => _cache.GetOrAddAsync(AString._, AFactory._, ACacheOption._))
                .Returns(Task.FromResult(profile));

            var otherUserId = Guid.NewGuid();

            Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _authorizedLookup.Get(profileId, otherUserId));
        }

        [Test]
        public void Get_none_existing_profile()
        {
            var profileId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            A.CallTo(() => _cache.GetOrAddAsync(AString._, AFactory._, ACacheOption._))
                .Returns(Task.FromResult<Profile>(null));

            Assert.ThrowsAsync<NotFoundException>(async () =>
                await _authorizedLookup.Get(profileId, userId));
        }
    }
}