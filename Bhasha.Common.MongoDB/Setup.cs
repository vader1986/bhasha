using System.Threading.Tasks;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public class Setup
    {
        public static async Task<IMongoDatabase> NewDatabase(MongoClient client)
        {
            var db = client.GetDatabase(Names.Database);

            await db.CreateCollectionAsync(Names.Collections.Translations);
            await db.CreateCollectionAsync(Names.Collections.Procedures);

            return db;
        }
    }
}
