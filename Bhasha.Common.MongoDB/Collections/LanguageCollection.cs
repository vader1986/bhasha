using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB.Collections
{
    public class LanguageCollection : IListable<Language>
    {
        private readonly IMongoDb _database;

        public LanguageCollection(IMongoDb database)
        {
            _database = database;
        }

        public async ValueTask<IEnumerable<Language>> List()
        {
            var languages = await _database.ListMany<TranslationDto, string>(
                Names.Collections.Translations,
                Names.Fields.LanguageId);

            return languages.Select(Language.Parse);
        }
    }
}
