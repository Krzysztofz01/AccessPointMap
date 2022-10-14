using AccessPointMap.Application.Settings;
using AccessPointMap.Infrastructure.MySql.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccessPointMap.API.Configuration
{
    public static class PersistenceConfiguration
    {
        public static IServiceCollection AddMySqlPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration
                .GetSection(nameof(DatabaseSettings))
                .Get<DatabaseSettings>();

            services.AddMySqlInfrastructure(databaseSettings.ConnectionString);

            return services;
        }

        public static IApplicationBuilder UseMySqlPersistence(this IApplicationBuilder app, IServiceProvider service)
        {
            app.UseMySqlInfrastructure(service);

            return app;
        }
    }
}
