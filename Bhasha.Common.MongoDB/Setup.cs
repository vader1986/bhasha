using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public static class Setup
    {
        private static async Task CreateIndices<T>(this IMongoCollection<T> collection, params Expression<Func<T, object>>[] selectors)
        {
            var indices = selectors
                .Select(Builders<T>.IndexKeys.Ascending)
                .Select(x => new CreateIndexModel<T>(x));

            await collection.Indexes.CreateManyAsync(indices);
        }

        private static async Task CreateIndices<T>(this IMongoCollection<T> collection, params string[] fields)
        {
            var indices = fields
                .Select(x => Builders<T>.IndexKeys.Ascending(x))
                .Select(x => new CreateIndexModel<T>(x));

            await collection.Indexes.CreateManyAsync(indices);
        }

        public static async Task<IMongoDatabase> NewDatabase(MongoClient client)
        {
            var db = client.GetDatabase(Names.Database);

            await db.CreateCollectionAsync(Names.Collections.Translations);

            var translations = db.GetCollection<TranslationDto>(Names.Collections.Translations);

            await translations.CreateIndices(
                x => x.Categories,
                x => x.Level,
                x => x.Label,
                x => x.TokenType);

            await translations.CreateIndices("Tokens.LanguageId");
            
            await db.CreateCollectionAsync(Names.Collections.Procedures);

            var procedures = db.GetCollection<ProcedureDto>(Names.Collections.Procedures);

            await procedures.CreateIndices(
                x => x.ProcedureId,
                x => x.Support);

            return db;
        }
    }
}
