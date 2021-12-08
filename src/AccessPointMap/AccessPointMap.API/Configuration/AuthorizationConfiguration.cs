using AccessPointMap.Application.Authorization;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class AuthorizationConfiguration
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration _)
        {
            services.AddScoped<IScopeWrapperService, ScopeWrapperService>();

            return services;
        }
    }
}
