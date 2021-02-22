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
        private readonly ILoadCategory _category;
        private readonly ILoadTranslations _translations;
        private readonly ILoadProcedures _procedures;

        public ChapterLoader(ILoadCategory category, ILoadTranslations translations, ILoadProcedures procedures)
        {
            _category = category;
            _translations = translations;
            _procedures = procedures;
        }

        public async ValueTask<Chapter?> NextChapter(UserProgress progress)
        {
            var category = await _category.NextCategory(progress);
            if (category == default)
            {
                return default;
            }

            var translations = await _translations.NextTranslations(progress, category);
            if (translations == default || translations.IsEmpty())
            {
                return default;
            }

            var procedures = await _procedures.NextProcedures(translations);

            return new Chapter(category, translations, procedures);
        }
    }
}
