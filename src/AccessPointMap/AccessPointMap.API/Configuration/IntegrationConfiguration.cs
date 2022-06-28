using AccessPointMap.Application.Integration.Aircrackng;
using AccessPointMap.Application.Integration.Wigle;
using AccessPointMap.Application.Oui.MacTwoVendor.Extensions;
using AccessPointMap.Application.Pcap.ApmPcapNative.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class IntegrationConfiguration
    {
        public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
        {
            // Internal services

            // MacTwoVendor internal OUI service provider
            services.AddMacTwoVendorOuiLookup();

            // ApmPcapNative internal PCAP service provider
            services.AddApmPcapNativePcapParser();

            // Integrations

            // WiGLE Integration
            services.AddScoped<IWigleIntegrationService, WigleIntegrationService>();

            // Aircrack-ng Integration
            services.AddScoped<IAircrackngIntegrationService, AircrackngIntegrationService>();

            return services;
        }
    }
}
