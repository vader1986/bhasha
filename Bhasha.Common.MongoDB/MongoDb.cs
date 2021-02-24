using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bhasha.Common.MongoDB
{
    public interface IMongoDb
    {
        ValueTask<IEnumerable<T>> Find<T>(string collectionName, Expression<Func<T, bool>> predicate, int maxItems);
        ValueTask<IEnumerable<V>> List<T, V>(string collectionName, Expression<Func<T, V>> selector);
        ValueTask<IEnumerable<V>> ListMany<T, V>(string collectionName, string fieldName);
    }

    public class MongoDb : IMongoDb
    {
        private readonly MongoClient _client;

        private MongoDb(MongoClient client)
        {
            _client = client;
        }

        public static async Task<MongoDb> Create(MongoClient client)
        {
            var dbNames = await client.ListDatabaseNames().ToListAsync();

            if (!dbNames.Contains(Names.Database))
            {
                await Setup.NewDatabase(client);
            }

            return new MongoDb(client);
        }

        public static async Task<MongoDb> Create(string connectionString)
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
            var queryable = GetCollection<T>(collectionName).AsQueryable().Where(predicate).Sample(maxItems);

            return await queryable.ToListAsync();
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
