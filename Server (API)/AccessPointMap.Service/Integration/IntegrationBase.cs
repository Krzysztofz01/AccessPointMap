using AccessPointMap.Domain;
using AccessPointMap.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration
{
    public class IntegrationBase
    {
        private readonly string _integrationName;
        private readonly IAccessPointRepository _accessPointRepository;

        public IntegrationBase(IAccessPointRepository accessPointRepository, string integrationName)
        {
            _accessPointRepository = accessPointRepository ??
                throw new ArgumentNullException(nameof(accessPointRepository));

            _integrationName = integrationName ??
                throw new ArgumentNullException(nameof(integrationName));
        }

        protected IEnumerable<AccessPoint> AccessPoints =>
            _accessPointRepository.GetAllIntegration();

        protected async Task AddToQueue(AccessPoint accessPoint)
        {
            accessPoint.SetNote($"$ {_integrationName}");

            await _accessPointRepository.Add(accessPoint);
        }

        protected async Task AddToQueue(IEnumerable<AccessPoint> accessPoints)
        {
            foreach (var accessPoint in AccessPoints) accessPoint.SetNote($"# {_integrationName}");

            await _accessPointRepository.AddRange(accessPoints);
        }

        protected void SetToUpdate(AccessPoint accessPoint) =>
            _accessPointRepository.UpdateState(accessPoint);

        protected async Task Commit() =>
            await _accessPointRepository.Save();
    }
}
