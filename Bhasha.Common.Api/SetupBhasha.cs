using System.Text.Json.Serialization;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB;
using Bhasha.Common.MongoDB.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Bhasha.Common.Api
{
    public static class SetupBhasha
    {
        private static void AddSerialization(this IMvcBuilder builder)
        {
            builder
                .AddJsonOptions(cfg =>
                {
                    cfg.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddNewtonsoftJson(cfg =>
                {
                    cfg.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
        }

        public static void AddBhasha(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMongoDB(MongoSettings.From(configuration))
                .AddBhashaServices();

            services
                .AddOpenApiDocument()
                .AddControllers()
                .AddSerialization();

            services.AddCors(cors => cors.AddPolicy(Names.CorsPolicy, x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
        }

        public static void UseBhasha(this IApplicationBuilder app)
        {
            //app.UseHttpsRedirection();
            app.UseCors(Names.CorsPolicy);
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
