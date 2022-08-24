using AccessPointMap.Domain.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace AccessPointMap.Application.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetRequestUri(this HttpRequest request)
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

        public static string GetIpAddressString(this HttpRequest httpRequest)
        {
            string ip = httpRequest.Headers["X-Forwared-For"].FirstOrDefault();

            if (!ip.IsEmpty()) ip = ip.Split('.').First().Trim();

            if (ip.IsEmpty()) ip = Convert.ToString(httpRequest.HttpContext.Connection.RemoteIpAddress);

            if (ip.IsEmpty()) ip = Convert.ToString(httpRequest.HttpContext.Connection.LocalIpAddress);

            if (ip.IsEmpty()) ip = httpRequest.Headers["REMOTE_ADDR"].FirstOrDefault();

            if (ip.IsEmpty()) ip = string.Empty;

            return ip;
        }
    }
}
