using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Application.GeoJson.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNativeGeoJsonParser(this IServiceCollection services)
        {
            services.AddScoped<IGeoJsonParsingService, GeoJsonParsingService>();

            return services;
        }
    }
}
