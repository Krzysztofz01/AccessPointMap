using AccessPointMap.Domain;
using AccessPointMap.Repository.MySql.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Repository.MySql
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AccessPointMapMySqlDbContext context) : base(context)
        {
        }

        public async Task<bool> EmailAvailable(string email)
        {
            bool exist = await entities.AnyAsync(u => u.Email == email);
            return !exist;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return entities;
        }

        public async Task<User> GetSingleUser(long userId)
        {
            return await entities
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetSingleUserByEmail(string email)
        {
            return await entities
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserWithToken(string token)
        {
            return await entities
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        }
    }
}
