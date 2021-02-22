namespace Bhasha.Common.Tests.Support
{
    public class TranslationBuilder
    {
        private Token _reference = TokenBuilder.Create();
        private LanguageToken _from = LanguageTokenBuilder.Create();
        private LanguageToken _to = LanguageTokenBuilder.Default.WithLanguage(Languages.Bengoli).Build();

        public static TranslationBuilder Default => new TranslationBuilder();
        public static Translation Create() => Default.Build();

        public TranslationBuilder WithReference(Token reference)
        {
            _reference = reference;
            return this;
        }

        public TranslationBuilder WithFrom(LanguageToken from)
        {
            _from = from;
            return this;
        }

        public TranslationBuilder WithTo(LanguageToken to)
        {
            _to = to;
            return this;
        }

        public Translation Build()
        {
            return new Translation(_reference, _from, _to);
        }
    }
}
