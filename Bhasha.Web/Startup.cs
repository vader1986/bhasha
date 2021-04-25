using System.IO;
using Bhasha.Common.Arguments;
using Bhasha.Common.Extensions;
using Bhasha.Common.Importers;
using Bhasha.Common.MongoDB.Extensions;
using Bhasha.Common.Services;
using Bhasha.Web.Services;
using LazyCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            var username = _configuration.GetValue<string>("MongoUser");
            var password = _configuration.GetValue<string>("MongoPassword");
            var host = _configuration.GetValue<string>("MongoHost");

            var connectionString = $"mongodb://{username}:{password}@{host}";

            System.Console.WriteLine(connectionString);

            services
                .AddMongoDB(connectionString)
                .AddBhashaServices()
                .AddSingleton<IAuthorizedProfileLookup, AuthorizedProfileLookup>()
                .AddSingleton<IAppCache, CachingService>()
                .AddSwaggerDocument()
                .AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            var dir = Directory.GetCurrentDirectory();
            
            if (Directory.Exists("wwwroot"))
            {
                logger.LogInformation($"Web app: {string.Join(", ", Directory.GetFiles("wwwroot"))}");
            }
            else
            {
                logger.LogError($"Missing root directory for web app: {dir}/wwwroot");
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();
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
