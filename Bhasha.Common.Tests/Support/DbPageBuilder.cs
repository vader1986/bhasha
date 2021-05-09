using System;
using Bhasha.Common.Database;

namespace Bhasha.Common.Tests.Support
{
    public class DbPageBuilder
    {
        private Guid _expressionId = Guid.NewGuid();
        private PageType _pageType = PageType.OneOutOfFour;

        public DbPageBuilder WithExpressionId(Guid expressionId)
        {
            _expressionId = expressionId;
            return this;
        }

        public static DbPageBuilder Default => new();

        public DbPage Build()
        {
            return new DbPage {
                ExpressionId = _expressionId,
                PageType = _pageType
            };
        }
    }
}
