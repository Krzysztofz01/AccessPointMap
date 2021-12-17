using AccessPointMap.API.Configuration;
using Hangfire;
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

            services.AddWebUtilities();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service, IRecurringJobManager jobManager)
        {
            app.UseWebUtilities(env);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseBackgroundJobs(jobManager, service, env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
