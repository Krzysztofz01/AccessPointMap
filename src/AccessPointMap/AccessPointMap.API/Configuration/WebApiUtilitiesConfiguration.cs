using AccessPointMap.API.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;

namespace AccessPointMap.API.Configuration
{
    internal static class WebApiUtilitiesConfiguration
    {
        public static IServiceCollection AddWebApiUtilities(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddHttpContextAccessor();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new AntiXssConverter());
            });

            return services;
        }

        public static IApplicationBuilder UseWebApiUtilities(this IApplicationBuilder app)
        {
            app.UseResponseCompression();

            return app;
        }

    }
}
