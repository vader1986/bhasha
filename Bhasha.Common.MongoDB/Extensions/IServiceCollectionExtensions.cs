using Bhasha.Common.Database;
using Microsoft.Extensions.DependencyInjection;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using MongoDB.Driver;

namespace Bhasha.Common.MongoDB.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, MongoSettings settings)
        {
            services
                .AddSingleton<IMongoClient>(new MongoClient(settings.ConnectionString))
                .AddSingleton<IDatabase, MongoDatabase>()
                .AddMigration(new MongoMigrationSettings {
                    Database = Names.Database,
                    ConnectionString = settings.ConnectionString
                });

            services
                .AddTransient<IStore<DbChapter>, MongoStore<DbChapter>>()
                .AddTransient<IStore<DbStats>, MongoStore<DbStats>>()
                .AddTransient<IStore<DbUserProfile>, MongoStore<DbUserProfile>>()
                .AddTransient<IStore<DbExpression>, MongoStore<DbExpression>>()
                .AddTransient<IStore<DbTranslatedChapter>, MongoStore<DbTranslatedChapter>>()
                .AddTransient<IStore<DbWord>, MongoStore<DbWord>>();

            return services;
        }
    }
}
