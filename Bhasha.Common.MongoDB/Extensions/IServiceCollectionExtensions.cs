using Bhasha.Common.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Bhasha.Common.MongoDB.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, MongoSettings settings)
        {
            services
                .AddSingleton<IDatabase, MongoDatabase>()
                .AddSingleton(MongoWrapper.Create(settings));

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
