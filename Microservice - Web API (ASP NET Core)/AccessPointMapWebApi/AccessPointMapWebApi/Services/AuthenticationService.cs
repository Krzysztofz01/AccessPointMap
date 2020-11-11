using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccessPointMapWebApi.Services
{
    public interface IAuthenticationService
    {
        public string GenerateToken(User user);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly string[] Roles = { "None", "Read", "Write", "Admin" };
        private readonly JsonWebTokenSettings jsonWebTokenSettings;

        public AuthenticationService(
            IOptions<JsonWebTokenSettings> jsonWebTokenSettings)
        {
            this.jsonWebTokenSettings = jsonWebTokenSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, GenerateRoleName(user))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jsonWebTokenSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(user.TokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRoleName(User user)
        {
            var permissionLevel = Roles[0];

            if (user.ReadPermission == true) permissionLevel = Roles[1];
            if (user.WritePermission == true) permissionLevel = Roles[2];
            if (user.AdminPermission == true) permissionLevel = Roles[3];

            return permissionLevel;
        }
    }
}
