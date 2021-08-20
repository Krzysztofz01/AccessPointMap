using AccessPointMap.Domain;
using AccessPointMap.Repository;
using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using AccessPointMap.Service.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public class AccessPointService : IAccessPointService
    {
        private readonly IAccessPointRepository accessPointRepository;
        private readonly IMacResolveService macResolveService;
        private readonly IMapper mapper;
        private readonly EncryptionTypeSettings encryptionTypeSettings;
        private readonly DeviceTypeSettings deviceTypeSettings;
        private readonly ILogger<AccessPointService> logger;

        public AccessPointService(
            IAccessPointRepository accessPointRepository,
            IMacResolveService macResolveService,
            IMapper mapper,
            IOptions<EncryptionTypeSettings> encryptionTypeSettings,
            IOptions<DeviceTypeSettings> deviceTypeSettings,
            ILogger<AccessPointService> logger)
        {
            this.accessPointRepository = accessPointRepository ??
                throw new ArgumentNullException(nameof(accessPointRepository));

            this.macResolveService = macResolveService ??
                throw new ArgumentNullException(nameof(macResolveService));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            this.encryptionTypeSettings = encryptionTypeSettings.Value ??
                throw new ArgumentNullException(nameof(encryptionTypeSettings));

            this.deviceTypeSettings = deviceTypeSettings.Value ??
                throw new ArgumentNullException(nameof(deviceTypeSettings));

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

                accesspoint.SetSerializedSecurityData(SerializeSecurityDataV1(ap.FullSecurityData));

                accesspoint.SetSecurityStatus(CheckIsSecure(ap.FullSecurityData));

                accesspoint.SetDeviceType(DetectDeviceTypeV1(ap.Ssid));

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

                accesspoint.SetSerializedSecurityData(SerializeSecurityDataV1(ap.FullSecurityData));

                accesspoint.SetSecurityStatus(CheckIsSecure(ap.FullSecurityData));

                accesspoint.SetDeviceType(DetectDeviceTypeV1(ap.Ssid));

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

            accessPoint.SetDisplay(accessPoint.Display);

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

                    masterAccessPoint.SetSerializedSecurityData(SerializeSecurityDataV1(masterAccessPoint.FullSecurityData));

                    masterAccessPoint.SetSecurityStatus(CheckIsSecure(masterAccessPoint.FullSecurityData));
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

        private string SerializeSecurityDataV1(string fullSecurityData)
        {
            var encryptionTypes = encryptionTypeSettings.EncryptionStandardsAndTypes;
            var types = new List<string>();

            foreach(var type in encryptionTypes)
            {
                if (fullSecurityData.Contains(type)) types.Add(type);
            }

            return JsonConvert.SerializeObject(types);
        }

        private string DetectDeviceTypeV1(string ssid)
        {
            ssid = ssid.ToLower().Trim();

            if (deviceTypeSettings.PrinterKeywords.Any(x => ssid.Contains(x.ToLower()))) return "Printer";
            if (deviceTypeSettings.AccessPointKeywords.Any(x => ssid.Contains(x.ToLower()))) return "Access point";
            if (deviceTypeSettings.TvKeywords.Any(x => ssid.Contains(x.ToLower()))) return "TV";
            if (deviceTypeSettings.CctvKeywords.Any(x => ssid.Contains(x.ToLower()))) return "CCTV";
            if (deviceTypeSettings.RepeaterKeywords.Any(x => ssid.Contains(x.ToLower()))) return "Repeater";
            if (deviceTypeSettings.IotKeywords.Any(x => ssid.Contains(x.ToLower()))) return "IoT";

            return null;
        }

        private bool CheckIsSecure(string fullSecurityData)
        {
            var secureEncryptionTypes = encryptionTypeSettings.SafeEncryptionStandards;
            string security = fullSecurityData.ToUpper();

            foreach(var type in secureEncryptionTypes)
            {
                if (security.Contains(type)) return true;
            }
            return false;
        }

        public async Task<ServiceResult<AccessPointStatisticsDto>> GetStats()
        {
            var encryptionTypes = encryptionTypeSettings.EncryptionStandardsForStatistics;

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
