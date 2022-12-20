using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Identities;
using System;
using System.Linq;
using Xunit;

namespace AccessPointMap.Domain.Test
{
    public class IdentityTests
    {
        [Fact]
        public void IdentityShouldCreate()
        {
            Identity.Factory.Create(new Events.V1.IdentityCreated
            {
                Name = "John",
                Email = "john96@balto.com",
                PasswordHash = Guid.NewGuid().ToString(),
                IpAddress = "127.0.0.1"
            });
        }

        [Fact]
        public void IdentityCreateShouldThrow()
        {
            Assert.Throws<ValueObjectValidationException>(() => Identity.Factory.Create(new Events.V1.IdentityCreated
            {
                Name = "John",
                Email = "invalid email",
                PasswordHash = Guid.NewGuid().ToString(),
                IpAddress = "127.0.0.1"
            }));
        }

        [Fact]
        public void IdentityShouldDelete()
        {
            var identity = MockupIdentity();

            identity.Apply(new Events.V1.IdentityDeleted
            {
                Id = identity.Id
            });

            var expectedValue = DateTime.Now;

            Assert.Equal(expectedValue.Date, identity.DeletedAt.Value.Date);
        }

        [Fact]
        public void IdentityShouldActivate()
        {
            var identity = MockupIdentity();

            bool expcetedValue = false;

            Assert.Equal(expcetedValue, identity.Activation);

            identity.Apply(new Events.V1.IdentityActivationChanged
            {
                Id = identity.Id,
                Activated = true
            });

            bool exptectedValueAfter = true;

            Assert.Equal(exptectedValueAfter, identity.Activation);
        }

        [Fact]
        public void IdentityRoleShouldChange()
        {
            var identity = MockupIdentity();

            var expectedValue = UserRole.Default;

            Assert.Equal(expectedValue, identity.Role);

            identity.Apply(new Events.V1.IdentityRoleChanged
            {
                Id = identity.Id,
                Role = UserRole.Admin
            });

            var expectedValueAfter = UserRole.Admin;

            Assert.Equal(expectedValueAfter, identity.Role);
        }

        [Fact]
        public void IdentityAuthenticationShouldThrowWithoutActivation()
        {
            var identity = MockupIdentity();

            Assert.Throws<BusinessLogicException>(() => identity.Apply(new Events.V1.IdentityAuthenticated
            {
                Id = identity.Id,
                TokenHash = Guid.NewGuid().ToString(),
                TokenExpirationDays = 7,
                IpAddress = "111.111.111.111"
            }));
        }

        [Fact]
        public void IdentityShouldAuthenticate()
        {
            var identity = MockupIdentity();

            identity.Apply(new Events.V1.IdentityActivationChanged
            {
                Id = identity.Id,
                Activated = true
            });

            identity.Apply(new Events.V1.IdentityAuthenticated
            {
                Id = identity.Id,
                TokenHash = Guid.NewGuid().ToString(),
                TokenExpirationDays = 7,
                IpAddress = "111.111.111.111"
            });

            string exptectedValue = "111.111.111.111";

            Assert.Equal(exptectedValue, identity.LastLogin.IpAddress);
        }

        [Fact]
        public void IdentityPasswordShouldChange()
        {
            var password = Guid.NewGuid().ToString();

            var identity = Identity.Factory.Create(new Events.V1.IdentityCreated
            {
                Name = "John",
                Email = "john96@balto.com",
                PasswordHash = password,
                IpAddress = "127.0.0.1"
            });

            var replacementPassword = Guid.NewGuid().ToString();

            identity.Apply(new Events.V1.IdentityPasswordChanged
            {
                Id = identity.Id,
                PasswordHash = replacementPassword
            });

            string expectedValue = replacementPassword;

            Assert.Equal(expectedValue, identity.PasswordHash);
        }

        [Fact]
        public void IdentityTokenRefreshShouldThrowWithoutActivation()
        {
            var identity = MockupIdentity();

            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenAfterAuth = Guid.NewGuid().ToString();

            Assert.Throws<BusinessLogicException>(() => identity.Apply(new Events.V1.IdentityTokenRefreshed
            {
                Id = identity.Id,
                TokenHash = refreshToken,
                TokenExpirationDays = 7,
                ReplacementTokenHash = refreshTokenAfterAuth,
                IpAddress = identity.LastLogin.IpAddress
            }));
        }

        [Fact]
        public void IdentityTokenShouldRefresh()
        {
            var identity = MockupIdentity();

            identity.Apply(new Events.V1.IdentityActivationChanged
            {
                Id = identity.Id,
                Activated = true
            });

            var refreshToken = Guid.NewGuid().ToString();

            identity.Apply(new Events.V1.IdentityAuthenticated
            {
                Id = identity.Id,
                TokenExpirationDays = 7,
                TokenHash = refreshToken,
                IpAddress = identity.LastLogin.IpAddress
            });

            var refreshTokenAfterAuth = Guid.NewGuid().ToString();

            identity.Apply(new Events.V1.IdentityTokenRefreshed
            {
                Id = identity.Id,
                TokenHash = refreshToken,
                TokenExpirationDays = 7,
                ReplacementTokenHash = refreshTokenAfterAuth,
                IpAddress = identity.LastLogin.IpAddress
            });

            bool firstTokenRevokeStatus = identity.Tokens.Single(t => t.TokenHash == refreshToken).IsRevoked;
            bool expectedFirstTokenRevokeStatus = true;

            Assert.Equal(expectedFirstTokenRevokeStatus, firstTokenRevokeStatus);

            string firstTokenReplacedByToken = identity.Tokens.Single(t => t.TokenHash == refreshToken).ReplacedByTokenHash;
            string expectedFirstTokenReplacedByToken = refreshTokenAfterAuth;

            Assert.Equal(expectedFirstTokenReplacedByToken, firstTokenReplacedByToken);

            bool newTokenRevokeStatus = identity.Tokens.Single(t => t.TokenHash == refreshTokenAfterAuth).IsRevoked;
            bool expectedNewTokenRevokeStatus = false;

            Assert.Equal(newTokenRevokeStatus, expectedNewTokenRevokeStatus);
        }

        private Identity MockupIdentity(string name = "John", string email = "john96@balto.com")
        {
            return Identity.Factory.Create(new Events.V1.IdentityCreated
            {
                Name = name,
                Email = email,
                PasswordHash = Guid.NewGuid().ToString(),
                IpAddress = "127.0.0.1"
            });
        }
    }
}
