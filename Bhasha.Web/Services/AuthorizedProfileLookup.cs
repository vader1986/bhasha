using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Web.Exceptions;
using LazyCache;

namespace Bhasha.Web.Services
{
    public interface IAuthorizedProfileLookup
    {
        Task<Profile> Get(Guid profileId, string userId);
    }

    public class AuthorizedProfileLookup : IAuthorizedProfileLookup
    {
        private readonly IStore<Profile> _profiles;
        private readonly IAppCache _cache;

        public AuthorizedProfileLookup(IAppCache cache, IStore<Profile> profiles)
        {
            _cache = cache;
            _profiles = profiles;
        }

        public async Task<Profile> Get(Guid profileId, string userId)
        {
            var profile = await _cache.GetOrAddAsync(profileId.ToString(), () => _profiles.Get(profileId));

            if (profile == null)
            {
                throw new NotFoundException($"Profile for ID {profileId} not found.");
            }

            if (profile.UserId != userId)
            {
                throw new UnauthorizedException($"Access to Profile {profileId} denied.");
            }

            return profile;
        }
    }
}
