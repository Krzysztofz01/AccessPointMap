using AccessPointMap.Domain;
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
        Task<AccessPoint> GetByIdQueue(long accessPointId);
        Task<AccessPoint> GetByIdGlobal(long accessPointId);
        IEnumerable<AccessPoint> SearchBySsid(string ssid);
        IEnumerable<AccessPoint> GetAllNoBrand();
        Task<AccessPoint> GetMasterWithGivenBssid(string bssid);

        Task<int> AllRecordsCount();
        Task<int> AllInsecureRecordsCount();
        Task<IEnumerable<string>> TopOccuringBrandsSorted();
        Task<IEnumerable<AccessPoint>> TopAreaAccessPointsSorted();
        Task<IEnumerable<string>> TopOccuringSecurityTypes();
        Task<IEnumerable<double>> TopOccuringFrequencies();

        Task<IEnumerable<AccessPoint>> UserAddedAccessPoints(long userId);
        Task<IEnumerable<AccessPoint>> UserModifiedAccessPoints(long userId);
    }
}
