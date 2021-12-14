using AccessPointMap.Application.Integration.Aircrackng;
using AccessPointMap.Application.Integration.Wigle;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Oui.MacToVendor;
using AccessPointMap.Application.Oui.MacToVendor.Database;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class IntegrationConfiguration
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
        {
            // OUI Provider
            services.AddDbContext<MacTwoVendorDbContext>();
            services.AddScoped<IOuiLookupService, MacTwoVendorOuiLookupService>();

            // WiGLE Integration
            services.AddScoped<IWigleIntegrationService, WigleIntegrationService>();

            // Aircrack-ng Integration
            services.AddScoped<IAircrackngIntegrationService, AircrackngIntegrationService>();

            return services;
        }
    }
}
