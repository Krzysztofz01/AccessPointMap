using AccessPointMap.Web.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AccessPointMap.Web.Initialization
{
    public static class MiddlewareInitialization
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<TelemetryMiddleware>();

            app.UseMiddleware<ExceptionMiddleware>();

            return app;
        }
    }
}
