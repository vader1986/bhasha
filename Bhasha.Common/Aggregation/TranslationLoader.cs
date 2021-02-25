using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Extensions;
using Bhasha.Common.Queries;

namespace Bhasha.Common.Aggregation
{
    public interface ILoadTranslations
    {
        ValueTask<Translation[]> NextTranslations(UserProgress progress);
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

        private readonly IQueryable<Translation, TranslationQuery> _translations;

        public TranslationLoader(IQueryable<Translation, TranslationQuery> translations)
        {
            _translations = translations;
        }

        public async ValueTask<Translation[]> NextTranslations(UserProgress progress)
        {
            var translations = new List<Translation>();
            var missing = 0;

            foreach (var token in Tokens)
            {
                var translationsFromDb = await _translations.Query(new TranslationQueryByGroupId(
                    progress.From,
                    progress.To,
                    progress.Stats.GroupId,
                    token.Key));

                var results = translationsFromDb
                    .Where(tr => !progress
                        .Stats
                        .CompletedSequenceNumbers
                        .Contains(tr.Reference.Id.SequenceNumber)).ToList();

                for (int i = 0; i < token.Value; i++)
                {
                    if (results.Any())
                    {
                        var translation = results.Random();
                        results.Remove(translation);
                        translations.Add(translation);
                    }
                    else
                    {
                        break;
                    }
                }

                if (results.Any())
                {
                    for (int i = 0; i < Math.Min(missing, results.Count); i++)
                    {
                        translations.Add(results[i]);
                        missing--;
                    }
                }
                else
                {
                    missing += Math.Max(0, token.Value - results.Count);
                }
            }

            return translations.ToArray();
        }
    }
}
