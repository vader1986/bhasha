﻿using System.Linq.Expressions;
using MongoDB.Driver;

namespace Bhasha.Web.Mongo
{
    public static class MongoExtensions
    {
        public static IMongoCollection<T> Collection<T>(this IMongoClient client, string dbName)
        {
            return client
                .GetDatabase(dbName)
                .GetCollection<T>(typeof(T).Name);
        }

        public static IEnumerable<string> CreateIndices<T>(this IMongoCollection<T> collection, params Expression<Func<T, object>>[] selectors)
        {
            var indices = selectors
                .Select(Builders<T>.IndexKeys.Ascending)
                .Select(x => new CreateIndexModel<T>(x));

            return collection.Indexes.CreateMany(indices);
        }

        public static IEnumerable<string> CreateIndices<T>(this IMongoCollection<T> collection, params string[] fields)
        {
            var indices = fields
                .Select(x => Builders<T>.IndexKeys.Ascending(x))
                .Select(x => new CreateIndexModel<T>(x));

            return collection.Indexes.CreateMany(indices);
        }
    }
}

