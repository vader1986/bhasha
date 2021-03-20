using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;

namespace Bhasha.Web.Services
{
    public interface IProfileLookup
    {
        Task<Profile> GetProfile(Guid profileId);
    }

    public class ProfileLookup : IProfileLookup
    {
        private readonly IStore<Profile> _profiles;

        public ProfileLookup(IStore<Profile> profiles)
        {
            _profiles = profiles;
        }

        public async Task<Profile> GetProfile(Guid profileId)
        {
            // TODO caching
            return await _profiles.Get(profileId);
        }
    }
}
