using System;
using System.Collections.Generic;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class DbExpressionBuilder
    {
        private Guid _id = Guid.NewGuid();
        private CEFR _cefr = Rnd.Create.Choose(Enum.GetValues<CEFR>());
        private ExpressionType _exprType = Rnd.Create.Choose(Enum.GetValues<ExpressionType>());
        private Dictionary<string, Guid[]> _translations = new();

        public static DbExpressionBuilder Default => new();

        public DbExpressionBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public DbExpressionBuilder WithCefr(CEFR cefr)
        {
            _cefr = cefr;
            return this;
        }

        public DbExpressionBuilder WithExprType(ExpressionType exprType)
        {
            _exprType = exprType;
            return this;
        }

        public DbExpressionBuilder WithTranslations(Dictionary<string, Guid[]> translations)
        {
            _translations = translations;
            return this;
        }

        public DbExpression Build()
        {
            return new DbExpression {
                Id = _id,
                Cefr = _cefr,
                ExprType = _exprType,
                Translations = _translations
            };
        }
    }
}
