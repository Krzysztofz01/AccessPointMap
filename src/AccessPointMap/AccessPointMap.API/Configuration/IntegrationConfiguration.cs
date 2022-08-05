using AccessPointMap.Application.Integration.Aircrackng;
using AccessPointMap.Application.Integration.Wigle;
using AccessPointMap.Application.Integration.Wireshark;
using AccessPointMap.Application.Kml.Sharpkml.Extensions;
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

            // Sharpkml internal KML service provider
            services.AddSharpkmlKmlParser();

            // Integrations

            // WiGLE Integration
            services.AddScoped<IWigleIntegrationService, WigleIntegrationService>();

            // Aircrack-ng Integration
            services.AddScoped<IAircrackngIntegrationService, AircrackngIntegrationService>();

            // Wireshark Integration
            services.AddScoped<IWiresharkIntegrationService, WiresharkIntegrationService>();

            return services;
        }
    }
}
