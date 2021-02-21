using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bhasha.Common.Queries;

namespace Bhasha.Common.MongoDB.Collections
{
    public class Translations : IQueryable<Translation, TranslationsQuery>
    {
        private readonly Database _database; // TODO

        public Translations(Database database)
        {
            _database = database;
        }

        public Task<IEnumerable<Translation>> Query(TranslationsQuery query)
        {
            switch (query)
            {
                case TranslationsTokenTypeQuery ttq:
                    return ExecuteQuery(ttq);

                case TranslationsCategoryQuery cq:
                    return ExecuteQuery(cq);

                case TranslationsLabelQuery lq:
                    return ExecuteQuery(lq);

                default:
                    throw new ArgumentOutOfRangeException(nameof(query),
                        $"Query type {query.GetType().FullName} not supported");
            }
        }

        private async Task<IEnumerable<Translation>> ExecuteQuery(TranslationsTokenTypeQuery query)
        {
            throw new Exception();
        }

        private async Task<IEnumerable<Translation>> ExecuteQuery(TranslationsCategoryQuery query)
        {
            throw new Exception();
        }

        private async Task<IEnumerable<Translation>> ExecuteQuery(TranslationsLabelQuery query)
        {
            throw new Exception();
        }
    }
}
