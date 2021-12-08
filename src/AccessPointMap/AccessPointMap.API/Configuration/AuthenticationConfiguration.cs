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
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthenticationWrapperService, AuthenticationWrapperService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            var jwtSettingsSection = configuration.GetSection(nameof(JsonWebTokenSettings));
            services.Configure<JsonWebTokenSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JsonWebTokenSettings>();
            var secret = Encoding.ASCII.GetBytes(jwtSettings.TokenSecret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
