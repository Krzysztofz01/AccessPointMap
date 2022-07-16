using AccessPointMap.Domain.Identities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.MySql.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly AccessPointMapDbContext _context;

        public IdentityRepository(AccessPointMapDbContext dbContext) =>
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));


        public Task Add(Identity identity)
        {
            _context.Identities.Add(identity);
            
            return Task.CompletedTask;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Identities
                .AsNoTracking()
                .AnyAsync(i => i.Id == id);
        }

        public async Task<Identity> Get(Guid id)
        {
            return await _context.Identities
                .Include(i => i.Tokens)
                .FirstAsync(i => i.Id == id);
        }
    }
}
