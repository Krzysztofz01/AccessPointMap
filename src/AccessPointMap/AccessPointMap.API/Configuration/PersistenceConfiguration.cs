using AccessPointMap.Application.Settings;
using AccessPointMap.Infrastructure.Core.Abstraction;
using AccessPointMap.Infrastructure.MySql;
using AccessPointMap.Infrastructure.MySql.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    public static class PersistenceConfiguration
    {
        public static IServiceCollection AddMySqlPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration
                .GetSection(nameof(DatabaseSettings))
                .Get<DatabaseSettings>();

            services.AddDbContext<AccessPointMapDbContext>(options =>
                options.UseMySql(databaseSettings.ConnectionString));

            services.AddScoped<IAuthenticationDataAccessService, AuthenticationDataAccessService>();

            services.AddScoped<IDataAccess, DataAccess>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
