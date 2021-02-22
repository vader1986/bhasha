using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public interface IDatabase
    {
        ValueTask<IEnumerable<T>> Find<T>(string collectionName, Expression<Func<T, bool>> predicate, int maxItems);
        ValueTask<IEnumerable<V>> List<T, V>(string collectionName, Expression<Func<T, V>> selector);
        ValueTask<IEnumerable<V>> ListMany<T, V>(string collectionName, string fieldName);
    }

    public class Database : IDatabase
    {
        private readonly MongoClient _client;

        private Database(MongoClient client)
        {
            _client = client;
        }

        public static async Task<Database> Create(MongoClient client)
        {
            var dbNames = await client.ListDatabaseNames().ToListAsync();

            if (!dbNames.Contains(Names.Database))
            {
                await Setup.NewDatabase(client);
            }

            return new Database(client);
        }

        public static async Task<Database> Create(string connectionString)
        {
            return await Create(new MongoClient(connectionString));
        }

        private IMongoCollection<T> GetCollection<T>(string name)
        {
            return _client
                .GetDatabase(Names.Database)
                .GetCollection<T>(name);
        }

        private static async ValueTask<IEnumerable<T>> GetResult<T>(IAsyncCursor<T> cursor)
        {
            return await cursor.MoveNextAsync() ? cursor.Current : new T[0];
        }

        public async ValueTask<IEnumerable<T>> Find<T>(string collectionName, Expression<Func<T, bool>> predicate, int maxItems)
        {
            var findOptions = new FindOptions<T> { BatchSize = maxItems };
            var cursor = await GetCollection<T>(collectionName).FindAsync(predicate, findOptions);

            return await GetResult(cursor);
        }

        public async ValueTask<IEnumerable<V>> List<T, V>(string collectionName, Expression<Func<T, V>> selector)
        {
            var result = await GetCollection<T>(collectionName).DistinctAsync(selector, x => true);

            return await GetResult(result);
        }

        public async ValueTask<IEnumerable<V>> ListMany<T, V>(string collectionName, string fieldName)
        {
            var collection = GetCollection<T>(collectionName);
            var result = await collection.DistinctAsync<V>(fieldName, FilterDefinition<T>.Empty);

            return await GetResult(result);
        }
    }
}
