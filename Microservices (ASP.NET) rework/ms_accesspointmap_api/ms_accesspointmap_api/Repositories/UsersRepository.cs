using Microsoft.EntityFrameworkCore;
using ms_accesspointmap_api.Models;
using ms_accesspointmap_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Repositories
{
    public interface IUserRepository
    {
        Task<string> Login(string email, string password, string ipAddress);
        Task<bool> Register(Users user);
        Task<bool> Update(Users user);
        Task<bool> Activate(int id, bool activate);
        Task<IEnumerable<Users>> GetAll();
        Task<Users> GetById(int id);
        Task<bool> Delete(int id);
    }

    public class UsersRepository : IUserRepository
    {
        private AccessPointMapContext context;
        private readonly IAuthenticationService authenticationService;

        public UsersRepository(
            AccessPointMapContext context,
            IAuthenticationService authenticationService)
        {
            this.context = context;
            this.authenticationService = authenticationService;
        }

        public async Task<bool> Activate(int id, bool activate)
        {
            var user = context.Users.Where(element => element.Id == id).First();
            if(user != null)
            {
                user.Active = true;
                context.Entry(user).State = EntityState.Modified;       
                if(await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var user = context.Users.Where(element => element.Id == id).First();
            if (user != null)
            {
                context.Entry(user).State = EntityState.Deleted;
                if (await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<Users>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<Users> GetById(int id)
        {
            return await context.Users.Where(element => element.Id == id).FirstAsync();
        }

        public async Task<string> Login(string email, string password, string ipAddress)
        {
            var user = context.Users.Where(element => element.Email == email).First();
            if(user != null)
            {
                if(BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    if(user.Active == true)
                    {
                        user.LastLoginDate = DateTime.Now;
                        user.LastLoginIp = (ipAddress != null) ? ipAddress : "";
                        context.Entry(user).State = EntityState.Modified;

                        await context.SaveChangesAsync();
                        return authenticationService.GenerateToken(user);
                    }
                    return "ERROR:ACTIVE";
                }
                return "ERROR:PASSWORD";
            }
            return "ERROR:EMAIL";
        }

        public async Task<bool> Update(Users user)
        {
            var existingUser = context.Users.First(element => element.Id == user.Id);
            if(existingUser != null)
            {
                if (user.WritePermission != existingUser.WritePermission) existingUser.WritePermission = user.WritePermission;
                if (user.ReadPermission != existingUser.ReadPermission) existingUser.ReadPermission = user.ReadPermission;
                if (user.AdminPermission != existingUser.AdminPermission) existingUser.AdminPermission = user.AdminPermission;
                if (!string.IsNullOrEmpty(user.Email))
                {
                    var usersWithGivenEmail = context.Users.First(element => element.Email == user.Email);
                    if(usersWithGivenEmail == null)
                    {
                        existingUser.Email = user.Email;
                    }
                }

                context.Entry(existingUser).State = EntityState.Modified;
                if(await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Register(Users user)
        {
            if(context.Users.First(element => element.Email == user.Email || element.Login == user.Login) == null)
            {
                user.TokenExpiration = 5;
                user.WritePermission = false;
                user.ReadPermission = false;
                user.AdminPermission = false;
                user.Active = false;

                var salt = BCrypt.Net.BCrypt.GenerateSalt(3);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

                context.Users.Add(user);
                if(await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
