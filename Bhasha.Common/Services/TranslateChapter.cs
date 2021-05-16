using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Arguments;
using Bhasha.Common.Database;

namespace Bhasha.Common.Services
{
    public class TranslateChapter : ITranslate<DbChapter, Chapter, Profile>
    {
        private readonly ITranslate<Guid, TranslatedExpression> _translator;
        private readonly IArgumentAssemblyProvider _arguments;

        public TranslateChapter(ITranslate<Guid, TranslatedExpression> translator, IArgumentAssemblyProvider arguments)
        {
            _translator = translator;
            _arguments = arguments;
        }

        private async Task<IDictionary<Guid, TranslatedExpression>?> GetTranslations(Language language, IEnumerable<Guid> expressionIds)
        {
            var translations = await Task.WhenAll(
                expressionIds
                    .Distinct()
                    .Select(id => _translator.Translate(id, language)));

            if (translations.Any(x => x == null))
            {
                return default;
            }

            return translations.ToDictionary(x => x!.Expression.Id, x => x)!;
        }

        public async Task<Chapter?> Translate(DbChapter chapter, Profile profile)
        {
            var expressionIds = chapter
                .Pages
                .Select(x => x.ExpressionId);

            var taskFrom = GetTranslations(
                profile.Native,
                expressionIds
                    .Append(chapter.NameId)
                    .Append(chapter.DescriptionId));

            var taskTo = GetTranslations(
                profile.Target,
                expressionIds);

            var translations = await Task.WhenAll(taskFrom, taskTo);
            if (translations.Any(x => x == null))
            {
                return default;
            }

            Page PageFor(DbPage dbPage, int index)
            {
                var translation = translations[0]![dbPage.ExpressionId];

                var arguments = _arguments
                    .GetAssembly(dbPage.PageType)
                    .Assemble(translations[1]!.Values, dbPage.ExpressionId);

                return new Page(dbPage.PageType, translation, arguments);
            }

            return new Chapter(
                chapter.Id,
                chapter.Level,
                translations[0]![chapter.NameId],
                translations[0]![chapter.DescriptionId],
                chapter.Pages.Select(PageFor).ToArray(),
                chapter.PictureId);
        }
    }
}
