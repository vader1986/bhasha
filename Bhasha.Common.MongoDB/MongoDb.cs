using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public interface IMongoDb
    {
        IMongoCollection<T> GetCollection<T>(string name);
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

        private IMongoCollection<T> Collection<T>(string name)
        {
            return _client.GetDatabase(Names.Database).GetCollection<T>(name);
        }

        public async ValueTask<IEnumerable<T>> Find<T>(
            string collectionName,
            Expression<Func<T, Guid>> selector,
            IEnumerable<Guid> ids)
        {
            var filter = Builders<T>.Filter.In(selector, ids);
            var result = await Collection<T>(collectionName).FindAsync(filter);

            return result.ToEnumerable();
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _client.GetDatabase(Names.Database).GetCollection<T>(name);
        }
    }
}
