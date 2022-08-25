using AccessPointMap.Application.Extensions;
using AccessPointMap.Domain.Core.Exceptions;
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
