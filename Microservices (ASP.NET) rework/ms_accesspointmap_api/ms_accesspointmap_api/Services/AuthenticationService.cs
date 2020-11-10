using ms_accesspointmap_api.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;
using ms_accesspointmap_api.Settings;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ms_accesspointmap_api.Services
{
    public interface IAuthenticationService
    {
        public string GenerateToken(Users user);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly string[] Roles = { "None", "Read", "Write", "ReadWrite", "Admin" };
        private readonly JsonWebTokenSettings jsonWebTokenSettings;

        public AuthenticationService(
            IOptions<JsonWebTokenSettings> jsonWebTokenSettings)
        {
            this.jsonWebTokenSettings = jsonWebTokenSettings.Value;
        }

        public string GenerateToken(Users user)
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

        private string GenerateRoleName(Users user)
        {
            var read = user.ReadPermission;
            var write = user.WritePermission;
            var admin = user.AdminPermission;

            if(read || write || admin)
            {
                if ((read) && (!write) && (!admin)) return Roles[1];
                if ((!read) && (write) && (!admin)) return Roles[2];
                if ((read) && (write) && (!admin)) return Roles[3];
                if ((read) && (write) && (admin)) return Roles[4];
            }
            return Roles[0];
        }
    }
}
