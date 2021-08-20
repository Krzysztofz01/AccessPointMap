using AccessPointMap.Service.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Web.Initialization
{
    public static class SettingsInitialization
    {
        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AdminSettings>(configuration.GetSection(nameof(AdminSettings)));
            services.Configure<EncryptionTypeSettings>(configuration.GetSection(nameof(EncryptionTypeSettings)));
            services.Configure<DeviceTypeSettings>(configuration.GetSection(nameof(DeviceTypeSettings)));

            return services;
        }
    }
}
