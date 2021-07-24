using AccessPointMap.Repository;
using AccessPointMap.Repository.SqlServer;
using AccessPointMap.Repository.SqlServer.Context;
using AccessPointMap.Service.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Web.Initialization
{
    public static class SqlServerContextInitialization
    {
        public static IServiceCollection AddSqlServerContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

            if (dbSettings.UseDefaultProvider)
            {
                services.AddDbContext<AccessPointMapDbContext>(opt =>
                opt.UseSqlServer(dbSettings.SqlServerConnectionString));

                //Repositories
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IAccessPointRepository, AccessPointRepository>();
            }

            return services;
        }
    }
}
