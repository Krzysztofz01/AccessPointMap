using System.Threading.Tasks;

namespace AccessPointMap.Application.Oui.Core
{
    public interface IOuiLookupService
    {
        Task<string> GetManufacturerName(string macAddress);
    }
}
