using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.Sqlite
{
    internal sealed class AuthenticationDataAccessService : IAuthenticationDataAccessService
    {
        private readonly AccessPointMapDbContext _context;

        public AuthenticationDataAccessService(AccessPointMapDbContext dbContext) =>
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<bool> AnyUserExistsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Identities
                .AsSingleQuery()
                .AsNoTracking()
                .AnyAsync(cancellationToken);
        }

        public async Task<Identity> GetUserOrDefaultByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Identities
                .Include(e => e.Tokens)
                .AsSingleQuery()
                .SingleOrDefaultAsync(e => e.Email.Value == email, cancellationToken);
        }

        public async Task<Identity> GetUserOrDefaultByActiveRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken = default)
        {
            var resultIdentity = await _context.Identities
                .Include(e => e.Tokens)
                .AsSingleQuery()
                .SingleOrDefaultAsync(e => e.Tokens.Any(t =>
                    t.TokenHash == refreshTokenHash),
                    cancellationToken);

            if (resultIdentity is null) return null;

            if (!resultIdentity.Tokens.Any(t => t.TokenHash == refreshTokenHash && t.IsActive)) return null;

            return resultIdentity;
        }

        public async Task<bool> AnyUserWithEmailExsitsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Identities
                .AsNoTracking()
                .AsSingleQuery()
                .AnyAsync(e => e.Email.Value == email, cancellationToken);
        }
    }
}
