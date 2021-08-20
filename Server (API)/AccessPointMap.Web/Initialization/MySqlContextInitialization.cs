using AccessPointMap.Repository;
using AccessPointMap.Repository.MySql;
using AccessPointMap.Repository.MySql.Context;
using AccessPointMap.Service.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Web.Initialization
{
    public static class MySqlContextInitialization
    {
        public static IServiceCollection AddMySqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

            if(!dbSettings.UseDefaultProvider)
            {
                //Database context
                services.AddDbContext<AccessPointMapMySqlDbContext>(opt =>
                    opt.UseMySql(dbSettings.MySqlConnectionString, ServerVersion.AutoDetect(dbSettings.MySqlConnectionString)));

                //Repositories
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IAccessPointRepository, AccessPointRepository>();
            }

            return services;
        }
    }
}
