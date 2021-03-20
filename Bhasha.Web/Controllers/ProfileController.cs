using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : BhashaController
    {
        private readonly IDatabase _database;
        private readonly IStore<Profile> _store;
        private readonly IStore<ChapterStats> _stats;
        private readonly IAuthorizedProfileLookup _profiles;

        public ProfileController(IDatabase database, IStore<Profile> store, IStore<ChapterStats> stats, IAuthorizedProfileLookup profiles)
        {
            _database = database;
            _store = store;
            _stats = stats;
            _profiles = profiles;
        }

        // Authorize User
        [HttpPost("create")]
        public async Task<Profile> Create(string from, string to)
        {
            return await _store.Add(new Profile(default, UserId, from, to, 1, 0));
        }

        // Authorize User
        [HttpGet("list")]
        public async Task<Profile[]> List()
        {
            var profiles = await _database.QueryProfilesByUserId(UserId);
            return profiles.ToArray();
        }

        // Authorize User
        [HttpDelete("delete")]
        public async Task Delete(Guid profileId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var stats = await _database.QueryStatsByProfileId(profileId);

            var deletions = stats
                .Select(x => _stats.Remove(x))
                .Append(_store.Remove(profile));

            await Task.WhenAll(deletions);
        }
    }
}
