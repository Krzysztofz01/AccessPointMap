using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Application.Identities;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    internal static class MapperConfiguration
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(options =>
            {
                options.AddProfile<AccessPointMapperProfile>();
                options.AddProfile<IdentityMapperProfile>();
            });

            return services;
        }
    }
}
