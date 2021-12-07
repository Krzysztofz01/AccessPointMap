using AccessPointMap.Domain.Identities;
using System;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    public interface IScopeWrapperService
    {
        string GetIpAddress();
        Guid GetUserId();
        UserRole GetUserRole();
        void SetRefreshTokenCookie(string refreshTokenCookieValue);
        string GetRefreshTokenCookie();
    }
}
