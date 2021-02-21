using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public class Database
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

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
