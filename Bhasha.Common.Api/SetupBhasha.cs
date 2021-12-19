using System.Text.Json.Serialization;
using Bhasha.Common.Api.Configuration;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using NSwag.Generation.Processors.Security;

namespace Bhasha.Common.Api
{
    public static class SetupBhasha
    {
        public static void AddAnyCors(this IServiceCollection services)
        {
            services
                .AddCors(cors => cors.AddPolicy(Names.CorsPolicy, x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
        }

        public static void AddSerialization(this IMvcBuilder builder)
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
            var mongo = MongoSettings.From(configuration);
            var auth = AuthSettings.From(configuration);

            services
                .AddMongoDB(mongo.ConnectionString, mongo.DatabaseName)
                .AddBhashaServices();

            services
                .AddOpenApiDocument(options =>
                {
                    options.AddSecurity("JWT", new[] { auth.Scope }, new NSwag.OpenApiSecurityScheme
                    {
                        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}."
                    });
                    options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                })
                .AddControllers()
                .AddSerialization();

            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.Authority = auth.AuthServer;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(Names.AuthPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", auth.Scope);
                });
            });

            services
                .AddAnyCors();
        }

        public static void UseBhasha(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseCors(Names.CorsPolicy);

            // where do we want to go?
            app.UseRouting();

            // who are you?
            app.UseAuthentication();

            // are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("ApiScope");
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3(options =>
            {
            });
        }
    }
}
