using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.Queries;

namespace Bhasha.Common.Aggregation
{
    public interface ILoadTranslations
    {
        ValueTask<Translation[]> NextTranslations(UserProgress progress, Category category);
    }

    public class TranslationLoader : ILoadTranslations
    {
        private static readonly IDictionary<TokenType, int> Tokens = new Dictionary<TokenType, int>
        {
            { TokenType.Noun, 4 },
            { TokenType.Verb, 2 },
            { TokenType.Adjective, 2 },
            { TokenType.Adverb, 1 },
            { TokenType.Phrase, 1 },
            { TokenType.Expression, 1 }
        };

        private readonly IQueryable<Translation, TranslationsQuery> _translations;

        public TranslationLoader(IQueryable<Translation, TranslationsQuery> translations)
        {
            _translations = translations;
        }

        public async ValueTask<Translation[]> NextTranslations(UserProgress progress, Category category)
        {
            var translations = new List<Translation>();

            foreach (var token in Tokens)
            {
                var query = new TranslationsTokenTypeQuery(
                    token.Value,
                    progress.From,
                    progress.To,
                    progress.Level,
                    category,
                    token.Key);

                translations.AddRange(await _translations.Query(query));
            }

            return translations.ToArray();
        }
    }
}
