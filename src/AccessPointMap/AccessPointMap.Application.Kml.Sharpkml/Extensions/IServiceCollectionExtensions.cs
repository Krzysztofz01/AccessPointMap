using AccessPointMap.Application.Kml.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.Application.Kml.Sharpkml.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSharpkmlKmlParser(this IServiceCollection services)
        {
            services.AddScoped<IKmlParsingService, SharpkmlKmlParsingService>();

            return services;
        }
    }
}
