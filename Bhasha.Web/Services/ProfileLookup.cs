using System;
using System.Threading.Tasks;
using Bhasha.Common;

namespace Bhasha.Web.Services
{
    public interface IProfileLookup
    {
        Task<Profile> GetProfile(Guid profileId);
    }

    public class ProfileLookup : IProfileLookup
    {
        private readonly IDatabase _database;

        public ProfileLookup(IDatabase database)
        {
            _database = database;
        }

        public async Task<Profile> GetProfile(Guid profileId)
        {
            // TODO caching
            return await _database.GetProfile(profileId);
        }
    }
}
