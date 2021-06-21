using AccessPointMap.Domain;
using AccessPointMap.Repository;
using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public class AccessPointService : IAccessPointService
    {
        private readonly IAccessPointRepository accessPointRepository;
        private readonly IGeoCalculationService geoCalculationService;
        private readonly IMacResolveService macResolveService;
        private readonly IMapper mapper;
        private readonly ILogger<AccessPointService> logger;

        public AccessPointService(
            IAccessPointRepository accessPointRepository,
            IGeoCalculationService geoCalculationService,
            IMacResolveService macResolveService,
            IMapper mapper,
            ILogger<AccessPointService> logger)
        {
            this.accessPointRepository = accessPointRepository ??
                throw new ArgumentNullException(nameof(accessPointRepository));

            this.geoCalculationService = geoCalculationService ??
                throw new ArgumentNullException(nameof(geoCalculationService));

            this.macResolveService = macResolveService ??
                throw new ArgumentNullException(nameof(macResolveService));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IServiceResult> Add(IEnumerable<AccessPointDto> accessPoints, long userId)
        {
            var computedAccessPoints = new List<AccessPointDto>();

            foreach(var accessPoint in accessPoints)
            {
                var ap = Normalize(accessPoint);

                ap.Fingerprint = GenerateFingerprintV1(ap);

                ap.SignalRadius = geoCalculationService.GetDistance(ap.MaxSignalLatitude, ap.MinSignalLatitude, ap.MaxSignalLongitude, ap.MinSignalLongitude);

                ap.SignalArea = geoCalculationService.GetArea(ap.SignalRadius);

                ap.SerializedSecurityData = SerializeSecurityDateV1(ap.FullSecurityData);

                ap.IsSecure = CheckIsSecure(ap.FullSecurityData);

                ap.DeviceType = DetectDeviceTypeV1(ap);

                ap.MasterGroup = false;

                ap.Display = false;

                ap.UserAddedId = userId;

                ap.UserModifiedId = userId;

                computedAccessPoints.Add(ap);
            }

            var accessPointsMapped = mapper.Map<IEnumerable<AccessPoint>>(computedAccessPoints);
            await accessPointRepository.AddRange(accessPointsMapped);

            if (await accessPointRepository.Save() > 0)
            {
                return new ServiceResult(ResultStatus.Sucess);
            }
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> AddMaster(IEnumerable<AccessPointDto> accessPoints, long userId)
        {
            bool macResolveLimit = false;
            var computedAccessPoints = new List<AccessPointDto>();

            foreach(var accessPoint in accessPoints)
            {
                var ap = Normalize(accessPoint);

                ap.Fingerprint = GenerateFingerprintV1(ap);

                ap.SignalRadius = geoCalculationService.GetDistance(ap.MaxSignalLatitude, ap.MinSignalLatitude, ap.MaxSignalLongitude, ap.MinSignalLongitude);

                ap.SignalArea = geoCalculationService.GetArea(ap.SignalRadius);

                ap.SerializedSecurityData = SerializeSecurityDateV1(ap.FullSecurityData);

                ap.DeviceType = DetectDeviceTypeV1(ap);

                ap.MasterGroup = false;

                ap.Display = false;

                ap.UserAddedId = userId;

                ap.UserModifiedId = userId;

                if (accessPointRepository.GetMasterWithGivenBssid(ap.Bssid) == null)
                {
                    ap.MasterGroup = true;

                    if (!macResolveLimit)
                    {
                        string manufacturerResult = await macResolveService.GetVendorV1(ap.Bssid);

                        if(manufacturerResult == "#ERROR")
                        {
                            macResolveLimit = true;
                        }
                        else
                        {
                            if(!string.IsNullOrEmpty(manufacturerResult))
                            {
                                ap.Manufacturer = manufacturerResult;
                            }
                        }
                    }
                }

                computedAccessPoints.Add(ap);
            }

            var accessPointsMapped = mapper.Map<IEnumerable<AccessPoint>>(computedAccessPoints);
            await accessPointRepository.AddRange(accessPointsMapped);

            if (await accessPointRepository.Save() > 0)
            {
                return new ServiceResult(ResultStatus.Sucess);
            }
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> ChangeDisplay(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdMaster(accessPointId);
            if (accessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            accessPoint.Display = !accessPoint.Display;

            accessPointRepository.UpdateState(accessPoint);

            if(await accessPointRepository.Save() > 0)
            {
                return new ServiceResult(ResultStatus.Sucess);
            }
            return new ServiceResult(ResultStatus.Failed);
        }

        public async Task<IServiceResult> Delete(long accessPointId)
        {
            var accessPoint = await accessPointRepository.GetByIdMaster(accessPointId);
            if (accessPoint is null) return new ServiceResult(ResultStatus.NotFound);

            accessPointRepository.Remove(accessPoint);

            if (await accessPointRepository.Save() > 0)
            {
                return new ServiceResult(ResultStatus.Sucess);
            }
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

            var masterAccessPoint = await accessPointRepository.GetMasterWithGivenBssid(queueAccessPoint.Bssid);
            
            //The queue accesspoint will become the master
            if (masterAccessPoint is null)
            {
                queueAccessPoint.MasterGroup = true;

                string manufacturerResult = await macResolveService.GetVendorV1(queueAccessPoint.Bssid);
                if (manufacturerResult != "#ERROR" && manufacturerResult != null)
                {
                    queueAccessPoint.Manufacturer = manufacturerResult;
                }

                masterAccessPoint.EditDate = DateTime.Now;

                accessPointRepository.UpdateState(queueAccessPoint);

                if (await accessPointRepository.Save() > 0)
                {
                    return new ServiceResult(ResultStatus.Sucess);
                }
                return new ServiceResult(ResultStatus.Failed);
            }

            //The master accesspoint will be updated with the queue accesspoints values

            bool changes = false;

            if (queueAccessPoint.MinSignalLevel < masterAccessPoint.MinSignalLevel)
            {
                masterAccessPoint.MinSignalLevel = queueAccessPoint.MinSignalLevel;
                masterAccessPoint.MinSignalLatitude = queueAccessPoint.MinSignalLatitude;
                masterAccessPoint.MinSignalLongitude = queueAccessPoint.MinSignalLongitude;
                changes = true;
            }

            if (queueAccessPoint.MaxSignalLevel > masterAccessPoint.MaxSignalLevel)
            {
                masterAccessPoint.MaxSignalLevel = queueAccessPoint.MaxSignalLevel;
                masterAccessPoint.MaxSignalLatitude = queueAccessPoint.MaxSignalLatitude;
                masterAccessPoint.MaxSignalLongitude = queueAccessPoint.MaxSignalLongitude;
                changes = true;
            }

            if (changes)
            {
                masterAccessPoint.SignalRadius = geoCalculationService
                    .GetDistance(masterAccessPoint.MinSignalLatitude, masterAccessPoint.MaxSignalLatitude, masterAccessPoint.MinSignalLongitude, masterAccessPoint.MaxSignalLongitude);

                masterAccessPoint.SignalArea = geoCalculationService.GetArea(masterAccessPoint.SignalRadius);

                masterAccessPoint.Fingerprint = GenerateFingerprintV1(mapper.Map<AccessPointDto>(masterAccessPoint));
            }

            if (masterAccessPoint.AddDate < queueAccessPoint.AddDate)
            {
                if (masterAccessPoint.Ssid != queueAccessPoint.Ssid)
                {
                    masterAccessPoint.Ssid = queueAccessPoint.Ssid;
                    changes = true;
                }

                if (masterAccessPoint.SerializedSecurityData != queueAccessPoint.SerializedSecurityData)
                {
                    masterAccessPoint.FullSecurityData = AppendFullSecurityDataV1(masterAccessPoint.FullSecurityData, queueAccessPoint.FullSecurityData);
                    masterAccessPoint.SerializedSecurityData = SerializeSecurityDateV1(masterAccessPoint.FullSecurityData);

                    masterAccessPoint.IsSecure = CheckIsSecure(masterAccessPoint.FullSecurityData);

                    changes = true;
                }
            }

            if (changes)
            {
                masterAccessPoint.UserModified = queueAccessPoint.UserAdded;
                masterAccessPoint.EditDate = DateTime.Now;

                accessPointRepository.UpdateState(masterAccessPoint);
                accessPointRepository.Remove(queueAccessPoint);
                
                if (await accessPointRepository.Save() > 0)
                {
                    return new ServiceResult(ResultStatus.Sucess);
                }
                return new ServiceResult(ResultStatus.Failed);
            }
            return new ServiceResult(ResultStatus.Sucess);
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
                baseAccessPoint.Note = accessPoint.Note;
                changes = true;
            }

            if (!string.IsNullOrEmpty(accessPoint.DeviceType) && accessPoint.DeviceType != baseAccessPoint.DeviceType)
            {
                baseAccessPoint.DeviceType = accessPoint.DeviceType;
                changes = true;
            }

            if (changes)
            {
                baseAccessPoint.EditDate = DateTime.Now;

                accessPointRepository.UpdateState(baseAccessPoint);

                if(await accessPointRepository.Save() > 0)
                {
                    return new ServiceResult(ResultStatus.Sucess);
                }
                return new ServiceResult(ResultStatus.Failed);
            }
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

                ap.Manufacturer = manufacturerResult;
                accessPointRepository.UpdateState(ap);

                updated = true;
            }

            if (updated)
            {
                await accessPointRepository.Save();
            }
        }

        private AccessPointDto Normalize(AccessPointDto accessPoint)
        {
            accessPoint.Ssid = accessPoint.Ssid.Trim();
            accessPoint.Bssid = accessPoint.Bssid.Trim();
            accessPoint.MaxSignalLatitude = Math.Round(accessPoint.MaxSignalLatitude, 7);
            accessPoint.MaxSignalLongitude = Math.Round(accessPoint.MaxSignalLongitude, 7);
            accessPoint.MinSignalLatitude = Math.Round(accessPoint.MinSignalLatitude, 7);
            accessPoint.MinSignalLongitude = Math.Round(accessPoint.MinSignalLongitude, 7);

            return accessPoint;
        }

        private string GenerateFingerprintV1(AccessPointDto accessPoint)
        {
            string latFactor = Math.Round((accessPoint.MinSignalLatitude + accessPoint.MaxSignalLatitude) / 2.0, 4).ToString();
            string lonFactor = Math.Round((accessPoint.MinSignalLongitude + accessPoint.MaxSignalLongitude) / 2.0, 4).ToString();

            using (var sha1 = SHA1.Create())
            {
                byte[] data = sha1.ComputeHash(Encoding.UTF8.GetBytes($"{latFactor}{lonFactor}"));

                var sb = new StringBuilder();

                for(int i=0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private string SerializeSecurityDateV1(string fullSecurityData)
        {
            string[] encryptionTypes = { "BSS", "ESS", "WEP", "WPA", "WPA2", "WPA3", "WPS", "PSK", "CCMP", "TKIP" };
            var types = new List<string>();

            foreach(var type in encryptionTypes)
            {
                if (fullSecurityData.Contains(type)) types.Add(type);
            }

            return JsonConvert.SerializeObject(types);
        }

        private string AppendFullSecurityDataV1(string baseSecurityData, string newSecurityData)
        {
            var newSecurityDataArr = newSecurityData.Split('[');
            var newDataToBeAdded = new List<string>();

            foreach(var part in newSecurityDataArr)
            {
                if(!baseSecurityData.Contains(part))
                {
                    newDataToBeAdded.Add(Regex.Replace(part, @"[\[\]']+", string.Empty));
                }
            }

            var sb = new StringBuilder(baseSecurityData);
            foreach(var element in newDataToBeAdded)
            {
                sb.Append($"[{element}]");
            }

            return sb.ToString();
        }

        private string DetectDeviceTypeV1(AccessPointDto accessPoint)
        {
            string cleanedSsid = accessPoint.Ssid.ToLower().Trim();

            if (cleanedSsid.Contains("printer") || cleanedSsid.Contains("print") || cleanedSsid.Contains("laser")) return "Printer";
            if (cleanedSsid.Contains("tv")) return "TV";
            if (cleanedSsid.Contains("cctv") || cleanedSsid.Contains("cam")) return "CCTV";
            if (cleanedSsid.Contains("iot")) return "IoT";
            return null;
        }

        private bool CheckIsSecure(string fullSecurityData)
        {
            string[] secureEncryptionTypes = { "WPA", "WPA2", "WPA3" };
            string security = fullSecurityData.ToUpper();

            foreach(var type in secureEncryptionTypes)
            {
                if (security.Contains(type)) return true;
            }
            return false;
        }

        public Task<ServiceResult<AccessPointStatisticsDto>> GetStats()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<AccessPointDto>>> GetUserAddedAccessPoints(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<AccessPointDto>>> GetUserModifiedAccessPoints(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
