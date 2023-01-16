using MongoDB.Driver;

namespace Bhasha.Web.Infrastructure.Mongo.Extensions;

public static class MongoClientExtensions
{
    public static IMongoCollection<T> Collection<T>(this IMongoClient client, string dbName)
    {
        return client
            .GetDatabase(dbName)
            .GetCollection<T>(typeof(T).Name);
    }

    public static IMongoCollection<T> GetCollection<T>(this IMongoClient client, string database)
    {
        return client.GetDatabase(database).GetCollection<T>(typeof(T).Name);
    }
}

