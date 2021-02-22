using System.Linq;

namespace Bhasha.Common
{
    public enum TokenType
    {
        Phrase,
        Expression,

        // part of speech (single words)
        Noun,
        Verb,
        Adjective,
        Adverb
    }

    public static class TokenTypeSupport
    {
        public static TokenType[] Words = new[] {
            TokenType.Noun,
            TokenType.Verb,
            TokenType.Adjective,
            TokenType.Adverb
        };

        public static bool IsWord(this TokenType tokenType)
        {
            return Words.Contains(tokenType);
        }
    }
}
