using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class ExpressionBuilder
    {
        private Guid _id = Guid.NewGuid();
        private ExpressionType _exprType = Rnd.Create.Choose(Enum.GetValues<ExpressionType>());
        private CEFR _cefr = Rnd.Create.Choose(Enum.GetValues<CEFR>());

        public static ExpressionBuilder Default => new();

        public ExpressionBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ExpressionBuilder WithExprType(ExpressionType exprType)
        {
            _exprType = exprType;
            return this;
        }

        public ExpressionBuilder WithCefr(CEFR cefr)
        {
            _cefr = cefr;
            return this;
        }

        public Expression Build()
        {
            return new Expression(_id, _exprType, _cefr);
        }
    }
}
