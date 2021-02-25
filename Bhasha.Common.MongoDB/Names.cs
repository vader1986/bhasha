namespace Bhasha.Common.MongoDB
{
    public class Names
    {
        public const string Database = "bhasha";

        public class Collections
        {
            public const string Translations = "translations";
            public const string Procedures = "procedures";
            public const string Users = "users";
        }

        public class Fields
        {
            public static string Categories = nameof(Dto.TranslationDto.Categories);
            public static string LanguageId = nameof(Dto.TranslationDto.Tokens) + "." + nameof(Dto.TokenDto.LanguageId);
        }
    }
}
