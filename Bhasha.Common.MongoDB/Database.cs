using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public interface IDatabase
    {
        ValueTask<IEnumerable<T>> Find<T>(string collectionName, Func<T, bool> predicate, int maxItems);
        ValueTask<IEnumerable<V>> List<T, V>(string collectionName, Func<T, V> selector);
        ValueTask<IEnumerable<V>> ListMany<T, V>(string collectionName, Func<T, V[]> selector);
    }

    public class Database : IDatabase
    {
        private readonly IMongoDatabase _database;

        private Database(IMongoDatabase database)
        {
            _database = database;
        }

        private static async Task<IMongoDatabase> GetDatabase(MongoClient client)
        {
            var dbNames = await client.ListDatabaseNames().ToListAsync();

            if (!dbNames.Contains(Names.Database))
            {
                return await Setup.NewDatabase(client);
            }

            return client.GetDatabase(Names.Database);
        }

        public static async Task<Database> Create(string connectionString)
        {
            return new Database(await GetDatabase(new MongoClient(connectionString)));
        }

        public async ValueTask<IEnumerable<T>> Find<T>(string collectionName, Func<T, bool> predicate, int maxItems)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var findOptions = new FindOptions<T> { BatchSize = maxItems };
            var result = await collection.FindAsync(x => predicate(x), findOptions);

            return result.Current;
        }

        public async ValueTask<IEnumerable<V>> List<T, V>(string collectionName, Func<T, V> selector)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var result = await collection.DistinctAsync(x => selector(x), x => true);
            return result.ToEnumerable();
        }

        public ValueTask<IEnumerable<V>> ListMany<T, V>(string collectionName, Func<T, V[]> selector)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var result = collection.AsQueryable().SelectMany(x => selector(x)).Distinct();
            return new ValueTask<IEnumerable<V>>(result.AsEnumerable());
        }
    }
}
