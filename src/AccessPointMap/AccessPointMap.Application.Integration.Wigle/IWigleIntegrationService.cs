using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Wigle
{
    public interface IWigleIntegrationService
    {
        Task Create(Requests.Create request);
    }
}
