using AccessPointMap.Application.Extensions;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.AspNetCore.Http;
using System;
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
            return _httpContextAccessor.HttpContext.Request.GetIpAddressString();
        }

        public Guid GetUserId()
        {
            var claimsValue = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claimsValue is null) throw new InvalidOperationException("Can not access the current user identifier.");

            return Guid.Parse(claimsValue);
        }

        public Guid? GetUserIdOrDefault()
        {
            var claimsValue = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claimsValue is null) return null;

            return Guid.Parse(claimsValue);
        }

        public UserRole? GetUserRole()
        {
            var claimsValue = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (claimsValue is null) return null;

            return (UserRole)Enum.Parse(typeof(UserRole), claimsValue);
        }
    }
}
