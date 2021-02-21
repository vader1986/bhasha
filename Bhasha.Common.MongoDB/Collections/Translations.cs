using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Collections
{
    public class Translations : IQueryable<Translation, TranslationsQuery>, IListable<Category>
    {
        private readonly Database _database;

        public Translations(Database database)
        {
            _database = database;
        }

        public ValueTask<IEnumerable<Category>> List()
        {
            var collection = _database.GetCollection<TranslationDto>(Names.Collections.Translations);
            var categories = collection.AsQueryable().SelectMany(x => x.Categories).Distinct();

            return new ValueTask<IEnumerable<Category>>(categories.Select(x => new Category(x)));
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
            var from = query.From.ToString();
            var to = query.To.ToString();

            var collection = _database.GetCollection<TranslationDto>(Names.Collections.Translations);
            var result = await collection.FindAsync(x => MatchCategory(query, x) && x.TokenType == query.TokenType.ToString());

            return result.ToEnumerable().Select(x => x.ToTranslation(from, to));
        }

        private async ValueTask<IEnumerable<Translation>> ExecuteQuery(TranslationsCategoryQuery query)
        {
            var from = query.From.ToString();
            var to = query.To.ToString();

            var collection = _database.GetCollection<TranslationDto>(Names.Collections.Translations);
            var result = await collection.FindAsync(x => MatchCategory(query, x));

            return result.ToEnumerable().Select(x => x.ToTranslation(from, to));
        }

        private async ValueTask<IEnumerable<Translation>> ExecuteQuery(TranslationsLabelQuery query)
        {
            var from = query.From.ToString();
            var to = query.To.ToString();

            var collection = _database.GetCollection<TranslationDto>(Names.Collections.Translations);
            var result = await collection.FindAsync(x => MatchLanguages(query, x) && x.Label == query.Label);

            return result.ToEnumerable().Select(x => x.ToTranslation(from, to));
        }
    }
}
