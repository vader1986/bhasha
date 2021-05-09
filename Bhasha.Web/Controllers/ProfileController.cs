using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Database;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : BaseController
    {
        private readonly IDatabase _database;
        private readonly IStore<DbUserProfile> _store;
        private readonly IStore<DbStats> _stats;
        private readonly IAuthorizedProfileLookup _profiles;
        private readonly IConvert<DbUserProfile, Profile> _converter;

        public ProfileController(IDatabase database, IStore<DbUserProfile> store, IStore<DbStats> stats, IAuthorizedProfileLookup profiles, IConvert<DbUserProfile, Profile> converter)
        {
            _database = database;
            _store = store;
            _stats = stats;
            _profiles = profiles;
            _converter = converter;
        }

        // Authorize User
        [HttpPost("create")]
        public async Task<Profile> Create(string native, string target)
        {
            var profile = new DbUserProfile {
                Id = default,
                UserId = UserId,
                Languages = new DbProfile
                {
                    Native = native,
                    Target = target
                },
                Level = 1,
                CompletedChapters = 0
            };

            return _converter.Convert(await _store.Add(profile));
        }

        // Authorize User
        [HttpGet("list")]
        public async Task<Profile[]> List()
        {
            var profiles = await _database.QueryProfiles(UserId);

            return profiles
                .Select(_converter.Convert)
                .ToArray();
        }

        // Authorize User
        [HttpGet("languages")]
        public Language[] Languages()
        {
            return Language.Supported.Values.ToArray();
        }

        // Authorize User
        [HttpDelete("delete")]
        public async Task Delete(Guid profileId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var stats = await _database.QueryStats(profileId);

            var deletions = stats
                .Select(x => _stats.Remove(x.Id))
                .Append(_store.Remove(profile.Id));

            await Task.WhenAll(deletions);
        }
    }
}
