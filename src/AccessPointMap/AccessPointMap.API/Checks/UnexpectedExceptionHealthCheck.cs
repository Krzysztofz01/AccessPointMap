using AccessPointMap.API.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.API.Checks
{
    internal class UnexpectedExceptionHealthCheck : IHealthCheck
    {
        private readonly IHealthStatusService _healthStatusService;

        public UnexpectedExceptionHealthCheck(IHealthStatusService healthStatusService) =>
            _healthStatusService = healthStatusService ?? throw new ArgumentNullException(nameof(healthStatusService));


        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_healthStatusService.IsHealthy())
                return Task.FromResult(HealthCheckResult.Healthy());

            return Task.FromResult(HealthCheckResult.Degraded());
        }
    }
}
