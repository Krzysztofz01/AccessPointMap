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
            services.AddHttps(Configuration);

            services.AddRoutingAndCors(Configuration);

            services.AddAuthentication(Configuration);

            services.AddAuthorization(Configuration);

            services.AddWebApiUtilities();

            services.AddPersistenceInfrastructure(Configuration);

            services.AddApplicationServices();

            services.AddBackgroundJobs();

            services.AddIntegrationServices();

            services.AddMapper();

            services.AddValidation();

            services.AddDocumentation(Configuration);

            services.AddServiceHealthChecks(Configuration);

            services.AddCaching();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {
            app.UseExceptionFilter();

            app.UseHttps(service);

            app.UseRoutingAndCors(service);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseWebApiUtilities();

            app.UsePersistenceInfrastructure(service);

            app.UseDocumentation(service);

            app.UseServiceHealthChecks(service);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
