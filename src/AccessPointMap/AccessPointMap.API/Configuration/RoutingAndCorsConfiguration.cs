using AccessPointMap.Application.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace AccessPointMap.API.Configuration
{
    internal static class RoutingAndCorsConfiguration
    {
        public const string _defaultCorsPolicyName = "DefaultCorsPolicy";
        public const string _secureCorsPolicyName = "SecureCorsPolicy";

        public static IServiceCollection AddRoutingAndCors(this IServiceCollection services, IConfiguration configuration)
        {
            // NOTE: Redundant settings initialization
            var securitySettingsSection = configuration.GetSection(nameof(SecuritySettings));
            services.Configure<SecuritySettings>(securitySettingsSection);
            var securitySettings = securitySettingsSection.Get<SecuritySettings>();

            services.AddCors(options =>
            {
                options.AddPolicy(_defaultCorsPolicyName, builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(options => true)
                        .AllowCredentials();
                });

                if (securitySettings.SecureMode)
                {
                    options.AddPolicy(_secureCorsPolicyName, builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .WithMethods("PUT", "POST", "OPTIONS", "GET", "DELETE")
                            .WithOrigins(securitySettings.Origins)
                            .AllowCredentials();
                    });
                }
            });

            return services;
        }

        public static IApplicationBuilder UseRoutingAndCors(this IApplicationBuilder app, IServiceProvider service)
        {
            var securitySettings = service.GetService<IOptions<SecuritySettings>>().Value ??
                throw new InvalidOperationException("Security settings are not available.");

            app.UseRouting();

            app.UseCors(securitySettings.SecureMode ? _secureCorsPolicyName : _defaultCorsPolicyName);

            return app;
        }
    }
}
