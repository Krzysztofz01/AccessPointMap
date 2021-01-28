using AccessPointMapWebApi.DatabaseContext;
using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Repositories
{
    public interface IUserRepository
    {
        Task<string> Login(string email, string password, string ipAddress);
        Task<bool> Register(UserFormDto userForm, string ipAddress);
        Task<bool> Update(User user);
        Task<bool> Activate(int id, bool activate);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<bool> Delete(int id);
    }

    public class UserRepository : IUserRepository
    {
        private AccessPointMapContext context;
        private readonly IAuthenticationService authenticationService;

        public UserRepository(
            AccessPointMapContext context,
            IAuthenticationService authenticationService)
        {
            this.context = context;
            this.authenticationService = authenticationService;
        }

        public async Task<bool> Activate(int id, bool activate)
        {
            var user = context.Users.Where(element => element.Id == id).FirstOrDefault();
            if (user != null)
            {
                user.Active = activate;
                context.Entry(user).State = EntityState.Modified;
                if (await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var user = context.Users.Where(element => element.Id == id).FirstOrDefault();
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

        public async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await context.Users.Where(element => element.Id == id).FirstOrDefaultAsync();
        }

        public async Task<string> Login(string email, string password, string ipAddress)
        {
            var user = context.Users.Where(element => element.Email == email).FirstOrDefault();
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    if (user.Active == true)
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

        public async Task<bool> Update(User user)
        {
            var existingUser = context.Users.FirstOrDefault(element => element.Id == user.Id);
            if (existingUser != null)
            {
                if (user.WritePermission != existingUser.WritePermission) existingUser.WritePermission = user.WritePermission;
                if (user.ReadPermission != existingUser.ReadPermission) existingUser.ReadPermission = user.ReadPermission;
                if (user.AdminPermission != existingUser.AdminPermission) existingUser.AdminPermission = user.AdminPermission;
                if (user.TokenExpiration != existingUser.TokenExpiration) existingUser.TokenExpiration = user.TokenExpiration;
                if (!string.IsNullOrEmpty(user.Email))
                {
                    var usersWithGivenEmail = context.Users.FirstOrDefault(element => element.Email == user.Email);
                    if (usersWithGivenEmail == null)
                    {
                        existingUser.Email = user.Email;
                    }
                }

                context.Entry(existingUser).State = EntityState.Modified;
                if (await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Register(UserFormDto userForm, string ipAddress)
        {
            if (context.Users.FirstOrDefault(element => element.Email == userForm.Email) == null)
            {
                var user = new User();
                user.Email = userForm.Email;
                user.TokenExpiration = 5;
                user.WritePermission = false;
                user.ReadPermission = false;
                user.AdminPermission = false;
                user.Active = false;
                user.LastLoginIp = (ipAddress != null) ? ipAddress : "";

                var salt = BCrypt.Net.BCrypt.GenerateSalt(5);
                user.Password = BCrypt.Net.BCrypt.HashPassword(userForm.Password, salt);

                context.Users.Add(user);
                if (await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
