using AccessPointMap.Service;
using AccessPointMap.Service.Maintenance;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccessPointMap.Web.Initialization
{
    public static class BackgroundJobsInitialization
    {
        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddHangfire(cfg =>
            {
                cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    .UseMemoryStorage();
            });

            return services;
        }

        public static IApplicationBuilder UseBackgroundJobs(this IApplicationBuilder app, IRecurringJobManager job, IServiceProvider service)
        {
            app.UseHangfireServer();

            job.AddOrUpdate("Update manufacturer information",
                () => service.GetService<IAccessPointService>().UpdateBrands(), Cron.Weekly);

            job.AddOrUpdate("Indicate instance uptime",
                () => service.GetService<ITelemetryService>().Ping(), Cron.Hourly);

            return app;
        }
    }
}
