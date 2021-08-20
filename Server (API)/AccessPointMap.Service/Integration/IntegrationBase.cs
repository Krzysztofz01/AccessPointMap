using AccessPointMap.Domain;
using AccessPointMap.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration
{
    public class IntegrationBase
    {
        private readonly IAccessPointRepository _accessPointRepository;

        public IntegrationBase(IAccessPointRepository accessPointRepository)
        {
            _accessPointRepository = accessPointRepository ??
                throw new ArgumentNullException(nameof(accessPointRepository));
        }

        protected IEnumerable<AccessPoint> AccessPoints => 
            _accessPointRepository.GetAllIntegration();

        protected async Task AddToQueue(AccessPoint accessPoint) =>
            await _accessPointRepository.Add(accessPoint);

        protected async Task AddToQueue(IEnumerable<AccessPoint> accessPoints) =>
            await _accessPointRepository.AddRange(accessPoints);

        protected void SetToUpdate(AccessPoint accessPoint) =>
            _accessPointRepository.UpdateState(accessPoint);

        protected async Task Commit() =>
            await _accessPointRepository.Save();
    }
}
