using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bhasha.PostgrSql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgrSql(this IServiceCollection services, IConfiguration configuration)
    {
        // ToDo
        
        return services;
    }

}