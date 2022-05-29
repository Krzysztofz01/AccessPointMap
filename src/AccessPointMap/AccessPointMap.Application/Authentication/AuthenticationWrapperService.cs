using AccessPointMap.Application.Settings;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AccessPointMap.Application.Authentication
{
    public class AuthenticationWrapperService : IAuthenticationWrapperService
    {
        private readonly JsonWebTokenSettings _jwtSettings;

        public AuthenticationWrapperService(IOptions<JsonWebTokenSettings> jsonWebTokenSettings)
        {
            _jwtSettings = jsonWebTokenSettings.Value ??
                throw new ArgumentNullException(nameof(jsonWebTokenSettings));
        }

        public string GenerateJsonWebToken(Identity identity)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, identity.Id.ToString()),
                new Claim(ClaimTypes.Email, identity.Email),
                new Claim(ClaimTypes.Name, identity.Name),
                new Claim(ClaimTypes.Role, identity.Role)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.TokenSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_jwtSettings.BearerTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        public string HashPassword(string contractPasswordString)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(5);

            return BCrypt.Net.BCrypt.HashPassword(contractPasswordString, salt);
        }

        public string HashString(string contractValue)
        {
            
            using var sha = SHA256.Create();
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(contractValue));

            var sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public bool ValidatePasswords(string contractPasswordString, string contractPasswordStringRepeat)
        {
            return contractPasswordString == contractPasswordStringRepeat;
        }

        public bool VerifyPasswordHashes(string contractPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(contractPassword, storedPasswordHash);
        }
    }
}
