using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;
using static AccessPointMap.Domain.AccessPoints.Events;

namespace AccessPointMap.Domain.AccessPoints.AccessPointStamps
{
    public class AccessPointStamp : Entity
    {
        public AccessPointSsid Ssid { get; private set; }
        public AccessPointFrequency Frequency { get; private set; }
        public AccessPointDeviceType DeviceType { get; private set; }
        public AccessPointContributorId ContributorId { get; private set; }
        public AccessPointCreationTimestamp CreationTimestamp { get; private set; }
        public AccessPointPositioning Positioning { get; private set; }
        public AccessPointSecurity Security { get; private set; }
        public AccessPointStampStatus Status { get; private set; }

        protected override void Handle(IEventBase @event)
        {
            switch(@event)
            {
                case V1.AccessPointMergedWithStamp e: When(e); break;
                case V1.AccessPointStampDeleted e: When(e); break;

                default: throw new BusinessLogicException("This entity can not handlethis type of event.");
            }
        }

        protected override void Validate()
        {
            bool isNull = Ssid == null || DeviceType == null || ContributorId == null ||
                Frequency == null || CreationTimestamp == null || Positioning == null || Security == null || Status == null;

            if (isNull)
                throw new BusinessLogicException("The accesspoint stamp entity properties can not be null.");
        }

        private void When(V1.AccessPointMergedWithStamp @event)
        {
            Status = AccessPointStampStatus.Verified;
        }

        private void When(V1.AccessPointStampDeleted @event)
        {
            DeletedAt = DateTime.Now;
        }

        private AccessPointStamp() { }

        public static class Factory
        {
            public static AccessPointStamp Create(V1.AccessPointStampCreated @event)
            {
                return new AccessPointStamp
                {
                    Ssid = AccessPointSsid.FromString(@event.Ssid),
                    Frequency = AccessPointFrequency.FromDouble(@event.Frequency),
                    DeviceType = AccessPointDeviceType.FromString(@event.Ssid),
                    ContributorId = AccessPointContributorId.FromGuid(@event.UserId),
                    CreationTimestamp = AccessPointCreationTimestamp.FromDateTime(@event.ScanDate),
                    Positioning = AccessPointPositioning.FromGpsAndRssi(
                        @event.LowSignalLevel,
                        @event.LowSignalLatitude,
                        @event.LowSignalLongitude,
                        @event.HighSignalLevel,
                        @event.HighSignalLatitude,
                        @event.HighSignalLongitude),
                    Security = AccessPointSecurity.FromString(@event.RawSecurityPayload),
                    Status = AccessPointStampStatus.Default
                };
            }
        }
    }
}
