using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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

                options.AddJobAndTrigger<AccessPointManufacturerJob>(
                    AccessPointManufacturerJob.JobName,
                    AccessPointManufacturerJob.CronExpression);

                options.AddJobAndTrigger<AccessPointPresenceJob>(
                    AccessPointPresenceJob.JobName,
                    AccessPointPresenceJob.CronExpression);
            });

            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return services;
        }
    }
}
