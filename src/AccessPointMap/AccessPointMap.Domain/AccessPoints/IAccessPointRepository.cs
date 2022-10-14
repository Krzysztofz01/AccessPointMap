using AccessPointMap.Domain.Core.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Domain.AccessPoints
{
    public interface IAccessPointRepository : IRepository<AccessPoint>
    {
        Task<AccessPoint> GetAsync(string bssid, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(string bssid, CancellationToken cancellationToken = default);
    }
}
