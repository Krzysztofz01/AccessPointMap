using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.AccessPoints.AccessPointStamps
{
    public class AccessPointStamp : Entity
    {
        public AccessPointSsid Ssid { get; private set; }
        public AccessPointDeviceType DeviceType { get; private set; }
        public AccessPointContributorId ContributorId { get; private set; }
        public AccessPointCreationTimestamp CreationTimestamp { get; private set; }
        public AccessPointPositioning Positioning { get; private set; }
        public AccessPointSecurity Security { get; private set; }
        public AccessPointStampStatus Status { get; private set; }

        protected override void Handle(IEventBase @event)
        {
            throw new NotImplementedException();
        }

        protected override void Validate()
        {
            bool isNull = Ssid == null || DeviceType == null || ContributorId == null ||
                CreationTimestamp == null || Positioning == null || Security == null || Status == null;

            if (isNull)
                throw new BusinessLogicException("The accesspoint stamp entity properties can not be null.");
        }

        private AccessPointStamp() { }
    }
}
