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

            return services;
        }

        public static IApplicationBuilder UseBackgroundJobs(this IApplicationBuilder app, IRecurringJobManager jobs, IServiceProvider service, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard();
            }

            return app;
        }
    }
}
