using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public interface IMacResolveService
    {
        Task<string> GetVendorV1(string bssid);
    }
}
