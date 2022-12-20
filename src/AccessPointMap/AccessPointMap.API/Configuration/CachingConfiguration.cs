using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class CachingConfiguration
    {
        internal static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();

            return services;
        }
    }
}
