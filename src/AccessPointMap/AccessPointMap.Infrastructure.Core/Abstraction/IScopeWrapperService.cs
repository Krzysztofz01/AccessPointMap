﻿using AccessPointMap.Domain.Identities;
using System;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    public interface IScopeWrapperService
    {
        string GetIpAddress();
        Guid GetUserId();
        Guid? GetUserIdOrDefault();
        UserRole? GetUserRole();
    }
}
