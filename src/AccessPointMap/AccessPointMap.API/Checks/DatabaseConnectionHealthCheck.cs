using AccessPointMap.Infrastructure.MySql;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.API.Checks
{
    internal class DatabaseConnectionHealthCheck : IHealthCheck
    {
        private readonly AccessPointMapDbContext _dbContext;

        public DatabaseConnectionHealthCheck(AccessPointMapDbContext dbContext) =>
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                return HealthCheckResult.Healthy();

            return HealthCheckResult.Unhealthy();
        }
    }
}
