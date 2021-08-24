using AccessPointMap.Domain;
using AccessPointMap.Repository;
using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public class AccessPointService : IAccessPointService
    {
        private readonly IAccessPointRepository accessPointRepository;
        private readonly IMacResolveService macResolveService;
        private readonly IMapper mapper;
        private readonly IAccessPointHelperService accessPointHelperService;
        private readonly ILogger<AccessPointService> logger;

        public AccessPointService(
            IAccessPointRepository accessPointRepository,
            IMacResolveService macResolveService,
            IMapper mapper,
            IAccessPointHelperService accessPointHelperService,
            ILogger<AccessPointService> logger)
        {
            this.accessPointRepository = accessPointRepository ??
                throw new ArgumentNullException(nameof(accessPointRepository));

            this.macResolveService = macResolveService ??
                throw new ArgumentNullException(nameof(macResolveService));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            this.accessPointHelperService = accessPointHelperService ??
                throw new ArgumentNullException(nameof(accessPointHelperService));

            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IServiceResult> Add(IEnumerable<AccessPointDto> accessPoints, long userId)
        {
            var computedAccessPoints = new List<AccessPoint>();

            foreach(var ap in accessPoints)
            {
                var accesspoint = AccessPoint.Factory.Create(
                    ap.Bssid,
                    ap.Ssid,
                    ap.Frequency,
                    ap.MaxSignalLevel,
                    ap.MaxSignalLatitude,
                    ap.MaxSignalLongitude,
                    ap.MinSignalLevel,
                    ap.MinSignalLatitude,
                    ap.MinSignalLongitude,
                    ap.FullSecurityData,
                    userId);

                accesspoint.SetSerializedSecurityData(accessPointHelperService.SerializeSecurityData(ap.FullSecurityData));

                accesspoint.SetSecurityStatus(accessPointHelperService.CheckIsSecure(ap.FullSecurityData));

                accesspoint.SetDeviceType(accessPointHelperService.DetectDeviceType(ap.Ssid));

                computedAccessPoints.Add(accesspoint);
            }

            await accessPointRepository.AddRange(computedAccessPoints);

            if (await accessPointRepository.Save() > 0)
            {
                logger.LogDebug($"User: {userId} added {computedAccessPoints.Count} accesspoints.");
                return new ServiceResult(ResultStatus.Sucess);
            }

            logger.LogDebug($"User: {userId} failed to add {computedAccessPoints.Count} accesspoints.");
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> AddMaster(IEnumerable<AccessPointDto> accessPoints, long userId)
        {
            bool macResolveLimit = false;
            var computedAccessPoints = new List<AccessPoint>();

            foreach (var ap in accessPoints)
            {
                var accesspoint = AccessPoint.Factory.Create(
                    ap.Bssid,
                    ap.Ssid,
                    ap.Frequency,
                    ap.MaxSignalLevel,
                    ap.MaxSignalLatitude,
                    ap.MaxSignalLongitude,
                    ap.MinSignalLevel,
                    ap.MinSignalLatitude,
                    ap.MinSignalLongitude,
                    ap.FullSecurityData,
                    userId);

                accesspoint.SetSerializedSecurityData(accessPointHelperService.SerializeSecurityData(ap.FullSecurityData));

                accesspoint.SetSecurityStatus(accessPointHelperService.CheckIsSecure(ap.FullSecurityData));

                accesspoint.SetDeviceType(accessPointHelperService.DetectDeviceType(ap.Ssid));

                //Check if accesspoint with given bssid exists before merging to master
                if (await accessPointRepository.GetByBssidMaster(accesspoint.Bssid) == null)
                {
                    accesspoint.SetMasterGroup(true);

                    if (!macResolveLimit)
                    {
                        string manufacturerResult = await macResolveService.GetVendorV1(ap.Bssid);

                        if (manufacturerResult == "#ERROR")
                        {
                            macResolveLimit = true;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(manufacturerResult))
                            {
                                accesspoint.SetManufacturer(manufacturerResult);
                            }
                        }
                    }
                }

                computedAccessPoints.Add(accesspoint);
            }

            await accessPointRepository.AddRange(computedAccessPoints);

            if (await accessPointRepository.Save() > 0)
            {
                logger.LogDebug($"User: {userId} added {computedAccessPoints.Count} accesspoints as master.");
                return new ServiceResult(ResultStatus.Sucess);
            }

            logger.LogDebug($"User: {userId} failed to add {computedAccessPoints.Count} accesspoints as master.");
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> ChangeDisplay(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdMaster(accessPointId);
            if (accessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            accessPoint.SetDisplay(!accessPoint.Display);

            accessPointRepository.UpdateState(accessPoint);

            if(await accessPointRepository.Save() > 0)
            {
                logger.LogDebug($"Changed display for accesspoint {accessPoint.Id} to {accessPoint.Display}.");
                return new ServiceResult(ResultStatus.Sucess);
            }

            logger.LogDebug($"Display change failed for accesspoint {accessPoint.Id} to {accessPoint.Display}.");
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> Delete(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdMaster(accessPointId);
            if (accessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            accessPointRepository.Remove(accessPoint);

            if (await accessPointRepository.Save() > 0)
            {
                logger.LogDebug($"Accesspoint {accessPoint.Id} deleted.");
                return new ServiceResult(ResultStatus.Sucess);
            }

            logger.LogDebug($"Accesspoint {accessPoint.Id} deleting failed.");
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<ServiceResult<IEnumerable<AccessPointDto>>> GetAllMaster()
        {
            var accessPoints = accessPointRepository.GetAllMaster();

            var accessPointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);

            return new ServiceResult<IEnumerable<AccessPointDto>>(accessPointsMapped);
        }

        public async Task<ServiceResult<IEnumerable<AccessPointDto>>> GetAllPublic()
        {
            var accessPoints = accessPointRepository.GetAllPublic();

            var accessPointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);

            return new ServiceResult<IEnumerable<AccessPointDto>>(accessPointsMapped);
        }

        public async Task<ServiceResult<IEnumerable<AccessPointDto>>> GetAllQueue()
        {
            var accessPoints = accessPointRepository.GetAllQueue();

            var accessPointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);

            return new ServiceResult<IEnumerable<AccessPointDto>>(accessPointsMapped);
        }

        public async Task<ServiceResult<AccessPointDto>> GetByIdMaster(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdMaster(accessPointId);
            if (accessPoint is null) return new ServiceResult<AccessPointDto>(ResultStatus.NotFound);

            var accessPointMapped = mapper.Map<AccessPointDto>(accessPoint);
            return new ServiceResult<AccessPointDto>(accessPointMapped);
        }

        public async Task<ServiceResult<AccessPointDto>> GetByIdPublic(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdPublic(accessPointId);
            if (accessPoint is null) return new ServiceResult<AccessPointDto>(ResultStatus.NotFound);

            var accessPointMapped = mapper.Map<AccessPointDto>(accessPoint);
            return new ServiceResult<AccessPointDto>(accessPointMapped);
        }

        public async Task<ServiceResult<AccessPointDto>> GetByIdQueue(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdQueue(accessPointId);
            if (accessPoint is null) return new ServiceResult<AccessPointDto>(ResultStatus.NotFound);

            var accessPointMapped = mapper.Map<AccessPointDto>(accessPoint);
            return new ServiceResult<AccessPointDto>(accessPointMapped);
        }

        // Merge rules
        // 1. We accept that the BSSID is a unique identifier
        // 2. If there is no master AP with given BSSID the merge AP will become the master
        // 3. If there is a master, the master will be updated with selected data
        // 4. Currenlty i wont implement the mergeall to avoid a potential loss of precise data

        public async Task<IServiceResult> MergeById(long accessPointId)
        {
            var queueAccessPoint = await accessPointRepository.GetByIdQueue(accessPointId);
            if (queueAccessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            var masterAccessPoint = await accessPointRepository.GetByBssidMaster(queueAccessPoint.Bssid);
            
            //The queue accesspoint will become the master
            if (masterAccessPoint is null)
            {
                queueAccessPoint.SetMasterGroup(true);

                string manufacturerResult = await macResolveService.GetVendorV1(queueAccessPoint.Bssid);
                if (manufacturerResult != "#ERROR" && manufacturerResult != null)
                {
                    queueAccessPoint.SetManufacturer(manufacturerResult);
                }

                accessPointRepository.UpdateState(queueAccessPoint);

                if (await accessPointRepository.Save() > 0)
                {
                    logger.LogDebug($"Accesspoint {queueAccessPoint.Id} merged without conflict.");
                    return new ServiceResult(ResultStatus.Sucess);
                }

                logger.LogDebug($"Accesspoint {queueAccessPoint.Id} merge without conflict failed.");
                return new ServiceResult(ResultStatus.Failed);
            }

            //The master accesspoint will be updated with the queue accesspoints values

            masterAccessPoint.UpdateLocation(
                queueAccessPoint.MinSignalLevel,
                queueAccessPoint.MinSignalLatitude,
                queueAccessPoint.MinSignalLongitude,
                queueAccessPoint.MaxSignalLevel,
                queueAccessPoint.MaxSignalLatitude,
                queueAccessPoint.MaxSignalLongitude);

            if (masterAccessPoint.AddDate < queueAccessPoint.AddDate)
            {
                masterAccessPoint.SetSsid(queueAccessPoint.Ssid);

                if (masterAccessPoint.SerializedSecurityData != queueAccessPoint.SerializedSecurityData)
                {
                    masterAccessPoint.SetSecurityData(queueAccessPoint.FullSecurityData);

                    masterAccessPoint.SetSerializedSecurityData(accessPointHelperService.SerializeSecurityData(masterAccessPoint.FullSecurityData));

                    masterAccessPoint.SetSecurityStatus(accessPointHelperService.CheckIsSecure(masterAccessPoint.FullSecurityData));
                }

                masterAccessPoint.SetUserModified(queueAccessPoint.UserAddedId.Value);
            }

            accessPointRepository.UpdateState(masterAccessPoint);

            accessPointRepository.Remove(queueAccessPoint);

            if (await accessPointRepository.Save() > 0)
            {
                logger.LogDebug($"Accesspoint ${masterAccessPoint.Id} merged with conflict resolved.");
                return new ServiceResult(ResultStatus.Sucess);
            }

            logger.LogDebug($"Accesspoint ${masterAccessPoint.Id} merge with conflict resolve failed.");
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<ServiceResult<IEnumerable<AccessPointDto>>> SearchBySsid(string ssid)
        {
            string ssidEscaped = ssid.Trim();

            var accessPoints = accessPointRepository.SearchBySsid(ssidEscaped);
            var accessPointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);

            return new ServiceResult<IEnumerable<AccessPointDto>>(accessPointsMapped);
        }

        //Possible changes: note, device type
        public async Task<IServiceResult> Update(long accessPointId, AccessPointDto accessPoint)
        {
            var baseAccessPoint = await accessPointRepository.GetByIdGlobal(accessPointId);
            if (baseAccessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            bool changes = false;

            if (!string.IsNullOrEmpty(accessPoint.Note) && accessPoint.Note != baseAccessPoint.Note)
            {
                baseAccessPoint.SetNote(accessPoint.Note);
                changes = true;
            }

            if (!string.IsNullOrEmpty(accessPoint.DeviceType) && accessPoint.DeviceType != baseAccessPoint.DeviceType)
            {
                baseAccessPoint.SetDeviceType(accessPoint.DeviceType);
                changes = true;
            }

            if (changes)
            {
                accessPointRepository.UpdateState(baseAccessPoint);

                if(await accessPointRepository.Save() > 0)
                {
                    logger.LogDebug($"Accesspoint {accessPointId} informations updated.");
                    return new ServiceResult(ResultStatus.Sucess);
                }

                logger.LogDebug($"Accesspoint {accessPointId} information update failed.");
                return new ServiceResult(ResultStatus.Failed);
            }

            logger.LogDebug($"Accesspoint {accessPointId} information update with no changes.");
            return new ServiceResult(ResultStatus.Sucess);
        }

        public async Task UpdateBrands()
        {
            var accessPoints = accessPointRepository.GetAllNoBrand();
            bool updated = false;

            foreach(var ap in accessPoints)
            {
                string manufacturerResult = await macResolveService.GetVendorV1(ap.Bssid);
                Thread.Sleep(3000);

                if (manufacturerResult == "#ERROR") break;
                if (manufacturerResult == null) continue;

                ap.SetManufacturer(manufacturerResult);
                accessPointRepository.UpdateState(ap);

                updated = true;
            }

            if (updated)
            {
                await accessPointRepository.Save();
            }
        }

        public async Task<ServiceResult<AccessPointStatisticsDto>> GetStats()
        {
            var encryptionTypes = accessPointHelperService.GetEncryptionsForStatistics();

            var statistics = new AccessPointStatisticsDto
            {
                AllRecords = await accessPointRepository.AllRecordsCount(),
                InsecureRecords = await accessPointRepository.AllInsecureRecordsCount(),
                TopBrands = accessPointRepository.TopOccuringBrandsSorted(),
                TopAreaAccessPoints = mapper.Map<IEnumerable<AccessPointDto>>(accessPointRepository.TopAreaAccessPointsSorted()),
                TopSecurityTypes = accessPointRepository.TopOccuringSecurityTypes(encryptionTypes),
                TopFrequencies = accessPointRepository.TopOccuringFrequencies()
            };

            return new ServiceResult<AccessPointStatisticsDto>(statistics);
        }

        public async Task<ServiceResult<IEnumerable<AccessPointDto>>> GetUserAddedAccessPoints(long userId)
        {
            var accessPoints = accessPointRepository.UserAddedAccessPoints(userId);
            if (accessPoints is null) return new ServiceResult<IEnumerable<AccessPointDto>>(ResultStatus.NotFound);

            var accessPointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);
            return new ServiceResult<IEnumerable<AccessPointDto>>(accessPointsMapped);
        }

        public async Task<ServiceResult<IEnumerable<AccessPointDto>>> GetUserModifiedAccessPoints(long userId)
        {
            var accessPoints = accessPointRepository.UserModifiedAccessPoints(userId);
            if (accessPoints is null) return new ServiceResult<IEnumerable<AccessPointDto>>(ResultStatus.NotFound);

            var accessPointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);
            return new ServiceResult<IEnumerable<AccessPointDto>>(accessPointsMapped);
        }

        public async Task<IServiceResult> UpdateSingleAccessPointManufacturer(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdGlobal(accessPointId);
            if (accessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            if (!string.IsNullOrEmpty(accessPoint.Manufacturer)) return new ServiceResult(ResultStatus.Conflict);

            string manufacturer = await macResolveService.GetVendorV1(accessPoint.Bssid);
            if (manufacturer == "#ERROR" || manufacturer is null) return new ServiceResult(ResultStatus.Failed);

            accessPoint.SetManufacturer(manufacturer);
            accessPointRepository.UpdateState(accessPoint);

            if (await accessPointRepository.Save() > 0)
            {
                logger.LogDebug($"Accesspoint {accessPoint.Id} manufacturer name updated.");
                return new ServiceResult(ResultStatus.Sucess);
            }

            logger.LogDebug($"Accesspoint {accessPoint.Id} manufacturer name update failed.");
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> UpdateQueue(long accessPointId, AccessPointDto accessPoint)
        {
            var baseAccessPoint = await accessPointRepository.GetByIdQueue(accessPointId);
            if (baseAccessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            bool changes = false;

            if (!string.IsNullOrEmpty(accessPoint.Note) && accessPoint.Note != baseAccessPoint.Note)
            {
                baseAccessPoint.SetNote(accessPoint.Note);
                changes = true;
            }

            if (!string.IsNullOrEmpty(accessPoint.DeviceType) && accessPoint.DeviceType != baseAccessPoint.DeviceType)
            {
                baseAccessPoint.SetDeviceType(accessPoint.DeviceType);
                changes = true;
            }

            if (changes)
            {
                accessPointRepository.UpdateState(baseAccessPoint);

                if (await accessPointRepository.Save() > 0)
                {
                    logger.LogDebug($"Accesspoint {accessPointId} informations updated (queue).");
                    return new ServiceResult(ResultStatus.Sucess);
                }

                logger.LogDebug($"Accesspoint {accessPointId} information update failed (queue).");
                return new ServiceResult(ResultStatus.Failed);
            }

            logger.LogDebug($"Accesspoint {accessPointId} information update with no changes (queue).");
            return new ServiceResult(ResultStatus.Sucess);
        }

        public async Task<ServiceResult<AccessPointDto>> GetByBssidMaster(string bssid)
        {
            var accessPoint = await accessPointRepository.GetByBssidMaster(bssid);
            if (accessPoint is null) return new ServiceResult<AccessPointDto>(ResultStatus.NotFound);

            var accessPointMapped = mapper.Map<AccessPointDto>(accessPoint);
            return new ServiceResult<AccessPointDto>(accessPointMapped);
        }
    }
}
