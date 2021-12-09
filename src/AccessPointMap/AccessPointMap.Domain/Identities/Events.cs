using AccessPointMap.Domain.Core.Events;
using System;

namespace AccessPointMap.Domain.Identities
{
    public static class Events
    {
        public static class V1
        {
            public class IdentityCreated : IEventBase
            {
                public string Name { get; set; }
                public string Email { get; set; }
                public string PasswordHash { get; set; }
                public string IpAddress { get; set; }
            }

            public class IdentityDeleted : IEvent
            {
                public Guid Id { get; set; }
            }

            public class IdentityActivationChanged : IEvent
            {
                public Guid Id { get; set; }
                public bool Activated { get; set; }
            }

            public class IdentityRoleChanged : IEvent
            {
                public Guid Id { get; set; }
                public UserRole Role { get; set; }
            }

            public class IdentityAuthenticated : IEvent
            {
                public Guid Id { get; set; }
                public string TokenHash { get; set; }
                public string IpAddress { get; set; }
                public int TokenExpirationDays { get; set; }
            }

            public class IdentityPasswordChanged : IEvent
            {
                public Guid Id { get; set; }
                public string PasswordHash { get; set; }
            }

            public class IdentityTokenRefreshed : IEvent
            {
                public Guid Id { get; set; }
                public string TokenHash { get; set; }
                public string ReplacementTokenHash { get; set; }
                public string IpAddress { get; set; }
                public int TokenExpirationDays { get; set; }
            }

            public class IdentityTokenRevoked : IEvent
            {
                public Guid Id { get; set; }
                public string TokenHash { get; set; }
                public string IpAddress { get; set; }
            }

            public class TokenReplacedByTokenChanged : IEventBase
            {
                public string TokenHash { get; set; }
                public string ReplacementTokenHash { get; set; }
            }

            public class TokenCreated : IEventBase
            {
                public string IpAddress { get; set; }
                public string TokenHash { get; set; }
                public int TokenExpirationDays { get; set; }
            }
        }
    }
}
