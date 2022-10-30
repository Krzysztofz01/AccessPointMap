using AccessPointMap.Application.Core;
using AccessPointMap.Application.Logging;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.Application.AccessPoints
{
    [DisallowConcurrentExecution]
    public class AccessPointManufacturerJob : IJob
    {
        public const string CronExpression = "0 0 4 1/1 * ? *";
        public const string JobName = "AccessPointManufacturerUpdate";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOuiLookupService _ouiLookupService;
        private readonly ILogger<AccessPointManufacturerJob> _logger;

        public AccessPointManufacturerJob(IUnitOfWork unitOfWork, IOuiLookupService ouiLookupService, ILogger<AccessPointManufacturerJob> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _ouiLookupService = ouiLookupService ??
                throw new ArgumentNullException(nameof(ouiLookupService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [Obsolete("This method has inconsistent usage of data access abstraction.")]
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogScheduledJobBehaviour("Scheduled job started.");

                var accessPointIdsResult = await _unitOfWork.AccessPointRepository
                    .GetAccessPointIdsWithoutManufacturerSpecified(context.CancellationToken);

                if (accessPointIdsResult.IsFailure && accessPointIdsResult.Error is NotFoundError)
                {
                    _logger.LogScheduledJobBehaviour("Quitting the scheduled job. No pending access point ids.");      
                    return;
                }

                foreach (var accessPointId in accessPointIdsResult.Value)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();

                    var accessPoint = await _unitOfWork.AccessPointRepository.GetAsync(Guid.Parse(accessPointId), context.CancellationToken); 

                    var ouiLookupResult = await _ouiLookupService.GetManufacturerNameAsync(accessPoint.Bssid.Value, context.CancellationToken);
                    if (ouiLookupResult.IsFailure) continue;

                    accessPoint.Apply(new Events.V1.AccessPointManufacturerChanged
                    {
                        Id = accessPoint.Id,
                        Manufacturer = ouiLookupResult.Value
                    });
                }

                await _unitOfWork.Commit(context.CancellationToken);

                _logger.LogScheduledJobBehaviour("Scheduled job finished.");
            }
            catch (TaskCanceledException)
            {
                _logger.LogScheduledJobBehaviour("Scheduled job interupted via task cancellation.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogScheduledJobBehaviour("Scheduled job failed.", ex);
            }
        }
    }
}
