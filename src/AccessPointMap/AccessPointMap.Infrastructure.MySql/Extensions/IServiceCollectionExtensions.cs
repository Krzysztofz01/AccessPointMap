using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AccessPointMap.Infrastructure.MySql.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMySqlInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AccessPointMapDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
                {
                    options.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}");

                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));

            services.AddScoped<IAuthenticationDataAccessService, AuthenticationDataAccessService>();

            services.AddScoped<IDataAccess, DataAccess>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
