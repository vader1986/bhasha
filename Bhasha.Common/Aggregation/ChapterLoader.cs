using System.Threading.Tasks;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Aggregation
{
    public interface ILoadChapter
    {
        ValueTask<Chapter?> NextChapter(UserProgress progress);
    }

    public class ChapterLoader : ILoadChapter
    {
        private readonly ILoadTranslations _translations;
        private readonly ILoadProcedures _procedures;

        public ChapterLoader(ILoadTranslations translations, ILoadProcedures procedures)
        {
            _translations = translations;
            _procedures = procedures;
        }

        public async ValueTask<Chapter?> NextChapter(UserProgress progress)
        {
            var translations = await _translations.NextTranslations(progress);
            if (translations == default || translations.IsEmpty())
            {
                return default;
            }

            var procedures = await _procedures.NextProcedures(translations);

            return new Chapter(translations, procedures);
        }
    }
}
