using AccessPointMap.Application.Integration.Aircrackng;
using AccessPointMap.Application.Integration.Wigle;
using AccessPointMap.Application.Oui.MacTwoVendor.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class IntegrationConfiguration
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
        {
            // OUI Provider
            services.AddMacTwoVendorOuiLookup();
          
            // WiGLE Integration
            services.AddScoped<IWigleIntegrationService, WigleIntegrationService>();

            // Aircrack-ng Integration
            services.AddScoped<IAircrackngIntegrationService, AircrackngIntegrationService>();

            return services;
        }
    }
}
