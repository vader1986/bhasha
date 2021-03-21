using System.Collections.Generic;
using Bhasha.Common;
using Bhasha.Common.Arguments;
using Bhasha.Common.MongoDB.Extensions;
using Bhasha.Common.Services;
using Bhasha.Web.Services;
using LazyCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var connectionString = _configuration.GetValue<string>("MongoDB");
            
            services
                .AddMongoDB(connectionString)
                .AddSingleton<IAuthorizedProfileLookup, AuthorizedProfileLookup>()
                .AddSingleton<IAppCache, CachingService>()
                .AddSwaggerDocument()
                .AddControllers();

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

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
