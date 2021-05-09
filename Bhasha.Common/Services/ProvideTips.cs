using System;
using System.Threading.Tasks;
using Bhasha.Common.Database;
using Bhasha.Common.Exceptions;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public interface IProvideTips
    {
        Task<string> GetTip(Profile profile, Guid chapterId, int pageIndex);
    }

    public class ProvideTips : IProvideTips
    {
        private readonly IStore<DbChapter> _chapters;
        private readonly ITranslate<Guid, TranslatedExpression> _expressions;
        private readonly ITranslate<Guid, TranslatedWord> _words;
        private readonly IUpdateStatsOnTipRequest _tipStatsUpdater;

        public ProvideTips(IStore<DbChapter> chapters, ITranslate<Guid, TranslatedExpression> expressions, ITranslate<Guid, TranslatedWord> words, IUpdateStatsOnTipRequest tipStatsUpdater)
        {
            _chapters = chapters;
            _expressions = expressions;
            _words = words;
            _tipStatsUpdater = tipStatsUpdater;
        }

        public async Task<string> GetTip(Profile profile, Guid chapterId, int pageIndex)
        {
            var chapter = await _chapters.Get(chapterId);

            if (chapter == null)
            {
                throw new ObjectNotFoundException(typeof(DbChapter), chapterId);
            }

            chapter.Validate();

            var expressionId = chapter.Pages![pageIndex].ExpressionId;
            var expression = await _expressions.Translate(expressionId, profile.Native);

            if (expression == null)
            {
                throw new ObjectNotFoundException(typeof(DbTranslatedExpression), expressionId);
            }

            var native = expression.Words.Random();
            var wordId = native.Word.Id;
            var target = await _words.Translate(wordId, profile.Target);

            if (target == null)
            {
                throw new ObjectNotFoundException(typeof(DbTranslatedExpression), wordId);
            }

            var tip = $"{native.Native} = {target.Native} [{target.Spoken}]";
            await _tipStatsUpdater.Update(profile, chapterId, pageIndex);

            return tip;
        }
    }
}
