using System;
using System.Threading.Tasks;
using Bhasha.Common;
using Bhasha.Common.Services;
using Bhasha.Student.Api.Services;
using LazyCache;
using Microsoft.AspNetCore.Mvc;

namespace Bhasha.Student.Api.Controllers
{
    [ApiController]
    [Route("api/page")]
    public class PageController : BaseController
    {
        private readonly IAppCache _cache;
        private readonly IAuthorizedProfileLookup _profiles;
        private readonly IEvaluateSubmit _evaluator;
        private readonly IProvideTips _tipsProvider;

        public PageController(IAppCache cache, IAuthorizedProfileLookup profiles, IEvaluateSubmit evaluator, IProvideTips tipsProvider)
        {
            _cache = cache;
            _profiles = profiles;
            _evaluator = evaluator;
            _tipsProvider = tipsProvider;
        }

        // Authorize User
        [HttpPost("submit")]
        public async Task<Evaluation> Submit(Guid profileId, Guid chapterId, int pageIndex, string solution)
        {
            var profile = await _profiles.Get(profileId, UserId);
            var submit = new Submit(chapterId, pageIndex, solution);
            var evaluation = await _evaluator.Evaluate(profile, submit);

            if (!profile.Equals(evaluation.Profile))
            {
                var cachedId = profile.Id.ToString();

                _cache.Remove(cachedId);
                _cache.Add(cachedId, profile);
            }

            return evaluation;
        }

        // Authorize User
        [HttpPost("tip")]
        public async Task<string> Tip(Guid profileId, Guid chapterId, int pageIndex)
        {
            return await _tipsProvider.GetTip(await _profiles.Get(profileId, UserId), chapterId, pageIndex);
        }
    }
}
