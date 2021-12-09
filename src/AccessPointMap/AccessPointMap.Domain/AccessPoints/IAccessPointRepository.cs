using System;
using System.Threading.Tasks;

namespace AccessPointMap.Domain.AccessPoints
{
    public interface IAccessPointRepository
    {
        Task Add(AccessPoint accessPoint);
        Task<AccessPoint> Get(Guid id);
        Task<AccessPoint> Get(string bssid);
        Task<bool> Exists(Guid id);
        Task<bool> Exists(string bssid);
    }
}
