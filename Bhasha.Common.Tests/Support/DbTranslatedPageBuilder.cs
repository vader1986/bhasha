using System;
using Bhasha.Common.Database;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class DbTranslatedPageBuilder
    {
        private DbTranslatedExpression _native = DbTranslatedExpressionBuilder.Default.Build();
        private DbTranslatedExpression _target = DbTranslatedExpressionBuilder.Default.Build();
        private PageType _pageType = Rnd.Create.Choose(Enum.GetValues<PageType>());

        public static DbTranslatedPageBuilder Default => new();

        public DbTranslatedPageBuilder WithNative(DbTranslatedExpression native)
        {
            _native = native;
            return this;
        }

        public DbTranslatedPageBuilder WithTarget(DbTranslatedExpression target)
        {
            _target = target;
            return this;
        }

        public DbTranslatedPageBuilder WithPageType(PageType pageType)
        {
            _pageType = pageType;
            return this;
        }

        public DbTranslatedPage Build()
        {
            return new DbTranslatedPage
            {
                Native = _native,
                Target = _target,
                PageType = _pageType
            };
        }
    }
}
