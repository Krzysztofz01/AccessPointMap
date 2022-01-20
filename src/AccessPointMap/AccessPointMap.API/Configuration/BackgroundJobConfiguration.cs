using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.AccessPoints;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;

namespace AccessPointMap.API.Configuration
{
    public static class BackgroundJobConfiguration
    {
        private const string _schedulerId = "AccessPointMapQuartz";

        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true;
                options.Scheduling.OverWriteExistingData = true;
            });

            services.AddQuartz(options =>
            {
                options.SchedulerId = _schedulerId; 

                options.UseMicrosoftDependencyInjectionJobFactory();
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
                options.UseDefaultThreadPool(tpool =>
                {
                    tpool.MaxConcurrency = 10;
                });

                options.ScheduleJob<AccessPointManufacturerJob>(trigger => trigger
                    .WithIdentity(AccessPointManufacturerJob.JobName)
                    .WithCronSchedule(AccessPointManufacturerJob.CronExpression)
                );
            });

            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });

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

            /*jobs.AddOrUpdate("AccessPointUpdateManufacturer", () => service
                .GetService<IAccessPointBackgroundJobs>().SetAccessPointManufacturer(), Cron.Daily);*/

            return app;
        }
    }
}
