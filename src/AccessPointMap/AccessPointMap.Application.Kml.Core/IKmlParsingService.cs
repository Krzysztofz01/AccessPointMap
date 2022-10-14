using AccessPointMap.Domain.AccessPoints;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Kml.Core
{
    public interface IKmlParsingService
    {
        Task<KmlResult> GenerateKmlAsync(IEnumerable<AccessPoint> accessPoints, CancellationToken cancellationToken = default);
    }
}
