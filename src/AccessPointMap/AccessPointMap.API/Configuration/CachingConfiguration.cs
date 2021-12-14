using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class CachingConfiguration
    {
        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddCaching();

            return services;
        }
    }
}
