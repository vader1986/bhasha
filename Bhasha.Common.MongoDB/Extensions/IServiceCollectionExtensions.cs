using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bhasha.Common.MongoDB.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, string connectionString)
        {
            var db = MongoDb.Create(connectionString);

            services
                .AddTransient<Converter>()
                .AddSingleton<IDatabase, MongoDbLayer>()
                .AddSingleton<IMongoDb>(db);

            services
                .AddTransient<IConvert<ChapterStatsDto, ChapterStats>, Converter>()
                .AddTransient<IConvert<GenericChapterDto, GenericChapter>, Converter>()
                .AddTransient<IConvert<ProfileDto, Profile>, Converter>()
                .AddTransient<IConvert<TipDto, Tip>, Converter>()
                .AddTransient<IConvert<TokenDto, Token>, Converter>()
                .AddTransient<IConvert<TranslationDto, Translation>, Converter>();

            services
                .AddTransient<IStore<GenericChapter>, MongoDbStore<GenericChapterDto, GenericChapter>>()
                .AddTransient<IStore<ChapterStats>, MongoDbStore<ChapterStatsDto, ChapterStats>>()
                .AddTransient<IStore<Profile>, MongoDbStore<ProfileDto, Profile>>()
                .AddTransient<IStore<Tip>, MongoDbStore<TipDto, Tip>>()
                .AddTransient<IStore<Token>, MongoDbStore<TokenDto, Token>>()
                .AddTransient<IStore<Translation>, MongoDbStore<TranslationDto, Translation>>();

            return services;
        }
    }
}
