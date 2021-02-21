using System;
using System.Linq;

namespace Bhasha.Common.MongoDB.Dto
{
    public class TranslationDto
    {
        public string Label { get; set; } = "";
        public string[] Categories { get; set; } = new string[0];
        public string Level { get; set; } = "";
        public string? PictureId { get; set; }
        public string TokenType { get; set; } = "";
        public TokenDto[] Tokens { get; set; } = new TokenDto[0];

        public class TokenDto
        {
            public string LanguageId { get; set; } = "";
            public string Native { get; set; } = "";
            public string Spoken { get; set; } = "";
            public string? AudioId { get; set; }
        }

        public Translation ToTranslation(string fromId, string toId)
        {
            var categories = Categories.Select(x => new Category(x)).ToArray();
            var level = Enum.Parse<LanguageLevel>(Level);
            var tokenType = Enum.Parse<TokenType>(TokenType);
            var pictureId = ResourceId.Create(PictureId);
            var token = new Token(Label, categories, level, tokenType, pictureId);

            var fromToken = Tokens.First(x => x.LanguageId == fromId);
            var fromAudioId = ResourceId.Create(fromToken.AudioId);
            var from = new LanguageToken(Language.Parse(fromId), fromToken.Native, fromToken.Spoken, fromAudioId);

            var toToken = Tokens.First(x => x.LanguageId == toId);
            var toAudioId = ResourceId.Create(toToken.AudioId);
            var to = new LanguageToken(Language.Parse(toId), toToken.Native, toToken.Spoken, toAudioId);

            return new Translation(token, from, to);
        }
    }
}
