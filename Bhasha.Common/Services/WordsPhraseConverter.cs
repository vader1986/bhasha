using System.Collections.Generic;
using System.Linq;

namespace Bhasha.Common.Services
{
    public class WordsPhraseConverter : IConvert<IEnumerable<string>, string>
    {
        private static readonly ISet<char> Signs = new HashSet<char> {
            '.', '?', '!', ',', '-', ';'
        };

        private static string Enclose(string word)
        {
            return word.Length == 1 && Signs.Contains(word[0]) ? $"{word} " : word;
        }

        public string Convert(IEnumerable<string> words)
        {
            return string.Join(" ", words.Select(Enclose));
        }
    }
}
