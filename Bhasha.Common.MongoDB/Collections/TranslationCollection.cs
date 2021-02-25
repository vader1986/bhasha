using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Collections
{
    public class TranslationCollection : IQueryable<Translation, TranslationQuery>
    {
        private readonly IMongoDb _database;

        public TranslationCollection(IMongoDb database)
        {
            _database = database;
        }

        public ValueTask<IEnumerable<Translation>> Query(TranslationQuery query)
        {
            return query switch
            {
                TranslationQueryByGroupId byGroupId => ExecuteQuery(byGroupId),
                _ => OnDefault(query)
            };
        }

        private static ValueTask<IEnumerable<Translation>> OnDefault(TranslationQuery query)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query),
                $"Query type {query.GetType().FullName} not supported");
        }

        private static bool MatchLanguages(TranslationQuery query, TranslationDto dto)
        {
            return dto.Tokens.Any(t => t.LanguageId == query.To.ToString()) &&
                   dto.Tokens.Any(t => t.LanguageId == query.From.ToString());
        }

        private static bool MatchGroupAndType(TranslationQueryByGroupId query, TranslationDto dto)
        {
            return MatchLanguages(query, dto) &&
                dto.GroupId == query.GroupId &&
                dto.TokenType == query.TokenType.ToString();
        }

        private async ValueTask<IEnumerable<Translation>> ExecuteQuery(TranslationQueryByGroupId query)
        {
            var result = await _database.Find<TranslationDto>(Names.Collections.Translations, x => MatchGroupAndType(query, x));
            return result.Select(x => Converter.Convert(x, query.From, query.To));
        }
    }
}
