using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.Database;

namespace Bhasha.Common.Services
{
    public class TranslateExpression : ITranslate<Guid, TranslatedExpression>
    {
        private readonly IStore<DbExpression> _expressions;
        private readonly ITranslate<Guid, TranslatedWord> _words;
        private readonly IConvert<IEnumerable<string>, string> _wordsToPhrase;

        public TranslateExpression(IStore<DbExpression> expressions, ITranslate<Guid, TranslatedWord> words, IConvert<IEnumerable<string>, string> wordsToPhrase)
        {
            _expressions = expressions;
            _words = words;
            _wordsToPhrase = wordsToPhrase;
        }

        public async Task<TranslatedExpression?> Translate(Guid expressionId, Language language)
        {
            var expression = await _expressions.Get(expressionId);
            if (expression == null)
            {
                return default;
            }

            var translations = expression.Translations;
            if (translations == null || !translations.ContainsKey(language))
            {
                return default;
            }

            var wordIds = translations[language];
            var words = await Task.WhenAll(wordIds.Select(id => _words.Translate(id, language)));

            if (words == null || words.Any(x => x == null))
            {
                return default;
            }

            var native = _wordsToPhrase.Convert(words.Select(word => word!.Native));
            var spoken = _wordsToPhrase.Convert(words.Select(word => word!.Spoken));
            var expr = new Expression(expression.Id, expression.ExprType, expression.Cefr);

            return new TranslatedExpression(expr, words!, native, spoken);
        }
    }
}
