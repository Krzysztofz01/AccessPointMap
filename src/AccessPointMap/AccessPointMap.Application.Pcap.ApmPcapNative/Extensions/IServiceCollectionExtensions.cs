using AccessPointMap.Application.Pcap.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Application.Pcap.ApmPcapNative.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApmPcapNativePcapParser(this IServiceCollection services) =>
            services.AddScoped<IPcapParsingService, ApmPcapNativePcapParsingService>();
    }
}
