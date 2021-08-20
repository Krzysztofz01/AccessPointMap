using AccessPointMap.Web.Initialization;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AccessPointMap.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string _corsPolicyName = "DefaultPolicy";
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            //Settings
            services.AddSettings(Configuration);

            //Automapper
            services.AddMapper();

            //Context
            services.AddMySqlContext(Configuration);
            services.AddSqlServerContext(Configuration);

            //Services
            services.AddServices();

            //Cross-Origin Resource Sharing
            services.AddCors(o => o.AddPolicy(_corsPolicyName, builder =>
            {
                builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(o => true).AllowCredentials();
            }));

            //JWT Authentication
            services.AddAuthentication();

            //Hangfire
            services.AddBackgroundJobs();

            //Web
            services.AddWebServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager job, IServiceProvider service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(cfg =>
                {
                    cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "AccessPointMap WebAPI v1");
                });
            }

            app.UseCustomMiddleware();

            app.UseRouting();

            app.UseCors(_corsPolicyName);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseBackgroundJobs(job, service);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
