using System.Linq;
using Bhasha.Common.MongoDB.Extensions;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public interface IMongoWrapper
    {
        IMongoCollection<T> GetCollection<T>();
    }

    public class MongoWrapper : IMongoWrapper
    {
        private readonly MongoClient _client;
        
        private MongoWrapper(MongoClient client)
        {
            _client = client;
        }

        public static IMongoWrapper Create(MongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dbNames = client.ListDatabaseNames().ToList();

            if (!dbNames.Contains(Names.Database))
            {
                client
                    .ExecuteAsync(Setup.NewDatabase)
                    .GetAwaiter()
                    .GetResult();
            }

            return new MongoWrapper(client);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return _client
                .GetDatabase(Names.Database)
                .GetCollection<T>(nameof(T));
        }
    }
}
