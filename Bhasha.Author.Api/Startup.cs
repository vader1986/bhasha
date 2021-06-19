using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bhasha.Author.Api
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
            services.AddBhasha(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
