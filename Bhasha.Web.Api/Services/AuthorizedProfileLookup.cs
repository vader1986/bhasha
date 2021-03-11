using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Web.Api.Exceptions;

namespace Bhasha.Web.Api.Services
{
    public interface IAuthorizedProfileLookup
    {
        Task<Profile> Get(Guid profileId, Guid userId);
    }

    public class AuthorizedProfileLookup : IAuthorizedProfileLookup
    {
        private readonly IProfileLookup _lookup;

        public AuthorizedProfileLookup(IProfileLookup lookup)
        {
            _lookup = lookup;
        }

        public async Task<Profile> Get(Guid profileId, Guid userId)
        {
            var profile = await _lookup.GetProfile(profileId);

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
