using Bhasha.Common.Database;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, string connectionString, string databaseName)
        {
            services
                .AddSingleton<IMongoClient>(new MongoClient(connectionString))
                .AddSingleton<IDatabase>(sp => new MongoDatabase(sp.GetRequiredService<IMongoClient>(), databaseName))
                .AddMigration(new MongoMigrationSettings {
                    Database = databaseName,
                    ConnectionString = connectionString
                });

            services
                .AddTransient<IStore<DbChapter>>(sp => new MongoStore<DbChapter>(sp.GetRequiredService<IMongoClient>(), databaseName))
                .AddTransient<IStore<DbStats>>(sp => new MongoStore<DbStats>(sp.GetRequiredService<IMongoClient>(), databaseName))
                .AddTransient<IStore<DbUserProfile>>(sp => new MongoStore<DbUserProfile>(sp.GetRequiredService<IMongoClient>(), databaseName))
                .AddTransient<IStore<DbExpression>>(sp => new MongoStore<DbExpression>(sp.GetRequiredService<IMongoClient>(), databaseName))
                .AddTransient<IStore<DbTranslatedChapter>>(sp => new MongoStore<DbTranslatedChapter>(sp.GetRequiredService<IMongoClient>(), databaseName))
                .AddTransient<IStore<DbWord>>(sp => new MongoStore<DbWord>(sp.GetRequiredService<IMongoClient>(), databaseName));

            return services;
        }
    }
}
