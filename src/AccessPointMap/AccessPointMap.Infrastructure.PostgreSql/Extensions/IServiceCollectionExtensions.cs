using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Infrastructure.PostgreSql.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgreSqlInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AccessPointMapDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IAuthenticationDataAccessService, AuthenticationDataAccessService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
