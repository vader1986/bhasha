using System.Collections.Generic;
using Bhasha.Common;
using Bhasha.Common.Arguments;
using Bhasha.Common.MongoDB;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Services;
using Bhasha.Web.Services;
using LazyCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bhasha.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionString = _configuration.GetValue<string>("MongoDB");
            var db = MongoDb.Create(dbConnectionString);

            services
                .AddSingleton<IDatabase>(_ => new MongoDbLayer(db, new Converter()))
                .AddSingleton<IMongoDb>(db)
                .AddSingleton<IAuthorizedProfileLookup, AuthorizedProfileLookup>()
                .AddSingleton<IAppCache, CachingService>()
                .AddSwaggerDocument()
                .AddControllers();
            
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

            services
                .AddTransient<ArgumentAssemblyProvider>(sp => key => {
                    return key switch
                    {
                        PageType.OneOutOfFour => sp.GetService<OneOutOfFourArgumentsAssembly>(),
                        _ => throw new KeyNotFoundException($"No {nameof(IAssembleArguments)} found for {key}"),
                    };
                })
                .AddTransient<IAssembleChapters, ChapterAssembly>()
                .AddTransient<ICheckResult, ResultChecker>()
                .AddTransient<IUpdateStatsForEvaluation, EvaluationStatsUpdater>()
                .AddTransient<IUpdateStatsForTip, TipStatsUpdater>()
                .AddTransient<IEvaluateSubmit, SubmitEvaluator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
//            app.UseHttpsRedirection();
            app.UseRouting();
//            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
