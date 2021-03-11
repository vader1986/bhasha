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

        public static MongoDb Create(string connectionString)
        {
            var client = new MongoClient(connectionString);

            CheckOrSetupDatabase(client);

            return new MongoDb(client);
        }

        private static void CheckOrSetupDatabase(MongoClient client)
        {
            var dbNames = client.ListDatabaseNames().ToList();

            if (!dbNames.Contains(Names.Database))
            {
                Setup
                    .NewDatabase(client)
                    .GetAwaiter()
                    .GetResult();
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _client
                .GetDatabase(Names.Database)
                .GetCollection<T>(name);
        }
    }
}
