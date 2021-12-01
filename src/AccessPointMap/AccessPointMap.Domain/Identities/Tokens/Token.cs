using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System;
using static AccessPointMap.Domain.Identities.Events;

namespace AccessPointMap.Domain.Identities.Tokens
{
    public class Token : Entity
    {
        public string TokenHash { get; private init; }

        public DateTime Expires { get; private set; }
        public DateTime Created { get; private set; }
        public string CreatedByIpAddress { get; private set; }
        public DateTime? Revoked { get; private set; }
        public string RevokedByIpAddress { get; private set; }
        public bool IsRevoked { get; private set; }
        public string ReplacedByTokenHash { get; private set; }

        public bool IsExpired => DateTime.Now >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;


        protected override void Handle(IEventBase @event)
        {
            switch(@event)
            {
                case V1.IdentityTokenRevoked e: When(e); break;
                case V1.TokenReplacedByTokenChanged e: When(e); break;
                default: throw new BusinessLogicException("This entity can not handle this type of event.");
            }
        }

        protected override void Validate()
        {
            bool isNull = TokenHash.IsEmpty();

            if (isNull)
                throw new BusinessLogicException("The token entity properties can not be null.");
        }

        private void When(V1.IdentityTokenRevoked @event)
        {
            IsRevoked = true;
            Revoked = DateTime.Now;
            RevokedByIpAddress = @event.IpAddress;
        }

        private void When(V1.TokenReplacedByTokenChanged @event)
        {
            ReplacedByTokenHash = @event.ReplacementTokenHash;
        }

        private Token() { }

        public static class Factory
        {
            public static Token Create(V1.TokenCreated @event)
            {
                return new Token
                {
                    TokenHash = @event.TokenHash,
                    Expires = DateTime.Now.AddDays(@event.TokenExpirationDays),
                    Created = DateTime.Now,
                    CreatedByIpAddress = @event.IpAddress,
                    IsRevoked = false,
                    Revoked = null
                };
            }
        }
    }
}
