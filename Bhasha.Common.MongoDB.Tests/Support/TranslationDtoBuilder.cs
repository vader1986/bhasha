using System.Linq;
using Bhasha.Common.MongoDB.Dto;

namespace Bhasha.Common.MongoDB.Tests.Support
{
    public class TranslationDtoBuilder
    {
        private string _label = "cat";
        private string _level = LanguageLevel.A1.ToString();
        private string _tokenType = TokenType.Noun.ToString();
        private string[] _categories = new[] { "pet", "animal" };
        private string _pictureId;

        private TokenDto[] _tokens = new []
        {
            TokenDtoBuilder.Create(),
            TokenDtoBuilder
                .Default
                .WithLanguageId(Languages.Bengoli)
                .WithNative("?")
                .WithSpoken("???")
                .Build()
        };

        public static TranslationDtoBuilder Default => new TranslationDtoBuilder();
        public static TranslationDto Create() => Default.Build();

        public TranslationDtoBuilder WithLabel(string label)
        {
            _label = label;
            return this;
        }

        public TranslationDtoBuilder WithLevel(string level)
        {
            _level = level;
            return this;
        }

        public TranslationDtoBuilder WithTokenType(string tokenType)
        {
            _tokenType = tokenType;
            return this;
        }

        public TranslationDtoBuilder WithCategories(params string[] categories)
        {
            _categories = categories;
            return this;
        }

        public TranslationDtoBuilder WithPictureId(string pictureId)
        {
            _pictureId = pictureId;
            return this;
        }

        public TranslationDtoBuilder WithTokens(params TokenDto[] tokens)
        {
            _tokens = tokens;
            return this;
        }

        public TranslationDto Build()
        {
            return new TranslationDto {
                Label = _label,
                Level = _level,
                TokenType = _tokenType,
                Categories = _categories,
                Tokens = _tokens.ToArray(),
                PictureId = _pictureId
            };
        }
    }
}
