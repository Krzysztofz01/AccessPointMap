using AccessPointMap.Repository;
using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public Task<IServiceResult> ChangeDisplay(long accessPointId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult> ChangeMasterAssignmentAll()
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult> ChangeMasterAssignmentById(long accessPointId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult> Delete()
        {
            throw new NotImplementedException();
        }

        public Task<SerivceResult<IEnumerable<AccessPointDto>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<string>>> GetAllBrands()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<AccessPointDto>> GetOneByBssid(string bssid)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<AccessPointDto>> GetOneById(long accessPointId)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult> Patch(AccessPointDto accessPoint)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult> Push(IEnumerable<AccessPointDto> accesspoints, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<IEnumerable<AccessPointDto>>> SearchBySsid(string ssid)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBrands()
        {
            throw new NotImplementedException();
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
    }
}
