using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Settings;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AccessPointMap.API.Configuration
{
    internal static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationWrapperService, AuthenticationWrapperService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>();

            var jwtSettingsSection = configuration.GetSection(nameof(JsonWebTokenSettings));
            services.Configure<JsonWebTokenSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JsonWebTokenSettings>();
            var secret = Encoding.ASCII.GetBytes(jwtSettings.TokenSecret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                if (securitySettings.SecureMode)
                {
                    options.Authority = jwtSettings.Audience;
                    options.Audience = jwtSettings.Audience;
                }

                options.RequireHttpsMetadata = securitySettings.SecureMode;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = securitySettings.SecureMode,
                    ValidateAudience = securitySettings.SecureMode,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
