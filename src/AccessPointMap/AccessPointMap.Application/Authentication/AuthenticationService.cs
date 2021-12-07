using AccessPointMap.Application.Settings;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityRepository _identityRepository;
        private readonly IAuthenticationDataAccessService _authenticationDataAccessService;
        private readonly IAuthenticationWrapperService _authenticationWrapperService;
        private readonly IScopeWrapperService _scopeWrapperService;
        private readonly JsonWebTokenSettings _jwtSettings;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IIdentityRepository identityRepository,
            IAuthenticationDataAccessService authenticationDataAccessService,
            IAuthenticationWrapperService authenticationWrapperService,
            IScopeWrapperService scopeWrapperService,
            IOptions<JsonWebTokenSettings> jsonWebTokenSettings)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _identityRepository = identityRepository ??
                throw new ArgumentNullException(nameof(identityRepository));

            _authenticationDataAccessService = authenticationDataAccessService ??
                throw new ArgumentNullException(nameof(authenticationDataAccessService));

            _authenticationWrapperService = authenticationWrapperService ??
                throw new ArgumentNullException(nameof(authenticationWrapperService));

            _scopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));

            _jwtSettings = jsonWebTokenSettings.Value ??
                throw new ArgumentNullException(nameof(jsonWebTokenSettings));
        }

        public async Task<Responses.V1.Login> Login(Requests.V1.Login request)
        {
            var identity = await _authenticationDataAccessService.GetUserByEmail(request.Email);

            if (!_authenticationWrapperService.VerifyPasswordHashes(request.Password, identity.PasswordHash))
                throw new InvalidOperationException("Invalid authentication credentials.");

            string bearerToken = _authenticationWrapperService.GenerateJsonWebToken(identity);

            string refreshToken = _authenticationWrapperService.GenerateRefreshToken();

            string refreshTokenHash = _authenticationWrapperService.HashString(refreshToken);

            identity.Apply(new Events.V1.IdentityAuthenticated
            {
                Id = identity.Id,
                TokenExpirationDays = _jwtSettings.RefreshTokenExpirationDays,
                IpAddress = _scopeWrapperService.GetIpAddress(),
                TokenHash = refreshTokenHash
            });

            await _unitOfWork.Commit();

            _scopeWrapperService.SetRefreshTokenCookie(refreshToken);

            return new Responses.V1.Login
            {
                JsonWebToken = bearerToken
            };
        }

        public async Task Logout(Requests.V1.Logout _)
        {
            var identity = await _identityRepository.Get(_scopeWrapperService.GetUserId());

            string refreshTokenHash = _authenticationWrapperService.HashString(_scopeWrapperService.GetRefreshTokenCookie());

            identity.Apply(new Events.V1.IdentityTokenRevoked
            {
                Id = identity.Id,
                IpAddress = _scopeWrapperService.GetIpAddress(),
                TokenHash = refreshTokenHash
            });

            await _unitOfWork.Commit();
        }

        public async Task PasswordReset(Requests.V1.PasswordReset request)
        {
            var identity = await _identityRepository.Get(_scopeWrapperService.GetUserId());

            if (!_authenticationWrapperService.ValidatePasswords(request.Password, request.PasswordRepeat))
                throw new InvalidOperationException("Invalid authentication credentials");

            var passwordHash = _authenticationWrapperService.HashPassword(request.Password);

            identity.Apply(new Events.V1.IdentityPasswordChanged
            {
                Id = identity.Id,
                PasswordHash = passwordHash
            });

            await _unitOfWork.Commit();
        }

        public async Task<Responses.V1.Refresh> Refresh(Requests.V1.Refresh _)
        {
            var refreshToken = _scopeWrapperService.GetRefreshTokenCookie();

            if (refreshToken.IsEmpty())
                throw new InvalidOperationException("Invalid authentication credentials.");

            var refreshTokenHash = _authenticationWrapperService.HashString(refreshToken);

            var identity = await _authenticationDataAccessService.GetUserByRefreshToken(refreshTokenHash);

            var bearerToken = _authenticationWrapperService.GenerateJsonWebToken(identity);

            var replacementToken = _authenticationWrapperService.GenerateRefreshToken();

            var replacementTokenHash = _authenticationWrapperService.HashString(replacementToken);

            identity.Apply(new Events.V1.IdentityTokenRefreshed
            {
                Id = identity.Id,
                IpAddress = _scopeWrapperService.GetIpAddress(),
                TokenExpirationDays = _jwtSettings.RefreshTokenExpirationDays,
                TokenHash = refreshTokenHash,
                ReplacementTokenHash = replacementTokenHash
            });

            await _unitOfWork.Commit();

            _scopeWrapperService.SetRefreshTokenCookie(replacementToken);

            return new Responses.V1.Refresh
            {
                JsonWebToken = bearerToken
            };
        }

        public async Task Register(Requests.V1.Register request)
        {
            if (await _authenticationDataAccessService.UserWithEmailExsits(request.Email))
                throw new InvalidOperationException("Invalid authentication credentials");

            if (!_authenticationWrapperService.ValidatePasswords(request.Password, request.PasswordRepeat))
                throw new InvalidOperationException("Invalid authentication credentials");

            string passwordHash = _authenticationWrapperService.HashPassword(request.Password);

            var identity = Identity.Factory.Create(new Events.V1.IdentityCreated
            {
                Email = request.Email,
                IpAddress = _scopeWrapperService.GetIpAddress(),
                Name = request.Name,
                PasswordHash = passwordHash
            });

            await _identityRepository.Add(identity);

            await _unitOfWork.Commit();
        }
    }
}