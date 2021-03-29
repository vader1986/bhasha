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
    public class ChapterController : BaseController
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
        public async Task<Chapter[]> List(Guid profileId, int level = int.MaxValue)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var chapters = await _database.QueryChaptersByLevel(Math.Min(profile.Level, level));

            var stats =
                await Task.WhenAll(chapters.Select(chapter =>
                    _database.QueryStatsByChapterAndProfileId(chapter.Id, profile.Id)));

            var completed = stats.Where(x => x != null).ToDictionary(x => x.ChapterId, x => x.Completed);
            var uncompletedChapters = chapters.Where(x => !(completed.ContainsKey(x.Id) && completed[x.Id]));
            var result = await Task.WhenAll(uncompletedChapters.Select(async x => await _chapters.Assemble(x, profile)));

            return result.Where(x => x != null).ToArray();
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
