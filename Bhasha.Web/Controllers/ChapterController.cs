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
    [Route("api/chapter")]
    public class ChapterController : BhashaController
    {
        private readonly IDatabase _database;
        private readonly IAssembleChapters _chapters;
        private readonly IAuthorizedProfileLookup _profiles;

        public ChapterController(IDatabase database, IAssembleChapters chapters, IAuthorizedProfileLookup profiles)
        {
            _database = database;
            _chapters = chapters;
            _profiles = profiles;
        }

        // Authorize User
        [HttpGet("list")]
        public async Task<GenericChapter[]> List(Guid profileId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var chapters = await _database.QueryChaptersByLevel(profile.Level);

            return chapters.ToArray();
        }

        // Authorize User
        [HttpGet("stats")]
        public async Task<ChapterStats> Stats(Guid profileId, Guid chapterId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var stats = await _database.QueryStatsByChapterAndProfileId(chapterId, profile.Id);

            return stats;
        }

        // Authorize Author
        [HttpPost("get")]
        public async Task<Chapter> Get(Guid profileId, Guid chapterId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var chapter = await _chapters.Assemble(chapterId, profile);

            return chapter;
        }
    }
}
