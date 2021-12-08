using AccessPointMap.Domain.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace AccessPointMap.API.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetCurrentUri(this HttpRequest request)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80),
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }

        public static string GetCurrentIp(this HttpRequest request)
        {
            string ip = request.Headers["X-Forwared-For"].FirstOrDefault();

            if (!ip.IsEmpty()) ip = ip.Split('.').First().Trim();

            if (ip.IsEmpty()) ip = Convert.ToString(request.HttpContext.Connection.RemoteIpAddress);

            if (ip.IsEmpty()) ip = Convert.ToString(request.HttpContext.Connection.LocalIpAddress);

            if (ip.IsEmpty()) ip = request.Headers["REMOTE_ADDR"].FirstOrDefault();

            if (ip.IsEmpty()) ip = "Unknown";

            return ip;
        }
    }
}
