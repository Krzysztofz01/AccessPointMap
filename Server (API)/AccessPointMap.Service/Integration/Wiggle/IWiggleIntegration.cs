using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration.Wiggle
{
    public interface IWiggleIntegration
    {
        Task Add(IFormFile file, long userId);
    }
}
