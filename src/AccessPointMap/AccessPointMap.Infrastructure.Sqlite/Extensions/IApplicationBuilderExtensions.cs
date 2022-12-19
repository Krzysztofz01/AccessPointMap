using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccessPointMap.Infrastructure.Sqlite.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSqliteInfrastructure(this IApplicationBuilder app, IServiceProvider service)
        {
            using var dbContext = service.GetRequiredService<AccessPointMapDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
