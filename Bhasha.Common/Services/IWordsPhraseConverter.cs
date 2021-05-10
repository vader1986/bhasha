using System.Collections.Generic;

namespace Bhasha.Common.Services
{
    public interface IWordsPhraseConverter : IConvert<IEnumerable<string>, string, Language>
    {
    }
}
