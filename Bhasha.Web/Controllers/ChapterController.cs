using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Database;
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
        private readonly IConvert<DbStats, Stats> _stats;

        public ChapterController(IDatabase database, IChaptersLookup chapters, IAuthorizedProfileLookup profiles, IConvert<DbStats, Stats> stats)
        {
            _database = database;
            _chapters = chapters;
            _profiles = profiles;
            _stats = stats;
        }

        // Authorize User
        [HttpGet("list")]
        public async Task<ChapterEnvelope[]> List(Guid profileId, int level = int.MaxValue)
        {
            return await _chapters.GetChapters(await _profiles.Get(profileId, UserId), level);
        }

        // Authorize User
        [HttpGet("stats")]
        public async Task<Stats> Stats(Guid profileId, Guid chapterId)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var stats = await _database.QueryStats(chapterId, profile.Id);

            stats.Validate();

            return _stats.Convert(stats);
        }
    }
}
