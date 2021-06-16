using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public interface IAccessPointService
    {
        Task<SerivceResult<IEnumerable<AccessPointDto>>> GetAll();
        Task<ServiceResult<AccessPointDto>> GetOneById(long accessPointId);
        Task<ServiceResult<AccessPointDto>> GetOneByBssid(string bssid);
        Task<ServiceResult<IEnumerable<AccessPointDto>>> SearchBySsid(string ssid);
        Task<ServiceResult<IEnumerable<string>>> GetAllBrands();
        Task<IServiceResult> Push(IEnumerable<AccessPointDto> accesspoints, long userId);
        Task<IServiceResult> ChangeMasterAssignmentAll();
        Task<IServiceResult> ChangeMasterAssignmentById(long accessPointId);
        Task<IServiceResult> ChangeDisplay(long accessPointId);
        Task<IServiceResult> Patch(AccessPointDto accessPoint);
        Task<IServiceResult> Delete();

        Task UpdateBrands();
    }
}
