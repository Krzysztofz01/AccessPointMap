using AccessPointMap.Application.Settings;
using AccessPointMap.Infrastructure.MySql.Extensions;
using AccessPointMap.Infrastructure.Sqlite.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace AccessPointMap.API.Configuration
{
    internal static class PersistenceConfiguration
    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettingsSection = configuration.GetSection(nameof(DatabaseSettings));
            services.Configure<DatabaseSettings>(databaseSettingsSection);

            var databaseSettings = databaseSettingsSection.Get<DatabaseSettings>();
            switch (databaseSettings.Driver.ToLower())
            {
                case "mysql": services.AddMySqlInfrastructure(databaseSettings.ConnectionString); break;
                case "sqlite": services.AddSqliteInfrastructure(databaseSettings.ConnectionString); break;
                default: throw new NotSupportedException("The selected database infrastructure driver is not supported.");
            }

            return services;
        }

        public static IApplicationBuilder UsePersistenceInfrastructure(this IApplicationBuilder app, IServiceProvider service)
        {
            var databaseSettings = service.GetService<IOptions<DatabaseSettings>>().Value ??
                throw new InvalidOperationException("Database settings are not available.");

            switch (databaseSettings.Driver.ToLower())
            {
                case "mysql": app.UseMySqlInfrastructure(service, databaseSettings.ApplyMigrations); break;
                case "sqlite": app.UseSqliteInfrastructure(service, databaseSettings.ApplyMigrations); break;
                default: throw new NotSupportedException("The selected database infrastructure driver is not supported.");
            }

            return app;
        }
    }
}
