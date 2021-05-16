using System;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Exceptions;

namespace Bhasha.Common.Services
{
    public class EvaluateSubmit : IEvaluateSubmit
    {
        private readonly ICheckResult _checker;
        private readonly ITranslate<Guid, TranslatedExpression> _translator;
        private readonly IUpdateStatsOnSubmit _stats;
        private readonly IStore<DbChapter> _chapters;
        
        public EvaluateSubmit(ICheckResult checker, IUpdateStatsOnSubmit stats, ITranslate<Guid, TranslatedExpression> translator, IStore<DbChapter> chapters)
        {
            _checker = checker;
            _stats = stats;
            _translator = translator;
            _chapters = chapters;
        }

        public async Task<Evaluation> Evaluate(Profile profile, Submit submit)
        {
            var chapter = await _chapters.Get(submit.ChapterId)
                ?? throw new ObjectNotFoundException(typeof(DbChapter), submit.ChapterId);

            chapter.Validate();

            if (submit.PageIndex < 0 || submit.PageIndex >= chapter.Pages!.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(submit.PageIndex));
            }

            var page = chapter.Pages[submit.PageIndex];

            var expected = await _translator.Translate(page.ExpressionId, profile.Target)
                ?? throw new ObjectNotFoundException(typeof(TranslatedExpression), page.ExpressionId);

            var result = _checker.Evaluate(expected.Native, submit.Solution);
            var evaluation = new Evaluation(result, submit, profile);

            return await _stats.Update(evaluation, chapter.Pages.Length);
        }
    }
}
