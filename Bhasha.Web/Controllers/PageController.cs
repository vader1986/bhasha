using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Extensions;
using Bhasha.Common.Services;
using Bhasha.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Web.Controllers
{
    [ApiController]
    [Route("api/page")]
    public class PageController : BhashaController
    {
        private readonly IDatabase _database;
        private readonly IStore<ChapterStats> _stats;
        private readonly IAuthorizedProfileLookup _profiles;
        private readonly IEvaluateSubmit _evaluator;

        public PageController(IDatabase database, IStore<ChapterStats> stats, IAuthorizedProfileLookup profiles, IEvaluateSubmit evaluator)
        {
            _database = database;
            _stats = stats;
            _profiles = profiles;
            _evaluator = evaluator;
        }

        // Authorize User
        [HttpPost("submit")]
        public async Task<ActionResult<Evaluation>> Submit(Guid profileId, Guid chapterId, int pageIndex, string solution)
        {
            var submit = new Submit(UserId, profileId, chapterId, pageIndex, solution);
            var evaluation = await _evaluator.Evaluate(submit);

            return Ok(evaluation);
        }

        // Authorize User
        [HttpPost("tip")]
        public async Task<ActionResult<Tip>> Tip(Guid profileId, Guid chapterId, int pageIndex)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var tips = await _database.QueryTips(chapterId, pageIndex);

            var stats = await _database.QueryStatsByChapterAndProfileId(profile.Id, chapterId);
            await _stats.Update(stats.WithTip(pageIndex));

            return tips.Random(); 
        }
    }
}
