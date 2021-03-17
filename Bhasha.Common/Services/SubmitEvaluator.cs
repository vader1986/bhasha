using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public class SubmitEvaluator : IEvaluateSubmit
    {
        private readonly IEvaluateSolution _evaluator;
        private readonly IDatabase _database;
        private readonly IStore<GenericChapter> _chapters;
        private readonly IStore<ChapterStats> _stats;

        public SubmitEvaluator(IEvaluateSolution evaluator, IDatabase database, IStore<GenericChapter> chapters, IStore<ChapterStats> stats)
        {
            _evaluator = evaluator;
            _database = database;
            _chapters = chapters;
            _stats = stats;
        }

        public async Task<Evaluation> Evaluate(Profile profile, Submit submit)
        {
            var chapter = await _chapters.Get(submit.ChapterId);
            var page = chapter.Pages[submit.PageIndex];

            var expected = await _database.QueryTranslationByTokenId(page.TokenId, profile.To);
            var evaluation = _evaluator.Evaluate(expected.Native, submit.Solution);

            var stats = await _database.QueryStatsByChapterAndProfileId(chapter.Id, profile.Id);

            if (stats == default)
            {
                stats = await _stats.Add(ChapterStats.Empty(profile.Id, submit.ChapterId, chapter.Pages.Length));
            }

            if (evaluation.Result == Result.Correct)
            {
                await _stats
                    .Update(stats
                    .WithSuccess(submit.PageIndex)
                    .WithCompleted());
            }
            else
            {
                await _stats
                    .Update(stats
                    .WithFailure(submit.PageIndex));
            }
            
            return evaluation;
        }
    }
}
