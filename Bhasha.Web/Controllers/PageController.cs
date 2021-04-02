using System;
using System.Linq;
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
    public class PageController : BaseController
    {
        private readonly IDatabase _database;
        private readonly IAuthorizedProfileLookup _profiles;
        private readonly IEvaluateSubmit _evaluator;
        private readonly IUpdateStatsForTip _tipStatsUpdater;

        public PageController(IDatabase database, IAuthorizedProfileLookup profiles, IEvaluateSubmit evaluator, IUpdateStatsForTip tipStatsUpdater)
        {
            _database = database;
            _profiles = profiles;
            _evaluator = evaluator;
            _tipStatsUpdater = tipStatsUpdater;
        }

        // Authorize User
        [HttpPost("submit")]
        public async Task<Evaluation> Submit(Guid profileId, Guid chapterId, int pageIndex, string solution)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var submit = new Submit(chapterId, pageIndex, solution);
            var evaluation = await _evaluator.Evaluate(profile, submit);

            return evaluation;
        }

        // Authorize User
        [HttpPost("tip")]
        public async Task<string> Tip(Guid profileId, Guid chapterId, [FromBody] Guid[] tipIds)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var tips = await Task.WhenAll(tipIds.Select(x => _database.QueryTranslationByTokenId(x, profile.To)));
            var tip = tips.Random();
            var translation = await _database.QueryTranslationByTokenId(tip.TokenId, profile.From);

            await _tipStatsUpdater.UpdateStats(chapterId, profile);

            return $"{tip.Native} ({tip.Spoken}) = {translation.Native}";
        }
    }
}
