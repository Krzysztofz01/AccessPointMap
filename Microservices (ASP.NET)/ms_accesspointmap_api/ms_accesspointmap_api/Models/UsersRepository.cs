using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Models
{
    public class UsersRepository : IUsersRepository, IDisposable
    {
        private AccessPointMapContext context;
        private readonly JWTSettings jwt;

        public UsersRepository(AccessPointMapContext context, IOptions<JWTSettings> jwt)
        {
            this.context = context;
            this.jwt = jwt.Value;
        }

        public async Task<string> Login(string login, string password, string ipAddress)
        {
            var user = context.Users.SingleOrDefault(element => (element.Login == login) && (element.Active == true));

            if(user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;

            user.LastLoginIp = (ipAddress != null) ? ipAddress : "";
            user.LastLoginDate = DateTime.Now;

            //JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwt.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim("permissionRead", user.ReadPermission.ToString()),
                    new Claim("permissionWrite", user.WritePermission.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(user.TokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            await context.SaveChangesAsync();
            return tokenHandler.WriteToken(token);
        }

        //IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
