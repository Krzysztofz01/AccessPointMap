﻿using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public interface IAccessPointService
    {
        Task<IServiceResult> Add(IEnumerable<AccessPointDto> accessPoints, long userId);
        Task<IServiceResult> AddMaster(IEnumerable<AccessPointDto> accessPoints, long userId);
        Task<ServiceResult<IEnumerable<AccessPointDto>>> GetAllPublic();
        Task<ServiceResult<AccessPointDto>> GetByIdPublic(long accessPointId);
        Task<ServiceResult<IEnumerable<AccessPointDto>>> SearchBySsid(string ssid);
        Task<ServiceResult<IEnumerable<AccessPointDto>>> GetAllMaster();
        Task<ServiceResult<AccessPointDto>> GetByIdMaster(long accessPointId);
        Task<ServiceResult<AccessPointDto>> GetByBssidMaster(string bssid);
        Task<IServiceResult> Delete(long accessPointId);
        Task<IServiceResult> Update(long accessPointId, AccessPointDto accessPoint);
        Task<IServiceResult> UpdateQueue(long accessPointId, AccessPointDto accessPoint);
        Task<ServiceResult<IEnumerable<AccessPointDto>>> GetAllQueue();
        Task<ServiceResult<AccessPointDto>> GetByIdQueue(long accessPointId);
        Task<IServiceResult> MergeById(long accessPointId);
        Task<IServiceResult> ChangeDisplay(long accessPointId);
        Task<ServiceResult<AccessPointStatisticsDto>> GetStats();
        Task<ServiceResult<IEnumerable<AccessPointDto>>> GetUserAddedAccessPoints(long userId);
        Task<ServiceResult<IEnumerable<AccessPointDto>>> GetUserModifiedAccessPoints(long userId);
        Task<IServiceResult> UpdateSingleAccessPointManufacturer(long accessPointId);

        Task UpdateBrands();
    }
}
