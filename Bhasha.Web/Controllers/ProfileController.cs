using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : BhashaController
    {
        private readonly IDatabase _database;
        private readonly IAuthorizedProfileLookup _profiles;

        public ProfileController(IDatabase database, IAuthorizedProfileLookup profiles)
        {
            _database = database;
            _profiles = profiles;
        }

        // Authorize User
        [HttpPost("create")]
        public async Task<ActionResult<Profile>> Create(string from, string to)
        {
            return await _database.CreateProfile(new Profile(default, UserId, from, to, 1));
        }

        // Authorize User
        [HttpGet("list")]
        public async Task<ActionResult<Profile[]>> List()
        {
            var profiles = await _database.GetProfiles(UserId);
            return profiles.ToArray();
        }

        // Authorize User
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid profileId)
        {
            var profile = await _profiles.Get(profileId, UserId);

            await Task.WhenAll(
                _database.DeleteProfile(profile.Id),
                _database.DeleteChapterStatsForProfile(profile.Id));

            return Ok();
        }
    }
}
