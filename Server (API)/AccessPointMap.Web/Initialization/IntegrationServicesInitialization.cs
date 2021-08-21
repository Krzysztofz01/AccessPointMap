using AccessPointMap.Service.Integration.Aircrackng;
using AccessPointMap.Service.Integration.Wiggle;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Web.Initialization
{
    public static class IntegrationServicesInitialization
    {
        public static IServiceCollection AddIntegrations(this IServiceCollection services)
        {
            services.AddScoped<IWiggleIntegration, WiggleIntegration>();

            services.AddScoped<IAircrackngIntegration, AircrackngIntegration>();

            return services;
        }
    }
}
