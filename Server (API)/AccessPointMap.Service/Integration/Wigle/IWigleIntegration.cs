using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration.Wigle
{
    public interface IWigleIntegration
    {
        Task Add(IFormFile file, long userId);
    }
}
