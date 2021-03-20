using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Tests.Support;
using Bhasha.Web.Exceptions;
using Bhasha.Web.Services;
using FakeItEasy;
using NUnit.Framework;

namespace Bhasha.Web.Tests.Services
{
    [TestFixture]
    public class AuthorizedProfileLookupTests
    {
        private IProfileLookup _lookup;
        private AuthorizedProfileLookup _authorizedLookup;

        [SetUp]
        public void Before()
        {
            _lookup = A.Fake<IProfileLookup>();
            _authorizedLookup = new AuthorizedProfileLookup(_lookup);
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

            A.CallTo(() => _lookup.GetProfile(profileId)).Returns(Task.FromResult(profile));

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

            A.CallTo(() => _lookup.GetProfile(profileId)).Returns(Task.FromResult(profile));

            var otherUserId = Guid.NewGuid();

            Assert.ThrowsAsync<UnauthorizedException>(async () =>
                await _authorizedLookup.Get(profileId, otherUserId));
        }

        [Test]
        public void Get_none_existing_profile()
        {
            var profileId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            A.CallTo(() => _lookup.GetProfile(profileId)).Returns(Task.FromResult<Profile>(default));

            Assert.ThrowsAsync<NotFoundException>(async () =>
                await _authorizedLookup.Get(profileId, userId));
        }
    }
}