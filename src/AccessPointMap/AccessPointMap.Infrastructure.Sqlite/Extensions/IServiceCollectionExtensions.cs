using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Infrastructure.Sqlite.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AccessPointMapDbContext>(options =>
                options.UseSqlite(connectionString, options =>
                {
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));

            services.AddScoped<IAuthenticationDataAccessService, AuthenticationDataAccessService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
