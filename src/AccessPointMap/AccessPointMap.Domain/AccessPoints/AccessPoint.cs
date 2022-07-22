using AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations;
using AccessPointMap.Domain.AccessPoints.AccessPointPackets;
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
        public AccessPointPresence Presence { get; private set; }

        private readonly List<AccessPointStamp> _stamps;
        public IReadOnlyCollection<AccessPointStamp> Stamps => _stamps.SkipDeleted().AsReadOnly();

        private readonly List<AccessPointAdnnotation> _adnnotations;
        public IReadOnlyCollection<AccessPointAdnnotation> Adnnotations => _adnnotations.SkipDeleted().AsReadOnly();

        private readonly List<AccessPointPacket> _packets;
        public IReadOnlyCollection<AccessPointPacket> Packets => _packets.SkipDeleted().AsReadOnly();

        protected override void Handle(IEventBase @event)
        {
            switch(@event)
            {
                case V1.AccessPointDeleted e: When(e); break;
                case V1.AccessPointUpdated e: When(e); break;
                case V1.AccessPointDisplayStatusChanged e: When(e); break;
                case V1.AccessPointManufacturerChanged e: When(e); break;
                case V1.AccessPointMergedWithStamp e: When(e); break;
                case V1.AccessPointPresenceStatusChanged e: When(e); break;

                case V1.AccessPointStampCreated e: When(e); break;
                case V1.AccessPointStampDeleted e: When(e); break;

                case V1.AccessPointAdnnotationCreated e: When(e); break;
                case V1.AccessPointAdnnotationDeleted e: When(e); break;

                case V1.AccessPointPacketCreated e: When(e); break;
                case V1.AccessPointPacketDeleted e: When(e); break;

                default: throw new BusinessLogicException("This entity can not handlethis type of event.");
            }
        }

        protected override void Validate()
        {
            bool isNull = Bssid == null || Manufacturer == null || Ssid == null || DeviceType == null || ContributorId == null ||
                Frequency == null || CreationTimestamp == null || VersionTimestamp == null || Positioning == null || Security == null || Note == null || DisplayStatus == null || Presence == null;

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

        private void When(V1.AccessPointPresenceStatusChanged @event)
        {
            Presence = AccessPointPresence.FromBool(@event.Presence);
        }

        private void When(V1.AccessPointMergedWithStamp @event)
        {
            var stamp = _stamps
                .SkipDeleted()
                .Single(s => s.Id == @event.StampId);

            AccessPointVersionTimestamp updatedTimestamp = VersionTimestamp;

            // Aplly SSID related changes if:
            // 1. The stamp is newer than the current state.
            // 2. We set the boolean value for merging SSID.
            if (stamp.CreationTimestamp.Value > VersionTimestamp.Value && @event.MergeSsid)
            {
                Ssid = AccessPointSsid.FromString(stamp.Ssid);

                // Check for device type to prevent device type data loss
                if (stamp.DeviceType != AccessPointDeviceType.DefaultDeviceType)
                {
                    DeviceType = AccessPointDeviceType.FromString(stamp.Ssid);
                }

                updatedTimestamp = AccessPointVersionTimestamp.FromDateTime(stamp.CreationTimestamp);
            }

            // Apply frequency related changes if:
            // 1. The stamp is newer than the current state.
            // 2. The value is not ,,default''
            if (stamp.CreationTimestamp.Value > VersionTimestamp.Value && stamp.Frequency.Value != default)
            {
                Frequency = AccessPointFrequency.FromDouble(stamp.Frequency);
            }

            // Apply security data related changes if:
            // 1. The stamp is newer than the current state.
            // 2. We set the boolean value for merging security changes.
            if (stamp.CreationTimestamp.Value > VersionTimestamp.Value && @event.MergeSecurityData)
            {
                // Append the raw security data
                Security = AccessPointSecurity
                    .FromString($"{Security.RawSecurityPayload}-{stamp.Security.RawSecurityPayload}");

                updatedTimestamp = AccessPointVersionTimestamp.FromDateTime(stamp.CreationTimestamp);
            }

            // Apply low singnal position changes if:
            // 1. The new low signal position level is lower
            // 2. We set the boolean value for merging low signal position
            int lowSignalLevel = Positioning.LowSignalLevel;
            double lowSignalLatitude = Positioning.LowSignalLatitude;
            double lowSignalLongitude = Positioning.LowSignalLongitude;
            if (stamp.Positioning.LowSignalLevel < Positioning.LowSignalLevel && @event.MergeLowSignalLevel)
            {
                lowSignalLevel = stamp.Positioning.LowSignalLevel;

                lowSignalLatitude = stamp.Positioning.LowSignalLatitude;
                lowSignalLongitude = stamp.Positioning.LowSignalLongitude;
            }

            // Apply high singnal position changes if:
            // 1. The new high signal position level is higher
            // 2. We set the boolean value for merging high signal position
            int highSignalLevel = Positioning.HighSignalLevel;
            double highSignalLatitude = Positioning.HighSignalLatitude;
            double highSignalLongitude = Positioning.HighSignalLongitude;
            if (stamp.Positioning.HighSignalLevel > Positioning.HighSignalLevel && @event.MergeHighSignalLevel)
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

            VersionTimestamp = updatedTimestamp;

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

        private void When(V1.AccessPointAdnnotationCreated @event)
        {
            var adnnotation = AccessPointAdnnotation.Factory.Create(@event);

            _adnnotations.Add(adnnotation);
        }

        private void When(V1.AccessPointAdnnotationDeleted @event)
        {
            var adnnotation = _adnnotations
                .SkipDeleted()
                .Single(s => s.Id == @event.AdnnotationId);

            adnnotation.Apply(@event);
        }

        private void When(V1.AccessPointPacketCreated @event)
        {
            if (@event.SourceAddress != Bssid.Value)
                throw new BusinessLogicException("The packet hardware addresses are not matching.");

            var packet = AccessPointPacket.Factory.Create(@event);

            _packets.Add(packet);
        }

        private void When(V1.AccessPointPacketDeleted @event)
        {
            var packet = _packets
                .SkipDeleted()
                .Single(p => p.Id == @event.PacketId);

            packet.Apply(@event);
        }

        private AccessPoint()
        {
            _stamps = new List<AccessPointStamp>();
            _adnnotations = new List<AccessPointAdnnotation>();
            _packets = new List<AccessPointPacket>();
        }

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
                    CreationTimestamp = AccessPointCreationTimestamp.FromDateTime(@event.ScanDate),
                    VersionTimestamp = AccessPointVersionTimestamp.FromDateTime(@event.ScanDate),
                    Positioning = AccessPointPositioning.FromGpsAndRssi(
                        @event.LowSignalLevel,
                        @event.LowSignalLatitude,
                        @event.LowSignalLongitude,
                        @event.HighSignalLevel,
                        @event.HighSignalLatitude,
                        @event.HighSignalLongitude),
                    Security = AccessPointSecurity.FromString(@event.RawSecurityPayload),
                    Note = AccessPointNote.Empty,
                    DisplayStatus = AccessPointDisplayStatus.Hidden,
                    Presence = AccessPointPresence.Present
                };
            }
        }
    }
}
