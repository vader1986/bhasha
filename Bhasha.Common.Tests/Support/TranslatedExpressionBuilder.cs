using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class TranslatedExpressionBuilder
    {
        private Expression _expression = ExpressionBuilder.Default.Build();
        private TranslatedWord[] _words = Enumerable.Range(1, 5).Select(_ => TranslatedWordBuilder.Default.Build()).ToArray();
        private string _native = Rnd.Create.NextString();
        private string _spoken = Rnd.Create.NextString();

        public static TranslatedExpressionBuilder Default => new();

        public TranslatedExpressionBuilder WithExpression(Expression expression)
        {
            _expression = expression;
            return this;
        }

        public TranslatedExpressionBuilder WithWords(TranslatedWord[] words)
        {
            _words = words;
            return this;
        }

        public TranslatedExpressionBuilder WithNative(string native)
        {
            _native = native;
            return this;
        }

        public TranslatedExpressionBuilder WithSpoken(string spoken)
        {
            _spoken = spoken;
            return this;
        }

        public TranslatedExpression Build()
        {
            return new TranslatedExpression(_expression, _words, _native, _spoken);
        }
    }
}
