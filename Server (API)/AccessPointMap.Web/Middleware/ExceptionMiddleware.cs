using AccessPointMap.Service.Maintenance;
using AccessPointMap.Service.Maintenance.Models;
using AccessPointMap.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AccessPointMap.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITelemetryService _telemetryService;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ITelemetryService telemetryService,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next ??
                throw new ArgumentNullException(nameof(next));

            _telemetryService = telemetryService ??
                throw new ArgumentNullException(nameof(telemetryService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string requestUrl = context.Request.GetUri().ToString();

            try
            {
                _logger.LogDebug(requestUrl);
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failure on: {e}");

                await HandleException(context, e);

                await _telemetryService.LogEvent(Report.Factory.Create(requestUrl, nameof(ExceptionMiddleware), e));
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case ArgumentNullException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ArgumentException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(new { message = exception?.Message, code = context.Response.StatusCode });

            await context.Response.WriteAsync(result);
        }
    }
}
