using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Extensions
{
    public static class IMongoCollectionExtensions
    {
        public static async Task CreateIndices<T>(this IMongoCollection<T> collection, params Expression<Func<T, object>>[] selectors)
        {
            var indices = selectors
                .Select(Builders<T>.IndexKeys.Ascending)
                .Select(x => new CreateIndexModel<T>(x));

            await collection.Indexes.CreateManyAsync(indices);
        }

        public static async Task CreateIndices<T>(this IMongoCollection<T> collection, params string[] fields)
        {
            var indices = fields
                .Select(x => Builders<T>.IndexKeys.Ascending(x))
                .Select(x => new CreateIndexModel<T>(x));

            await collection.Indexes.CreateManyAsync(indices);
        }

        public static async Task InsertRangeAsync<T>(this IMongoCollection<T> collection, params T[] docs)
        {
            await collection.InsertManyAsync(docs);
        }
    }
}
