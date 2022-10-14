using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.AccessPoints
{
    [DisallowConcurrentExecution]
    public sealed class AccessPointPresenceJob : IJob
    {
        public const string CronExpression = "0 0 4 ? * MON";
        public const string JobName = "AccessPointPresenceUpdate";

        public const double _metersThreshold = 5.0d;
        public const double _amountThresholdPercent = 80.0d;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccessPointPresenceJob> _logger;

        public AccessPointPresenceJob(IUnitOfWork unitOfWork, ILogger<AccessPointPresenceJob> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation($"{JobName} scheduled job started.");

                var accessPointsTracked = await _unitOfWork.AccessPointRepository.Entities
                    .Include(a => a.Stamps)
                    .Where(a => !a.DeletedAt.HasValue)
                    .ToListAsync();

                var accessPointsRepresentation = accessPointsTracked
                    .Select(a => new AccessPointRepresentation(a));

                foreach (var accessPointRepresentation in accessPointsRepresentation)
                {
                    if (!IsPresent(accessPointRepresentation, accessPointsRepresentation))
                    {
                        var targetAccessPointEntity = accessPointsTracked.Single(a => a.Id == accessPointRepresentation.Id);

                        targetAccessPointEntity.Apply(new Events.V1.AccessPointPresenceStatusChanged
                        {
                            Id = targetAccessPointEntity.Id,
                            Presence = false
                        });
                    }
                }

                await _unitOfWork.Commit();

                _logger.LogInformation($"{JobName} scheduled job finished.");         
            }
            catch (Exception ex)
            {
                _logger.LogError($"{JobName} scheduled job failed.", ex);
            }
        }

        private static bool IsPresent(AccessPointRepresentation accessPoint, IEnumerable<AccessPointRepresentation> accessPointCollection)
        {
            var accessPointsInArea = accessPointCollection
                .Where(a => a.Bssid != accessPoint.Bssid)
                .Where(a => _metersThreshold >= CalculateSignalRadius(
                    accessPoint.Latitude,
                    accessPoint.Longitude,
                    a.Latitude,
                    a.Longitude))
                .ToList();

            if (accessPointsInArea.Count == 0) return true;

            var accessPointsWithRecentTimestamp = accessPointsInArea
                .Where(a => accessPoint.Timestamp < a.Timestamp)
                .ToList();

            var recentPercent = (accessPointsWithRecentTimestamp.Count * 100.0d) / accessPointsInArea.Count;

            return _amountThresholdPercent >= recentPercent;
        }

        // Alghoritm from the AccessPointPositioning domain value object
        private static double CalculateSignalRadius(
            double lowSignalLatitude,
            double lowSignalLongitude,
            double highSignalLatitude,
            double highSignalLongitude)
        {
            const double _pi = 3.1415;
            double o1 = lowSignalLatitude * _pi / 180.0;
            double o2 = highSignalLatitude * _pi / 180.0;

            double so = (highSignalLatitude - lowSignalLatitude) * _pi / 180.0;
            double sl = (highSignalLongitude - lowSignalLongitude) * _pi / 180.0;

            double a = Math.Pow(Math.Sin(so / 2.0), 2.0) + Math.Cos(o1) * Math.Cos(o2) * Math.Pow(Math.Sin(sl / 2.0), 2.0);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            return Math.Round(6371e3 * c, 2);
        }

        private class AccessPointRepresentation
        {
            public Guid Id { get; set; }
            public string Bssid { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public DateTime Timestamp { get; set; }

            public AccessPointRepresentation(AccessPoint accessPoint)
            {
                Id = accessPoint.Id;
                Bssid = accessPoint.Bssid.Value;
                Latitude = accessPoint.Positioning.HighSignalLatitude;
                Longitude = accessPoint.Positioning.HighSignalLongitude;
                Timestamp = accessPoint.CreationTimestamp.Value;

                if (accessPoint.Stamps.Count > 0)
                {
                    var recentStampTimestamp = accessPoint.Stamps
                        .MaxBy(s => s.CreationTimestamp.Value)
                        .CreationTimestamp.Value;

                    if (recentStampTimestamp > Timestamp)
                        Timestamp = recentStampTimestamp;
                }
            }
        }
    }
}
