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
        private readonly ITelemetryService _telemetryService;

        public TelemetryMiddleware(
            RequestDelegate next,
            ITelemetryService telemetryService)
        {
            _next = next ??
                throw new ArgumentNullException(nameof(next));

            _telemetryService = telemetryService ??
                throw new ArgumentNullException(nameof(telemetryService));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string requestUrl = context.Request.GetUri().ToString();

            await _next(context);

            await _telemetryService.LogEvent(Report.Factory.Create(requestUrl, nameof(TelemetryMiddleware)));
        }
    }
}
