using AccessPointMap.Application.Core;
using AccessPointMap.Domain.AccessPoints;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.GeoJson.Core
{
    public sealed class GeoJsonParsingService : IGeoJsonParsingService
    {
        public async Task<Result<AccessPointGeoJsonFeatureCollection>> GenerateGeoJsonAsync(IEnumerable<AccessPoint> accessPoints, CancellationToken cancellationToken = default)
        {
            var accessPointsGeoJson = AccessPointGeoJsonFeatureCollection.Create(accessPoints);

            var result = Result.Success(accessPointsGeoJson);
            return await Task.FromResult(result);
        }
    }
}
