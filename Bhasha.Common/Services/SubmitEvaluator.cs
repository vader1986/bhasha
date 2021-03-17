﻿using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public class SubmitEvaluator : IEvaluateSubmit
    {
        private readonly IEvaluateSolution _evaluator;
        private readonly IDatabase _database;
        private readonly IStore<GenericChapter> _chapters;
        private readonly IStore<Profile> _profiles;
        private readonly IStore<ChapterStats> _stats;

        public SubmitEvaluator(IEvaluateSolution evaluator, IDatabase database, IStore<GenericChapter> chapters, IStore<Profile>  profiles, IStore<ChapterStats> stats)
        {
            _evaluator = evaluator;
            _database = database;
            _chapters = chapters;
            _profiles = profiles;
            _stats = stats;
        }

        public async Task<Evaluation> Evaluate(Submit submit)
        {
            var chapter = await _chapters.Get(submit.ChapterId);
            var page = chapter.Pages[submit.PageIndex];
            var profile = await _profiles.Get(submit.ProfileId);

            var expected = await _database.QueryTranslationByTokenId(page.TokenId, profile.To);
            var evaluation = _evaluator.Evaluate(expected.Native, submit.Solution);

            var stats = await _database.QueryStatsByChapterAndProfileId(chapter.Id, profile.Id);

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
