using AccessPointMap.Domain.AccessPoints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.MySql.Repositories
{
    internal sealed class AccessPointRepository : IAccessPointRepository
    {
        private readonly AccessPointMapDbContext _context;

        public AccessPointRepository(AccessPointMapDbContext dbContext) =>
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public IQueryable<AccessPoint> Entities
        {
            get => _context.AccessPoints
                .AsSplitQuery()
                .AsNoTracking();
        }

        public Task AddAsync(AccessPoint entity, CancellationToken cancellationToken = default)
        {
            _ = _context.AccessPoints.Add(entity);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(string bssid, CancellationToken cancellationToken = default)
        {
            return await _context.AccessPoints
                .AsSingleQuery()
                .AsNoTracking()
                .AnyAsync(a => a.Bssid.Value == bssid, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.AccessPoints
                .AsSingleQuery()
                .AsNoTracking()
                .AnyAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<AccessPoint> GetAsync(string bssid, CancellationToken cancellationToken = default)
        {
            return await _context.AccessPoints
                .Include(a => a.Stamps)
                .Include(a => a.Adnnotations)
                .Include(a => a.Packets)
                .AsSingleQuery()
                .AsTracking()
                .FirstAsync(a => a.Bssid.Value == bssid, cancellationToken);
        }

        public async Task<AccessPoint> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.AccessPoints
                .Include(a => a.Stamps)
                .Include(a => a.Adnnotations)
                .Include(a => a.Packets)
                .AsSingleQuery()
                .AsTracking()
                .FirstAsync(a => a.Id == id, cancellationToken);
        }
    }
}
