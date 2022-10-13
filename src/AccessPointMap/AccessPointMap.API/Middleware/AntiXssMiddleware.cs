using AccessPointMap.Application.Extensions;
using Ganss.Xss;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccessPointMap.API.Middleware
{
    public class AntiXssMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AntiXssMiddleware> _logger;

        public AntiXssMiddleware(
            RequestDelegate next,
            ILogger<AntiXssMiddleware> logger)
        {
            _next = next ??
                throw new ArgumentNullException(nameof(next));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            using var streamReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true);

            var rawRequestBody = await streamReader.ReadToEndAsync();
            var newLineSafeRawRequestBody = rawRequestBody.Replace("\r\n", "\n");

            var sanitizer = new HtmlSanitizer();
            var sanitizedRequestBody = sanitizer.Sanitize(rawRequestBody);

            if (newLineSafeRawRequestBody != sanitizedRequestBody)
            {
                _logger.LogWarning("Request from address: {IpAddress} requesting the resource under: {Uri} blocked by XSS middleware.",
                    httpContext.Request.GetIpAddressString(),
                    httpContext.Request.GetRequestUri());

                throw new BadHttpRequestException("XSS injection detected.");
            } 

            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next.Invoke(httpContext);
        }
    }
}
