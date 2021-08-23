using AccessPointMap.Service.Maintenance;
using AccessPointMap.Service.Maintenance.Models;
using AccessPointMap.Web.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.Web.Middleware
{
    public class TelemetryMiddleware
    {
        private readonly RequestDelegate _next;
        
        public TelemetryMiddleware(RequestDelegate next)
        {
            _next = next ??
                throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context, ITelemetryService telemetryService)
        {
            string requestUrl = context.Request.GetUri().ToString();

            await _next(context);

            await telemetryService.LogEvent(Report.Factory.Create(requestUrl, nameof(TelemetryMiddleware)));
        }
    }
}
