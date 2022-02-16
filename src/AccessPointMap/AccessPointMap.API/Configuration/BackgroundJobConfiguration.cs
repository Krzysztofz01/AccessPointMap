using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.AccessPoints;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AccessPointMap.API.Configuration
{
    public static class BackgroundJobConfiguration
    {
        private const string _utcCronExpression = "0 4 * * * ";

        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddHangfire(options =>
            {
                options
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseDefaultTypeSerializer()
                    .UseMemoryStorage();
            });

            services.AddHangfireServer();

            services.AddScoped<IAccessPointBackgroundJobs, AccessPointBackgroundJobs>();

            return services;
        }

        public static IApplicationBuilder UseBackgroundJobs(this IApplicationBuilder app, IRecurringJobManager jobs, IServiceProvider service, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard();
            }

            jobs.AddOrUpdate("AccessPointUpdateManufacturer", () => service
                .GetService<IAccessPointBackgroundJobs>().SetAccessPointManufacturer(), _utcCronExpression);

            return app;
        }
    }
}
