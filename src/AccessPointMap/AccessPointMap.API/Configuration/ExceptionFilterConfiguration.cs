using AccessPointMap.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AccessPointMap.API.Configuration
{
    internal static class ExceptionFilterConfiguration
    {
        public static IApplicationBuilder UseExceptionFilter(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
