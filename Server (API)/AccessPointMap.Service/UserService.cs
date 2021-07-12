using AccessPointMap.Domain;
using AccessPointMap.Repository;
using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using AccessPointMap.Service.Settings;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public class UserService : IUserService
    {
        private readonly JWTSettings jwtSettings;
        private readonly AdminSettings adminSettings;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(
            IOptions<JWTSettings> jwtSettings,
            IOptions<AdminSettings> adminSettings,
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.jwtSettings = jwtSettings.Value ??
                throw new ArgumentNullException(nameof(jwtSettings));

            this.adminSettings = adminSettings.Value ??
                throw new ArgumentNullException(nameof(adminSettings));

            this.userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IServiceResult> Activation(long userId)
        {
            var user = await userRepository.GetSingleUser(userId);
            if (user is null) return new ServiceResult(ResultStatus.NotFound);

            if (user.AdminPermission) return new ServiceResult(ResultStatus.NotPermited);

            user.IsActivated = !user.IsActivated;

            if(!user.IsActivated)
            {
                //Revoke all refresh tokens
                var refreshTokens = user.RefreshTokens.Where(x => x.Revoked == null);
                foreach (var refreshToken in refreshTokens)
                {
                    refreshToken.Revoked = DateTime.Now;
                    refreshToken.RevokedByIp = string.Empty;
                }
            }

            userRepository.UpdateState(user);

            if (await userRepository.Save() > 0) return new ServiceResult(ResultStatus.Sucess);
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<ServiceResult<AuthDto>> Authenticate(string email, string password, string ipAddress)
        {
            var user = await userRepository.GetSingleUserByEmail(email);
            if (user is null) return new ServiceResult<AuthDto>(ResultStatus.NotFound);

            if (!user.IsActivated) return new ServiceResult<AuthDto>(ResultStatus.NotPermited);

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                string token = GenerateJsonWebToken(user);

                var refreshToken = GenerateRefreshToken(user, ipAddress);

                user.LastLoginDate = DateTime.Now;
                user.LastLoginIp = ipAddress;
                user.RefreshTokens.Add(refreshToken);
                userRepository.UpdateState(user);

                if(await userRepository.Save() > 0)
                {
                    return new ServiceResult<AuthDto>(new AuthDto
                    {
                        Token = token,
                        RefreshToken = refreshToken.Token,
                        Email = user.Email
                    });
                }
                return new ServiceResult<AuthDto>(ResultStatus.Failed);
            }
            return new ServiceResult<AuthDto>(ResultStatus.NotPermited);
        }

        public async Task<IServiceResult> Delete(long userId)
        {
            var user = await userRepository.GetSingleUser(userId);
            if (user is null) return new ServiceResult(ResultStatus.NotFound);

            if (user.AdminPermission) return new ServiceResult(ResultStatus.NotPermited);

            userRepository.Remove(user);
            
            if (await userRepository.Save() > 0) return new ServiceResult(ResultStatus.Sucess);
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<ServiceResult<UserDto>> Get(long userId)
        {
            var user = await userRepository.GetSingleUser(userId);
            if (user is null) return new ServiceResult<UserDto>(ResultStatus.NotFound);

            var userMapped = mapper.Map<UserDto>(user);
            return new ServiceResult<UserDto>(userMapped);
        }

        public async Task<ServiceResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await userRepository.GetAll();
            var usersMapped = mapper.Map<IEnumerable<UserDto>>(users);

            return new ServiceResult<IEnumerable<UserDto>>(usersMapped);
        }

        public long GetUserIdFromPayload(IEnumerable<Claim> claims)
        {
            return Convert.ToInt64(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        public async Task<ServiceResult<AuthDto>> RefreshToken(string token, string ipAddress)
        {
            var user = await userRepository.GetUserWithToken(token);
            if (user is null) return new ServiceResult<AuthDto>(ResultStatus.NotFound);

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) return new ServiceResult<AuthDto>(ResultStatus.Failed);

            var newRefreshToken = GenerateRefreshToken(user, ipAddress);
            
            refreshToken.Revoked = DateTime.Now;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            user.RefreshTokens.Add(newRefreshToken);
            userRepository.UpdateState(user);

            if(await userRepository.Save() > 0)
            {
                var jwtToken = GenerateJsonWebToken(user);

                return new ServiceResult<AuthDto>(new AuthDto
                {
                    Token = jwtToken,
                    RefreshToken = newRefreshToken.Token,
                    Email = user.Email
                });
            }
            return new ServiceResult<AuthDto>(ResultStatus.Failed);
        }

        public async Task<IServiceResult> Register(string name, string email, string password, string ipAddress)
        {
            if(await userRepository.EmailAvailable(email))
            {
                var user = new User()
                {
                    Name = name,
                    Email = email,
                    LastLoginDate = DateTime.Now,
                    LastLoginIp = ipAddress
                };

                //Check if user is a pre-selected admin
                if (adminSettings.Emails.Any(x => x == email))
                {
                    user.AdminPermission = true;
                    user.IsActivated = true;
                }

                string salt = BCrypt.Net.BCrypt.GenerateSalt(5);
                user.Password = BCrypt.Net.BCrypt.HashPassword(password, salt);

                await userRepository.Add(user);
                if (await userRepository.Save() > 0) return new ServiceResult(ResultStatus.Sucess);
                return new ServiceResult(ResultStatus.Failed);
            }
            return new ServiceResult(ResultStatus.Conflict);
        }

        public async Task<IServiceResult> Reset(string email, string password, string ipAddress)
        {
            var user = await userRepository.GetSingleUserByEmail(email);
            if (user is null) return new ServiceResult(ResultStatus.NotFound);

            string salt = BCrypt.Net.BCrypt.GenerateSalt(5);
            user.Password = BCrypt.Net.BCrypt.HashPassword(password, salt);

            //Revoke all refresh tokens
            var refreshTokens = user.RefreshTokens.Where(x => x.Revoked == null);
            foreach(var refreshToken in refreshTokens)
            {
                refreshToken.Revoked = DateTime.Now;
                refreshToken.RevokedByIp = ipAddress;
            }

            userRepository.UpdateState(user);
            if (await userRepository.Save() > 0) return new ServiceResult(ResultStatus.Sucess);
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> RevokeToken(string token, string ipAddress)
        {
            var user = await userRepository.GetUserWithToken(token);
            if (user is null) return new ServiceResult(ResultStatus.NotFound);

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            refreshToken.Revoked = DateTime.Now;
            refreshToken.RevokedByIp = ipAddress;

            userRepository.UpdateState(user);
            if (await userRepository.Save() > 0) return new ServiceResult(ResultStatus.Sucess);
            return new ServiceResult(ResultStatus.Failed);
        }

        private string GenerateJsonWebToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, RoleParser(user))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(jwtSettings.TokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string RoleParser(User user)
        {
            if (user.AdminPermission) return "Admin";
            if (user.ModPermission) return "Mod";
            return "Default";
        }

        private RefreshToken GenerateRefreshToken(User user, string ipAddress)
        {
            using (var rngCryptoProviderService = new RNGCryptoServiceProvider())
            {
                var rndBytes = new byte[64];
                rngCryptoProviderService.GetBytes(rndBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(rndBytes),
                    Expires = DateTime.Now.AddDays(jwtSettings.RefreshTokenExpirationsDays),
                    Created = DateTime.Now,
                    CreatedByIp = ipAddress,
                    User = user
                };
            }
        }

        public async Task<IServiceResult> Update(UserDto user, long userId)
        {
            var exisitngUser = await userRepository.GetSingleUser(userId);
            if (exisitngUser is null) return new ServiceResult(ResultStatus.NotFound);

            bool changes = false;

            if(exisitngUser.ModPermission != user.ModPermission)
            {
                changes = true;
                exisitngUser.ModPermission = user.ModPermission;
            }

            if(exisitngUser.Name != user.Name)
            {
                changes = true;
                exisitngUser.Name = user.Name;
            }

            if(changes)
            {
                userRepository.UpdateState(exisitngUser);
                if (await userRepository.Save() == 0) return new ServiceResult(ResultStatus.Failed);
            }
            return new ServiceResult(ResultStatus.Sucess);
        }

        public UserDto GetUserFromPayload(IEnumerable<Claim> claims)
        {
            var idClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim is null) throw new ArgumentException(nameof(idClaim));

            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim is null) throw new ArgumentException(nameof(emailClaim));

            return new UserDto
            {
                Id = Convert.ToInt64(idClaim.Value),
                Email = emailClaim.Value
            };
        }
    }
}
