using AccessPointMap.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Repository
{
    public interface IAccessPointRepository : IRepository<AccessPoint>
    {
        IEnumerable<AccessPoint> GetAllPublic();
        IEnumerable<AccessPoint> GetAllMaster();
        IEnumerable<AccessPoint> GetAllQueue();
        Task<AccessPoint> GetByIdPublic(long accessPointId);
        Task<AccessPoint> GetByIdMaster(long accessPointId);
        Task<AccessPoint> GetByBssidMaster(string bssid);
        Task<AccessPoint> GetByIdQueue(long accessPointId);
        Task<AccessPoint> GetByIdGlobal(long accessPointId);
        IEnumerable<AccessPoint> SearchBySsid(string ssid);
        IEnumerable<AccessPoint> GetAllNoBrand();

        Task<int> AllRecordsCount();
        Task<int> AllInsecureRecordsCount();
        IEnumerable<Tuple<string, int>> TopOccuringBrandsSorted();
        IEnumerable<AccessPoint> TopAreaAccessPointsSorted();
        IEnumerable<Tuple<string, int>> TopOccuringSecurityTypes(IEnumerable<string> securityTypes);
        IEnumerable<Tuple<double, int>> TopOccuringFrequencies();

        IEnumerable<AccessPoint> UserAddedAccessPoints(long userId);
        IEnumerable<AccessPoint> UserModifiedAccessPoints(long userId);
    }
}
