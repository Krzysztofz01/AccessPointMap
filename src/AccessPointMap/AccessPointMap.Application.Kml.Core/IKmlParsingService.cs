using AccessPointMap.Application.Core;
using AccessPointMap.Domain.AccessPoints;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Kml.Core
{
    public interface IKmlParsingService
    {
        Task<Result<KmlResult>> GenerateKmlAsync(IEnumerable<AccessPoint> accessPoints, CancellationToken cancellationToken = default);
    }
}
