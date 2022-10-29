using AccessPointMap.Application.Core;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Oui.Core
{
    public interface IOuiLookupService
    {
        Task<Result<string>> GetManufacturerNameAsync(string macAddress, CancellationToken cancellationToken = default);
        Task<Result<IDictionary<string, string>>> GetManufacturerLookupDictionaryAsync(IEnumerable<string> macAddresses, CancellationToken cancellationToken = default);
    }
}
