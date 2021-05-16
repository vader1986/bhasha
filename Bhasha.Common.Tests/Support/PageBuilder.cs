using System;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Tests.Support
{
    public class PageBuilder
    {
        private PageType _pageType = Rnd.Create.Choose(Enum.GetValues<PageType>());
        private TranslatedExpression _translation = TranslatedExpressionBuilder.Default.Build();
        private object _arguments = new();

        public static PageBuilder Default => new();

        public PageBuilder WithPageType(PageType pageType)
        {
            _pageType = pageType;
            return this;
        }

        public PageBuilder WithTranslation(TranslatedExpression translation)
        {
            _translation = translation;
            return this;
        }

        public PageBuilder WithArguments(object arguments)
        {
            _arguments = arguments;
            return this;
        }

        public Page Build()
        {
            return new Page(_pageType, _translation, _arguments);
        }
    }
}
