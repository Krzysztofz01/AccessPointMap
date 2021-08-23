using AccessPointMap.Service;
using AccessPointMap.Service.Maintenance;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Web.Initialization
{
    public static class ServicesInitialization
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessPointService, AccessPointService>();

            services.AddScoped<IMacResolveService, MacResolveService>();
            
            services.AddScoped<IAccessPointHelperService, AccessPointHelperService>();

            services.AddScoped<ITelemetryService, TelemetryService>();

            return services;
        }
    }
}
