using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Web.Initialization
{
    public static class MapperInitialization
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<Service.Profiles.UserProfile>();
                cfg.AddProfile<Service.Profiles.AccessPointProfile>();

                cfg.AddProfile<Web.Profiles.UserProfile>();
                cfg.AddProfile<Web.Profiles.AccessPointProfile>();
                cfg.AddProfile<Web.Profiles.AccessPointStatisticsProfile>();
            });

            return services;
        }
    }
}
