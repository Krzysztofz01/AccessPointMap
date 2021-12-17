using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public interface IAircrackngIntegrationService
    {
        Task Create(Requests.Create request);
    }
}
