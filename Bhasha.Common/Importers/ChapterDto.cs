namespace Bhasha.Common.Importers
{
    public class ChapterDto
    {
        public string From { get; set; } = Language.English;
        public string To { get; set; } = Language.Bengali;
        public int Level { get; set; }

        public ExpressionDto Name { get; set; }
        public ExpressionDto Description { get; set; }

        public PageDto[] Pages { get; set; }

        public class ExpressionDto
        {
            public TokenDto Token { get; set; }
            public TranslationDto Native { get; set; }
        }

        public class PageDto
        {
            public TokenDto Token { get; set; }
            public TranslationDto From { get; set; }
            public TranslationDto To { get; set; }
            public ExpressionDto[] Tips { get; set; }
        }

        public class TranslationDto
        {
            public string Native { get; set; }
            public string Spoken { get; set; }
        }

        public class TokenDto
        {
            public string Label { get; set; }
            public int? Level { get; set; }
            public string Cefr { get; set; } = "A1";
            public string TokenType { get; set; } = "Noun";
            public string[] Categories { get; set; } = new string[0];
        }
    }
}
