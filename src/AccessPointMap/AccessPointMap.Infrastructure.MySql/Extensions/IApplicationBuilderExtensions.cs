using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccessPointMap.Infrastructure.MySql.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMySqlInfrastructure(this IApplicationBuilder app, IServiceProvider service, bool applyMigrations)
        {
            using var dbContext = service.GetRequiredService<AccessPointMapDbContext>();

            if (applyMigrations) dbContext.Database.Migrate();

            return app;
        }
    }
}
