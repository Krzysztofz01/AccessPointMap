using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.AccessPoints
{
    [DisallowConcurrentExecution]
    public class AccessPointManufacturerJob : IJob
    {
        public const string CronExpression = "0 4 * * * ";
        public const string JobName = "AccessPointManufacturerUpdate";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataAccess _dataAccess;
        private readonly IOuiLookupService _ouiLookupService;
        private readonly ILogger<AccessPointManufacturerJob> _logger;

        public AccessPointManufacturerJob(IUnitOfWork unitOfWork, IDataAccess dataAccess, IOuiLookupService ouiLookupService, ILogger<AccessPointManufacturerJob> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _dataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));

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
                _logger.LogInformation($"{JobName} scheduled job started.");

                var accessPoints = _dataAccess.AccessPointsTracked
                    .Where(a => a.Manufacturer.Value == string.Empty);

                foreach (var accessPoint in accessPoints)
                {
                    var manufacturerName = await _ouiLookupService.GetManufacturerName(accessPoint.Bssid.Value);

                    accessPoint.Apply(new Events.V1.AccessPointManufacturerChanged
                    {
                        Id = accessPoint.Id,
                        Manufacturer = manufacturerName
                    });
                }

                await _unitOfWork.Commit();

                _logger.LogInformation($"{JobName} scheduled job finished.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{JobName} scheduled job failed.", ex);
            }
        }
    }
}
