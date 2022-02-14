using AccessPointMap.API.Extensions;
using AccessPointMap.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccessPointMap.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next ??
                throw new ArgumentNullException(nameof(next));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogDebug($"{ context.Request.GetCurrentIp() } - { context.Request.GetCurrentUri() }");

                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                InvalidOperationException _ => (int)HttpStatusCode.BadRequest,
                ArgumentException _ => (int)HttpStatusCode.BadRequest,
                BusinessLogicException _ => (int)HttpStatusCode.BadRequest,
                ValueObjectValidationException _ => (int)HttpStatusCode.BadRequest,
                BadHttpRequestException _ => (int)HttpStatusCode.BadRequest,
                SystemAuthorizationException _ => (int)HttpStatusCode.Forbidden,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            var serializedResponse = JsonSerializer.Serialize(new
            {
                context.Response.StatusCode,
                ex.Message
            });

            await context.Response.WriteAsync(serializedResponse);
        }
    }
}
