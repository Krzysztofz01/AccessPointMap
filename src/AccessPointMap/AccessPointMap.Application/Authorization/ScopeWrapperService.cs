using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace AccessPointMap.Application.Authorization
{
    public class ScopeWrapperService : IScopeWrapperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ScopeWrapperService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetIpAddress()
        {
            string ip = _httpContextAccessor.HttpContext.Request.Headers["X-Forwared-For"].FirstOrDefault();

            if (!ip.IsEmpty()) ip = ip.Split('.').First().Trim();

            if (ip.IsEmpty()) ip = Convert.ToString(_httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress);

            if (ip.IsEmpty()) ip = Convert.ToString(_httpContextAccessor.HttpContext.Request.HttpContext.Connection.LocalIpAddress);

            if (ip.IsEmpty()) ip = _httpContextAccessor.HttpContext.Request.Headers["REMOTE_ADDR"].FirstOrDefault();

            if (ip.IsEmpty()) ip = string.Empty;

            return ip;
        }

        public Guid GetUserId()
        {
            var claimsValue = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claimsValue is null) SystemAuthorizationException.Unauthenticated();

            return Guid.Parse(claimsValue);
        }

        public UserRole GetUserRole()
        {
            var claimsValue = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (claimsValue is null) SystemAuthorizationException.Unauthenticated();

            return (UserRole)Enum.Parse(typeof(UserRole), claimsValue);
        }
    }
}
