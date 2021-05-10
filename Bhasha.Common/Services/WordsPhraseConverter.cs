using System.Collections.Generic;
using System.Linq;
using Bhasha.Common.Extensions;

namespace Bhasha.Common.Services
{
    public class WordsPhraseConverter : IWordsPhraseConverter
    {
        public string Convert(IEnumerable<string> words, Language language)
        {
            return words.Aggregate((x, y) => {

                if (y.StartsWithSign())
                {
                    return x + y;
                }

                if (x.EndsWithSign())
                {
                    return x + " " + y.ToUpperFirstLetter();
                }

                return x + " " + y;
            });
        }
    }
}
