using AccessPointMap.Domain;
using AccessPointMap.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Repository
{
    public class AccessPointRepository : Repository<AccessPoint>, IAccessPointRepository
    {
        public AccessPointRepository(AccessPointMapDbContext context): base(context)
        {
        }

        public Task<int> AllInsecureRecordsCount()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> AllRecordsCount()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<AccessPoint> GetAllMaster()
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .Where(x => x.MasterGroup);
        }

        public IEnumerable<AccessPoint> GetAllNoBrand()
        {
            return entities
                .Where(x => x.DeleteDate == null)
                .Where(x => string.IsNullOrEmpty(x.Manufacturer) && x.MasterGroup);
        }

        public IEnumerable<AccessPoint> GetAllPublic()
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .Where(x => x.MasterGroup && x.Display);
        }

        public IEnumerable<AccessPoint> GetAllQueue()
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .Where(x => !x.MasterGroup);
        }

        public async Task<AccessPoint> GetByIdGlobal(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public async Task<AccessPoint> GetByIdMaster(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .Where(x => x.MasterGroup)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public async Task<AccessPoint> GetByIdPublic(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .Where(x => x.MasterGroup && x.Display)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public async Task<AccessPoint> GetByIdQueue(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .Where(x => !x.MasterGroup)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public async Task<AccessPoint> GetMasterWithGivenBssid(string bssid)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .SingleOrDefaultAsync(x => x.MasterGroup && x.Bssid == bssid);
        }

        public IEnumerable<AccessPoint> SearchBySsid(string ssid)
        {
            string cleanedSsid = ssid.ToLower().Trim();

            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.DeleteDate == null)
                .Where(x => x.Ssid.ToLower().Contains(cleanedSsid))
                .Take(10);
        }

        public Task<IEnumerable<AccessPoint>> TopAreaAccessPointsSorted()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> TopOccuringBrandsSorted()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<double>> TopOccuringFrequencies()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<string>> TopOccuringSecurityTypes()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AccessPoint>> UserAddedAccessPoints(long userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AccessPoint>> UserModifiedAccessPoints(long userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
