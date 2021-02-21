using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB.Collections
{
    public class Categories : IListable<Category>
    {
        private readonly IDatabase _database;

        public Categories(IDatabase database)
        {
            _database = database;
        }

        public async ValueTask<IEnumerable<Category>> List()
        {
            var categories = await _database.ListMany<TranslationDto, string>(
                Names.Collections.Translations,
                x => x.Categories);

            return categories.Select(x => new Category(x));
        }
    }
}
