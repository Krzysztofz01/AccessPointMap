using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Application.Identities;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class ApplicationServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccessPointService, AccessPointService>();
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
