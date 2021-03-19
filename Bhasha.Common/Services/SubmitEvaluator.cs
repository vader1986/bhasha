using System.Threading.Tasks;

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
        private readonly IUpdateStats _updateStats;
        private readonly IStore<GenericChapter> _chapters;
        
        public SubmitEvaluator(ICheckResult checker, IUpdateStats updateStats, IDatabase database, IStore<GenericChapter> chapters)
        {
            _checker = checker;
            _updateStats = updateStats;
            _database = database;
            _chapters = chapters;
        }

        public async Task<Evaluation> Evaluate(Profile profile, Submit submit)
        {
            var chapter = await _chapters.Get(submit.ChapterId);
            var page = chapter.Pages[submit.PageIndex];

            var expected = await _database.QueryTranslationByTokenId(page.TokenId, profile.To);
            var result = _checker.Evaluate(expected.Native, submit.Solution);

            var evaluation = new Evaluation(result, submit);

            await _updateStats.FromEvaluation(evaluation, profile, chapter);

            return evaluation;
        }
    }
}
