#nullable enable
namespace Bhasha.Common.Storage
{
    public class QueryParams
    {
        public string? Label { get; }
        public string[]? Categories { get; }
        public LanguageLevel? Level { get; }
        public TokenType? TokenType { get; }

        public QueryParams(
            string? label = default,
            string[]? categories = default,
            LanguageLevel? level = default,
            TokenType? tokenType = default)
        {
            Label = label;
            Categories = categories;
            Level = level;
            TokenType = tokenType;
        }
    }
}
