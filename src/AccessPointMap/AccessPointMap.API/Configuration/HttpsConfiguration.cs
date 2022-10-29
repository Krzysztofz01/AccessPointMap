using AccessPointMap.Application.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net;

namespace AccessPointMap.API.Configuration
{
    internal static class HttpsConfiguration
    {
        public static IServiceCollection AddHttps(this IServiceCollection services, IConfiguration configuration)
        {
            // NOTE: Redundant settings initialization
            var securitySettingsSection = configuration.GetSection(nameof(SecuritySettings));
            services.Configure<SecuritySettings>(securitySettingsSection);
            var securitySettings = securitySettingsSection.Get<SecuritySettings>();

            if (securitySettings.SecureMode)
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }

            return services;
        }
        
        public static IApplicationBuilder UseHttps(this IApplicationBuilder app, IServiceProvider service)
        {
            var securitySettings = service.GetService<IOptions<SecuritySettings>>().Value ??
                throw new InvalidOperationException("Security settings are not available.");

            if (securitySettings.SecureMode)
            {
                app.UseHsts();

                app.UseHttpsRedirection();
            }

            return app;
        }
    }
}
