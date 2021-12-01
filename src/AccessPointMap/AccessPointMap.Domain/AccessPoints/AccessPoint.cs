using AccessPointMap.Domain.AccessPoints.AccessPointStamps;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPoint : AggregateRoot
    {
        public AccessPointBssid Bssid { get; private set; }
        public AccessPointManufacturer Manufacturer { get; private set; }
        public AccessPointSsid Ssid { get; private set; }
        public AccessPointDeviceType DeviceType { get; private set; }
        public AccessPointContributorId ContributorId { get; private set; }
        public AccessPointCreationTimestamp CreationTimestamp { get; private set; }
        public AccessPointModificationTimestamp ModificationTimestamp { get; private set; }
        public AccessPointPositioning Positioning { get; private set; }
        public AccessPointSecurity Security { get; private set; }
        public AccessPointNote Note { get; private set; }
        public AccessPointDisplayStatus DisplayStatus { get; private set; }

        private readonly List<AccessPointStamp> _stamps;
        public IReadOnlyCollection<AccessPointStamp> Stamps => _stamps.SkipDeleted().AsReadOnly();

        protected override void Handle(IEventBase @event)
        {
            throw new NotImplementedException();
        }

        protected override void Validate()
        {
            bool isNull = Bssid == null || Manufacturer == null || Ssid == null || DeviceType == null || ContributorId == null ||
                CreationTimestamp == null || ModificationTimestamp == null || Positioning == null || Security == null || Note == null || DisplayStatus == null;

            if (isNull)
                throw new BusinessLogicException("The accesspoint aggregate properties can not be null.");
        }

        private AccessPoint() =>
            _stamps = new List<AccessPointStamp>();
    }
}
