using System;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Kml.Core
{
    public interface IKmlParsingService
    {
        Task<KmlResult> GenerateKml(Action<KmlGenerationOptions> options);
    }
}
