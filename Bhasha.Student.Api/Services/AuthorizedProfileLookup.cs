using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Database;
using Bhasha.Common.Exceptions;
using LazyCache;

namespace Bhasha.Student.Api.Services
{
    public interface IAuthorizedProfileLookup
    {
        Task<Profile> Get(Guid profileId, string userId);
    }

    public class AuthorizedProfileLookup : IAuthorizedProfileLookup
    {
        private readonly IAppCache _cache;
        private readonly IStore<DbUserProfile> _profiles;
        private readonly IConvert<DbUserProfile, Profile> _converter;

        public AuthorizedProfileLookup(IAppCache cache, IStore<DbUserProfile> profiles, IConvert<DbUserProfile, Profile> converter)
        {
            _cache = cache;
            _profiles = profiles;
            _converter = converter;
        }

        public async Task<Profile> Get(Guid profileId, string userId)
        {
            var profile = await _cache.GetOrAddAsync(profileId.ToString(),
                async () => _converter.Convert(await _profiles.Get(profileId)));

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
