using System;

namespace Bhasha.Common
{
    public class Translation
    {
        public Token Reference { get; set; }
        public LanguageToken From { get; set; }
        public LanguageToken To { get; set; }

        public Translation(Token reference, LanguageToken from, LanguageToken to)
        {
            if (from.Language == to.Language)
            {
                throw new ArgumentException("Tokens must reference different languages");
            }

            Reference = reference;
            From = from;
            To = to;
        }
    }
}
