using AccessPointMap.Domain.Identities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.Sqlite.Repositories
{
    internal sealed class IdentityRepository : IIdentityRepository
    {
        private readonly AccessPointMapDbContext _context;

        public IdentityRepository(AccessPointMapDbContext dbContext) =>
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public IQueryable<Identity> Entities
        {
            get => _context.Identities
                .AsSplitQuery()
                .AsNoTracking();
        }

        public Task AddAsync(Identity entity, CancellationToken cancellationToken = default)
        {
            _ = _context.Identities.Add(entity);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Identities
                .AsSingleQuery()
                .AsNoTracking()
                .AnyAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<Identity> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Identities
                .Include(i => i.Tokens)
                .AsSingleQuery()
                .AsTracking()
                .FirstAsync(a => a.Id == id, cancellationToken);
        }
    }
}
