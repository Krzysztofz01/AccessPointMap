using AccessPointMap.Application.Core;
using AccessPointMap.Domain.AccessPoints;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.GeoJson.Core
{
    public interface IGeoJsonParsingService
    {
        Task<Result<AccessPointGeoJsonFeatureCollection>> GenerateGeoJsonAsync(IEnumerable<AccessPoint> accessPoints, CancellationToken cancellationToken = default);
    }
}
