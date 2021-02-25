using System.Linq;

namespace Bhasha.Common
{
    public enum TokenType
    {
        // multiple words
        Expression,
        Phrase,
        Text,

        // single word
        Noun,
        Pronoun,
        Preposition,
        Conjunction,
        Verb,
        Adjective,
        Adverb
    }

    public static class TokenTypeSupport
    {
        public static TokenType[] Words = new[] {
            TokenType.Noun,
            TokenType.Pronoun,
            TokenType.Preposition,
            TokenType.Conjunction,
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
