using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration.Aircrackng
{
    public interface IAircrackngIntegration
    {
        Task Add(IFormFile file, long userId);
    }
}
