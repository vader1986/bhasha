using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Collections
{
    public class Translations : IQueryable<Translation, TranslationsQuery>
    {
        private readonly IDatabase _database;

        public Translations(IDatabase database)
        {
            _database = database;
        }

        public ValueTask<IEnumerable<Translation>> Query(TranslationsQuery query)
        {
            return query switch
            {
                TranslationsTokenTypeQuery ttq => ExecuteQuery(ttq),
                TranslationsCategoryQuery cq => ExecuteQuery(cq),
                TranslationsLabelQuery lq => ExecuteQuery(lq),
                _ => OnDefault(query)
            };
        }

        private static ValueTask<IEnumerable<Translation>> OnDefault(TranslationsQuery query)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query),
                $"Query type {query.GetType().FullName} not supported");
        }

        private static bool MatchLanguages(TranslationsQuery query, TranslationDto dto)
        {
            return dto.Tokens.Any(t => t.LanguageId == query.To.ToString()) &&
                   dto.Tokens.Any(t => t.LanguageId == query.From.ToString());
        }

        private static bool MatchCategory(TranslationsCategoryQuery query, TranslationDto dto)
        {
            return MatchLanguages(query, dto) &&
                   dto.Level == query.Level.ToString() &&
                   dto.Categories.Contains(query.Category.Id);
        }

        private async ValueTask<IEnumerable<Translation>> ExecuteQuery(TranslationsTokenTypeQuery query)
        {
            var result = await _database.Find<TranslationDto>(Names.Collections.Translations, x => MatchCategory(query, x) && x.TokenType == query.TokenType.ToString());
            return result.Select(x => x.ToTranslation(query.From, query.To));
        }

        private async ValueTask<IEnumerable<Translation>> ExecuteQuery(TranslationsCategoryQuery query)
        {
            var result = await _database.Find<TranslationDto>(Names.Collections.Translations, x => MatchCategory(query, x));
            return result.Select(x => x.ToTranslation(query.From, query.To));
        }

        private async ValueTask<IEnumerable<Translation>> ExecuteQuery(TranslationsLabelQuery query)
        {
            var result = await _database.Find<TranslationDto>(Names.Collections.Translations, x => MatchLanguages(query, x) && x.Label == query.Label);
            return result.Select(x => x.ToTranslation(query.From, query.To));
        }
    }
}
