using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.AccessPoints
{
    public class AccessPointBackgroundJobs : IAccessPointBackgroundJobs
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataAccess _dataAccess;
        private readonly IOuiLookupService _ouiLookupService;
        private readonly ILogger<AccessPointBackgroundJobs> _logger;

        public AccessPointBackgroundJobs(IUnitOfWork unitOfWork, IDataAccess dataAccess, IOuiLookupService ouiLookupService, ILogger<AccessPointBackgroundJobs> logger)
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

        //TODO: The tracked data access will be removed in the future
        public async Task SetAccessPointManufacturer()
        {
            try
            {
                _logger.LogInformation($"SetAccessPointManufacturer job started.");

                var accessPoints = _dataAccess.AccessPointsTracked
                    .Where(a => a.Manufacturer.Value == string.Empty);

                foreach(var accessPoint in accessPoints)
                {
                    var manufacturerName = await _ouiLookupService.GetManufacturerName(accessPoint.Bssid.Value);

                    accessPoint.Apply(new Events.V1.AccessPointManufacturerChanged
                    {
                        Id = accessPoint.Id,
                        Manufacturer = manufacturerName
                    });
                }

                await _unitOfWork.Commit();

                _logger.LogInformation($"SetAccessPointManufacturer job finished.");
            }
            catch(Exception ex)
            {
                _logger.LogError("SetAccessPointManufacturer job failed.", ex);
            }
        }
    }
}
