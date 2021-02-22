using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public static class Setup
    {
        private static async Task CreateIndices<T>(this IMongoCollection<T> collection, params Func<T, object>[] selectors)
        {
            await collection.Indexes
                .CreateManyAsync(selectors
                    .Select(selector => new CreateIndexModel<T>(
                        Builders<T>.IndexKeys.Ascending(x => selector(x)))));
        }

        public static async Task<IMongoDatabase> NewDatabase(MongoClient client)
        {
            var db = client.GetDatabase(Names.Database);

            await db.CreateCollectionAsync(Names.Collections.Translations);

            var translations = db.GetCollection<TranslationDto>(Names.Collections.Translations);

            await translations.CreateIndices(
                x => x.Categories,
                x => x.Tokens.Select(t => t.LanguageId).ToArray(),
                x => x.Level,
                x => x.Label,
                x => x.TokenType);

            await db.CreateCollectionAsync(Names.Collections.Procedures);

            var procedures = db.GetCollection<ProcedureDto>(Names.Collections.Procedures);

            await procedures.CreateIndices(
                x => x.ProcedureId,
                x => x.Support);

            return db;
        }
    }
}
