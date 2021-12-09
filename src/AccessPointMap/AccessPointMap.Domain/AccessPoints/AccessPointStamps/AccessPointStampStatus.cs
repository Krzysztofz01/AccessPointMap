using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointStamps
{
    public class AccessPointStampStatus : ValueObject<AccessPointStampStatus>
    {
        public bool Value { get; private set; }

        private AccessPointStampStatus() { }
        private AccessPointStampStatus(bool value)
        {
            Value = value;
        }

        public static implicit operator bool(AccessPointStampStatus stampStatus) => stampStatus.Value;

        public static AccessPointStampStatus Verified => new AccessPointStampStatus(true);
        public static AccessPointStampStatus Default => new AccessPointStampStatus(false);
        public static AccessPointStampStatus FromBool(bool stampStatus) => new AccessPointStampStatus(stampStatus);
    }
}
