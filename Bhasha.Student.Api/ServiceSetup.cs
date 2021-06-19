﻿using System.Text.Json.Serialization;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB;
using Bhasha.Common.MongoDB.Extensions;
using Bhasha.Student.Api.Services;
using LazyCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Bhasha.Student.Api
{
    public static class ServiceSetup
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

        private static void AddWebApi(this IServiceCollection services)
        {
            services
                .AddSingleton<IAuthorizedProfileLookup, AuthorizedProfileLookup>()
                .AddSingleton<IAppCache, CachingService>()
                .AddOpenApiDocument()
                .AddControllers()
                .AddSerialization();
        }

        public static void AddBhasha(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMongoDB(MongoSettings.From(configuration))
                .AddBhashaServices()
                .AddWebApi();
        }
    }
}
