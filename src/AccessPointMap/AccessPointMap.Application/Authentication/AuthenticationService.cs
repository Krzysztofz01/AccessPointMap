using AccessPointMap.Application.Core;
using AccessPointMap.Application.Settings;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using static AccessPointMap.Application.Authentication.AuthenticationErrors;

namespace AccessPointMap.Application.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationDataAccessService _authenticationDataAccessService;
        private readonly IAuthenticationWrapperService _authenticationWrapperService;
        private readonly IScopeWrapperService _scopeWrapperService;
        private readonly JsonWebTokenSettings _jwtSettings;
        private readonly AuthorizationSettings _authorizationSettings;

        public AuthenticationService(
            IUnitOfWork unitOfWork,
            IAuthenticationDataAccessService authenticationDataAccessService,
            IAuthenticationWrapperService authenticationWrapperService,
            IScopeWrapperService scopeWrapperService,
            IOptions<JsonWebTokenSettings> jsonWebTokenSettings,
            IOptions<AuthorizationSettings> authorizationSettings)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _authenticationDataAccessService = authenticationDataAccessService ??
                throw new ArgumentNullException(nameof(authenticationDataAccessService));

            _authenticationWrapperService = authenticationWrapperService ??
                throw new ArgumentNullException(nameof(authenticationWrapperService));

            _scopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));

            _jwtSettings = jsonWebTokenSettings.Value ??
                throw new ArgumentNullException(nameof(jsonWebTokenSettings));

            _authorizationSettings = authorizationSettings.Value ??
                throw new ArgumentNullException(nameof(authorizationSettings));
        }

        public async Task<Result<Responses.V1.Login>> Login(Requests.V1.Login request, CancellationToken cancellationToken = default)
        {
            var identity = await _authenticationDataAccessService.GetUserOrDefaultByEmailAsync(request.Email, cancellationToken);
            
            if (identity is null) return Result.Failure<Responses.V1.Login>(AuthenticationCredentialsError.IdentityWithEmailNotFound);

            if (!_authenticationWrapperService.VerifyPasswordHashes(request.Password, identity.PasswordHash))
                return Result.Failure<Responses.V1.Login>(AuthenticationCredentialsError.IdentityAccessWithWrongPassword);

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

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success(new Responses.V1.Login
            {
                JsonWebToken = bearerToken,
                RefreshToken = refreshToken
            });
        }

        public async Task<Result> Logout(Requests.V1.Logout request, CancellationToken cancellationToken = default)
        {
            var identity = await _unitOfWork.IdentityRepository.GetAsync(_scopeWrapperService.GetUserId(), cancellationToken);
            if (identity is null) return Result.Failure(AuthenticationIdentityError.UserNotFound);

            string refreshTokenHash = _authenticationWrapperService.HashString(request.RefreshToken);

            identity.Apply(new Events.V1.IdentityTokenRevoked
            {
                Id = identity.Id,
                IpAddress = _scopeWrapperService.GetIpAddress(),
                TokenHash = refreshTokenHash
            });

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> PasswordReset(Requests.V1.PasswordReset request, CancellationToken cancellationToken = default)
        {
            var identity = await _unitOfWork.IdentityRepository.GetAsync(_scopeWrapperService.GetUserId(), cancellationToken);
            if (identity is null) return Result.Failure(AuthenticationIdentityError.UserNotFound);

            if (!_authenticationWrapperService.ValidatePasswords(request.Password, request.PasswordRepeat))
                return Result.Failure(AuthenticationCredentialsError.CredentialsNotMatching);

            var passwordHash = _authenticationWrapperService.HashPassword(request.Password);

            identity.Apply(new Events.V1.IdentityPasswordChanged
            {
                Id = identity.Id,
                PasswordHash = passwordHash
            });

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<Responses.V1.Refresh>> Refresh(Requests.V1.Refresh request, CancellationToken cancellationToken = default)
        {
            if (request.RefreshToken.IsEmpty()) return Result.Failure<Responses.V1.Refresh>(AuthenticationCredentialsError.InvalidRefreshToken);

            var refreshTokenHash = _authenticationWrapperService.HashString(request.RefreshToken);

            var identity = await _authenticationDataAccessService.GetUserOrDefaultByActiveRefreshTokenAsync(refreshTokenHash, cancellationToken);
            if (identity is null) return Result.Failure<Responses.V1.Refresh>(AuthenticationCredentialsError.InvalidRefreshToken);

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

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success(new Responses.V1.Refresh
            {
                JsonWebToken = bearerToken,
                RefreshToken = replacementToken
            });
        }

        public async Task<Result> Register(Requests.V1.Register request, CancellationToken cancellationToken = default)
        {
            if (await _authenticationDataAccessService.AnyUserWithEmailExsitsAsync(request.Email, cancellationToken))
                return Result.Failure(AuthenticationCredentialsError.IdentityWithEmailAlreadyExist);

            if (!_authenticationWrapperService.ValidatePasswords(request.Password, request.PasswordRepeat))
                return Result.Failure(AuthenticationCredentialsError.CredentialsNotMatching);

            string passwordHash = _authenticationWrapperService.HashPassword(request.Password);

            var identity = Identity.Factory.Create(new Events.V1.IdentityCreated
            {
                Email = request.Email,
                IpAddress = _scopeWrapperService.GetIpAddress(),
                Name = request.Name,
                PasswordHash = passwordHash
            });

            if (_authorizationSettings.PromoteFirstAccount && !await _authenticationDataAccessService.AnyUserExistsAsync(cancellationToken))
            {
                identity.Apply(new Events.V1.IdentityActivationChanged
                {
                    Id = identity.Id,
                    Activated = true
                });

                identity.Apply(new Events.V1.IdentityRoleChanged
                {
                    Id = identity.Id,
                    Role = UserRole.Admin
                });
            }

            await _unitOfWork.IdentityRepository.AddAsync(identity, cancellationToken);

            await _unitOfWork.Commit(cancellationToken);

            return Result.Success();
        }
    }
}