using AccessPointMap.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccessPointMap.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMySqlPersistence(Configuration);

            services.AddAuthentication(Configuration);

            services.AddAuthorization(Configuration);

            services.AddApplicationServices();

            services.AddIntegrationServices();

            services.AddMapper();

            services.AddBackgroundJobs();

            services.AddCaching();

            services.AddWebUtilities(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            app.UseWebUtilities(env, service);

            app.UseMySqlPersistence(service);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
