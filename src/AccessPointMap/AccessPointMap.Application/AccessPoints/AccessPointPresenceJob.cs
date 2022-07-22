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
    public class AccessPointPresenceJob : IJob
    {
        public const string CronExpression = "0 0 4 1/1 * ? *";
        public const string JobName = "AccessPointPresenceUpdate";

        public const int _metersThreshold = 50;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDataAccess _dataAccess;
        private readonly ILogger<AccessPointPresenceJob> _logger;

        public AccessPointPresenceJob(IUnitOfWork unitOfWork, IDataAccess dataAccess, ILogger<AccessPointPresenceJob> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _dataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"{JobName} scheduled job started.");

                var accessPoints = _dataAccess.AccessPointsTracked
                    .Where(a => !a.DeletedAt.HasValue);

                foreach (var accessPoint in accessPoints)
                {
                    accessPoint.Apply(new Events.V1.AccessPointPresenceStatusChanged
                    {
                        Id = accessPoint.Id,
                        Presence = IsPresent(accessPoint)
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

        private bool IsPresent(AccessPoint accessPoint)
        {
            return _dataAccess.AccessPoints
                .Where(a => !a.DeletedAt.HasValue)
                .Where(a => _metersThreshold < MetersBetweenPositions(
                    accessPoint.Positioning.HighSignalLatitude,
                    accessPoint.Positioning.HighSignalLongitude,
                    a.Positioning.HighSignalLatitude,
                    a.Positioning.HighSignalLongitude))
                .Where(a => a.VersionTimestamp.Value > accessPoint.VersionTimestamp.Value)
                .Any();
        }

        private static double MetersBetweenPositions(
            double aLatitude,
            double aLongitude,
            double bLatitude,
            double bLongitude)
        {
            const double _pi = 3.1415;
            double o1 = aLatitude * _pi / 180.0;
            double o2 = bLatitude * _pi / 180.0;

            double so = (bLatitude - aLatitude) * _pi / 180.0;
            double sl = (bLongitude - aLongitude) * _pi / 180.0;

            double a = Math.Pow(Math.Sin(so / 2.0), 2.0) + Math.Cos(o1) * Math.Cos(o2) * Math.Pow(Math.Sin(sl / 2.0), 2.0);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            return Math.Round(6371e3 * c, 2);
        }
    }
}
