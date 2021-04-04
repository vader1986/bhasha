using System.Threading.Tasks;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Services
{
    public interface IEvaluateSubmit
    {
        Task<Evaluation> Evaluate(Profile profile, Submit submit);
    }

    public class SubmitEvaluator : IEvaluateSubmit
    {
        private readonly ICheckResult _checker;
        private readonly IDatabase _database;
        private readonly IUpdateStatsForEvaluation _updateStats;
        private readonly IStore<GenericChapter> _chapters;
        
        public SubmitEvaluator(ICheckResult checker, IUpdateStatsForEvaluation updateStats, IDatabase database, IStore<GenericChapter> chapters)
        {
            _checker = checker;
            _updateStats = updateStats;
            _database = database;
            _chapters = chapters;
        }

        public async Task<Evaluation> Evaluate(Profile profile, Submit submit)
        {
            var chapter = await _chapters.Get(submit.ChapterId);

            if (chapter == null)
            {
                throw new ObjectNotFoundException(typeof(GenericChapter), submit.ChapterId);
            }

            var page = chapter.Pages[submit.PageIndex];

            var expected = await _database.QueryTranslationByTokenId(page.TokenId, profile.To)
                ?? throw new ObjectNotFoundException(typeof(Translation), page.TokenId);

            var result = _checker.Evaluate(expected.Native, submit.Solution);
            var updatedProfile = await _updateStats.UpdateStats(result, submit.PageIndex, profile, chapter);

            return new Evaluation(result, submit, updatedProfile);
        }
    }
}
