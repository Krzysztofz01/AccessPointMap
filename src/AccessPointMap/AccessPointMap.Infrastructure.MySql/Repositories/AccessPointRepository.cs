using AccessPointMap.Domain.AccessPoints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.MySql.Repositories
{
    public class AccessPointRepository : IAccessPointRepository
    {
        private readonly AccessPointMapDbContext _context;

        public AccessPointRepository(AccessPointMapDbContext dbContext) =>
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public Task Add(AccessPoint accessPoint)
        {
            _context.AccessPoints.Add(accessPoint);
            
            return Task.CompletedTask;
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.AccessPoints
                .AsNoTracking()
                .AnyAsync(a => a.Id == id);
        }

        public async Task<bool> Exists(string bssid)
        {
            return await _context.AccessPoints
                .AsNoTracking()
                .AnyAsync(a => a.Bssid.Value == bssid);
        }

        public async Task<AccessPoint> Get(Guid id)
        {
            return await _context.AccessPoints
                .Include(a => a.Stamps)
                .Include(a => a.Adnnotations)
                .Include(a => a.Packets)
                .FirstAsync(a => a.Id == id);
        }

        public async Task<AccessPoint> Get(string bssid)
        {
            return await _context.AccessPoints
                .Include(a => a.Stamps)
                .Include(a => a.Adnnotations)
                .Include(a => a.Packets)
                .FirstAsync(a => a.Bssid.Value == bssid);
        }
    }
}
