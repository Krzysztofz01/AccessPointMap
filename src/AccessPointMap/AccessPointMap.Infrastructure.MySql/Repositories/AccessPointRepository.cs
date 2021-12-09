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

        public async Task Add(AccessPoint accessPoint)
        {
            _ = await _context.AccessPoints.AddAsync(accessPoint); 
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.AccessPoints.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> Exists(string bssid)
        {
            return await _context.AccessPoints.AnyAsync(a => a.Bssid.Value == bssid);
        }

        public async Task<AccessPoint> Get(Guid id)
        {
            return await _context.AccessPoints
                .Include(a => a.Stamps)
                .SingleAsync(a => a.Id == id);
        }

        public async Task<AccessPoint> Get(string bssid)
        {
            return await _context.AccessPoints
                .Include(a => a.Stamps)
                .SingleAsync(a => a.Bssid.Value == bssid);
        }
    }
}
