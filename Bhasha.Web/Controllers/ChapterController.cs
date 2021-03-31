using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/chapter")]
    public class ChapterController : BaseController
    {
        private readonly IDatabase _database;
        private readonly IChaptersLookup _chapters;
        private readonly IAuthorizedProfileLookup _profiles;

        public ChapterController(IDatabase database, IChaptersLookup chapters, IAuthorizedProfileLookup profiles)
        {
            _database = database;
            _chapters = chapters;
            _profiles = profiles;
        }

        // Authorize User
        [HttpGet("list")]
        public async Task<Chapter[]> List(Guid profileId, int level = int.MaxValue)
        {
            var profile = await _profiles.Get(profileId, UserId);
            return await _chapters.GetChapters(profile, level);
        }

        // Authorize User
        [HttpGet("stats")]
        public async Task<ChapterStats> Stats(Guid profileId, Guid chapterId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var stats = await _database.QueryStatsByChapterAndProfileId(chapterId, profile.Id);

            return stats;
        }
    }
}
