using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/chapter")]
    public class ChapterController : BhashaController
    {
        private readonly IDatabase _database;
        private readonly IAuthorizedProfileLookup _profiles;

        public ChapterController(IDatabase database, IAuthorizedProfileLookup profiles)
        {
            _database = database;
            _profiles = profiles;
        }

        // Authorize User
        [HttpGet("list")]
        public async Task<ActionResult<Chapter[]>> List(Guid profileId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var chapters = await _database.GetChapters(profile.Level);

            return chapters.ToArray();
        }

        // Authorize User
        [HttpGet("stats")]
        public async Task<ActionResult<ChapterStats>> Stats(Guid profileId, Guid chapterId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var stats = await _database.GetChapterStats(profile.Id, chapterId);

            return stats;
        }

        // Authorize Author
        [HttpPost("create")]
        public async Task<ActionResult<Chapter>> Create([FromBody] Chapter chapter)
        {
            return await _database.CreateChapter(chapter);
        }

        // Authorize Author
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid chapterId)
        {
            await Task.WhenAll(
                _database.DeleteChapter(chapterId),
                _database.DeleteChapterStatsForChapter(chapterId));

            return Ok();
        }
    }
}
