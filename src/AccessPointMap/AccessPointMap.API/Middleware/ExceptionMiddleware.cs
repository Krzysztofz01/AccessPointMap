using AccessPointMap.API.Services;
using AccessPointMap.Application.Extensions;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccessPointMap.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHealthStatusService _healthStatusService;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHealthStatusService healthStatusService)
        {
            _next = next ??
                throw new ArgumentNullException(nameof(next));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            _healthStatusService = healthStatusService ??
                throw new ArgumentNullException(nameof(healthStatusService));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                LogCurrentBehaviour(context, "Request piped to the global error filter.");

                await _next(context);
            }
            catch (TaskCanceledException ex)
            {
                LogCurrentBehaviour(context, "Request has been canceled from the client-side.");

                await HandleRequestException(context, ex);
            }
            catch (DomainException ex)
            {
                LogCurrentBehaviourForException(
                    context,
                    "Request caused an unexpected failure of the business logic.",
                    ex);

                _healthStatusService.SetHealthStatusDegraded();

                await HandleRequestException(context, "Unexpected busniess logic failure.");
            }
            catch (IntegrationException ex)
            {
                LogCurrentBehaviourForException(
                    context,
                    "Request caused an unexpected failure of one of the integration services.",
                    ex);

                _healthStatusService.SetHealthStatusDegraded();

                await HandleRequestException(context, "Unexpected integration service failure.");
            }
            catch (Exception ex)
            {
                LogCurrentBehaviourForException(
                    context,
                    "Request caused an undefined unexpected failure of the application.",
                    ex);

                _healthStatusService.SetHealthStatusDegraded();

                await HandleRequestException(context, "Unexpected application failure.");
            }
        }

        private static async Task HandleRequestException(HttpContext context, TaskCanceledException _)
        {
            context.Response.ContentType = "application/json";

            // NOTE: Unofficial status code. Nginx is using 499 status code as "Client Closed Request"
            context.Response.StatusCode = 499;

            var problemDetails = GetProblemDetails(
                context,
                499,
                "Task cancelled. Client closed the request.");

            var serializedResponse = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(serializedResponse);
        }

        private static async Task HandleRequestException(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = GetProblemDetails(
                context,
                (int)HttpStatusCode.InternalServerError,
                message);

            var serializedResponse = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(serializedResponse);
        }

        private static ProblemDetails GetProblemDetails(HttpContext context, int statusCode, string title)
        {
            var problemDetailsFactory = context?.RequestServices?.GetRequiredService<ProblemDetailsFactory>();
            if (problemDetailsFactory is not null)
            {
                var problemDetails = problemDetailsFactory.CreateProblemDetails(context);
                problemDetails.Detail = null;
                problemDetails.Title = title;
                problemDetails.Status = statusCode;
                return problemDetails;
            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Detail = null,
                    Instance = context.Request.Path,
                    Status = statusCode,
                    Title = title,
                    Type = null,
                };

                var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
                if (traceId is not null)
                {
                    problemDetails.Extensions["traceId"] = traceId;
                }

                return problemDetails;
            }
        }

        private void LogCurrentBehaviour(HttpContext context, string beahaviorDescription)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                string hostAddress = context.Request.GetIpAddressString() ?? string.Empty;
                string requestPath = context.Request.GetRequestUri().AbsolutePath ?? string.Empty;

                _logger.LogInformation("Middleware: {MiddlewareName} | Behaviour: {BehaviourDescription} | Path: {RequestPath} | Host: {HostAddress}",
                        GetType().Name,
                        beahaviorDescription,
                        requestPath,
                        hostAddress);

            }
        }

        private void LogCurrentBehaviourForException(HttpContext context, string beahaviorDescription, Exception exception)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                string hostAddress = context.Request.GetIpAddressString() ?? string.Empty;
                string requestPath = context.Request.GetRequestUri().AbsolutePath ?? string.Empty;

                _logger.LogError("Middleware: {MiddlewareName} | Behaviour: {BehaviourDescription} | Path: {RequestPath} | Host: {HostAddress}\n    {Exception}",
                        GetType().Name,
                        beahaviorDescription,
                        requestPath,
                        hostAddress,
                        exception);
            }
        }
    }
}
