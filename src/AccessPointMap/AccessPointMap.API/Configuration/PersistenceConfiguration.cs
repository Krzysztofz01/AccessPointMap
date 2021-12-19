using AccessPointMap.Application.Settings;
using AccessPointMap.Infrastructure.Core.Abstraction;
using AccessPointMap.Infrastructure.MySql;
using AccessPointMap.Infrastructure.MySql.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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

            services.AddDbContext<AccessPointMapDbContext>(options =>
                options.UseMySql(databaseSettings.ConnectionString));

            services.AddScoped<IAuthenticationDataAccessService, AuthenticationDataAccessService>();

            services.AddScoped<IDataAccess, DataAccess>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IApplicationBuilder UseMySqlPersistence(this IApplicationBuilder app, IServiceProvider service)
        {
            using var dbContext = service.GetRequiredService<AccessPointMapDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
