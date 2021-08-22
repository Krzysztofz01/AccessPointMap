using AccessPointMap.Service.Integration.Aircrackng;
using AccessPointMap.Service.Integration.Wigle;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Web.Initialization
{
    public static class IntegrationServicesInitialization
    {
        public static IServiceCollection AddIntegrations(this IServiceCollection services)
        {
            services.AddScoped<IWigleIntegration, WigleIntegration>();

            services.AddScoped<IAircrackngIntegration, AircrackngIntegration>();

            return services;
        }
    }
}
