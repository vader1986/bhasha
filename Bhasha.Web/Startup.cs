using Bhasha.Common;
using Bhasha.Common.Arguments;
using Bhasha.Common.MongoDB;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Services;
using Bhasha.Web.Services;
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
                .AddSingleton<IProfileLookup, ProfileLookup>()
                .AddSingleton<IAuthorizedProfileLookup, AuthorizedProfileLookup>()
                .AddControllers();

            services
                .AddTransient<IStore<GenericChapter>, MongoDbStore<GenericChapterDto, GenericChapter>>()
                .AddTransient<IStore<Profile>, MongoDbStore<ProfileDto, Profile>>()
                .AddTransient<IStore<Tip>, MongoDbStore<TipDto, Tip>>()
                .AddTransient<IStore<Token>, MongoDbStore<TokenDto, Token>>()
                .AddTransient<IStore<Translation>, MongoDbStore<TranslationDto, Translation>>()
                .AddTransient<IStore<User>, MongoDbStore<UserDto, User>>();

            services
                .AddTransient<IAssembleArguments, OneOutOfFourArgumentsAssembly>()
                .AddTransient<IAssembleChapters, ChapterAssembly>()
                .AddTransient<ICheckResult, ResultChecker>()
                .AddTransient<IUpdateStats, StatsUpdater>()
                .AddTransient<IEvaluateSubmit, SubmitEvaluator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
