using Quartz;

namespace AccessPointMap.Application.Extensions
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<TJob>(this IServiceCollectionQuartzConfigurator quartz, string jobName, string jobCronExpression) where TJob : IJob
        {
            quartz.AddJob<TJob>(options => options
                .WithIdentity(jobName));

                quartz.AddTrigger(options => options
                .ForJob(jobName)
                .WithIdentity($"{jobName}-trigger")
                .WithCronSchedule(jobCronExpression));          
        }
    }
}
