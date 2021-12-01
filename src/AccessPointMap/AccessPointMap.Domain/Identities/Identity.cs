using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using AccessPointMap.Domain.Identities.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using static AccessPointMap.Domain.Identities.Events;

namespace AccessPointMap.Domain.Identities
{
    public class Identity : AggregateRoot
    {
        public IdentityName Name { get; private set; }
        public IdentityEmail Email { get; private set; }
        public IdentityPasswordHash PasswordHash { get; private set; }
        public IdentityLastLogin LastLogin { get; private set; }
        public IdentityRole Role { get; private set; }
        public IdentityActivation Activation { get; private set; }

        private readonly List<Token> _tokens;
        public IReadOnlyCollection<Token> Tokens => _tokens.AsReadOnly();

        protected override void Handle(IEventBase @event)
        {
            switch(@event)
            {
                case V1.IdentityDeleted e: When(e); break;
                case V1.IdentityActivationChanged e: When(e); break;
                case V1.IdentityRoleChanged e: When(e); break;
                case V1.IdentityAuthenticated e: When(e); break;
                case V1.IdentityPasswordChanged e: When(e); break;
                case V1.IdentityTokenRefreshed e: When(e); break;
                case V1.IdentityTokenRevoked e: When(e); break;
                default: throw new BusinessLogicException("This entity can not handle this type of event.");
            }
        }

        protected override void Validate()
        {
            bool isNull = Name == null || Email == null || PasswordHash == null ||
                LastLogin == null || Role == null || Activation == null || Tokens == null;

            if (isNull)
                throw new BusinessLogicException("The identity aggregate properties can not be null.");
        }

        private void When(V1.IdentityDeleted _)
        {
            DeletedAt = DateTime.Now;

            _tokens.Clear();
        }

        private void When(V1.IdentityActivationChanged @event)
        {
            Activation = @event.Activated ?
                IdentityActivation.Active : IdentityActivation.Inactive;
        }

        private void When(V1.IdentityRoleChanged @event)
        {
            Role = IdentityRole.FromUserRole(@event.Role);
        }

        private void When(V1.IdentityAuthenticated @event)
        {
            if (!Activation)
                throw new InvalidOperationException("Given identity is not active.");

            LastLogin = IdentityLastLogin.FromString(@event.IpAddress);

            _tokens.Add(Token.Factory.Create(new V1.TokenCreated
            {
                TokenHash = @event.TokenHash,
                TokenExpirationDays = @event.TokenExpirationDays,
                IpAddress = @event.IpAddress
            }));
        }

        private void When(V1.IdentityPasswordChanged @event)
        {
            PasswordHash = IdentityPasswordHash.FromString(@event.PasswordHash);

            var activeTokens = _tokens.Where(t => t.IsActive);
            
            foreach(var token in activeTokens)
            {
                token.Apply(new V1.IdentityTokenRevoked
                {
                    Id = Id,
                    IpAddress = string.Empty,
                    TokenHash = token.TokenHash
                });
            }
        }

        private void When(V1.IdentityTokenRefreshed @event)
        {
            if (!Activation)
                throw new InvalidOperationException("Given identity is not active.");

            //Create and push a new token
            var replacementToken = Token.Factory.Create(new V1.TokenCreated
            {
                IpAddress = @event.IpAddress,
                TokenExpirationDays = @event.TokenExpirationDays,
                TokenHash = @event.ReplacementTokenHash
            });

            _tokens.Add(replacementToken);

            //Mark the old token as revoked
            var token = _tokens.Single(t => t.TokenHash == @event.TokenHash);

            token.Apply(new V1.IdentityTokenRevoked
            {
                Id = @event.Id,
                TokenHash = token.TokenHash,
                IpAddress = @event.IpAddress
            });

            //Indicate that the old token was replaced by the new one
            token.Apply(new V1.TokenReplacedByTokenChanged
            {
                TokenHash = token.TokenHash,
                ReplacementTokenHash = @event.ReplacementTokenHash
            });
        }

        private void When(V1.IdentityTokenRevoked @event)
        {
            var token = _tokens.Single(t => t.TokenHash == @event.TokenHash);

            token.Apply(new V1.IdentityTokenRevoked
            {
                Id = @event.Id,
                TokenHash = token.TokenHash,
                IpAddress = @event.IpAddress
            });
        }

        private Identity()
        {
            _tokens = new List<Token>();
        }

        public static class Factory
        {
            public static Identity Create(V1.IdentityCreated @event)
            {
                return new Identity
                {
                    Name = IdentityName.FromString(@event.Name),
                    Email = IdentityEmail.FromString(@event.Email),
                    PasswordHash = IdentityPasswordHash.FromString(@event.PasswordHash),
                    LastLogin = IdentityLastLogin.FromString(@event.IpAddress),
                    Role = IdentityRole.Default,
                    Activation = IdentityActivation.Inactive
                };
            }
        }
    }
}
