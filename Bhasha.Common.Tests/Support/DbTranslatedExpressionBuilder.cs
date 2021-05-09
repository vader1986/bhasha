using System;
using System.Linq;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class DbTranslatedExpressionBuilder
    {
        private Guid _expressionId = Guid.NewGuid();
        private CEFR _cefr = Rnd.Create.Choose(Enum.GetValues<CEFR>());
        private ExpressionType _exprType = Rnd.Create.Choose(Enum.GetValues<ExpressionType>());
        private DbTranslatedWord[] _words = Enumerable.Range(1, 5).Select(_ => DbTranslatedWordBuilder.Default.Build()).ToArray();

        public static DbTranslatedExpressionBuilder Default => new();

        public DbTranslatedExpressionBuilder WithExpressionId(Guid expressionId)
        {
            _expressionId = expressionId;
            return this;
        }

        public DbTranslatedExpressionBuilder WithCefr(CEFR cefr)
        {
            _cefr = cefr;
            return this;
        }

        public DbTranslatedExpressionBuilder WithExprTpe(ExpressionType exprType)
        {
            _exprType = exprType;
            return this;
        }

        public DbTranslatedExpressionBuilder WithWords(DbTranslatedWord[] words)
        {
            _words = words;
            return this;
        }

        public DbTranslatedExpression Build()
        {
            return new DbTranslatedExpression
            {
                ExpressionId = _expressionId,
                Cefr = _cefr,
                ExprType = _exprType,
                Words = _words
            };
        }
    }
}
