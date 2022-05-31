using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AccessPointMap.API.Services
{
    public interface IHealthStatusService
    {
        bool IsHealthy();
        void SetHealthStatusDegraded();
    }

    public class HealthStatusService : IHealthStatusService
    {
        private HealthStatus healthStatus = HealthStatus.Healthy;

        public HealthStatusService() { }

        public bool IsHealthy()
        {
            return healthStatus == HealthStatus.Healthy;
        }

        public void SetHealthStatusDegraded()
        {
            healthStatus = HealthStatus.Degraded;
        }
    }
}
