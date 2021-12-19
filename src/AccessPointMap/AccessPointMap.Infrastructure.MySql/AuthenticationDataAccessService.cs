using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.MySql
{
    public class AuthenticationDataAccessService : IAuthenticationDataAccessService
    {
        private readonly AccessPointMapDbContext _context;

        public AuthenticationDataAccessService(AccessPointMapDbContext dbContext) =>
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<bool> AnyUsersExists()
        {
            return await _context.Identities
                .AnyAsync();
        }

        public async Task<Identity> GetUserByEmail(string email)
        {
            return await _context.Identities
                .Include(e => e.Tokens)
                .SingleAsync(e => e.Email.Value == email);
        }

        public async Task<Identity> GetUserByRefreshToken(string refreshTokenHash)
        {
            return await _context.Identities
                .Include(e => e.Tokens)
                .SingleAsync(e => e.Tokens.Any(t => t.TokenHash == refreshTokenHash));
        }

        public async Task<bool> UserWithEmailExsits(string email)
        {
            return await _context.Identities
                .AnyAsync(e => e.Email.Value == email);
        }
    }
}
