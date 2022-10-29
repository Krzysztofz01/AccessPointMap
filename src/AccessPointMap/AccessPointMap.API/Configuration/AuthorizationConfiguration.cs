using AccessPointMap.Application.Authorization;
using AccessPointMap.Application.Settings;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    internal static class AuthorizationConfiguration
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthorizationSettings>(configuration.GetSection(nameof(AuthorizationSettings)));

            services.AddScoped<IScopeWrapperService, ScopeWrapperService>();

            return services;
        }
    }
}
