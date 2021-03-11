using Bhasha.Common;
using Bhasha.Common.MongoDB;
using Bhasha.Web.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bhasha.Web.Api
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
                .AddSingleton<IDatabase>(_ => new MongoDbLayer(db))
                .AddSingleton<IProfileLookup, ProfileLookup>()
                .AddSingleton<IAuthorizedProfileLookup, AuthorizedProfileLookup>()
                .AddControllers();
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
