using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bhasha.Common.Extensions;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB
{
    public interface IMongoDb
    {
        IMongoCollection<T> GetCollection<T>();
    }

    public class MongoDb : IMongoDb
    {
        private readonly MongoClient _client;
        private readonly IDictionary<Type, string> _collections;
        
        private MongoDb(MongoClient client)
        {
            _client = client;
            _collections = Assembly
                .GetExecutingAssembly()
                .GetTypesWithAttribute<MongoCollectionAttribute>()
                .ToDictionary(x => x.Key, x => x.Value.CollectionName);
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
        public IMongoCollection<T> GetCollection<T>()
        {
            return _client
                .GetDatabase(Names.Database)
                .GetCollection<T>(_collections[typeof(T)]);
        }
    }
}
