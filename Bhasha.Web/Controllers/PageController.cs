using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Extensions;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/page")]
    public class PageController : BhashaController
    {
        private readonly IDatabase _database;
        private readonly IAuthorizedProfileLookup _profiles;

        public PageController(IDatabase database, IAuthorizedProfileLookup profiles)
        {
            _database = database;
            _profiles = profiles;
        }

        // Authorize User
        [HttpPost("submit")]
        public async Task<IActionResult> Submit()
        {
            // TODO
            // * define submit content
            // * validation
            // * update stats
            // * return validation result
            return Ok();
        }

        // Authorize User
        [HttpPost("tip")]
        public async Task<ActionResult<Tip>> Tip(Guid profileId, Guid chapterId, int pageIndex)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var tips = await _database.GetTips(chapterId, pageIndex);

            var chapterStats = await _database.GetChapterStats(profile.Id, chapterId);
            chapterStats.Tips[pageIndex]++;

            await _database.UpdateChapterStats(chapterStats);

            return tips.Random(); 
        }
    }
}
