using AccessPointMap.Application.Integration.Wigle;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class IntegrationConfiguration
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
        {
            // WiGLE Integration
            services.AddScoped<IWigleIntegrationService, WigleIntegrationService>();

            return services;
        }
    }
}
