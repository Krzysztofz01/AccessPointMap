using AccessPointMap.Domain.AccessPoints.AccessPointStamps;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static AccessPointMap.Domain.AccessPoints.Events;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPoint : AggregateRoot
    {
        public AccessPointBssid Bssid { get; private set; }
        public AccessPointManufacturer Manufacturer { get; private set; }
        public AccessPointSsid Ssid { get; private set; }
        public AccessPointFrequency Frequency { get; private set; }
        public AccessPointDeviceType DeviceType { get; private set; }
        public AccessPointContributorId ContributorId { get; private set; }
        public AccessPointCreationTimestamp CreationTimestamp { get; private set; }
        public AccessPointVersionTimestamp VersionTimestamp { get; private set; }
        public AccessPointPositioning Positioning { get; private set; }
        public AccessPointSecurity Security { get; private set; }
        public AccessPointNote Note { get; private set; }
        public AccessPointDisplayStatus DisplayStatus { get; private set; }

        private readonly List<AccessPointStamp> _stamps;
        public IReadOnlyCollection<AccessPointStamp> Stamps => _stamps.SkipDeleted().AsReadOnly();

        protected override void Handle(IEventBase @event)
        {
            switch(@event)
            {
                case V1.AccessPointDeleted e: When(e); break;
                case V1.AccessPointUpdated e: When(e); break;
                case V1.AccessPointDisplayStatusChanged e: When(e); break;
                case V1.AccessPointManufacturerChanged e: When(e); break;
                case V1.AccessPointMergedWithStamp e: When(e); break;

                case V1.AccessPointStampCreated e: When(e); break;
                case V1.AccessPointStampDeleted e: When(e); break;

                default: throw new BusinessLogicException("This entity can not handlethis type of event.");
            }
        }

        protected override void Validate()
        {
            bool isNull = Bssid == null || Manufacturer == null || Ssid == null || DeviceType == null || ContributorId == null ||
                Frequency == null || CreationTimestamp == null || VersionTimestamp == null || Positioning == null || Security == null || Note == null || DisplayStatus == null;

            if (isNull)
                throw new BusinessLogicException("The accesspoint aggregate properties can not be null.");
        }

        private void When(V1.AccessPointDeleted _)
        {
            DeletedAt = DateTime.Now;
        }

        private void When(V1.AccessPointUpdated @event)
        {
            Note = AccessPointNote.FromString(@event.Note);
        }

        private void When(V1.AccessPointDisplayStatusChanged @event)
        {
            DisplayStatus = AccessPointDisplayStatus.FromBool(@event.Status);
        }

        private void When(V1.AccessPointManufacturerChanged @event)
        {
            Manufacturer = AccessPointManufacturer.FromString(@event.Manufacturer);
        }

        //TODO: Separate merge for measurement-dependent and time-dependent data
        //TODO: The creation should be defined by scan date and not update date
        //TODO: Check for default frequency, because of integration inconsistency
        //TODO: ValueObject helper collections should be static
        //TODO: Move all constants to one place
        //TODO: More selective merge options
        //TODO: More ,,freedom'' in update??
        private void When(V1.AccessPointMergedWithStamp @event)
        {
            var stamp = _stamps
                .SkipDeleted()
                .Single(s => s.Id == @event.StampId);

            // Apply SSID related changes only if the stamp is newer than the current state
            if (stamp.CreationTimestamp.Value > VersionTimestamp.Value)
            {
                Ssid = AccessPointSsid.FromString(stamp.Ssid);

                // Check for device type to prevent device type data loss
                if (stamp.DeviceType != AccessPointDeviceType.DefaultDeviceType)
                {
                    DeviceType = AccessPointDeviceType.FromString(stamp.Ssid);
                }

                // Append the raw security data
                Security = AccessPointSecurity
                    .FromString($"{Security.RawSecurityPayload}-{stamp.Security.RawSecurityPayload}");

                VersionTimestamp = AccessPointVersionTimestamp.FromDateTime(stamp.CreationTimestamp);
            }

            // Compare positions in order to get most accurate data

            int lowSignalLevel = Positioning.LowSignalLevel;
            double lowSignalLatitude = Positioning.LowSignalLatitude;
            double lowSignalLongitude = Positioning.LowSignalLongitude;

            int highSignalLevel = Positioning.HighSignalLevel;
            double highSignalLatitude = Positioning.HighSignalLatitude;
            double highSignalLongitude = Positioning.HighSignalLongitude;

            // Set the position of the lowest signal strength
            if (stamp.Positioning.LowSignalLevel < Positioning.LowSignalLevel)
            {
                lowSignalLevel = stamp.Positioning.LowSignalLevel;

                lowSignalLatitude = stamp.Positioning.LowSignalLatitude;
                lowSignalLongitude = stamp.Positioning.LowSignalLongitude;
            }

            // Set the position of the highest signal strength
            if (stamp.Positioning.HighSignalLevel > Positioning.HighSignalLevel)
            {
                highSignalLevel = stamp.Positioning.HighSignalLevel;

                highSignalLatitude = stamp.Positioning.HighSignalLatitude;
                highSignalLongitude = stamp.Positioning.HighSignalLongitude;
            }

            Positioning = AccessPointPositioning.FromGpsAndRssi(
                lowSignalLevel,
                lowSignalLatitude,
                lowSignalLongitude,
                highSignalLevel,
                highSignalLatitude,
                highSignalLongitude);

            // Send the domain event to stamp in order to set it as verified
            stamp.Apply(@event);
        }

        private void When(V1.AccessPointStampCreated @event)
        {
            var stamp = AccessPointStamp.Factory.Create(@event);

            _stamps.Add(stamp);
        }

        private void When(V1.AccessPointStampDeleted @event)
        {
            var stamp = _stamps
                .SkipDeleted()
                .Single(s => s.Id == @event.StampId);

            stamp.Apply(@event);
        }

        private AccessPoint() =>
            _stamps = new List<AccessPointStamp>();

        public static class Factory
        {
            public static AccessPoint Create(V1.AccessPointCreated @event)
            {
                return new AccessPoint
                {
                    Bssid = AccessPointBssid.FromString(@event.Bssid),
                    Manufacturer = AccessPointManufacturer.Unknown,
                    Ssid = AccessPointSsid.FromString(@event.Ssid),
                    Frequency = AccessPointFrequency.FromDouble(@event.Frequency),
                    DeviceType = AccessPointDeviceType.FromString(@event.Ssid),
                    ContributorId = AccessPointContributorId.FromGuid(@event.UserId),
                    CreationTimestamp = AccessPointCreationTimestamp.Now,
                    VersionTimestamp = AccessPointVersionTimestamp.Now,
                    Positioning = AccessPointPositioning.FromGpsAndRssi(
                        @event.LowSignalLevel,
                        @event.LowSignalLatitude,
                        @event.LowSignalLongitude,
                        @event.HighSignalLevel,
                        @event.HighSignalLatitude,
                        @event.HighSignalLongitude),
                    Security = AccessPointSecurity.FromString(@event.RawSecurityPayload),
                    Note = AccessPointNote.Empty,
                    DisplayStatus = AccessPointDisplayStatus.Hidden
                };
            }
        }
    }
}
